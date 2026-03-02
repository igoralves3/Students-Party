using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TMPro;

public class RulesInfo : MonoBehaviour
{

    public Canvas canvas;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GameObject.Find("TextRules").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
       // if (Input.GetKey(KeyCode.Space))
        //{
          //  SceneManager.LoadScene(GameManager.game, LoadSceneMode.Single);
        //}
    }

    void OnGUI()
    {
        string newText = "";

        switch (GameManager.game)
        {
            case "Coconut Clash":
                newText += "Coconut Clash\n" + "\nThrow coconuts on your teacher!"
                           + "\nArrow keys: Move"
                            +"\nSpace bar: Throw Coconut";
                
                break;
            case "Hallway Flag":
                newText += "Hallway Flag\n" + "\nAvoid the hurdles and get the flag first!"
                           + "\nArrow keys: Move"
                            + "\nSpace bar: Jump";
                
                break;
            case "Trouble Tag":
                newText += "Trouble Tag\n" + "\nDon't get caught by your teacher!"
                           + "\nArrow keys: Move";
               
                break;

            case "Bag Capture":
                newText += "Bag Capture\n" + "\nDon't get caught by your teacher!"
                    + "\nBehold the bag unil the time runs out!"
                           + "\nArrow keys: Move";
                           
                
                break;
            case "Gridroom Paint":
                newText += "Gridroom Paint\n" + "\nPaint the most floor parts when the time runs out!"
                   
                          + "\nArrow keys:  Change move direction";
                
                break;
            case "Splash Fun":
                
                newText += "Splash Fun\n" + "\nThrow water or balloons on classmates!"

                                         + "\nArrow keys: Move" + "\nZ: Fire water" + "\nX: Throw balloon";

                break;
            case "Top Spin Slide":
               

                newText += "Top Spin Slide\n" + "\nKnock the tops to earn points!"

                                        + "\nSpace bar: Slide your top";

                break;
            case "Pitch Battle":
                

                newText += "Pitch Battle\n" + "\nThrow balls on opponents!"

                                        + "\nArrow keys: Select Target"
                                        + "\nHold space bar: Charge pitch"
                                         + "\nRelease space bar: Pitch";

                break;
            case "Treasure Hunt":
                
                newText += "Treasure Hunt\n" + "\nFind treasures inside backpacks!"

                                        + "\nArrow keys: Movet"
                                        + "\nHold space bar: Open backpack";

                break;
            default:
                break;
        }

        text.text = newText;
    }
}
