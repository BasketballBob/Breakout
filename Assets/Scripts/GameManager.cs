using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //Game Manager Variables
    public static int Score;
    public static int BallCount;
    public bool GameOver = false;
    public const float BreakOutZone = 80;
    float PaddleWidth = 100;
    float PaddleWidthDefault = 100;
    float PaddleWidthSmall = 50;
    float PaddleX1 = 20;
    float PaddleX2 = 1004;
    float WallWidth = 20;
    float BallDefaultSpeed = 130f;
    float BallResetAlarm = 1f;
    float BallResetTime = 1f;
    Color White = new Color(211f / 255f, 211f / 255f, 211f / 255f);

    //Grid Variables
    float GridX;
    float GridY;
    float GridWidth = 14;
    float GridHeight = 8;
    float RectWidth = 60;
    float RectHeight = 25;
    float BevelWidth;
    float BevelHeight = 5;

    //Integer Drawing Variables
    int drawGridWidth = 3;
    int drawGridHeight = 5;
    float blockWidth = 15;
    float blockHeight = 20;
    float blockSpacing = 0;
    float IntSpacing = 30;

    //Visual Variables
    float[,] BallTrail;
    int BallTrailLength = 10;

    //Rectangle Drawing Variables
    Texture2D RectTexture;
    GUIStyle RectStyle;

    //Important Game Object References
    Collider Paddle;
    Collider RightWall, LeftWall, TopWall;
    Ball BallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //Reset Initial Variables
        Score = 0;
        BallCount = 3;

        //Define Misc Variables
        BallTrail = new float[BallTrailLength, 2];
        Cursor.visible = false;

        //Define Rect Drawing Assets
        RectTexture = new Texture2D(1, 1);
        RectStyle = new GUIStyle();

        //Create Important Game Objects
        Paddle = new Collider(100, 700, PaddleWidth, 20, 0, new Color(0, 96f / 255f, 148f / 255f));
        Paddle.Paddle = true;
        RightWall = new Collider(1024 - WallWidth / 2, 768 / 2, WallWidth, 768, 0, White);
        LeftWall = new Collider(0 + WallWidth / 2, 768 / 2, WallWidth, 768, 0, White);
        TopWall = new Collider(1024 / 2, WallWidth / 2, 1024, WallWidth, 0, White);

        //Create Ball Prefab
        BallPrefab = new Ball(1024 / 2, 600, 20, 20, 1, White, BallDefaultSpeed, BallDefaultSpeed, true);
        //Collider.ColliderList.Remove(BallPrefab);
        //PhysicsObject.PhysicsList.Remove(BallPrefab);

        //Ball tvInst = BallPrefab;

        //Calculate Grid Variables
        GridX = WallWidth + RectWidth / 2;
        GridY = WallWidth + RectHeight / 2 + BreakOutZone;
        BevelWidth = (1024 - RectWidth * GridWidth - WallWidth * 2) / (GridWidth + 1);

        //Generate Grid Of Block
        for (int xPos = 0; xPos < GridWidth; xPos++)
        {
            for (int yPos = 0; yPos < GridHeight; yPos++)
            {
                //Set Variables On Row
                int BlockScore = 0;
                Color BlockColor = new Color(1f, 1f, 1f);

                //Set Block Values
                if (yPos <= 1)
                {
                    BlockScore = 7;
                    BlockColor = new Color(164f / 255f, 7f / 255f, 0f / 255f);
                }
                else if (yPos <= 3)
                {
                    BlockScore = 5;
                    BlockColor = new Color(201f / 255f, 127f / 255f, 0f / 255f);
                }
                else if (yPos <= 5)
                {
                    BlockScore = 3;
                    BlockColor = new Color(0f / 255f, 128f / 255f, 33f / 255f);
                }
                else if (yPos <= 7)
                {
                    BlockScore = 1;
                    BlockColor = new Color(201f / 255f, 201f / 255f, 31f / 255f);
                }

                //Spawn Block
                Collider tvCol = new Collider(GridX + xPos * RectWidth + (xPos + 1) * BevelWidth,
                GridY + yPos * RectHeight + (yPos + 1) * BevelHeight, RectWidth, RectHeight, 0, Color.red);
                tvCol.SetDestructable(true);
                tvCol.Score = BlockScore;
                tvCol.color = BlockColor;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Prompt Game Over
        if(BallCount <= 0 && !GameOver)
        {
            GameOver = true;         
        }

        //Manage Game Over
        if(GameOver)
        {
            //Reset Game
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                //Reset Collision Lists
                Collider.ColliderList = new List<Collider>();
                PhysicsObject.PhysicsList = new List<PhysicsObject>();

                //Reload Scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            return;
        }

        //Manage Paddle Scale
        if (!BallPrefab.BrokenOut) Paddle.Width = PaddleWidthDefault;
        else Paddle.Width = PaddleWidthSmall;

        //Player Mouse Paddle Control
        Paddle.x = Input.mousePosition.x;
        if (Paddle.x < PaddleX1 + Paddle.Width / 2) Paddle.x = PaddleX1 + Paddle.Width / 2;
        if (Paddle.x > PaddleX2 - Paddle.Width / 2) Paddle.x = PaddleX2 - Paddle.Width / 2;

        //Prompt Ball Reset
        if (BallPrefab.y > 800 || BallPrefab.y < -100)
        {
            BallResetAlarm = BallResetTime;
            BallCount -= 1;
        }

        //Actually Reset Ball
        if (BallResetAlarm > 0)
        {
            //Deduct Alarm 
            BallResetAlarm -= Time.deltaTime;

            //Stick Ball To Paddle
            BallPrefab.hSpeed = 0;
            BallPrefab.vSpeed = 0;
            BallPrefab.x = Paddle.x;
            BallPrefab.y = Paddle.y - 30f;
            BallPrefab.BrokenOut = false;

            //Free Ball
            if (BallResetAlarm <= 0)
            {
                BallPrefab.hSpeed = BallDefaultSpeed;
                BallPrefab.vSpeed = BallDefaultSpeed;
                BallPrefab.SetTrajectory(Random.Range(30, 150));

                BallPrefab.SpeedMultiplier = 1f;                                 
            }
        }
        else BallResetAlarm = 0;

    }

    private void OnGUI()
    {
        //Draw Black Background 
        DrawRect(1024 / 2, 768 / 2, 1024, 768, Color.black);

        //Draw Game Over Screen
        if (GameOver)
        {
            DrawRect(1024 / 2, 768 / 2, 1024, 768, Color.black);
            DrawNumber(1024/2, 768/2, 1);
            return;
        }

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
            //DrawRect(element.x, element.y, 10, 10, Color.yellow);
        }

        //Draw Ball Trail
        /*if(true)
        {
            //Update Ball Trail Array
            for(int tvPos = BallTrailLength-1; tvPos > 0+1; tvPos--)
            {
                BallTrail[tvPos, 0] = BallTrail[tvPos - 1, 0];
                BallTrail[tvPos, 1] = BallTrail[tvPos - 1, 1];
            }
            BallTrail[0, 0] = BallPrefab.x;
            BallTrail[0, 1] = BallPrefab.y;

            //Draw Ball Array
            for(int tvPos = 0; tvPos < BallTrailLength; tvPos++)
            {
                DrawRect(BallTrail[tvPos, 0], BallTrail[tvPos, 1], BallPrefab.Width, BallPrefab.Height,
                new Color(White.r, White.g, White.b, 1f)); // ((BallTrailLength - tvPos) / BallTrailLength+1)));
            }
        }*/

        //Draw UI Values
        DrawInt(400, 400, Score);
        DrawInt(200, 400, BallCount);
    }

    public void DrawRect(float x, float y, float width, float height, Color color)
    {
        //Set Texture Variabless
        if (color != RectTexture.GetPixel(1, 1))
        {
            RectTexture.SetPixel(1, 1, color);
            RectTexture.wrapMode = TextureWrapMode.Repeat;
            RectTexture.Apply();
            RectStyle.normal.background = RectTexture;
        }

        //Draw Rectangle
        GUI.Label(new Rect(x - width/2, y - height/2, width, height), GUIContent.none, RectStyle);
    }

    public void DrawInt(float xPos, float yPos, int Number)
    {
        //Determine Length Of Integer
        float IntRef = (float)Number;
        int IntLength = 0;
        while(Mathf.Floor(IntRef) > 0)
        {
            IntRef /= 10;
            IntLength++;
        }

        //EDGECASE (Print Zero If Value <= 0)
        if (IntLength == 0) IntLength = 1;

        //Get Draw Number Values
        int[] DrawVal = new int[IntLength];
        int SubtractVal = 0;
        for(int tvVal = 0;tvVal < IntLength;tvVal++)
        {
            //Redefine Int Reference
            IntRef = (float)Number;

            //Divide Integer Reference To Appropriate Value
            for(int tvPos = 0;tvPos < IntLength-1-tvVal;tvPos++)
            {
                IntRef /= 10;
            }

            //Set Draw Val (On Array)
            DrawVal[tvVal] = (int)Mathf.Floor(IntRef) - SubtractVal;

            //Set Subtract Value (Eliminate Unecessary Larger Vals)
            SubtractVal = (SubtractVal + DrawVal[tvVal]) * 10;
        }


        //Integer Drawing Variables
        float IntWidth = drawGridWidth * blockWidth + (drawGridWidth - 1) * blockSpacing;
        //IntSpacing

        //Draw Integer
        for(int tvPos = 0;tvPos < IntLength;tvPos++)
        {
            DrawNumber(xPos + tvPos * IntWidth + (tvPos + 1) * IntSpacing, yPos, DrawVal[tvPos]);
        }
    }

    public void DrawNumber(float xPos, float yPos, int DrawNumber)
    {

        //Number Drawing Values
        int[,] NumberDrawVal = new int[10, 15]
        {
            {1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1}, //0
            {1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 1, 1}, //1
            {1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1}, //2
            {1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1}, //3
            {1, 0, 1, 1, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1}, //4
            {1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 1}, //5
            {1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1}, //6 
            {1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1}, //7
            {1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1}, //8
            {1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1}  //9
        };

        //Draw Integer
        for(int tvX = 0;tvX < drawGridWidth;tvX++)
        {
            for(int tvY = 0;tvY < drawGridHeight;tvY++)
            {
                //Only Draw Filled Values
                if (NumberDrawVal[DrawNumber, (tvY * drawGridWidth + tvX)] == 1)
                {
                    DrawRect(xPos + tvX * blockWidth + (tvX + 1) * blockSpacing, yPos + tvY * blockHeight + (tvY + 1) * blockSpacing, blockWidth, blockHeight, new Color(1f, 1f, 1f, .3f));
                }
            }
        }
    }
}
