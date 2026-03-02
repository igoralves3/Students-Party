
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SliderTop : MonoBehaviour
{

    public TopSpinSlidePlayer owner;

    public SpriteRenderer sprR;

    public Sprite spr1, spr2, spr3, spr4, spr5, sprO;

    public int Number;

    private float dirX;

    // Start is called before the first frame update
    void Start()
    {
        sprR =GetComponent<SpriteRenderer>();

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
        if (transform.position.x + dirX >= 11f || transform.position.x + dirX <= -11f)
        {
            Destroy(gameObject);
        }

        transform.position += transform.right * dirX * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Top")
        {

        }
    }
}
