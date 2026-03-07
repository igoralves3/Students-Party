using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterDrop : MonoBehaviour
{
    public bool standard=true;

    public float dirX, dirY;

    public int frames = 0;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (standard)
        {
            transform.position += new Vector3(dirX * 10f, dirY * 10f, 0f) * Time.deltaTime;
        }
        else
        {
            frames++;
            if (frames >= 60)
            {
                frames = 0;
                Destroy(gameObject);
            }
            transform.position += new Vector3(dirX * 10f, dirY * 10f, 0f) * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if ( col.gameObject.tag == "Wall" || col.gameObject.tag == "Balloon")
        {
           
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "Player")
        {
            if (owner != col.gameObject.GetComponent<SplashFunPlayer>()) {
                owner.score++;
                Destroy(gameObject);
            }
        }
        if (col.gameObject.tag == "Water")
        {
            if (owner != col.gameObject.GetComponent<WaterDrop>().owner) {
                Destroy(gameObject);
            }
        }
    }
}
