
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.AI;
public class TeacherDodgingCoconut : MonoBehaviour
{


    private Transform transform;
    private bool indoParaCima = false;
    private bool indoParaEsquerda = false;

    private int pluffFrames = 0;
    private Vector3 target;

   

    public float distanciaSeguir = 10f; // Distância em que o jogador começa a fugir
    public float speed = 0.5f;

    public NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        indoParaCima = false;
        indoParaEsquerda = false;
        pluffFrames = 0;

        transform.position = new Vector3(0f,0f,0f);

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;


    }

    // Update is called once per frame
    void Update()
    {

       

        
        
        var coconuts = GameObject.FindGameObjectsWithTag("Coconut");

        if (coconuts.Length == 0)
        {
            pluffFrames++;
            if (pluffFrames >= 60)
            {
                pluffFrames = 0;
                agent.SetDestination(new Vector3(Random.Range(-1f,1f),Random.Range(-1f, 1f), 0f) - transform.position);
            }
        }


        foreach (var c in coconuts)
        {
            // Calcular a distância entre o jogador e o objeto perigoso
            float distancia = Vector2.Distance(transform.position, c.transform.position);

            // Se o objeto estiver dentro da distância de fuga, calculate a direção de fuga
            if (distancia < 10)
            {
                agent.SetDestination(c.transform.position - transform.position);
            }
        }
       


       
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Coconut"))
        {
            Destroy(collision.gameObject);
        }
    }
   
}
