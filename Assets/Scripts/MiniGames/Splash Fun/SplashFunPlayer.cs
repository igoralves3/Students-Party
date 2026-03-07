

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;


public class SplashFunPlayer : MonoBehaviour
{
    public int PlayerNumber;
    public bool isAI;
    public int Rank;
    public int score;

    private Transform transform;

    public GameObject balloon;

    public int dir;
    
    public bool podeAtirar = true;
    public const int leftDir = 1, rightDir = 2, upDir = 3, downDir = 4;

    public int balloonsLeft = 3;
    private bool canThrowBalloon = true;

    public int delayShootFrames = 0;
    private int delayB;
    public float dirThrow = 0f, dirThrowY=0f;

    public NavMeshAgent agent;

    public int STATE_SEARCH = 1, STATE_CHASE = 2;
    public int curState;
    public GameObject curPlayer;

    private Vector3 move_direction;
    private float wander_time;

    private bool zWasPressedLastFrame = false;
    private bool xWasPressedLastFrame = false;
    private PlayerInput playerInput;

    public BoxCollider2D bc;

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

        bc = GetComponent<BoxCollider2D>();

        dirThrow = -1f;
        delayShootFrames = 0;
        delayB = 0;
        balloonsLeft= 3;
        canThrowBalloon= true;
        podeAtirar=true;
        transform = GetComponent<Transform>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;


        switch (PlayerNumber)
        {
            case 1:
                dirThrow = 1f;
                dirThrowY = 0f;
                dir = rightDir;
               
                break;
            case 2:
                dirThrow = 0f;
                dirThrowY = -1f;
                dir = downDir;
                break;
            case 3:
                dirThrow = 0f;
                dirThrowY = 1f;
                dir = upDir;
                break;
            case 4:
                dirThrow = -1f;
                dirThrowY = 0f;
                dir = leftDir;
                break;
            default:
                break;
        }

        

