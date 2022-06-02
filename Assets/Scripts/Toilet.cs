using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class Toilet
{
    
    public const float FreshWaterTreshhold = 0.05f;
    public const float WasteWaterTreshhold = 0.9f;
    
    public Vector2 coordinates;
    public float FreshWater;
    public float WasteWater1;
    public float WasteWater2;
    public float OccupiedFor;
    public float TimeOccupied;
    public float TimeUnoccupied;
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

    public bool IsAvailable()
    {
        return OccupiedFor <= 0.0f && !IsFull();
    }


    public bool IsFull()
    {
        bool full =  FreshWater < FreshWaterTreshhold * FreshWaterMax || WasteWater1 > WasteWaterTreshhold * WasteWater1Max ||
               WasteWater2 > WasteWaterTreshhold * WasteWater2Max;
        return full;
    }

    public void Empty()
    {
        FreshWater = FreshWaterMax;
        WasteWater1 = 0;
        WasteWater2 = 0;
    }

    public void Tick(float dt)
    {
        if (!IsFull())
        {
            if (IsAvailable())
            {
                TimeUnoccupied += dt;
            }
            else
            {
                TimeOccupied += dt;
            }
        }
    }

    public float GetOccudiedPercent()
    {
        return TimeOccupied * 100 / (TimeOccupied + TimeUnoccupied + 0.000001f);
    }
}














