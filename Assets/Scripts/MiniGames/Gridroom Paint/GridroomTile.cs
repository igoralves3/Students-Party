
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GridroomTile : MonoBehaviour
{
    GridroomPaintPlayer p1, p2, p3, p4;
    public SpriteRenderer sprR;

    public int i, j;

    // Start is called before the first frame update
    void Start()
    {
        p1 = GameObject.Find("Player1").GetComponent<GridroomPaintPlayer>();
        p2 = GameObject.Find("Player2").GetComponent<GridroomPaintPlayer>();
        p3 = GameObject.Find("Player3").GetComponent<GridroomPaintPlayer>();
        p4 = GameObject.Find("Player4").GetComponent<GridroomPaintPlayer>();
        sprR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Pinta(p1);
        Pinta(p2);
        Pinta(p3);
        Pinta(p4);

    }


    void Pinta(GridroomPaintPlayer p)
    {
        var deltaX = Mathf.Abs(transform.position.x - p.transform.position.x);
        var deltaY = Mathf.Abs(transform.position.y - p.transform.position.y);
        if (deltaX <= 0.1f && deltaY <= 0.1f)
        {
            
            if (p.color != sprR.color)
            {
                if (sprR.color == Color.cyan)
                {

                    if (p1.score > 0)
                    {
                        p1.score--;
                    }
                }
                else if (sprR.color == Color.red) { 
                    if (p2.score > 0)
                    {
                        p2.score--;
                    }
                }
                else if (sprR.color == Color.green) { 
                    if (p3.score > 0)
                    {
                        p3.score--;
                    }
                }
                else if (sprR.color == Color.yellow) {
                        if (p4.score > 0)
                    {
                        p4.score--;
                    }
                
               
                }
                sprR.color = p.color;
                p.score++;

                p.currentTile = this;
                p.transform.position =transform.position;
                p.canChange = true;
            }
        }
    }
}
