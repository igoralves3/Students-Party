
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
public class Bag : MonoBehaviour
{
    public GameObject owner;
    public float scoreFrames;

    Transform transform;
    public UnityEngine.AI.NavMeshAgent agent;

    public BoxCollider2D bc;
    public Rigidbody2D rb;

    public int frames = 0;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        owner = null;
        scoreFrames = 0;

        transform = GetComponent<Transform>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        transform.position = new Vector3(0f,0f,0f);
    }

    // Update is called once per frame
    void Update()
    {

        if (owner != null)
        {
           
            transform.position = owner.transform.position;

            frames++;
            if (frames >= 300) {
                frames = 0;
                owner.GetComponent<BagCapturePlayer>().score++;
            }
        }
       
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            if (owner != null)
            {


                owner = collision.gameObject;
                   

                    transform.position = owner.transform.position;
                    
                    //owner.withBag = true;

                    
                    

                    scoreFrames = 0;

                    bc.enabled =false;
                    rb.bodyType = RigidbodyType2D.Static;
                //}
            }
           
        }
    }

    
}
