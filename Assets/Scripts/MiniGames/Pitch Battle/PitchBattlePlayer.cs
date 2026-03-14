
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine;
using UnityEngine.InputSystem;


public class PitchBattlePlayer : MonoBehaviour
{

    public int playerNumber;
    public bool isAI;
    public int score;
    public int Rank;

    public Baseball ball;

    private float dirX;
    private float dirY;

    private float strength;
    private bool charging = false;

    private Transform transform;

    private Vector3 dir1, dir2, dir3;

    private bool recharging;
    private int rechargingFrames;

    private PlayerInput playerInput;

    private bool wasPressedLastFrame = false;


    private bool up = false, down = false, left = false, right = false;

    void Awake()
    {
        if (!isAI)
        {
            playerInput = GetComponent<PlayerInput>();

            if (playerInput.currentControlScheme == "Gamepad")
            {

                playerInput.actions["Left Stick"].started += OnMove;

                playerInput.actions["D-Pad"].started += OnMove;


            }
            else
            {

                playerInput.actions["Up"].started += OnMoveUp;
                playerInput.actions["Up"].canceled += ctx => { up = false; };

                playerInput.actions["Down"].started += OnMoveDown;
                playerInput.actions["Down"].canceled += ctx => { down = false; };

                playerInput.actions["Left"].started += OnMoveLeft;
                playerInput.actions["Left"].canceled += ctx => { left = false; };

                playerInput.actions["Right"].started += OnMoveRight;
                playerInput.actions["Right"].canceled += ctx => { right = false; };


            }
        }
    }

    private void OnMoveUp(InputAction.CallbackContext ctx)
    {
        up = true;
       // down = false;
    }

    private void OnMoveDown(InputAction.CallbackContext ctx)
    {
        down = true;
       // up = false;
    }

    private void OnMoveLeft(InputAction.CallbackContext ctx)
    {
       left = true;
      //  right= false;
    }

    private void OnMoveRight(InputAction.CallbackContext ctx)
    {
       right = true;
       // left = false;
    }


    private void OnMove(InputAction.CallbackContext ctx)
    {
       
            var moveInput = ctx.ReadValue<Vector2>().normalized;

            if (moveInput.x < 0)
            {
            left = true;
          //  right = false;
            }


            if (moveInput.x > 0)
            {
            right = true;
           // left= false;
            }
            if (moveInput.y > 0)
            {
            up = true;
          //  down = false;
            }
            if (moveInput.y < 0)
            {
            down = true;
          //  up = false;
            }
        




    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.playersIngame >= playerNumber)
        {
            if (playerNumber == 1)
            {
                playerInput.SwitchCurrentControlScheme(
                    "Keyboard&Mouse",
                Keyboard.current,
                Mouse.current
            );
            }
            else
            {
                playerInput = GetComponent<PlayerInput>();

                var gamepad = Gamepad.all[playerNumber - 2]; // primeiro controle
                playerInput.SwitchCurrentControlScheme(gamepad);
            }
        }
        else
        {
            isAI = true;
        }

        recharging = false;
        rechargingFrames = 0;

        score = 0;
        Rank = 0;

        strength = 1.0f;
        charging = false;

        transform = GetComponent<Transform>();

