

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class HallwayFlagPlayer : MonoBehaviour
{
    public Camera mc;

    public int PlayerNumber;
    public bool isAI;
    public int score;
    public int Rank;

    public GameObject flag;
    

    private Transform transform;
    private bool canJump = true;

    public Rigidbody2D rb;

    public bool KO = false;

    private PlayerInput playerInput;

    private float aiSpeed=2.0f;

    private float speedX = 0f;

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

               
                playerInput.actions["Left"].started += OnMoveLeft;
                playerInput.actions["Left"].canceled += ctx => { speedX = 0f; };

                playerInput.actions["Right"].started += OnMoveRight;
                playerInput.actions["Right"].canceled += ctx => { speedX = 0f; };


            }
        }
    }

    private void OnMoveLeft(InputAction.CallbackContext ctx)
    {

        speedX = -1f;
        //transform.position += new Vector3(speedX, 0f, 0f) * 3.0f * Time.deltaTime;
    }

    private void OnMoveRight(InputAction.CallbackContext ctx)
    {

        speedX = 1f;

        //transform.position += new Vector3(speedX, 0f, 0f) * 3.0f * Time.deltaTime;

    }


    private void OnMove(InputAction.CallbackContext ctx)
    {
        var moveInput = ctx.ReadValue<Vector2>().normalized;


        speedX = moveInput.x;


        //transform.position += new Vector3(speedX, 0f, 0f) * 3.0f * Time.deltaTime;
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
                playerInput = GetComponent<PlayerInput>();

                var gamepad = Gamepad.all[PlayerNumber - 2]; // primeiro controle
                playerInput.SwitchCurrentControlScheme(gamepad);
            }
        }
        else
        {
            isAI = true;
        }


        mc = Camera.main;

        KO = false;

        transform = GetComponent<Transform>();
        flag = GameObject.FindWithTag("Flag");
        rb = GetComponent<Rigidbody2D>();

        canJump = false;

        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var p in players)
        {
            if (p != this)
            {
                Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), p.GetComponent<BoxCollider2D>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < mc.transform.position.x - 15f || transform.position.x > mc.transform.position.x + 15f)
        {
            KO = true;
        }
        if (!KO) {
            if (!isAI)
            {
                transform.position += Vector3.right * 2.0f * Time.deltaTime;
                if (playerInput.actions["Space"].WasPressedThisFrame() && canJump)
                {
                    canJump = false;
                    rb.AddForce(transform.up * 10f, ForceMode2D.Impulse);
                }
                /*
                if (playerInput.actions["Left"].IsPressed())
                {
                    transform.position -= Vector3.right * 3.0f * Time.deltaTime;
                }
                else if (playerInput.actions["Right"].IsPressed())
                {
                    transform.position += Vector3.right * 3.0f * Time.deltaTime;
                }*/
                if (speedX != 0) {
                    transform.position += new Vector3(speedX, 0f, 0f) * 3.0f * Time.deltaTime;
                }
            }
            else
            {
                transform.position += Vector3.right * 2.0f * Time.deltaTime;
                if (aiSpeed == 0.0f)
                {
                    var players = GameObject.FindGameObjectsWithTag("Player");
                    var mustSpeedUp = false;
                    foreach (var p in players)
                    {
                        if (p != this.gameObject)
                        {
                            var delta = Vector3.Distance(transform.position, p.transform.position);
                            if (delta <= 2.5f && transform.position.x > p.transform.position.x && transform.position.x + 3.0f < mc.transform.position.x + 11f)
                            {
                                Debug.Log("speed up " + PlayerNumber);
                                transform.position += Vector3.right * 3.0f * Time.deltaTime;


                                mustSpeedUp = true;
                                break;
                            }

                        }

                    }
                    if (mustSpeedUp == true)
                    {
                        transform.position += Vector3.right * 3.0f * Time.deltaTime;

                    }
                }
                else
                {
                    var players = GameObject.FindGameObjectsWithTag("Player");
                    var canSlowDown = false;
                    foreach (var p in players)
                    {
                        if (p != this.gameObject)
                        {
                            var delta = Vector3.Distance(transform.position, p.transform.position);
                            if (delta > 2.5f && transform.position.x > p.transform.position.x && transform.position.x - 3.0f > mc.transform.position.x - 11f)
                            {
                                Debug.Log("slow down " + PlayerNumber);
                                aiSpeed = 0.0f;
                                canSlowDown = true;
                                break;
                            }
                        }
                    }
                    if (canSlowDown == false)
                    {
                        transform.position += Vector3.right * 3.0f * Time.deltaTime;

                    }
                }


                if (Random.Range(0f, 10f) > 5f)
                {
                    if (Mathf.Abs(transform.position.x - mc.transform.position.x) <= 2.5f)
                    {
                        transform.position += Vector3.right * 3.0f * Time.deltaTime;
                    }


                }

                var hurdles = GameObject.FindGameObjectsWithTag("Hurdle");
                foreach (var h in hurdles)
                {

                    if (Mathf.Abs(transform.position.x - h.transform.position.x) <= 2.5f && canJump)
                    {
                        if (Random.Range(0f, 10f) > 5f) {
                            canJump = false;
                            rb.AddForce(transform.up * 10f, ForceMode2D.Impulse);
                        }
                    }

                }
            } 
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = true;
        }
        if (collision.gameObject.tag == "Hurdle")
        {
            if (transform.position.y > collision.gameObject.transform.position.y+1.5f)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                Rank = HallwayFlagManager.studentsLeft;
                HallwayFlagManager.studentsLeft -= 1;

                KO = true;
            }
        }
        if (collision.gameObject.tag == "Flag")
        {


            Rank = 1;

            var players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < players.Length - 1; i++)
            {
                for (int j = i; j < players.Length; j++)
                {
                    if (players[i].transform.position.x > players[j].transform.position.x)
                    {
                        var aux = players[i];
                        players[i] = players[j];
                        players[j] = aux;
                    }

                }

            }

            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<HallwayFlagPlayer>().Rank = 4 - i;

            }

            HallwayFlagManager.finished = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = false; 
        }
    }
}
