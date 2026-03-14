
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class TroubleTagTeacher : MonoBehaviour
{

    private Transform transform;

    public GameObject currentTarget;

    public static int playersLeft;

    public NavMeshAgent agent;

    private bool nearTarget = false;

    private int maxSpeedFrames = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = 2;

        playersLeft = 4;

        var players = GameObject.FindGameObjectsWithTag("Player");
        int index = (int)Random.Range(0, playersLeft);

        

        currentTarget = players[index];

        var solo = GameObject.FindGameObjectsWithTag("Floor");
        foreach (var b in solo)
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), b.GetComponent<TilemapCollider2D>());

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playersLeft <= 1)
        {
            SceneManager.LoadScene("MiniGameRanking");
        }

        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.transform.position);
            
        }
        else
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            int index = (int)Random.Range(0, playersLeft);
            while (currentTarget == null)
            {
                index = (int)Random.Range(0, playersLeft);
                currentTarget = players[index];
            }

            
        }
        foreach (var p in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (p != currentTarget)
            {
                var d = Vector3.Distance(transform.position, p.transform.position);
                if (d <= 2.5f)
                {
                    currentTarget = p;
                    break;
                }
            }
        }

       
        /*
        var delta = Vector3.Distance(transform.position,currentTarget.transform.position);
        if (delta <= 2.5f)
        {
            nearTarget = true;
            
        }

        if (nearTarget)
        {
            if (Random.Range(1, 11) > 8)
            {
               // agent.speed = 2;
                //maxSpeedFrames = 0;

            }
            else
            {
                agent.speed = 1;
            }
        }
        else
        {
            agent.speed = 1;
        }
        if (agent.speed == 2)
        {
            maxSpeedFrames++;
            if (maxSpeedFrames >= 60)
            {
                maxSpeedFrames = 0;
                agent.speed = 1;
            }
        }*/

        Debug.Log("teacher spd = "+agent.speed);
    }
}
