using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : PhysicsObject
{
    //Ball Variables 
    float[] Trajectory = new float[2];
    float BallSpeed = 1;
    float SpeedMultiplier = 1;
    float MultiplierRate;

    public Ball(float x, float y, float Width, float Height, int ColType, Color color, float vSpeed, float hSpeed, bool Bouncy) : base(x, y, Width, Height, ColType, color, vSpeed, hSpeed, Bouncy)
    {

    }

    public override void ProcessPhysics()
    {
        //Set Ball Speed

        base.ProcessPhysics();

        //Increment Speed Multiplier
        if(BounceOccured)
        {
            SpeedMultiplier += MultiplierRate;
        }
    }
}
