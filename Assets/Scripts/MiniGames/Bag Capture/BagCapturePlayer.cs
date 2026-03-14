
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Tilemaps;

public class BagCapturePlayer : MonoBehaviour
{
    public int PlayerNumber;
    public bool isAI;
    public int score;
    public int Rank;
    public bool withBag = false;
    public bool stunned = false;

    public bool canSteal = false, stealing = false;

    public bool canAttack = false;

    private Transform transform;
    private GameObject bag;

    private float speed;

    public int stunnedFrames = 0;
    public int attackFrames = 0;
    public int scoreFrames;

    public NavMeshAgent agent;

    private RigidbodyType2D tipoOriginal;

    public bool stopped;
    public BoxCollider2D bc;
    public Rigidbody2D rb;

    public float tempoParado = 1f;


   
    private Vector3 direcaoAtual;

    private bool fugindo = false;

    float timer;
    public float intervalo = 0.3f;

    private PlayerInput playerInput;

    private float speedX = 0f, speedY = 0f;

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
                playerInput.actions["Up"].canceled += ctx => { if (speedY > 0) speedY = 0f; };

                playerInput.actions["Down"].started += OnMoveDown;
                playerInput.actions["Down"].canceled += ctx => { if (speedY < 0) speedY = 0f; };

                playerInput.actions["Left"].started += OnMoveLeft;
                playerInput.actions["Left"].canceled += ctx => { if (speedX < 0) speedX = 0f; };

