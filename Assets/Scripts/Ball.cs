using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : PhysicsObject
{
    //Ball Variables 
    float[] Trajectory = new float[2];
    float MultiplierRate = .05f;
    float MultiplierCap = 3f;
    public bool BrokenOut = false;

    public Ball(float x, float y, float Width, float Height, int ColType, Color color, float vSpeed, float hSpeed, bool Bouncy) : base(x, y, Width, Height, ColType, color, vSpeed, hSpeed, Bouncy)
    {

    }

    public override void ProcessPhysics()
    {
        base.ProcessPhysics();

        //Increment Speed Multiplier (and Set Speed)
        if(BlockCollision)
        {
            if (SpeedMultiplier + MultiplierRate < MultiplierCap)
            {
                SpeedMultiplier += MultiplierRate;
            }
            else SpeedMultiplier = MultiplierCap;                    
        }

        //Breakout Zone Multiplier
        if(!BrokenOut && y < GameManager.BreakOutZone)
        {
            SpeedMultiplier = MultiplierCap;

            BrokenOut = true;
        }

    }
}
