

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridroomPaintPlayer : MonoBehaviour
{

    public int PlayerNumber;
    public bool isAI;
    public int score;
    public int Rank;
    public Color color;

    public GridroomTile[][] tiles = new GridroomTile[8][];
    public GridroomTile currentTile;
    public float dirX = 0f;
    public float dirY = 0f;
    public float speed;

    public int posX, posY;
    public bool canChange = false;
    private bool changing = false;

    private float speedFrames;

    private PlayerInput playerInput;

    void Awake()
    {
        if (!isAI)
        {
            playerInput = GetComponent<PlayerInput>();


        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.playersIngame >= PlayerNumber)
        {
            if (PlayerNumber == 1)
            {
                playerInput.SwitchCurrentControlScheme(
                    "Keyboard&Mouse",
                Keyboard.current,
                Mouse.current
            );
            }
            else
            {
                var gamepad = Gamepad.all[PlayerNumber - 2]; // primeiro controle
                playerInput.SwitchCurrentControlScheme(gamepad);
            }
        }
        else
        {
            isAI = true;
        }

        canChange = false;
        speed = 1f;
        var listTiles = GameObject.FindGameObjectsWithTag("Tile");
        int pos = 0;
        for (int i = 0; i < 8; i++)
        {
            tiles[i] = new GridroomTile[8];
            for (int j = 0; j < 8; j++)
            {
                tiles[i][j] = listTiles[pos].GetComponent<GridroomTile>();
                tiles[i][j].i = i;
                tiles[i][j].j = j;
                //Debug.Log(tiles[i][j].transform.position);
                pos++;
            }
        }
        switch (PlayerNumber)
        {
            case 1:
                color = Color.cyan;
                posX = 0;
                posY = 0;

                dirX = 1f;
                break;
            case 2:
                color = Color.red;
                posX = 7; posY = 0;
                dirY = -1f;
                break;
            case 3:
                color = Color.green;
                posX = 0; posY = 7;

                dirY = 1f;
                break;
            case 4:
                color = Color.yellow;
                posX = 7; posY = 7;

                dirX = -1f;
                break;
            default:
                break;

        }
        currentTile = tiles[posX][posY];
    }

    // Update is called once per frame
    void Update()
    {

        if (!isAI)
        {
            if (canChange)
            {

                if (playerInput.actions["Left"].IsPressed())
                {
                    canChange = false;
                    if (dirX != 1f)
                    {
                        dirX = -1f;
                        dirY = 0f;

                        transform.position = new Vector3(transform.position.x, currentTile.transform.position.y, 0f);
                    }
                }
                

                if (playerInput.actions["Right"].IsPressed())
                {
                    canChange = false;
                    if (dirX != -1f)
                    {
                        dirX = 1f;
                        dirY = 0f;

                        transform.position = new Vector3(transform.position.x, currentTile.transform.position.y, 0f);
                    }
                }
                if (playerInput.actions["Up"].IsPressed())
                {
                    canChange = false;
                    if (dirY != -1f)
                    {
                        dirY = 1f;
                        dirX = 0f;


                        transform.position = new Vector3(currentTile.transform.position.x, transform.position.y, 0f);
                    }
                }
                if (playerInput.actions["Down"].IsPressed())
                {
                    canChange = false;
                    if (dirY != 1f)
                    {
                        dirY = -1f;
                        dirX = 0f;


                        transform.position = new Vector3(currentTile.transform.position.x, transform.position.y, 0f);
                    }
                }
            }
        }
        else
        {

            if (canChange)
            {
                canChange = false;
                var r = Random.Range(0f, 10f);

                if (r <= 2.5f)
                {
                    if (dirX != 1f)
                    {
                        dirX = -1f;
                        dirY = 0f;
                    


                    transform.position = new Vector3(transform.position.x, currentTile.transform.position.y, 0f);
                    }
                }
                else if (r > 2.5f && r <= 5f)
                {
                    if (dirX != -1f)
                    {
                        dirX = 1f;
                        dirY = 0f;

                        transform.position = new Vector3(transform.position.x, currentTile.transform.position.y, 0f);
                    }
                }
                else if (r > 5f && r <= 7.5f)
                {
                    if (dirY != -1f)
                    {
                        dirY = 1f;
                        dirX = 0f;


                        transform.position = new Vector3(currentTile.transform.position.x, transform.position.y, 0f);
                    }
                }
                else
                {
                    if (dirY != 1f)
                    {
                        dirY = -1f;
                        dirX = 0f;

                        transform.position = new Vector3(currentTile.transform.position.x, transform.position.y, 0f);
                    }
                }
                
            }
            

        }

        

        var d = speed * Time.deltaTime;
        transform.position += new Vector3(dirX, dirY, 0f) * d;
        
        posX += (int)dirX;
        posY -= (int)dirY;

    }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                var t = col.gameObject.transform;
                if (transform.position.x != t.position.x)
                {
                    dirX = -dirX;
                }
                if (transform.position.y != t.position.y)
                {
                    dirY = -dirY;
                }
        }
        if (col.gameObject.tag == "Wall")
        {
            var t = col.gameObject.transform;
            if (dirX != 0)
            {
                dirX = -dirX;
            }
            if (dirY != 0)
            {
                dirY = -dirY;
            }
        }
            

        }
    
   
}
