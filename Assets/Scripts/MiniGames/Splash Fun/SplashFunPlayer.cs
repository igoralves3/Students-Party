

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;

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

    public int STATE_SEARCH = 1, STATE_CHASE = 2, STATE_DODGE = 3;
    public int curState;
    public GameObject curPlayer;

    public Vector3 move_direction;
    private float wander_time;

    private bool zWasPressedLastFrame = false;
    private bool xWasPressedLastFrame = false;
    private PlayerInput playerInput;

    private bool atirou=false;

    private int framesNear = 0;

    public BoxCollider2D bc;

    public GameObject waterDrop;

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

                if (podeAtirar)
                {

                    podeAtirar = false;
                    delayShootFrames = 0;

                    atirou = true;
                    FireWater();
                }
                
            }
            if (!podeAtirar)
            {
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
                    delayB = 0;
                    balloonsLeft--;
                    canThrowBalloon = false;
                    var newDrop = Instantiate(balloon, new Vector3(transform.position.x + dirThrow, transform.position.y + dirThrowY, 0), Quaternion.identity);
                    var c = newDrop.GetComponent<Balloon>();

                    c.dirX = (int)dirThrow * 2;
                    c.dirY = (int)dirThrowY * 2;
                    c.owner = this;
                }
            }
            if(!canThrowBalloon)
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
        wander_time = Random.Range(5, 10);

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

        
        RaycastHit2D hit = Physics2D.Raycast(transform.position+move_direction, move_direction * wander_time, wander_time);

        if (hit.collider != null && hit.collider.tag == "Wall" && hit.collider.gameObject != gameObject)
        {
            //Debug.Log("wall "+PlayerNumber.ToString());
            if (transform.position.x < hit.point.x && move_direction.x != 0)
            {
                move_direction.x = -1;
            }
            else if(transform.position.x > hit.point.x && move_direction.x != 0)
            {
                move_direction.x = 1;
            }
            if (transform.position.y < hit.point.y && move_direction.y != 0)
            {
                move_direction.y = -1;
            }
            else if(transform.position.y > hit.point.y && move_direction.y != 0)
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


    }

    void RandomizeDodge(Vector3 aim)
    {
        //Debug.Log("Dodging");
        wander_time = Random.Range(5, 10);

        var r = Random.Range(1,3);
        if (aim.x != 0)
        {
            
            if (r == 1)
            {
                move_direction.x = 0;
                move_direction.y = 1;
            }
            else
            {
                move_direction.x = 0;
                move_direction.y = -1;
            }

        }else if (aim.y != 0)
        {
            if (r == 1)
            {
                move_direction.y = 0;
                move_direction.x = 1;
            }
            else
            {
                move_direction.y = 0;
                move_direction.x = -1;
            }
        }

        var dir = new Vector3(dirThrow, dirThrowY, 0f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position+move_direction, move_direction * wander_time, wander_time);

        if (hit.collider != null && hit.collider.tag == "Wall" && hit.collider.gameObject != gameObject)
        {
            //Debug.Log("wall " + PlayerNumber.ToString());
            if (transform.position.x < hit.point.x && move_direction.x != 0)
            {
                move_direction.x = -1;
            }
            else if (transform.position.x > hit.point.x && move_direction.x != 0)
            {
                move_direction.x = 1;
            }
            if (transform.position.y < hit.point.y && move_direction.y != 0)
            {
                move_direction.y = -1;
            }
            else if (transform.position.y > hit.point.y && move_direction.y != 0)
            {
                move_direction.y = 1;
            }
        }

        if (move_direction.x > 0)
        {
            dirThrow = 1f;
            //dirThrowY = 0;
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

                }
                else if (move_direction.x < 0)
                {
                    dirThrow = -1f;

                }
                else
                {
                    dirThrow = 0f;
                }

                if (move_direction.y > 0)
                {

                    dirThrowY = 1f;
                }
                else if (move_direction.y < 0)
                {

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



                        if (deltaN <= 3f)
                        {
                            curState = STATE_CHASE;
                            curPlayer = player;




                            return;

                        }
                    }
                }
              
                
            }
            else
            {

                RandomizeWander();

            }
        }
        else if(curState==STATE_CHASE)
        {
           

            var d = Vector3.Distance(transform.position, curPlayer.transform.position);
            if (d > 3)
            {
                curState = STATE_SEARCH;
                RandomizeWander();
                return;

            }
            if (d <=3)
            {

                move_direction.x = 0;
                move_direction.y = 0;

                var delta = curPlayer.transform.position - transform.position;
                var aim = delta.normalized;

                if (aim.x > 0)
                {
                    aim.x = 1;
                }
                else if (aim.x < 0)
                {
                    aim.x = -1;
                }
                if (aim.y > 0)
                {
                    aim.y = 1;
                }
                else if (aim.y < 0)
                {
                    aim.y = -1;
                }
                
                if ((aim.x != 0) && (aim.y != 0))
                {
                   
                        if (Random.Range(0f, 10f) > 5f)
                        {
                            aim.x = 0;
                        }
                        else
                        {
                            aim.y = 0;
                        }
                    
                }

               
                    

                    //curState = STATE_DODGE;
                    //RandomizeDodge(move_direction);

                    //move_direction = aim;

                    dirThrow = aim.x;
                    dirThrowY = aim.y;


                    CheckOpponentShoot(aim);


                //TargetRaycast(aim)
                //
                transform.position += move_direction * Time.deltaTime;

                //curState = STATE_DODGE;
               // RandomizeDodge(aim);
            }
           
        }
        else
        {
            transform.position += move_direction * Time.deltaTime;
            wander_time -= Time.deltaTime;

            if (wander_time<=0)
            {
                foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (player != this.gameObject)
                    {
                        var component = player.GetComponent<SplashFunPlayer>();

                        var deltaN = Vector3.Distance(transform.position, component.transform.position);



                        if (deltaN <= 5f)
                        {
                            curState = STATE_CHASE;
                            curPlayer = player;




                            return;
                        }

                    }
                }

                curState = STATE_SEARCH;
                //RandomizeWander();
            }
        }
    }

    public void CheckOpponentShoot(Vector3 aim)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + aim, aim, 10f);

        if (hit.collider != null && (hit.collider.tag == "Balloon" || hit.collider.tag == "Water") && hit.collider.gameObject != gameObject)
        {
           


            move_direction = aim;

           

            dirThrow = move_direction.x;
            dirThrowY = move_direction.y;

           // wander_time = Random.Range(5,10);
            curState = STATE_DODGE;

           RandomizeDodge(aim);
        }
        else if(hit.collider != null && (hit.collider.tag == "Player") && hit.collider.gameObject != gameObject)
        {

            //Debug.Log("player dir = " + gameObject.name + " " + hit.collider.gameObject.name);
             move_direction = aim;
           
                dirThrow = aim.x;
                dirThrowY = aim.y;

         
           
                TargetRaycast(aim);
            
           

        }
    }

    public void TargetRaycast(Vector3 aim)
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position + aim, aim, 5f);

        if (hit.collider != null && hit.collider.tag == "Player" && hit.collider.gameObject != gameObject)
        {

            move_direction = new Vector3(0f,0f,0f);

            //move_direction = aim;

            dirThrow = aim.x;
            dirThrowY =aim.y;
           
            var v1 = new Vector2(hit.point.x, hit.point.y);
            var v2 = new Vector2(transform.position.x, transform.position.y);


            var deltaN = (v1 - v2).sqrMagnitude;
            var r = Random.Range(1, 11);
            if (r > 4)
            {



                if (podeAtirar)
                {
                    Debug.Log("water dir = " + aim.ToString() + " " + hit.collider.name);
                    podeAtirar = false;
                    delayShootFrames = 0;

                    atirou = true;
                    FireWater();
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
            else //if (deltaN > 2 && deltaN <= 5f)
            {

                if (balloonsLeft > 0)
                {
                    if (canThrowBalloon)
                    {
                        Debug.Log("balloon dir = " + aim.ToString() + " " + hit.collider.name);

                        balloonsLeft--;
                        canThrowBalloon = false;
                        var newDrop = Instantiate(balloon, new Vector3(transform.position.x + dirThrow, transform.position.y + dirThrowY, 0), Quaternion.identity);
                        var c = newDrop.GetComponent<Balloon>();



                        c.dirX = (int)dirThrow * 2;
                        c.dirY = (int)dirThrowY * 2;
                        c.owner = this;
                        delayB = 0;

                        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), c.GetComponent<BoxCollider2D>());

                        atirou = true;
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
                        Debug.Log("water dir = " + aim.ToString() + " " + hit.collider.name);
                        podeAtirar = false;
                        delayShootFrames = 0;

                        FireWater();
                    }
                    else
                    {
                        delayShootFrames++;
                        if (delayShootFrames >= 600)
                        {
                            delayShootFrames = 0;
                            podeAtirar = true;

                            atirou = true;
                        }
                    }
                }

                

            }

         
        }
    }

    void FireWater()
    {
        var newDrop = Instantiate(waterDrop, new Vector3(transform.position.x +dirThrow*0.5f, transform.position.y + dirThrowY*0.5f, 0), Quaternion.identity);
        var c = newDrop.GetComponent<WaterDrop>();

        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), c.gameObject.GetComponent<BoxCollider2D>());

       

        c.dirX = dirThrow * 1f;
        c.dirY = dirThrowY * 1f;
        c.owner = this;
    }

}
