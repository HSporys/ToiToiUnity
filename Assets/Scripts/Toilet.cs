using System.Collections;
using System.Collections.Generic;

public class Toilet :
{
    public Vec2 coordinates;
    public float freshWater;
    public float wasteWater1;
    public float wasteWater2;
    public float occupiedFor;
    public readonly float freshWaterMax;
    public readonly float wasteWater1Max;
    public readonly float wasteWater2Max;

    public Toilet(float x, float y)
    {
        coordinates.x = x;
        coordinates.y = y;
        freshWaterMax = 100.0f;
        wasteWater1Max = 100.0f;
        wasteWater2Max = 100.0f;
        freshWater = freshWaterMax;
        wasteWater1 = wasteWater1Max;
        wasteWater2 = wasteWater2Max
    }

    public bool IsFree()
    {
        return occupiedFor <= 0.0f;
    }

    public void tick(float deltat)
    {
        
    }

}














