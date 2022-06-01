using System;
using System.Collections.Generic;
using System.Numerics;

namespace Assets.Scripts
{
    public class SimulationHandler
    {
        private static Random rng = new Random();

        public static void SimultationStep(List<Toilet> toilets, float dt)
        {
            if (toilets.Count == 0) return;
            // new person based on time
            if (new Random().NextDouble() < dt)
            {
                var bestT = toilets[0];
                var pos = new Vector2((float)rng.NextDouble(), (float)rng.NextDouble());
                var bestV = float.PositiveInfinity;
                foreach (var toilet in toilets)
                {
                    if (Vector2.Distance(pos, toilet.coordinates) < bestV && toilet.IsFree() && !toilet.IsFull())
                    {
                        bestV = Vector2.Distance(pos, toilet.coordinates);
                        bestT = toilet;
                    }
                }
                bestT.OccupiedFor = (float)(100 * rng.NextDouble());
            }

            foreach (var toilet in toilets)
            {
                if (toilet.IsFull() && toilet.IsFree())
                {
                    toilet.Empty();
                }
            }
        }
    }
}