                playerInput.actions["Right"].started += OnMoveRight;
                playerInput.actions["Right"].canceled += ctx => { if (speedX > 0) speedX = 0f; };


            }
        }
    }

    private void OnMoveUp(InputAction.CallbackContext ctx)
    {

        
                speedY = 1f;
               

    }

    private void OnMoveDown(InputAction.CallbackContext ctx)
    {
       
                speedY = -1f;
             
    }

    private void OnMoveLeft(InputAction.CallbackContext ctx)
    {
       
                speedX = -1f;
                
    }

    private void OnMoveRight(InputAction.CallbackContext ctx)
    {
       
                speedX = 1f;
             
    }


    private void OnMove(InputAction.CallbackContext ctx)
    {
        var moveInput = ctx.ReadValue<Vector2>().normalized;
       
       
                speedX =moveInput.x;
              
                speedY = moveInput.y;
             
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

        direcaoAtual = Vector2.zero;

        transform = GetComponent<Transform>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        speed = 2f;
        bag = GameObject.FindGameObjectWithTag("Bag");

        if (isAI) {
            agent.speed = 1f;
            agent.acceleration = 100f;
            agent.angularSpeed = 120f;
            agent.autoBraking = false;
            agent.stoppingDistance = 0f;

            agent.updateRotation = false; 
            agent.updateUpAxis = false;   

            agent.SetDestination(bag.transform.position);
        }
        else
        {
            agent.enabled = false;
        }
        withBag = false;
        stunned = false;
        canSteal = false;
        canAttack = false;

        rb = GetComponent<Rigidbody2D>();
        tipoOriginal = rb.bodyType;

        bc = GetComponent<BoxCollider2D>();

        var solo = GameObject.FindGameObjectsWithTag("Floor");
        foreach (var b in solo)
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), b.GetComponent<TilemapCollider2D>());

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stopped)
        {
           StartCoroutine(PararTemporariamente());
            return;
        }

       
       

        if (bag.GetComponent<Bag>().owner != gameObject)
        {
            if (isAI) {
                agent.enabled = true;
            }
            speed = 2f;
        }
        else
        {
            speed = 1f;
            if (isAI)
            {
                agent.enabled = false;
                agent.speed = speed;
            }
            
            
        }
       
        if (!isAI)
        {
            
          


                if (speedX < 0)
                {
                    transform.position -= Vector3.right * speed * Time.deltaTime;

                }

                if (speedX > 0)
                {
                    transform.position += Vector3.right * speed * Time.deltaTime;

                }
                if (speedY > 0)
                {
                    transform.position += Vector3.up * speed * Time.deltaTime;

                }
                if (speedY < 0)
                {
                    transform.position -= Vector3.up * speed * Time.deltaTime;

                }
           

           // transform.position += new Vector3(speedX, speedY,0f) * speed * Time.deltaTime;
        }
        else
        {
            agent.speed = speed;
            //agent.velocity = agent.desiredVelocity.normalized * speed;


           



            if (bag.GetComponent<Bag>().owner != this.gameObject)
                {
                    var players = GameObject.FindGameObjectsWithTag("Player");
                    bool bagWithOwner = false;
                    GameObject targetPlayer = null;
                    foreach (var p in players)
                    {
                        if (bag.GetComponent<Bag>().owner == p)
                        {
                            bagWithOwner = true;
                            targetPlayer = p;
                            break;
                        }
                    }

                    if (targetPlayer == null)
                    {
                        agent.SetDestination(bag.transform.position);
                    }
                    else
                    {
                        var tt = targetPlayer.transform;

                        var dir = (tt.transform.position - transform.position).normalized;


                        
                        //dir = Quaternion.AngleAxis(Random.Range(0, 179), Vector3.forward) * dir;
                        agent.SetDestination(tt.transform.position);
                    }
                }
                else
                {

               
               
                


                var players = GameObject.FindGameObjectsWithTag("Player");


                    Vector3 direcaoFuga = Vector3.zero;
                    int playersProximos = 0;

                    foreach (GameObject p in players)
                    {
                        Vector3 diferenca = transform.position - p.transform.position;
                        float distancia = diferenca.magnitude;

                        if (distancia < 3f)
                        {
                            // Quanto mais perto, maior o peso
                            float peso = 1f / Mathf.Max(distancia, speed);

                        direcaoFuga += diferenca.normalized;// * peso;
                            playersProximos++;
                        }
                    }

                    if (playersProximos > 0)
                    {
                        if (!fugindo)
                        {
                            direcaoAtual = transform.position;
                        direcaoFuga = direcaoAtual;
                            fugindo = true;
                        }

                        direcaoFuga.Normalize();

                        //transform.position += direcaoFuga * Time.deltaTime;

                        //  Suavização remove zigzag
                        //direcaoAtual = Vector2.Lerp(direcaoAtual, direcaoFuga, Time.deltaTime);

                        if (Vector3.Distance(transform.position, direcaoAtual) < 3f) {
                            //Vector3 destino = transform.position + direcaoFuga;

                            transform.position += direcaoFuga * speed * Time.deltaTime;
                        /*
                            if (NavMesh.SamplePosition(destino, out NavMeshHit hit, speed, NavMesh.AllAreas))
                            {
                                // Só atualiza se mudou bastante (evita micro ajustes)
                               if (Vector3.Distance(agent.destination, hit.position) > 0.5f)
                                {
                                    agent.SetDestination(hit.position);
                                }
                               
                            }*/
                        }
                        else
                        {
                           // agent.ResetPath();
                        }
                           // agent.velocity = agent.desiredVelocity.normalized * speed;
                    }
                    else
                    {
                        fugindo = false;
                       // agent.ResetPath();
                    }

                
            }
        }
    }

    public GameObject EncontrarMaisProximoObjeto()
    {
        GameObject[] objetos = GameObject.FindGameObjectsWithTag("Player");
      GameObject maisProximo = null;
        float menorDistancia = Mathf.Infinity;

        foreach (var obj in objetos)
        {
            float distancia = (obj.transform.position - transform.position).sqrMagnitude; // mais rápido que magnitude
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                maisProximo = obj;
            }
        }

        return maisProximo;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bag")
        {
            if (bag.GetComponent<Bag>().owner != this)
            {
               
                withBag = true;
                bag.GetComponent<Bag>().owner = this.gameObject;
                
                   // canSteal = false;
                   //stealing = true;

            }
        }
        if (collision.gameObject.tag == "Player")
        {
            var cp = collision.gameObject.GetComponent<BagCapturePlayer>();
            
            if (bag.GetComponent<Bag>().owner == collision.gameObject && stopped == false) {
                // canSteal = true;

                
                
                
                bag.GetComponent<Bag>().owner = this.gameObject;

                bag.transform.position = transform.position;

                cp.stopped = true;
                //cp.StartCoroutine(PararTemporariamente());

            }
            
            

        }
    }
    

    public IEnumerator PararTemporariamente()
    {
        stopped = true;
        rb.bodyType = RigidbodyType2D.Static; // congela totalmente o corpo
       // bc.enabled = false; // desativa colisões

       

       
        yield return new WaitForSeconds(tempoParado);

       
        rb.bodyType = tipoOriginal;
        //bc.enabled = true;

        

        stopped = false;

        if (isAI)
        {
           

            foreach (var p in GameObject.FindGameObjectsWithTag("Player"))
            {
                var pc = p.GetComponent<BagCapturePlayer>();
                if (p != this)
                {
                    if (bag.GetComponent<Bag>().owner == p)
                    {
                       // agent.SetDestination(pc.transform.position);
                    }
                }
            }
        }
    }
}
