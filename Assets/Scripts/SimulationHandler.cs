using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using Vector2 = System.Numerics.Vector2;

namespace Assets.Scripts
{
    public enum MaintenanceMode
    {
        CallOnFull,
        PredictiveCall
    }

    public class SimulationHandler
    {
        private const float ToiletBaseDuration = 1f;
        private const float PersonsPerSecondPerArea = 20f / 1000f;
        private const float FreshWaterUsageBase = 2;
        private const float WasteWater1UsageBase = 1.9f;
        private const float WasteWater2UsageBase = 2.1f;
        private const float XAreaMin = -35;
        private const float YAreaMin = -20;
        private const float XAreaMax = 35;
        private const float YAreaMax = 20;
        private const float ServiceCallDelay = 20;
        private static Random rng = new Random();
        private static float MaintinenceTimer = -1;
        private static int QueueCount = 0;

        private static MaintenanceMode maintenanceMode = MaintenanceMode.PredictiveCall;


        private static int PoissonRNG(float rate)
        {
            var rand = rng.NextDouble();
            int count = 0;
            while (rand > PoissonCDF(rate, count))
            {
                count++;
            }

            return count;
        }

        private static float PoissonCDF(float lambda, int k)
        {
            float sum = 0;
            for (int i = 0; i <= k; i++)
            {
                float prod = 1;
                for (int j = 0; j < i; j++)
                {
                    prod *= lambda / (j + 1);
                }

                sum += prod; //(float)Math.Pow(lambda, i) / (float)Faculty(i);
            }

            return (float)Math.Exp(-lambda) * sum;
        }

        public static void CallForMaintenance()
        {
            if (MaintinenceTimer <= 0)
                MaintinenceTimer = ServiceCallDelay;
        }
        
        public static void SimultationStep(List<Toilet> toilets, List<SpawnArea> spawnAreas, float dt)
        {
            var predicedIssueCount = 0; 
            foreach (var toilet in toilets)
            {

                switch (maintenanceMode)
                {
                    case MaintenanceMode.PredictiveCall:
                        var fwUsage = FreshWaterUsageBase/2 * toilet.GetOccudiedPercent() / 100 / (ToiletBaseDuration / 2);
                        var ttFWIssue = (toilet.FreshWater - Toilet.FreshWaterTreshhold * toilet.FreshWaterMax) / fwUsage; 
                        var ww1Usage = WasteWater1UsageBase/2 * toilet.GetOccudiedPercent() / 100 / (ToiletBaseDuration / 2);
                        var ttWW1Issue = (toilet.WasteWater1Max * Toilet.WasteWaterTreshhold - toilet.WasteWater1) / ww1Usage; 
                        var ww2Usage = WasteWater2UsageBase/2 * toilet.GetOccudiedPercent() / 100 / (ToiletBaseDuration / 2);
                        var ttWW2Issue = (toilet.WasteWater1Max * Toilet.WasteWaterTreshhold - toilet.WasteWater1) / ww2Usage;
                        var ttIssue = Math.Min(Math.Min(ttFWIssue, ttWW1Issue), ttWW2Issue);
                        if (ttIssue < ServiceCallDelay)
                        {
                            predicedIssueCount++;
                        }
                        break;
                    case MaintenanceMode.CallOnFull:
                        // request service for full toilets 
                        if (toilet.IsFull())
                        {
                            CallForMaintenance();
                        }
                        break;
                }

                // update OccupiedFor
                if (toilet.OccupiedFor <= 0) continue;
                toilet.OccupiedFor -= dt;
                if (toilet.OccupiedFor > 0) continue;
                toilet.OccupiedFor = 0;
                toilet.FreshWater -= FreshWaterUsageBase * (float)rng.NextDouble();
                toilet.WasteWater1 += WasteWater1UsageBase * (float)rng.NextDouble();
                toilet.WasteWater2 += WasteWater2UsageBase * (float)rng.NextDouble();
                toilet.FreshWater = toilet.FreshWater < 0 ? 0 : toilet.FreshWater;
            }

            if (predicedIssueCount > 0.1 * toilets.Count)
            {
                CallForMaintenance();
            }
            
            // new persons based on time
            Toilet bestT;
            foreach (var spawnArea in spawnAreas)
            {
                int newCount = PoissonRNG(dt * PersonsPerSecondPerArea * spawnArea.Size()) + QueueCount;
                QueueCount = 0;
                for (int i = 0; i < newCount; i++)
                {
                    bestT = null;
                    var pos = new Vector2(
                        (float)rng.NextDouble() * (spawnArea.EndPoint.X - spawnArea.StartPoint.X) +
                        spawnArea.StartPoint.X,
                        (float)rng.NextDouble() * (spawnArea.EndPoint.Y - spawnArea.StartPoint.Y) +
                        spawnArea.StartPoint.Y);
                    var bestV = float.PositiveInfinity;
                    foreach (var toilet in toilets.Where(toilet =>
                                 Vector2.Distance(pos, toilet.coordinates) < bestV && toilet.IsAvailable()))
                    {
                        bestV = Vector2.Distance(pos, toilet.coordinates);
                        bestT = toilet;
                    }

                    if (bestT != null)
                    {
                        bestT.OccupiedFor = (float)(ToiletBaseDuration * rng.NextDouble());
                    }
                    else
                    {
                        QueueCount++;
                    }
                }

                Debug.Log("Queue: " + QueueCount);
            }

            if (MaintinenceTimer > 0)
            {
                MaintinenceTimer -= dt;
                if (MaintinenceTimer <= 0)
                {
                    foreach (var toilet in toilets)
                    {
                        // empty full toilets 
                        if (toilet.IsFull())
                        {
                            toilet.Empty();
                        }
                    }
                }
            }
        }
    }
}