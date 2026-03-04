using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class CoconutThrow : MonoBehaviour
{
    public int dirX, dirY;
    private Transform transform;
    public Rigidbody2D rb;

    public CoconutClashPlayer owner;
    public NavMeshAgent agent;

    float frames = 0;

    public bool colidiu = false;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        
        rb = GetComponent<Rigidbody2D>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        var borders = GameObject.FindGameObjectsWithTag("Floor");
        foreach (var b in borders)
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(),b.GetComponent<TilemapCollider2D>());

        }
        
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (colidiu)
        {
            rb.linearVelocity = new Vector3(0f,0f,0f);
            frames += Time.deltaTime;
            if (frames >= 60)
            {
                Destroy(gameObject);
            }
            return;
        }
        //transform.position += new Vector3(dirX,dirY,0f) * 3.0f * Time.deltaTime;

       

        rb.linearVelocity = new Vector3(dirX, dirY, 0f) * 100 * Time.deltaTime;


        //rb.MovePosition(rb.position + new Vector2(dirX, dirY) * 3.0f * Time.deltaTime);

        if (transform.position.x < -5 || transform.position.x > 5 || transform.position.y < -5 || transform.position.y > 5)
        {
            // Debug.Log("colidiu");
            colidiu = true;
            Destroy(gameObject,5);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player") && !colidiu)
        {
            colidiu = true;
            //Destroy(gameObject);
        }
        if ( collision.gameObject.CompareTag("Teacher") && !colidiu)
        {
            colidiu = true;
            owner.score++;
            //Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Coconut"))
        {
            if (owner.PlayerNumber != collision.gameObject.GetComponent<CoconutThrow>().owner.PlayerNumber && !colidiu)
            {
                colidiu = true;
                //Destroy(gameObject);
            }
        }
    }
}
