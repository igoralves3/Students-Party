using UnityEngine;

public class CaughtTreasure : MonoBehaviour
{

    public Sprite spr1, spr2, spr3, sprCurrent;

    public SpriteRenderer sprR;

    private int frames;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprR = GetComponent<SpriteRenderer>();
        frames = 0;

        sprR.sprite = sprCurrent;
    }

    // Update is called once per frame
    void Update()
    {
        frames++;
        if (frames >= 600)
        {
            Destroy(gameObject);
        }
    }
}
