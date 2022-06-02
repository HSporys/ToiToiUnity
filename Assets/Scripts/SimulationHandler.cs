using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using Vector2 = System.Numerics.Vector2;

namespace Assets.Scripts
{
    public class SimulationHandler
    {
        private const float ToiletBaseDuration = 1f;
        private const float PersonsPerSecond = 20f;
        private const float FreshWaterUsageBase = 2;
        private const float WasteWater1UsageBase = 1.9f;
        private const float WasteWater2UsageBase = 2.1f;
        private const float XAreaMin = -35;
        private const float YAreaMin = -20;
        private const float XAreaMax = 35;
        private const float YAreaMax = 20;
        private static Random rng = new Random();
        private static float MaintinenceTimer = -1;
        private static int QueueCount = 0;


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
            return (float)Math.Exp(-lambda) *  sum;
        }

        private static int Faculty(int k)
        {
            int prod = 1;
            for (int i = k; i > 1; i--)
            {
                prod *= i;
            }
            return prod;
        }
        
        public static void SimultationStep(List<Toilet> toilets, float dt)
        {
            foreach (var toilet in toilets)
            {
                // request service for full toilets 
                if (toilet.IsFull())
                {
                    if(MaintinenceTimer <= 0)
                        MaintinenceTimer = 20;
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
            // new persons based on time
            Toilet bestT;
            QueueCount = 0;
            int newCount = PoissonRNG(dt * PersonsPerSecond) + QueueCount;
            for(int i = 0; i < newCount; i++)
            {
                bestT = null;
                var pos = new Vector2((float)rng.NextDouble() * (XAreaMax- XAreaMin) + XAreaMin, (float)rng.NextDouble() * (YAreaMax- YAreaMin) + YAreaMin);
                var bestV = float.PositiveInfinity;
                foreach (var toilet in toilets.Where(toilet => Vector2.Distance(pos, toilet.coordinates) < bestV && toilet.IsAvailable()))
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
            if (QueueCount > 0) {
                Debug.Log(QueueCount);
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