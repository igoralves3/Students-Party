using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    public Sprite openSpr, closedSpr;

    public SpriteRenderer sprR;

    public bool isOpen = false;

    public Sprite diamondSpr, rubySpr, amethystSpr;

    public Sprite treasureSpr;

    public BoxCollider2D bc;

    public GameObject currentJewel;

    // Start is called before the first frame update
    void Start()
    {
        sprR = GetComponent<SpriteRenderer>();

        bc = GetComponent<BoxCollider2D>();

        isOpen = false;

        var r = Random.Range(0f,6f);
        if (r <= 3f)
        {
            treasureSpr = amethystSpr;

        }else if (r > 3f && r <= 5f)
        {
            treasureSpr = rubySpr;
        }
        else
        {
            treasureSpr = diamondSpr;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            var pc = collision.gameObject.GetComponent<TreasureHuntPlayer>();
            if (!isOpen && pc.opening)
            {
                AplicaPontoMochila(pc);
            }
        }
    }

    public void AplicaPontoMochila(TreasureHuntPlayer pc)
    {
        isOpen = true;

        sprR.sprite = openSpr;
        if (treasureSpr == amethystSpr)
        {
            pc.score += 1;



        }
        else if (treasureSpr == rubySpr)
        {
            pc.score += 2;


        }
        else if (treasureSpr == diamondSpr)
        {
            pc.score += 3;
        }

        var cj = Instantiate(currentJewel, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);

        var cjS = cj.GetComponent<CaughtTreasure>();

        cjS.sprCurrent = treasureSpr;

        pc.opening = false;

      

        Physics2D.IgnoreCollision(bc, pc.bc);
        bc.enabled = false;
    }
}