        var solo = GameObject.FindGameObjectsWithTag("Floor");
        foreach (var b in solo)
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), b.GetComponent<TilemapCollider2D>());

        }
        if (isAI)
        {
            curState = STATE_SEARCH;

            RandomizeWander();
            
            //agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAI)
        {
            if (playerInput.actions["Left"].IsPressed())
            {
                dirThrow = -1f;
                dirThrowY = 0f;
                dir = leftDir;
                transform.position -= Vector3.right * Time.deltaTime;
            }
            else if (playerInput.actions["Right"].IsPressed())
            {
                dirThrow = 1f;
                dirThrowY = 0f;
                dir = rightDir;
                transform.position += Vector3.right * Time.deltaTime;
            }

            else if (playerInput.actions["Up"].IsPressed())
            {
                dir = upDir;
                dirThrow = 0f;
                dirThrowY = 1f;
                transform.position += Vector3.up * Time.deltaTime;
            }
            else if (playerInput.actions["Down"].IsPressed())
            {
                dir = downDir;
                dirThrow = 0f;
                dirThrowY = -1f;
                transform.position -= Vector3.up * Time.deltaTime;
            }

            // Lê o valor atual do botão
            bool zIsPressed = playerInput.actions["Z"].ReadValue<float>() > 0f;
            bool xIsPressed = playerInput.actions["X"].ReadValue<float>() > 0f;


            if (zWasPressedLastFrame && !zIsPressed)
            {

                podeAtirar = false;
                delayShootFrames = 0;
            }
            else
            {
                podeAtirar = true;
                delayShootFrames++;
                if (delayShootFrames >= 600)
                {
                    delayShootFrames = 0;
                    podeAtirar = true;
                }
            }
            zWasPressedLastFrame = zIsPressed;

            if (xWasPressedLastFrame && !xIsPressed)
            {
                if (balloonsLeft > 0)
                {
                    balloonsLeft--;
                    canThrowBalloon = false;
                    var newDrop = Instantiate(balloon, new Vector3(transform.position.x + dirThrow, transform.position.y + dirThrowY, 0), Quaternion.identity);
                    var c = newDrop.GetComponent<Balloon>();

                    c.dirX = (int)dirThrow * 2;
                    c.dirY = (int)dirThrowY * 2;
                    c.owner = this;
                }
            }
            else
            {
                delayB++;
                if (delayB >= 600)
                {
                    delayB = 0;
                    canThrowBalloon = true;
                }
            }

            xWasPressedLastFrame = xIsPressed;
        }
        else
        {
            AtualizaIAJr();

        }
    }

    void RandomizeWander()
    {
        wander_time = Random.Range(1, 10);

        var r = Random.Range(1,5);

        switch (r) {
            case 1:
                move_direction.x = 1;
                move_direction.y = 0;
                break;
            case 2:
                move_direction.x = -1;
                move_direction.y = 0;
                break;
            case 3:
                move_direction.x = 0;
                move_direction.y = 1;
                break;
            case 4:
                move_direction.x = 0;
                move_direction.y = -1;
                break;
            default:
                break;
        
        }


        RaycastHit2D hit = Physics2D.Raycast(transform.position + move_direction, move_direction, wander_time);

        if (hit.collider != null && hit.collider.tag == "Wall")
        {
            Debug.Log("Atingiu: " + hit.collider.name);
            if (move_direction.x < hit.collider.transform.position.x && move_direction.x != 0)
            {
                move_direction.x = -1;
            }
            else if(move_direction.x > hit.collider.transform.position.x && move_direction.x != 0)
            {
                move_direction.x = 1;
            }
            if (move_direction.y < hit.collider.transform.position.y && move_direction.y != 0)
            {
                move_direction.y = -1;
            }
            else if(move_direction.y > hit.collider.transform.position.y && move_direction.y != 0)
            {
                move_direction.y = 1;
            }
        }

        if (move_direction.x > 0)
        {
            dirThrow = 1f;
            //dirThrowY = 0;
        }
        else if(move_direction.x < 0)
        {
            dirThrow = - 1f;
            //dirThrowY = 0;
        }

        if (move_direction.y > 0)
        {
            //dirThrow = 0;
            dirThrowY = 1f;
        }
        else if (move_direction.y < 0)
        {
            //dirThrow = 0;
            dirThrowY = - 1f;
        }
        
       
    }

    void AtualizaIAJr()
    {
        if (curState == STATE_SEARCH)
        {



            if (wander_time > 0)
            {
                wander_time -= Time.deltaTime;

                if (move_direction.x > 0)
                {
                    dirThrow = 1f;
                    // dirThrowY = 0;
                }
                else if (move_direction.x < 0)
                {
                    dirThrow = -1f;
                    //dirThrowY = 0;
                }
                else
                {
                    dirThrow = 0f;
                }

                if (move_direction.y > 0)
                {
                    //dirThrow = 0;
                    dirThrowY = 1f;
                }
                else if (move_direction.y < 0)
                {
                    //dirThrow = 0;
                    dirThrowY = -1f;
                }
                else
                {
                    dirThrowY = 0f;
                }

                transform.position += move_direction * Time.deltaTime;

                var players = GameObject.FindGameObjectsWithTag("Player");
                foreach (var player in players)
                {
                    if (player != this.gameObject)
                    {
                        var component = player.GetComponent<SplashFunPlayer>();
                       
                        var deltaN = Vector3.Distance(transform.position, component.transform.position);

                        

                        if (deltaN < 3.0f)
                        {
                            curState = STATE_CHASE;
                            curPlayer = player;




                            return;
                        }


                        
                        

                    }
                }
                

                RaycastHit2D hit = Physics2D.Raycast(transform.position +move_direction, move_direction, 5f);

                if (hit.collider != null && hit.collider.tag == "Player")
                {
                    Debug.Log("Atingiu: " + hit.collider.name);

                    var v1 = new Vector2(hit.point.x, hit.point.y);
                    var v2 = new Vector2(transform.position.x, transform.position.y);


                    var deltaN = (v1- v2).sqrMagnitude;
                    Debug.Log(deltaN);
                   if (deltaN < 4)
                    {
                        if (balloonsLeft > 0)
                        {
                            if (canThrowBalloon)
                            {
                                

                                balloonsLeft--;
                                canThrowBalloon = false;
                                var newDrop = Instantiate(balloon, new Vector3(transform.position.x + dirThrow, transform.position.y + dirThrowY, 0), Quaternion.identity);
                                var c = newDrop.GetComponent<Balloon>();



                                c.dirX = (int)dirThrow * 2;
                                c.dirY = (int)dirThrowY * 2;
                                c.owner = this;
                                delayB = 0;

                                Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), c.GetComponent<BoxCollider2D>());
                            }
                            else
                            {
                                delayB++;
                                if (delayB >= 600)
                                {
                                    delayB = 0;
                                    canThrowBalloon = true;
                                }
                            }
                        }
                        else
                        {
                            if (podeAtirar)
                            {
                                podeAtirar = false;
                                delayShootFrames = 0;
                            }
                            else
                            {
                                delayShootFrames++;
                                if (delayShootFrames >= 600)
                                {
                                    delayShootFrames = 0;
                                    podeAtirar = true;
                                }
                            }
                        }

                    }
                    else if ((deltaN <= 25))
                    {
                        if (podeAtirar)
                        {
                            podeAtirar = false;
                            delayShootFrames = 0;
                        }
                        else
                        {
                            delayShootFrames++;
                            if (delayShootFrames >= 600)
                            {
                                delayShootFrames = 0;
                                podeAtirar = true;
                            }
                        }

                    }
                }

            }
            else
            {

                RandomizeWander();

            }
        }
        else
        {


            var d = Vector3.Distance(transform.position, curPlayer.transform.position);
            if ( d > 3 || d < 1)
            {
                curState = STATE_SEARCH;
                RandomizeWander();
                return;
                
            }
            var deltaX = curPlayer.transform.position.x - transform.position.x;
            var deltaY = curPlayer.transform.position.y - transform.position.y;

            move_direction = new Vector2(-deltaX, -deltaY).normalized;

            if (move_direction.x != 0 && move_direction.y != 0)
            {
                if (Random.Range(0f,10f) > 5f)
                {
                    move_direction.x = 0;
                }
                else
                {
                    move_direction.y = 0;
                }

            }
          

            if (move_direction.x > 0)
            {
                dirThrow = 1f;
               // dirThrowY = 0;
            }
            else if (move_direction.x < 0)
            {
                dirThrow = -1f;
                //dirThrowY = 0;
            }
            else
            {
                dirThrow = 0f;
            }

            if (move_direction.y > 0)
            {
                //dirThrow = 0;
                dirThrowY = 1f;
            }
            else if (move_direction.y < 0)
            {
                //dirThrow = 0;
                dirThrowY = -1f;
            }
            else
            {
                dirThrowY = 0f;
            }

                transform.position += move_direction * Time.deltaTime;



            var deltaN = (curPlayer.transform.position- transform.position).sqrMagnitude;





            

            if (deltaN < 4)
            {

                

                if (balloonsLeft > 0)
                {
                    if (canThrowBalloon)
                    {


                        balloonsLeft--;
                        canThrowBalloon = false;
                        var newDrop = Instantiate(balloon, new Vector3(transform.position.x + dirThrow, transform.position.y + dirThrowY, 0), Quaternion.identity);
                        var c = newDrop.GetComponent<Balloon>();



                        c.dirX = (int)dirThrow * 2;
                        c.dirY = (int)dirThrowY * 2;
                        c.owner = this;
                        delayB = 0;

                        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), c.GetComponent<BoxCollider2D>());
                    }
                    else
                    {
                        delayB++;
                        if (delayB >= 600)
                        {
                            delayB = 0;
                            canThrowBalloon = true;
                        }
                    }
                }
                else
                {
                    if (podeAtirar)
                    {
                        podeAtirar = false;
                        delayShootFrames = 0;
                    }
                    else
                    {
                        delayShootFrames++;
                        if (delayShootFrames >= 600)
                        {
                            delayShootFrames = 0;
                            podeAtirar = true;
                        }
                    }
                }

            }
            else if (deltaN <= 25)
            {
                if (podeAtirar)
                {
                    podeAtirar = false;
                    delayShootFrames = 0;
                }
                else
                {
                    delayShootFrames++;
                    if (delayShootFrames >= 600)
                    {
                        delayShootFrames = 0;
                        podeAtirar = true;
                    }
                }

            }





        }
       
    }
}
