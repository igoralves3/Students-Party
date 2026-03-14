
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopSpinSlidePlayer : MonoBehaviour
{
    public int PlayerNumber;
    public bool isAI;
    public int score;
    public int Rank;

    public PlayerTop top;
  

    private bool canSlide = true;
    private int frames = 0;
    
    private int actionFrames=0;

    private PlayerInput playerInput;

    void Awake()
    {
        if (!isAI)
        {
            playerInput = GetComponent<PlayerInput>();


        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.playersIngame >= PlayerNumber)
        {
            if (PlayerNumber == 1)
            {
                playerInput.SwitchCurrentControlScheme(
                    "Keyboard&Mouse",
                Keyboard.current,
                Mouse.current
            );
            }
            else
            {
                playerInput = GetComponent<PlayerInput>();

                var gamepad = Gamepad.all[PlayerNumber - 2]; // primeiro controle
                playerInput.SwitchCurrentControlScheme(gamepad);
            }
        }
        else
        {
            isAI = true;
        }


        score = 0;
        canSlide = true;

        actionFrames = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAI)
        {
            if (playerInput.actions["Space"].WasPressedThisFrame())
            {
                if (canSlide) {
                    canSlide = false;
                    frames = 0;

                    var t = Instantiate(top, transform.position, Quaternion.identity);


                    t.owner = this;
                }
            }
            if (!canSlide)
            {
                frames++;
                if (frames >= 60)
                {
                    frames = 0;

                   

                    canSlide=true;
                }
            }
        }
        else
        {
            


                var peoes = GameObject.FindGameObjectsWithTag("OtherTop");
                var despara = false;

                var i = 0;

                foreach (var p in peoes)
                {


                    var delta = Vector3.Distance(transform.position, p.transform.position);

                    var deltaX = Mathf.Abs(transform.position.x - p.transform.position.x);
                    var deltaY = Mathf.Abs(transform.position.y - p.transform.position.y);

                    var maxDelta = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);

                    if (delta / 2f >= maxDelta / 2f && delta / 2f <= (maxDelta / 2f) + 2f)
                    {

                        despara = true;
                        break;

                    }

                    i++;
                    if (i >= 10)
                    {
                        break;
                    }
                }

                if (despara && Random.Range(0f,100f) <= 10f)
                {

                    if (canSlide)
                    {
                        canSlide = false;
                        frames = 0;
                    }

                }
                if (!canSlide)
                {
                    frames++;
                    if (frames >= 1200)
                    {
                        frames = 0;

                        var t = Instantiate(top, transform.position, Quaternion.identity);
                        t.owner = this;

                        canSlide = true;
                    }

                
            }
        }
    }
}
