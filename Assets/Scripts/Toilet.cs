using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class Toilet
{
    public Vector2 coordinates;
    public float FreshWater;
    public float WasteWater1;
    public float WasteWater2;
    public float OccupiedFor;
    public readonly float FreshWaterMax;
    public readonly float WasteWater1Max;
    public readonly float WasteWater2Max;

    public Toilet(float x, float y)
    {
        coordinates.X = x;
        coordinates.Y = y;
        FreshWaterMax = 100.0f;
        WasteWater1Max = 100.0f;
        WasteWater2Max = 100.0f;
        Empty();
    }

    public bool IsFree()
    {
        return OccupiedFor <= 0.0f;
    }

    public void Tick(float delta)
    {
        OccupiedFor = OccupiedFor - delta > 0 ? OccupiedFor - delta : 0;
    }

    public bool IsFull()
    {
        return FreshWater < 0.1 * FreshWaterMax || WasteWater1 > 0.9 * WasteWater1Max ||
               WasteWater2 > 0.9 * WasteWater2Max;
    }

    public void Empty()
    {
        FreshWater = FreshWaterMax;
        WasteWater1 = WasteWater1Max;
        WasteWater2 = WasteWater2Max;
    }
}














