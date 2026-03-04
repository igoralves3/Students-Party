
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

using UnityEngine.InputSystem;

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

        direcaoAtual = Vector2.zero;

        transform = GetComponent<Transform>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        speed = 1f;
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

            speed = 1f;
        }
        else
        {
            speed = 0.5f;
            if (isAI)
            {
                agent.speed = speed;
            }
            
            
        }
       
        if (!isAI)
        {
            if (playerInput.actions["Left"].IsPressed())
            {
                transform.position -= Vector3.right * speed * Time.deltaTime;

            }

            if (playerInput.actions["Right"].IsPressed())
            {
                transform.position += Vector3.right * speed * Time.deltaTime;

            }
            if (playerInput.actions["Up"].IsPressed())
            {
                transform.position += Vector3.up * speed * Time.deltaTime;

            }
            if (playerInput.actions["Down"].IsPressed())
            {
                transform.position -= Vector3.up * speed * Time.deltaTime;

            }
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

               
                timer += Time.deltaTime;

                if (timer >= intervalo)
                {
                    if (agent.velocity.magnitude > speed)
                    {
                        agent.velocity = (agent.velocity.normalized) / 250.0f;
                    }
                    timer = 0f;
                


                var players = GameObject.FindGameObjectsWithTag("Player");


                    Vector3 direcaoFuga = Vector3.zero;
                    int playersProximos = 0;

                    foreach (GameObject p in players)
                    {
                        Vector3 diferenca = transform.position - p.transform.position;
                        float distancia = diferenca.magnitude;

                        if (distancia < 6f)
                        {
                            // Quanto mais perto, maior o peso
                            float peso = 1f / Mathf.Max(distancia, 0.1f);

                            direcaoFuga += diferenca.normalized;// * peso;
                            playersProximos++;
                        }
                    }

                    if (playersProximos > 0)
                    {
                        if (!fugindo)
                        {
                            direcaoAtual = transform.position;
                            fugindo = true;
                        }

                        direcaoFuga.Normalize();

                        //  Suavização remove zigzag
                        //direcaoAtual = Vector2.Lerp(direcaoAtual, direcaoFuga, Time.deltaTime);

                        if (Vector3.Distance(transform.position, direcaoAtual) < 6f) {
                            Vector3 destino = transform.position + direcaoFuga * speed;

                            if (NavMesh.SamplePosition(destino, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                            {
                                // Só atualiza se mudou bastante (evita micro ajustes)
                               if (Vector3.Distance(agent.destination, hit.position) > 0.5f)
                                {
                                    agent.SetDestination(hit.position);
                                }
                               
                            }
                        }
                        else
                        {
                            agent.ResetPath();
                        }
                           // agent.velocity = agent.desiredVelocity.normalized * speed;
                    }
                    else
                    {
                        fugindo = false;
                        agent.ResetPath();
                    }

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
        //bc.enabled = false; // desativa colisões

       

       
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
                        agent.SetDestination(pc.transform.position);
                    }
                }
            }
        }
    }
}
