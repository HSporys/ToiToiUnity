using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Vector2 = System.Numerics.Vector2;

namespace Assets.Scripts
{
    public class SimulationHandler
    {
        private const float ToiletBaseDuration = .5f;
        private const float PersonsPerSecond = 1f;
        private const float FreshWaterUsageBase = 2;
        private const float WasteWater1UsageBase = 1.9f;
        private const float WasteWater2UsageBase = 2.1f;
        private const float XAreaMin = -10;
        private const float YAreaMin = -10;
        private const float XAreaMax = 10;
        private const float YAreaMax = 10;
        private static Random rng = new Random();

        public static void SimultationStep(List<Toilet> toilets, float dt)
        {
            Debug.Log(toilets.Count);
            if (toilets.Count == 0) return;
            // new persons based on time
            while (new Random().NextDouble() < dt * PersonsPerSecond)
            {
                Toilet bestT = null;
                var pos = new Vector2((float)rng.NextDouble() * (XAreaMax- XAreaMin) + XAreaMin, (float)rng.NextDouble() * (YAreaMax- YAreaMin) + YAreaMin);
                var bestV = float.PositiveInfinity;
                foreach (var toilet in toilets)
                {
                    if (Vector2.Distance(pos, toilet.coordinates) < bestV && toilet.IsAvailable())
                    {
                        bestV = Vector2.Distance(pos, toilet.coordinates);
                        bestT = toilet;
                    }
                }

                if (bestT != null)
                {
                    bestT.OccupiedFor = (float)(ToiletBaseDuration * rng.NextDouble());
                }

                dt -= 1;
            }

            
            foreach (var toilet in toilets)
            {
                // empty full toilets 
                if (toilet.IsFull())
                {
                    toilet.Empty();
                } 
                // update OccupiedFor
                if (toilet.OccupiedFor > 0)
                {
                    toilet.OccupiedFor -= dt;
                    if (toilet.OccupiedFor <= 0)
                    {
                        toilet.OccupiedFor = 0;
                        toilet.FreshWater -= FreshWaterUsageBase;
                        toilet.WasteWater1 += WasteWater1UsageBase;
                        toilet.WasteWater2 += WasteWater2UsageBase;
                        toilet.FreshWater = toilet.FreshWater < 0 ? 0 : toilet.FreshWater;
                    }
                }
            }
        }
    }
}