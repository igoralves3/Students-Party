

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

using UnityEngine.InputSystem.Users;

public class CoconutClashPlayer : MonoBehaviour
{
    public int PlayerNumber;
    public bool isAI;
    public int score;
    public int Rank;

    public GameObject coconut;

    private Transform transform;
    private GameObject teacher;

    private float waitThrowFrames = 0;
    private bool canThrow = true;


    private PlayerInput playerInput;
    private InputDevice device;

    public float speedX = 0f;
    public float speedY = 0f;

    public Rigidbody2D rb;
    private RigidbodyType2D tipoOriginal;

    private BoxCollider2D bc;
    private bool stopped = false;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.SwitchCurrentActionMap("Player" + PlayerNumber);
    }

    void Awake()
    {
        if (!isAI)
        {
            playerInput = GetComponent<PlayerInput>();

            // Aqui você recebe o controle que está usando
            //device = playerInput.devices[PlayerNumber - 1];

            if (playerInput.currentControlScheme == "Gamepad")
            {

                playerInput.actions["Left Stick"].started += OnMove;

                playerInput.actions["D-Pad"].started += OnMove;

                playerInput.actions["Space"].started += OnSpacePress;
            }
            else
            {
                playerInput.actions["Up"].started += OnMoveUp;
                playerInput.actions["Up"].canceled += ctx => { speedX = 0f; speedY = 0f; };

                playerInput.actions["Down"].started += OnMoveDown;
                playerInput.actions["Down"].canceled += ctx => { speedX = 0f; speedY = 0f; };

                playerInput.actions["Left"].started += OnMoveLeft;
                playerInput.actions["Left"].canceled += ctx => { speedX = 0f; speedY = 0f; };

                playerInput.actions["Right"].started += OnMoveRight;
                playerInput.actions["Right"].canceled += ctx => { speedX = 0f; speedY = 0f; };

                playerInput.actions["Space"].started += OnSpacePress;
            }
        }
    }

    // Start is called before the first frame update
   void Start()
    {
       
        if (GameManager.playersIngame >= PlayerNumber)
        {
            if (PlayerNumber==1)
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

               
                var gamepad = Gamepad.all[PlayerNumber-2]; // primeiro controle
                Debug.Log(gamepad);

               

               

                playerInput.SwitchCurrentControlScheme(gamepad);
               
            }
            isAI = false;
        }
        else
        {
            isAI = true;
        }


            rb = GetComponent<Rigidbody2D>();

        
        tipoOriginal = rb.bodyType;

        bc = GetComponent<BoxCollider2D>();

        score = 0;
        transform = GetComponent<Transform>();

        teacher = GameObject.FindWithTag("Teacher");

        waitThrowFrames = 0;
        canThrow = true;

       
    }

    private void OnMoveUp(InputAction.CallbackContext ctx)
    {
       
            if (PlayerNumber == 1 || PlayerNumber == 2)
            {
                if (transform.position.y <= 2)
                {
                speedY = 2f;
                //transform.position += Vector3.up * 5f * Time.deltaTime;
            }
            else
            {
               
                speedY = 0f;
            }
            }
        
    }

    private void OnMoveDown(InputAction.CallbackContext ctx)
    {
        if (PlayerNumber == 1 || PlayerNumber == 2)
        {
            if (transform.position.y>= -2)
            {
                speedY = -2f;
                // transform.position -= Vector3.up * Time.deltaTime;
            }
            else
            {
               
                speedY = 0f;
            }
        }
    }

    private void OnMoveLeft(InputAction.CallbackContext ctx)
    {
        if (PlayerNumber == 3 || PlayerNumber == 4)
        {
            if (transform.position.x >= -2)
            {
                speedX = -2f;
               // transform.position -= Vector3.right * Time.deltaTime;
            }
            else
            {
             
                speedX = 0f;
            }
        }
    }

    private void OnMoveRight(InputAction.CallbackContext ctx)
    {
        if (PlayerNumber == 3 || PlayerNumber == 4)
        {
            if (transform.position.x <= 2)
            {
                speedX = 2f;
                ///transform.position += Vector3.right * Time.deltaTime;
            }
            else
            {
               
                speedX = 0f;
            }
        }
    }

    private void OnSpacePress(InputAction.CallbackContext ctx)
    {
       
       
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var moveInput = context.ReadValue<Vector2>().normalized;

        if (PlayerNumber == 1 || PlayerNumber == 2)
        {
            if (transform.position.y >= -2)
            {
                speedY = -2f * moveInput.x;
                // transform.position -= Vector3.up * Time.deltaTime;
            }
            else
            {

                speedY = 0f;
            }
        }

        if (PlayerNumber == 3 || PlayerNumber == 4)
        {
            if (transform.position.x <= 2)
            {
                speedY = -2f * moveInput.y;
                ///transform.position += Vector3.right * Time.deltaTime;
            }
            else
            {

                speedX = 0f;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (stopped)
        {
            return;
        }
        if (!isAI)
        {
            transform.position += new Vector3(speedX, speedY, 0f) * Time.deltaTime;

           

            if (PlayerNumber==1 || PlayerNumber==2)
            {
                if (transform.position.y >= 2)
                {
                    transform.position = new Vector3(transform.position.x, 2f,0f);
                    speedY = 0f;
                }else if (transform.position.y <= -2)
                    {
                        transform.position = new Vector3(transform.position.x, -2f, 0f);
                        speedY = 0f;
                    }
            }
            else if (PlayerNumber == 3 || PlayerNumber == 4)
            {
                if (transform.position.x >= 2)
                {
                    transform.position = new Vector3(2f, transform.position.y,0f);
                    speedX = 0f;
                }
                else if (transform.position.x <= -2)
                    {
                        transform.position = new Vector3(-2f, transform.position.y, 0f);
                        speedX = 0f;
                    }
            }


           


                if (playerInput.actions["Space"].WasPressedThisFrame())
                {
                    if (canThrow)
                    {
                        canThrow = false;
                        waitThrowFrames = 0;
                        GameObject c;
                        c = Instantiate(coconut, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);

                        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), c.GetComponent<BoxCollider2D>());
                        var borders = GameObject.FindGameObjectsWithTag("Floor");
                        foreach (var b in borders)
                        {
                            Physics2D.IgnoreCollision(c.GetComponent<BoxCollider2D>(), b.GetComponent<TilemapCollider2D>());

                        }
                        var ct = c.GetComponent<CoconutThrow>();

                      


                        ct.owner = this;

                        if (PlayerNumber == 3)
                        {
                            c.GetComponent<CoconutThrow>().dirY = 2;

                        }
                        else if (PlayerNumber == 4)
                        {
                            c.GetComponent<CoconutThrow>().dirY = -2;
                        }
                        if (PlayerNumber == 1)
                        {
                            c.GetComponent<CoconutThrow>().dirX = 2;

                        }
                        else if (PlayerNumber == 2)
                        {
                            c.GetComponent<CoconutThrow>().dirX = -2;
                        }
                    }

                }


                if (!canThrow)
                {
                    waitThrowFrames++;
                    if (waitThrowFrames >= 60)
                    {
                        waitThrowFrames = 0;
                        canThrow = true;
                    }

                    // canThrow = true;
                }
           
        }
        else
        {
            if (PlayerNumber == 1 || PlayerNumber == 2)
            {
                if (teacher.transform.position.y > transform.position.y && transform.position.y <= 2)
                {
                    transform.position += Vector3.up * Time.deltaTime;
                }
                if (teacher.GetComponent<Transform>().position.y < transform.position.y && transform.position.y >= -2)
                {
                    transform.position -= Vector3.up * Time.deltaTime;
                }

                var delta = teacher.GetComponent<Transform>().position.y - transform.position.y;

                if (delta > -1.0 && delta < 1.0)
                {
                    if (canThrow && Random.Range(0f, 20f) >= 10f)
                    {
                        canThrow = false;
                        GameObject c;
                        c = Instantiate(coconut, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);

                        var ct = c.GetComponent<CoconutThrow>();

                        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), c.GetComponent<BoxCollider2D>());
                        var borders = GameObject.FindGameObjectsWithTag("Floor");
                        foreach (var b in borders)
                        {
                            Physics2D.IgnoreCollision(c.GetComponent<BoxCollider2D>(), b.GetComponent<TilemapCollider2D>());

                        }



                        ct.owner = this;

                        if (PlayerNumber == 3)
                        {
                            c.GetComponent<CoconutThrow>().dirY = 2;

                        }
                        else if (PlayerNumber == 4)
                        {
                            c.GetComponent<CoconutThrow>().dirY = -2;
                        }
                        if (PlayerNumber == 1)
                        {
                            c.GetComponent<CoconutThrow>().dirX = 2;

                        }
                        else if (PlayerNumber == 2)
                        {
                            c.GetComponent<CoconutThrow>().dirX = -2;
                        }
                    }

                }

                if (!canThrow)
                {
                    waitThrowFrames++;
                    if (waitThrowFrames >= 600)
                    {
                        waitThrowFrames = 0;
                        canThrow = true;
                    }

                }
            }
            else
            {
                if (teacher.GetComponent<Transform>().position.x < transform.position.x && transform.position.x >= -2)
                {
                    transform.position -= Vector3.right * Time.deltaTime;
                }
                if (teacher.GetComponent<Transform>().position.x > transform.position.x && transform.position.x <= 2)
                {
                    transform.position += Vector3.right * Time.deltaTime;
                }

                var delta = teacher.GetComponent<Transform>().position.x - transform.position.x;

                if (delta > -1.0 && delta < 1.0)
                {
                    if (canThrow && Random.Range(0f, 20f) >= 10f)
                    {
                        canThrow = false;
                        var c = Instantiate(coconut, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);

                        var ct = c.GetComponent<CoconutThrow>();

                        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), c.GetComponent<BoxCollider2D>());
                        var borders = GameObject.FindGameObjectsWithTag("Floor");
                        foreach (var b in borders)
                        {
                            Physics2D.IgnoreCollision(c.GetComponent<BoxCollider2D>(), b.GetComponent<TilemapCollider2D>());

                        }



                        ct.owner = this;

                        if (PlayerNumber == 3)
                        {
                            c.GetComponent<CoconutThrow>().dirY = 2;

                        }
                        else if(PlayerNumber == 4)
                        {
                            c.GetComponent<CoconutThrow>().dirY = -2;
                        }
                        if (PlayerNumber == 1)
                        {
                            c.GetComponent<CoconutThrow>().dirX = 2;

                        }
                        else if (PlayerNumber == 2)
                        {
                            c.GetComponent<CoconutThrow>().dirX = -2;
                        }
                    }

                }

                if (!canThrow)
                {
                    waitThrowFrames++;
                    if (waitThrowFrames >= 600)
                    {
                        waitThrowFrames = 0;
                        canThrow = true;
                    }

                    // canThrow = true;
                }
            }

        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        
        if (collision.gameObject.CompareTag("Coconut") || collision.gameObject.CompareTag("Teacher"))
        {
           
                StartCoroutine(PararTemporariamente());
            
        }

    }

    IEnumerator PararTemporariamente()
    {
        stopped = true;
        Debug.Log("parado");
        rb.bodyType = RigidbodyType2D.Static; // congela totalmente o corpo
                                              // bc.enabled = false; // desativa colisões

        bc.enabled = false;


        yield return new WaitForSeconds(1f);


        rb.bodyType = tipoOriginal;
        bc.enabled = true;
        stopped = false;
    }
}
   


