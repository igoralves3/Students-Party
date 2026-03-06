
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class SliderTop : MonoBehaviour
{

    public TopSpinSlidePlayer owner;

    public SpriteRenderer sprR;

    public Sprite spr1, spr2, spr3, spr4, spr5, sprO;

    public int Number;

    public BoxCollider2D bc;
    public Rigidbody2D rb;

    private float dirX;
    private float dirY = 0f;

    private bool knocked = false;

    // Start is called before the first frame update
    void Start()
    {
        sprR =GetComponent<SpriteRenderer>();

        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        var r = Random.Range(0f,12f);

        if (r <= 2f)
        {
            sprR.sprite = sprO;
            Number = 0;
        }else if (r > 2f && r <= 4f)
        {
            sprR.sprite = spr1;
            Number = 1;
        }
        else if (r > 4f && r <= 6f)
        {
            sprR.sprite = spr2;
            Number = 2;
        }
        else if (r > 6f && r <= 8f)
        {
            sprR.sprite = spr3;
            Number = 3;
        }
        else if (r > 8f && r <= 10f)
        {
            sprR.sprite = spr4;
            Number = 4;
        }
        else
        {
            sprR.sprite = spr5;
            Number = 5;
        }
        if (transform.position.x < 0)
        {
            dirX = 1f;
        }
        else
        {
            dirX = -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (knocked)
        {
           
            return;
        }
        if (transform.position.x + dirX >= 11f || transform.position.x + dirX <= -11f || transform.position.y + dirY >= 11f || transform.position.y + dirY <= -11f)
        {
            Destroy(gameObject);
        }

        var delta = new Vector3(dirX,dirY,0f);

        transform.position += delta * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "OtherTop")
        {

            //Destroy(gameObject);
            if (transform.position.x > collision.gameObject.transform.position.x)
            {
                dirX = 1f;
            }else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                dirX = -1f;
            }
        }
        if (collision.gameObject.tag == "Top")
        {
            if (transform.position.x > collision.gameObject.transform.position.x)
            {
                dirX = 1f;
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                dirX = -1f;
            }
            else
            {
                dirX = 0f;
            }
            if (transform.position.y > collision.gameObject.transform.position.y)
            {
                dirY = 1f;
            }
            else if (transform.position.y < collision.gameObject.transform.position.y)
            {
                dirY = -1f;
            }
            else
            {
                dirY = 0f;
            }
            Knockback(new Vector2(dirX,dirY),1f);
        }
    }

    public void Knockback(Vector2 direction, float force)
    {
        bc.enabled = false;

        rb.velocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
