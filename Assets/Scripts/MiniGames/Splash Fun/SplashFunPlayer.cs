

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

using UnityEngine.InputSystem;

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
    public SplashFunPlayer curPlayer;

    private Vector3 move_direction;
    private float wander_time;

    private bool zWasPressedLastFrame = false;
    private bool xWasPressedLastFrame = false;
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
            RandomizeWander();



            //agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
            //curState = STATE_SEARCH;
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
           
            if (playerInput.actions["Up"].IsPressed())
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
        move_direction.x = Random.Range(-1f, 1f);
        move_direction.y = Random.Range(-1f, 1f);
        if (move_direction.x > 0)
        {
            dirThrow = 1f;
            dirThrowY = 0;
        }
        else if(move_direction.x < 0)
        {
            dirThrow = - 1f;
            dirThrowY = 0;
        }

        if (move_direction.y > 0)
        {
            dirThrow = 0;
            dirThrowY = 1f;
        }
        else if (move_direction.y < 0)
        {
            dirThrow = 0;
            dirThrowY = - 1f;
        }
        
        wander_time = Random.Range(1, 5);
    }

    void AtualizaIAJr()
    {
        
        if (wander_time > 0)
        {
            wander_time -= Time.deltaTime;
            transform.position += move_direction * Time.deltaTime;
        }
        else
        {
            RandomizeWander();
        }

        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players) {
            if (player.GetComponent<SplashFunPlayer>() != this)
            {
                var component = player.GetComponent<SplashFunPlayer>();
                var deltaX = transform.position.x - component.transform.position.x;
                var deltaY = transform.position.y - component.transform.position.y;

                var deltaN = Vector3.Distance(transform.position, component.transform.position);

                var podeVerOponente = false;

                if (deltaX < 0 && dirThrow > 0)
                {
                    podeVerOponente = true;
                }

                else if (deltaX > 0 && dirThrow < 0)
                {
                    podeVerOponente = true;
                }
                else if (deltaY < 0 && dirThrowY > 0)
                {
                    podeVerOponente = true;
                }
                else if (deltaY > 0 && dirThrowY < 0)
                {
                    podeVerOponente = true;
                }

                if (deltaN <= 20 && deltaN > 10 && podeVerOponente)
                {
                    if (podeAtirar)
                    {
                        podeAtirar = false;
                        delayShootFrames = 0;
                    }
                    else
                    {
                        delayShootFrames++;
                        if (delayShootFrames >= 6000)
                        {
                            delayShootFrames = 0;
                            podeAtirar = true;
                        }
                    }
                    
                }

                if (deltaN <= 10 && podeVerOponente)
                {
                    if (balloonsLeft > 0 && Random.Range(0f, 100f) >= 60f)
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
                if (podeVerOponente)
                {
                    break;
                }
            }
        
        }

    }

    void AtualizaIA()
    {
        if (curState == STATE_SEARCH)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            var nearestPlayer = this;
            var deltaN = 50f;
            foreach (var p in players)
            {
                if (p.GetComponent<SplashFunPlayer>() != this)
                {
                    float delta = Vector3.Distance(transform.position, p.transform.position);
                    if (delta <= deltaN)
                    {
                        nearestPlayer = p.GetComponent<SplashFunPlayer>();
                        deltaN = delta;
                    }
                }
            }

            if (nearestPlayer != this)
            {
                if (deltaN <= 20f)
                {



                    curState = STATE_CHASE;

                    curPlayer = nearestPlayer;
                    agent.SetDestination(nearestPlayer.transform.position);



                }
            }
            else
            {
                var deltaX = transform.position.x - nearestPlayer.transform.position.x;
                var deltaY = transform.position.y - nearestPlayer.transform.position.y;

                if (deltaX > 0f)
                {
                    dirThrow = -1f;
                    //dirThrowY = 0f;
                }
                else if (deltaX < 0f)
                {
                    dirThrow = 1f;
                    // dirThrowY = 0f;
                }
                else
                {
                    dirThrow = 0f;
                }
                if (deltaY > 0f)
                {
                    dirThrowY = -1f;
                    //dirThrow = 0f;
                }
                else if (deltaY < 0f)
                {
                    dirThrowY = 1f;
                    //dirThrow = 0f;
                }
                else
                {
                    dirThrowY = 0f;
                }






            }
            }
        else if (curState == STATE_CHASE)
        {
            var deltaX = transform.position.x - agent.destination.x;
            var deltaY = transform.position.y - agent.destination.y;

            if ((deltaX > 20f && deltaY > 20f) || curPlayer == null)
            {
                curState = STATE_SEARCH;
                agent.SetDestination(transform.position);   
            }
            if (deltaX < 10f && deltaY < 10f)
            {
                agent.SetDestination(transform.position);
            }
            
            if (deltaX > 0f)
            {
                dirThrow = -1f;
                //dirThrowY = 0f;
            }
            else if (deltaX < 0f)
            {
                dirThrow = 1f;
                // dirThrowY = 0f;
            }
            else
            {
             dirThrow = 0f;
            }
            if (deltaY > 0f)
            {
                dirThrowY = -1f;
                //dirThrow = 0f;
            }
            else if (deltaY < 0f)
            {
                dirThrowY = 1f;
                //dirThrow = 0f;
            }
            else
            {
                dirThrowY = 0f;
            }

            agent.SetDestination(curPlayer.transform.position);

            float deltaN = Vector3.Distance(transform.position, agent.destination);

            if (deltaN <= 20f && deltaN > 10f)
            {


                if (podeAtirar)
                {
                    podeAtirar = false;
                    delayShootFrames = 0;
                }
                else
                {
                    delayShootFrames++;
                    if (delayShootFrames >= 6000)
                    {
                        delayShootFrames = 0;
                        podeAtirar = true;
                    }
                }


            }else if (deltaN <= 10f)
            {


                if (balloonsLeft > 0 && Random.Range(0f, 100f) >= 60f)
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
                    }else
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


        }
            
            
           
           

       
    }
}
