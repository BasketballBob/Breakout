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
    bool Bouncy;
    public bool BounceOccured;

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
        BounceOccured = false;

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
                //Destroy Others
                if(InstanceMeeting(x, y + MinMove * Sign(vSpeed), 0).Destructable)
                {
                    ColliderList.Remove(InstanceMeeting(x, y + MinMove * Sign(vSpeed), 0));
                }

                //Bounce
                if (Bouncy) vSpeed *= -1;
                else vSpeed = 0;

                BounceOccured = true;
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
            //Destroy Others
            if (InstanceMeeting(x + MinMove * Sign(hSpeed), y, 0).Destructable)
            {
                ColliderList.Remove(InstanceMeeting(x + MinMove * Sign(hSpeed), y, 0));
            }

            //Bounce
            if (Bouncy) hSpeed *= -1;
            else hSpeed = 0;

            BounceOccured = true;
        }    
        
    }//PHYSICS PROCESSING

    public float Sign(float Input)
    {
        if (Input > 0) return 1;
        else if (Input < 0) return -1;
        else return 0;        
    }

}
