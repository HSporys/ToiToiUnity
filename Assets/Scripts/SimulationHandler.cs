using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using Vector2 = System.Numerics.Vector2;

namespace Assets.Scripts
{
    public class SimulationHandler
    {
        private const float ToiletBaseDuration = .5f;
        private const float PersonsPerSecond = 10f;
        private const float FreshWaterUsageBase = 2;
        private const float WasteWater1UsageBase = 1.9f;
        private const float WasteWater2UsageBase = 2.1f;
        private const float XAreaMin = -35;
        private const float YAreaMin = -20;
        private const float XAreaMax = 35;
        private const float YAreaMax = 20;
        private static Random rng = new Random();

        public static void SimultationStep(List<Toilet> toilets, float dt)
        {
            if (toilets.Count == 0) return;
            // new persons based on time
            Toilet bestT;
            var count_time = dt * PersonsPerSecond;
            while (new Random().NextDouble() < count_time)
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

                count_time -= 1;
            }

            
            foreach (var toilet in toilets)
            {
                // empty full toilets 
                if (toilet.IsFull())
                {
                    toilet.Empty();
                } 
                // update OccupiedFor
                if (!(toilet.OccupiedFor > 0)) continue;
                toilet.OccupiedFor -= dt;
                if (!(toilet.OccupiedFor <= 0)) continue;
                toilet.OccupiedFor = 0;
                toilet.FreshWater -= FreshWaterUsageBase * (float)rng.NextDouble();
                toilet.WasteWater1 += WasteWater1UsageBase * (float)rng.NextDouble();
                toilet.WasteWater2 += WasteWater2UsageBase * (float)rng.NextDouble();
                toilet.FreshWater = toilet.FreshWater < 0 ? 0 : toilet.FreshWater;
            }
        }
    }
}