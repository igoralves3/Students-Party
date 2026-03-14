using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections.Specialized;

public class TreasureHuntPlayer : MonoBehaviour
{

    public int PlayerNumber;
    public bool isAI;
    public int score;
    public int Rank;

    private float speed;

    public bool opening;

    public BoxCollider2D bc;

    public NavMeshAgent agent;
    public Backpack mochilaAtual;

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


        speedX = moveInput.x;

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

        score = 0;
        Rank = 0;

        speed = 1f;
        opening = false;

        bc = GetComponent<BoxCollider2D>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = 1f;

        var floorTiles = GameObject.FindGameObjectsWithTag("Floor");
        foreach (var t in floorTiles)
        {
            Physics2D.IgnoreCollision(bc, t.GetComponent<TilemapCollider2D>());
        }
        if (isAI) {
            EscolheMochila();
        }
    }

    // Update is called once per frame
    void Update()
    {
      opening = false;
        if (!isAI)
        {
            
            Vector3 move = new Vector3(speedX, speedY, 0).normalized;
            move = Vector3.ClampMagnitude(move, 1f);
            Debug.Log("Speed " + move + " " + PlayerNumber);

            transform.position += move * speed * Time.deltaTime;


            if (playerInput.actions["Space"].IsPressed())
            {

                opening = true;
            }
          
            
            
        }
        else
        {
            
            if (mochilaAtual.isOpen || agent.destination == null)
            {
                EscolheMochila();
               
            }
          
        }
    }

    void EscolheMochila()
    {
        var mochilas = GameObject.FindGameObjectsWithTag("Backpack");

        var mochilasFechadas = new List<Backpack>();

        foreach (var m in mochilas)
        {
            if (m.GetComponent<Backpack>().isOpen == false)
            {
                mochilasFechadas.Add(m.GetComponent<Backpack>());
            }
        }
        if (mochilasFechadas.Count > 0) {
            var indexMochila = Random.Range(0, mochilasFechadas.Count);
            mochilaAtual = mochilasFechadas[indexMochila];

            agent.SetDestination(mochilaAtual.transform.position);

            
        }
    }

   

    void OnCollisionEnter2D(Collision2D collision)
    {
   
        if (collision.gameObject.tag == "Backpack")
        {
            if (isAI) {
                
                    opening = true;
                var m = collision.gameObject.GetComponent<Backpack>();

                m.isOpen = true;
                if (m.isOpen)
                {
                    m.AplicaPontoMochila(this);
                    EscolheMochila();
                }
            }
        }
    }
}
