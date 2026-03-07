using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Balloon : MonoBehaviour
{
    public float dirX, dirY;

    public int frames = 0;
    public GameObject particle;
    public SplashFunPlayer owner;

    // Start is called before the first frame update
    void Start()
    {
        frames = 0;

        var solo = GameObject.FindGameObjectsWithTag("Floor");
        foreach (var b in solo)
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), b.GetComponent<TilemapCollider2D>());

        }
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), owner.GetComponent<BoxCollider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        frames++;
        if (frames >= 600)
        {
            SpawnParticles();
            Destroy(gameObject);
        }
        transform.position += new Vector3(dirX, dirY, 0f) * Time.deltaTime;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Balloon")
        {
          



            SpawnParticles();

            Destroy(gameObject);
        }
        if (col.gameObject.tag == "Player")
        {
            if (owner != col.gameObject.GetComponent<SplashFunPlayer>()) {
                SpawnParticles();
                Destroy(gameObject);
            }
        }
    }

    void SpawnParticles()
    {
        for (int i = 0; i < 16; i++)
        {
            var newAngleX = Math.Cos(i);
            var newAngleY = Math.Sin(i);

            var newDrop = Instantiate(particle, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            var c = newDrop.GetComponent<WaterDrop>();
            c.standard = false;
            c.dirX =(float) newAngleX;
            c.dirY = (float)newAngleY;
            c.owner = owner;

            
        }
    }
}
