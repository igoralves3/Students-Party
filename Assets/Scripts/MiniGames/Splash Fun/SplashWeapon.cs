using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SplashWeapon : MonoBehaviour
{
    public SplashFunPlayer player;
    public SpriteRenderer sprR;

    public float dir = 0.1f,dirY=0.1f;

    public GameObject waterDrop;
    public bool atirando,recarregando;

    private int frames;

    // Start is called before the first frame update
    void Start()
    {
        frames = 0;
        dir = 0.2f;
        dirY = 0.2f;
        atirando = false;
        recarregando = false;
        player = GetComponentInParent<SplashFunPlayer>();
        sprR = GetComponent<SpriteRenderer>();
        transform.position = player.transform.position + new Vector3(dir, 0.0f, -1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        switch (player.dir)
        {
            case SplashFunPlayer.leftDir:
                dir = -0.2f;
                dirY = 0f;
                transform.position = player.transform.position+new Vector3(dir,0.0f,-1.0f);
              
                sprR.flipX=true;
                break;
            case SplashFunPlayer.rightDir:
                dir = 0.2f;
                dirY = 0f;
                transform.position = player.transform.position + new Vector3(dir, 0.0f, -1.0f);
               
                sprR.flipX = false;
                break;
            case SplashFunPlayer.upDir:
                dirY = 0.2f;
                dir = 0f;
                transform.position = player.transform.position + new Vector3(dir, 0.0f, -1.0f);
              
                break;
            case SplashFunPlayer.downDir:
                dirY = -0.2f;
                dir = 0f;
                transform.position = player.transform.position + new Vector3(dir, 0.0f, -1.0f);
               
                break;
            default:
                break;
        }*/

        if (player.dirThrow > 0)
        {
            dir = 0.2f;
            //dirY = 0f;
        }
        else if (player.dirThrow < 0)
        {
            dir = -0.2f;
            //dirY = 0f;
        }
        else
        {
            dir = 0f;
        }
        if (player.dirThrowY > 0)
        {
            dirY = 0.2f;
            //dir = 0f;
        }
        else if (player.dirThrowY < 0)
        {
            dirY = -0.2f;
            //dir = 0f;
        }
        else
        {
            dirY = 0f;
        }



        if (!player.podeAtirar)
        {
              
            atirando = true;
            
        }
        
        
        else if (atirando)
        {
            atirando = false;
            recarregando = true;
            frames = 0;
            var newDrop = Instantiate(waterDrop, new Vector3(transform.position.x+2f*dir, transform.position.y+2f*dirY, 0), Quaternion.identity);
            var c = newDrop.GetComponent<WaterDrop>();
            c.dirX = dir * 1f;
            c.dirY = dirY * 1f;
            c.owner = player;
        }
       else // if (recarregando)
        {
            frames++;
            if (frames>=600)
            {
                frames = 0;
                recarregando=false;
                player.podeAtirar = true;
            }
        }
    }
}
