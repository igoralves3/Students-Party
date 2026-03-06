using Microsoft.VisualBasic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baseball : MonoBehaviour
{
    public PitchBattlePlayer owner;
    public float dirX, dirY;

    public Rigidbody2D rb;
    public CircleCollider2D bc;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            if (collision.gameObject.GetComponent<Baseball>().owner != owner) {
                //bc.enabled = false;
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            owner.score++;
            Destroy(gameObject);
        }
    }
}
