using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : Collider
{
    //Physics object List
    public static List<PhysicsObject> PhysicsList;

    //Physics Object Variables
    public const float MinMove = .001f;
    public float vSpeed;
    public float hSpeed;
    public float SpeedMultiplier = 1f;
    bool Bouncy;
    public bool BlockCollision;

    public PhysicsObject(float x, float y, float Width, float Height, int ColType, Color color, float vSpeed, float hSpeed, bool Bouncy) : base(x, y, Width, Height, ColType, color)
    {
        //Physics Object Variables
        this.vSpeed = vSpeed;
        this.hSpeed = hSpeed;
        this.Bouncy = Bouncy;

        //Define List If Necessary
        if(PhysicsList == null)
        {
            PhysicsList = new List<PhysicsObject>();
        }

        //Add Self To Physics Object List
        PhysicsList.Add(this);
    }

    public virtual void ProcessPhysics()
    {
        //Reset Bounce Tracker
        BlockCollision = false;

        //Manage Movement Speed Multiplier
        vSpeed *= SpeedMultiplier;
        hSpeed *= SpeedMultiplier;

        //Manage Vertical Collision
        if (vSpeed != 0)
        {
            if(!PlaceMeeting(x, y + vSpeed * Time.deltaTime, 0))
            {
                y += vSpeed * Time.deltaTime;
            }
            else if(!PlaceMeeting(x, y + MinMove * Sign(vSpeed), 0))
            {
                do
                {
                    y += MinMove * Sign(vSpeed);
                }
                while (!PlaceMeeting(x, y + MinMove * Sign(vSpeed), 0));
            }

            //React On Collision
            if(PlaceMeeting(x, y + MinMove * Sign(vSpeed), 0))
            {
                //Collision Instance
                Collider tvCollider = InstanceMeeting(x, y + MinMove * Sign(vSpeed), 0);

                //Destroy Others
                if (tvCollider.Destructable)
                {
                    GameManager.Score += InstanceMeeting(x, y + MinMove * Sign(vSpeed), 0).Score;
                    ColliderList.Remove(InstanceMeeting(x, y + MinMove * Sign(vSpeed), 0));

                    BlockCollision = true;
                }

                //Bounce
                if (Bouncy) vSpeed *= -1;
                else vSpeed = 0;

                //Set Random Trajectory (On Collision With Paddle)
                if (tvCollider.Paddle)
                {
                    SetTrajectory(Random.Range(30, 150));
                }
            }
        }

        //Manage Horizontal Collision
        if(hSpeed != 0)
        {
            if(!PlaceMeeting(x + hSpeed * Time.deltaTime, y, 0))
            {
                x += hSpeed * Time.deltaTime;
            }
            else if(!PlaceMeeting(x + MinMove * Sign(hSpeed), y, 0))
            {
                do
                {
                    x += MinMove * Sign(hSpeed);
                }
                while (!PlaceMeeting(x + MinMove * Sign(hSpeed), y, 0));
            }
        }

        //React On Collision
        if (PlaceMeeting(x + MinMove * Sign(hSpeed), y, 0))
        {
            //Collision Instance
            Collider tvCollider = InstanceMeeting(x + MinMove * Sign(hSpeed), y, 0);

            //Destroy Others
            if (tvCollider.Destructable)
            {
                GameManager.Score += InstanceMeeting(x + MinMove * Sign(hSpeed), y, 0).Score;
                ColliderList.Remove(InstanceMeeting(x + MinMove * Sign(hSpeed), y, 0));

                BlockCollision = true;
            }
        
            //Bounce
            if (Bouncy) hSpeed *= -1;
            else hSpeed = 0;
          
        }

        //Reset Multiplier
        vSpeed = vSpeed / SpeedMultiplier;
        hSpeed = hSpeed / SpeedMultiplier;

        
    }//PHYSICS PROCESSING

    public void SetTrajectory(float Angle)
    {
        //Calculate New Trajectory
        float Magnitude = Mathf.Sqrt(Mathf.Pow(hSpeed, 2) + Mathf.Pow(vSpeed, 2));
        float xDist = Mathf.Cos((Angle / 180) * Mathf.PI);
        float yDist = Mathf.Sin((Angle / 180) * Mathf.PI);
        hSpeed = Magnitude * (xDist / (Mathf.Sqrt(Mathf.Pow(xDist, 2) + Mathf.Pow(yDist, 2))));
        vSpeed = -Magnitude * (yDist / (Mathf.Sqrt(Mathf.Pow(xDist, 2) + Mathf.Pow(yDist, 2))));
    }


    public float Sign(float Input)
    {
        if (Input > 0) return 1;
        else if (Input < 0) return -1;
        else return 0;        
    }

}
