
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class TroubleTagTeacher : MonoBehaviour
{

    private Transform transform;

    public GameObject currentTarget;

    public static int playersLeft;

    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

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
    }
}
