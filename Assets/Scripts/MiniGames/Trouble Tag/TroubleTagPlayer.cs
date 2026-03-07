

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class TroubleTagPlayer : MonoBehaviour
{
    public int PlayerNumber;
    public bool isAI;
    

    public int Rank;

    public bool canSlide = true;

    private Transform transform;
    private Rigidbody2D rb;

    private TroubleTagTeacher teacher;
    public NavMeshAgent agent;

    

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


        transform = GetComponent<Transform>();

        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);


        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        rb = GetComponent<Rigidbody2D>();

        teacher = GameObject.FindWithTag("Teacher").GetComponent<TroubleTagTeacher>();

        var solo = GameObject.FindGameObjectsWithTag("Floor");
        foreach (var b in solo)
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), b.GetComponent<TilemapCollider2D>());

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAI)
        {
            if (playerInput.actions["Left"].IsPressed())
            {
                transform.position -= Vector3.right * Time.deltaTime;
              
            }
            if (playerInput.actions["Right"].IsPressed())
            {
                transform.position += Vector3.right * Time.deltaTime;
                
            }
            if (playerInput.actions["Up"].IsPressed())
            {
                transform.position += Vector3.up * Time.deltaTime;
               
            }
            if (playerInput.actions["Down"].IsPressed())
            {
                transform.position -= Vector3.up * Time.deltaTime;
               
            }

        }
        else
        {
            if (Vector3.Distance(transform.position, teacher.transform.position) <= 10f) {
                

                var dir = (teacher.transform.position - transform.position).normalized;

                float angle = Random.Range(-90f, 90f);

                dir = Quaternion.AngleAxis(angle, Vector3.forward) * dir;
               

                agent.SetDestination(transform.position - dir * 10f);
            }
            
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Teacher")
        {
           

            switch (PlayerNumber)
            {
                case 1:
                    GameManager.p1.Rank = TroubleTagTeacher.playersLeft;
                    break;
                case 2:
                    GameManager.p2.Rank = TroubleTagTeacher.playersLeft;
                    break;
                case 3:
                    GameManager.p3.Rank = TroubleTagTeacher.playersLeft;
                    break;
                case 4:
                    GameManager.p4.Rank = TroubleTagTeacher.playersLeft;
                    break;
                default:break;
            }
            TroubleTagTeacher.playersLeft--;

            if (TroubleTagTeacher.playersLeft == 1)
            {
                var players = GameObject.FindGameObjectsWithTag("Player");
                foreach (var p in players)
                {
                    if (p.GetComponent<TroubleTagPlayer>().PlayerNumber != PlayerNumber)
                    {
                        switch (p.GetComponent<TroubleTagPlayer>().PlayerNumber)
                        {
                            case 1:
                                GameManager.p1.Rank = 1;
                                break;
                            case 2:
                                GameManager.p2.Rank = 1;
                                break;
                            case 3:
                                GameManager.p3.Rank = 1;
                                break;
                            case 4:
                                GameManager.p4.Rank = 1;
                                break;
                            default: break;
                        }
                    }
                    else
                    {
                        switch (p.GetComponent<TroubleTagPlayer>().PlayerNumber)
                        {
                            case 1:
                                GameManager.p1.Rank = 2;
                                break;
                            case 2:
                                GameManager.p2.Rank = 2;
                                break;
                            case 3:
                                GameManager.p3.Rank = 2;
                                break;
                            case 4:
                                GameManager.p4.Rank = 2;
                                break;
                            default: break;
                        }
                    }
                }

                
                
            }

            Destroy(gameObject);
        }
    }
}
