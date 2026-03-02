using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class PlayerTop : MonoBehaviour
{

    public TopSpinSlidePlayer owner;

    public SpriteRenderer sprR;

    public Sprite spr1, spr2, spr3, spr4;
    public int Number;

    // Start is called before the first frame update
    void Start()
    {
        sprR=GetComponent<SpriteRenderer>();
        switch (owner.PlayerNumber)
        {
            case 1:
                sprR.sprite = spr1;
                break;
            case 2:
                sprR.sprite = spr2;
                break;
            case 3:
                sprR.sprite = spr3;
                break;
            case 4:
                sprR.sprite = spr4;
                break;
            default:
                break;
        }
        Number = owner.PlayerNumber;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= 2f * transform.up * Time.deltaTime;
        if (transform.position.y <= -5)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "OtherTop")
        {
            var c = collision.gameObject.GetComponent<SliderTop>();
            if (owner.PlayerNumber == c.Number)
            {
                owner.score+=2;
            }else if (c.Number == 0)
            {
                owner.score += 5;
            }
            else
            {
                owner.score++;
            }
            Destroy(collision.gameObject);
        }
    }
}
