using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider
{
    //Collider List
    public static List<Collider> ColliderList;

    //Collider Variables
    public float x;
    public float y;
    public float Width;
    public float Height;
    public int ColType;
    public Color color;
    public bool Destructable = false;
    public int Score;
    public bool Paddle = false;

    //Collision Types: 
    //0 - Wall / Paddle
    //1 - Ball
    //2 - Paddle

    public Collider(float x, float y, float Width, float Height, int ColType, Color color)
    {
        //Define Variables
        this.x = x;
        this.y = y;
        this.Width = Width;
        this.Height = Height;
        this.ColType = ColType;
        this.color = color;

        //Define List If Necessary 
        if(ColliderList == null)
        {
            ColliderList = new List<Collider>();
        }

        //Add Self To Collider List
        ColliderList.Add(this);
    }

    public bool PlaceMeeting(float xPos, float yPos, int CollisionType)
    {
        //Initialize Return Variable
        bool ReturnVal = false;

        //Reset Position
        float prevX = x;
        float prevY = y;
        x = xPos;
        y = yPos;

        //Check For Collision
        foreach(Collider element in ColliderList)
        {
            //Check For Correct Collision Type
            if (element.ColType == CollisionType)
            {
                //Avoid Checking Collisions With Self
                if(element != this)
                {
                    
                    //Check For Box Collision
                    if(Mathf.Min(x + Width/2, element.x + element.Width/2) > Mathf.Max(x - Width/2, element.x - element.Width/2)
                    && Mathf.Min(y + Height / 2, element.y + element.Height / 2) > Mathf.Max(y - Height / 2, element.y - element.Height / 2))
                    {
                        ReturnVal = true;
                        break;
                    }
                }
            }
        }

        //Return To Inital Position
        x = prevX;
        y = prevY;

        //Return Value
        return ReturnVal;

    }//PLACEMEETING

    public Collider InstanceMeeting(float xPos, float yPos, int CollisionType)
    {
        //Initialize Return Variable
        Collider ReturnVal = null;

        //Reset Position
        float prevX = x;
        float prevY = y;
        x = xPos;
        y = yPos;

        //Check For Collision
        foreach (Collider element in ColliderList)
        {
            //Check For Correct Collision Type
            if (element.ColType == CollisionType)
            {
                //Avoid Checking Collisions With Self
                if (element != this)
                {
                    //Check For Box Collision
                    if (Mathf.Min(x + Width / 2, element.x + element.Width / 2) > Mathf.Max(x - Width / 2, element.x - element.Width / 2)
                    && Mathf.Min(y + Height / 2, element.y + element.Height / 2) > Mathf.Max(y - Height / 2, element.y - element.Height / 2))
                    {
                        ReturnVal = element;
                        break;
                    }
                }
            }
        }

        //Return To Inital Position
        x = prevX;
        y = prevY;

        //Return Value
        return ReturnVal;

    }//INSTANCE MEETING

    public void SetDestructable(bool InputBool)
    {
        Destructable = InputBool;
    }
}
