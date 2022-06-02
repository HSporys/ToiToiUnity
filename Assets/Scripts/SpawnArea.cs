using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class SpawnArea
{
    public Vector2 StartPoint;
    public Vector2 EndPoint;

    public float Size()
    {
        return Math.Abs((StartPoint.X - EndPoint.X) * (StartPoint.Y - EndPoint.Y));
    }
}