        switch (playerNumber)
        {
            case 1:
                dirX = 1f;
                dirY = -1f;

                dir1 = new Vector3(1f,0f,0f);
                dir2 = new Vector3(1f, -1f, 0f);
                dir3 = new Vector3(0f, -1f, 0f);
                break;
            case 2:
                dirX = -1f;
                dirY = -1f;

                dir1 = new Vector3(-1f, 0f, 0f);
                dir2 = new Vector3(-1f, -1f, 0f);
                dir3 = new Vector3(0f, -1f, 0f);

                break;
            case 3:
                dirX = 1f;
                dirY = 1f;

                dir1 = new Vector3(1f, 0f, 0f);
                dir2 = new Vector3(1f, 1f, 0f);
                dir3 = new Vector3(0f, 1f, 0f);
                break;
            case 4:
                dirX = -1f;
                dirY = 1f;

                dir1 = new Vector3(-1f, 0f, 0f);
                dir2 = new Vector3(-1f, 1f, 0f);
                dir3 = new Vector3(0f, 1f, 0f);
                break;
            default: break;
        }
    }

    string ChecaAmbiente(Vector3 curDir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, curDir);

        if (hit)
        {
            return hit.collider.tag;
        }return null;
    }

    // Update is called once per frame
    void Update()
    {

       

        if (!isAI)
        {
            //bool up = false, down = false, left = false, right = false;
           
        /*
            if (playerInput.actions["Left"].IsPressed())
            {
                left = true;
            }
            if (playerInput.actions["Right"].IsPressed())
            {
                right = true;
            }
            if (playerInput.actions["Up"].IsPressed())
            {
                up = true;
            }
            if (playerInput.actions["Down"].IsPressed())
            {
                down = true;
            }*/

            if ((right && down) && playerNumber==1)
            {
                dirX = 1f;
                dirY = -1f;
            }
            if ((left && down) && playerNumber == 2)
            {
                dirX = -1f;
                dirY = -1f;
            }
            if ((right && up) && playerNumber == 3)
            {
                dirX = 1f;
                dirY = 1f;
            }
            if ((left && up) && playerNumber==4)
            {
                dirX = -1f;
                dirY = 1f;
            }

            if (left && !up && !down)
            {

                if (playerNumber == 2 || playerNumber == 4)
                {
                    dirX = -1f;
                    dirY = 0f;
                }
            }
            if (right && !up && !down)
            {
                if (playerNumber == 1 || playerNumber == 3)
                {
                    dirX = 1f;
                    dirY = 0f;
                }
            }
            if (up && !left && !right)
            {
                if (playerNumber == 3 || playerNumber == 4)
                {
                    dirX = 0f;
                    dirY = 1f;
                }
            }
            if (down && !left && !right)
            {
                if (playerNumber == 1 || playerNumber == 2)
                {
                    dirX = 0f;
                    dirY = -1f;
                }
            }

            if (!recharging)
            {

                bool isPressed = playerInput.actions["Space"].ReadValue<float>() > 0f;


                if (isPressed)
                {
                    if (!charging)
                    {
                        charging = true;
                    }
                }
                if (wasPressedLastFrame && !isPressed)
                {
                    if (charging)
                    {
                        charging = false;
                        var b = Instantiate(ball, transform.position + new Vector3(dirX, dirY), Quaternion.identity);

                        b.owner = this;
                        var bRB = b.rb;
                        bRB.AddForce(new Vector2(dirX, dirY) * strength, ForceMode2D.Impulse);

                        recharging = true;
                        rechargingFrames = 0;
                    }
                }
                if (charging)
                {
                    strength += 1f + Time.deltaTime;
                    if (strength >= 20f)
                    {
                        strength = 1f;
                    }
                }

                wasPressedLastFrame = isPressed;
            }
            else
            {
                rechargingFrames += 1 + (int)Time.deltaTime;
                if (rechargingFrames >= 60)
                {
                    recharging = false;
                    rechargingFrames = 0;
                }
            }
        }
        else
        {
            string str1, str2, str3;

            str1 = ChecaAmbiente(dir1);
            str2 = ChecaAmbiente(dir2);
            str3 = ChecaAmbiente(dir3);

            if (str1 == "Player" && Random.Range(0f,100f) >= 50f)
            {
                dirX = dir1.x; 
                dirY = dir1.y;
            }
            
            if (str2 == "Player" && Random.Range(0f, 100f) >= 50f)
            {
                dirX = dir2.x;
                dirY = dir2.y;
            }
            
            if (str3 == "Player" && Random.Range(0f, 100f) >= 50f)
            {
                dirX = dir3.x;
                dirY = dir3.y;
            }


            if (!recharging) {
                if (!charging && Random.Range(0f, 100f) >= 50f)
                {
                    charging = true;
                }


                if (charging && Random.Range(0f, 100f) >= 80f)
                {

                    charging = false;
                    var b = Instantiate(ball, transform.position + new Vector3(dirX, dirY), Quaternion.identity);

                    b.owner = this;
                    var bRB = b.rb;
                    bRB.AddForce(new Vector2(dirX, dirY) * strength, ForceMode2D.Impulse);

                    recharging = true;
                    rechargingFrames = 0;
                }


                if (charging)
                {
                    strength += 1f + Time.deltaTime;
                    if (strength >= 10f)
                    {
                        strength = 1f;
                    }
                }
            }
            else
            {
                rechargingFrames+= 1 + (int) Time.deltaTime;
                if (rechargingFrames >= 600)
                {
                    recharging = false;
                    rechargingFrames = 0;
                }
            }
        }
    }
}
