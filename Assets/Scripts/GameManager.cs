using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //Game Manager Variables
    float PaddleSpeed = 50;
    float PaddleWidth = 100;
    float PaddleX1 = 0;
    float PaddleX2 = 1024;
    float WallWidth = 20;

    //Grid Variables
    float GridX = 10;
    float GridY = 10;
    float GridWidth = 10;
    float GridHeight = 5;
    float RectWidth = 30;
    float RectHeight = 20;
    float GridBevel = 5;

    //Rectangle Drawing Variables
    Texture2D RectTexture;
    GUIStyle RectStyle;

    //Important Game Object References
    Collider Paddle;
    Collider RightWall, LeftWall, TopWall;

    // Start is called before the first frame update
    void Start()
    {
        //Define Rect Drawing Assets
        RectTexture = new Texture2D(1, 1);
        RectStyle = new GUIStyle();

        //Create Important Game Objects
        Paddle = new Collider(100, 700, PaddleWidth, 20, 0, Color.magenta);
        RightWall = new Collider(1024 - WallWidth / 2, 768 / 2, WallWidth, 768, 0, Color.white);
        LeftWall = new Collider(0 + WallWidth / 2, 768 / 2, WallWidth, 768, 0, Color.white);
        TopWall = new Collider(1024 / 2, 768 - WallWidth/2, 0, WallWidth, 0, Color.white);

        //Create Ball Prefab


        //Generate Grid Of Block
        for (int xPos = 0; xPos < GridWidth; xPos++)
        {
            for(int yPos = 0; yPos < GridHeight; yPos++)
            {
                //Spawn Block
                Collider tvCol = new Collider(GridX + xPos*RectWidth + (xPos+1)*GridBevel, 
                GridY + yPos*RectHeight + (yPos+1)*GridBevel, RectWidth, RectHeight, 0, Color.red);
                tvCol.SetDestructable(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Player Input 
        //bool right = Input.GetKey(KeyCode.RightArrow);
        //bool left = Input.GetKey(KeyCode.LeftArrow);

        //Control Paddle
        //if (right) Paddle.hSpeed = PaddleSpeed;
        //else if (left) Paddle.hSpeed = -PaddleSpeed;
        //else Paddle.hSpeed = 0;

        //Player Mouse Paddle Control
        Paddle.x = Input.mousePosition.x;
        if (Paddle.x < PaddleX1+PaddleWidth/2) Paddle.x = PaddleX1+PaddleWidth/2;
        if (Paddle.x > PaddleX2-PaddleWidth/2) Paddle.x = PaddleX2-PaddleWidth/2;
        
    }

    private void OnGUI()
    {
        //Manage Physics Objects 
        if (PhysicsObject.PhysicsList != null)
        {
            foreach (PhysicsObject element in PhysicsObject.PhysicsList)
            {
                element.ProcessPhysics();
            }
        }

        //Draw All Colliders
        foreach(Collider element in Collider.ColliderList)
        {
            DrawRect(element.x, element.y, element.Width, element.Height, element.color);
            DrawRect(element.x, element.y, 10, 10, Color.yellow);
        }
    }

    public void DrawRect(float x, float y, float width, float height, Color color)
    {
        //Set Texture Variabless
        RectTexture.SetPixel(1, 1, color);
        RectTexture.wrapMode = TextureWrapMode.Repeat;
        RectTexture.Apply();
        RectStyle.normal.background = RectTexture;

        //Draw Rectangle
        GUI.Label(new Rect(x - width/2, y - height/2, width, height), GUIContent.none, RectStyle);
    }
}
