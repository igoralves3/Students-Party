using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TMPro;

public class RulesInfo : MonoBehaviour
{
    [SerializeField] private GameObject[] paineis;

    [SerializeField] private UnityEngine.UI.Button[] botoes;

    [SerializeField] private TextMeshProUGUI miniGameTitle;

    public Canvas canvas;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GameObject.Find("TextRules").GetComponent<TextMeshProUGUI>();

        var backButton = GameObject.Find("BackButton").GetComponent<UnityEngine.UI.Button>();

        miniGameTitle = GameObject.Find("MinigameTitle").GetComponent<TextMeshProUGUI>();
        if (GameManager.mode != "Free Battle")
        {
            backButton.enabled = false;
        }

        AtualizaTexto(0);
    }

    // Update is called once per frame
    void Update()
    {
      
    }


    public void TrocarPainel(int index)
    {
        for (int i = 0; i < botoes.Length; i++)
        {
            botoes[i].interactable=true;
        }

        botoes[index].interactable=false;

        AtualizaTexto(index);
    }

    void AtualizaTexto(int index)
    {
        string newText = "";

        switch (GameManager.game)
        {
            case "Coconut Clash":
                miniGameTitle.text = "Coconut Clash";

                switch (index) {
                    case 0:
                     newText += "\nThrow coconuts on your teacher!";
                        break;
                    case 1:
                        newText += "\nArrow keys: Move"
                            + "\nSpace bar: Throw Coconut";
                        break;
                    case 2:
                        newText += "\nD-pad/Left Stick: Move"
                            + "\nSouth button: Throw Coconut";
                        break;
                    default: break;

                 }
                break;
            case "Hallway Flag":
                miniGameTitle.text = "Hallway Flag\n";
                switch (index)
                {
                    case 0:
                        newText += "\nAvoid the hurdles and get the flag first!";
                        break;
                    case 1:
                        newText += "\nArrow keys: Move"
                            + "\nSpace bar: Jump";
                        break;
                    case 2:
                        newText += "\nD-pad/Left Stick: Move"
                            + "\nSouth button: Jump";
                        break;
                    default: break;

                }
                break;
            case "Trouble Tag":
                miniGameTitle.text = "Trouble Tag\n";
                switch (index)
                {
                    case 0:
                        newText += "\nDon't get caught by your teacher!";
                        break;
                    case 1:
                        newText += "\nArrow keys: Move";
                        break;
                    case 2:
                        newText += "\nD-pad/Left Stick: Move";
                        break;
                    default: break;

                }
                break;

            case "Bag Capture":
                miniGameTitle.text = "Bag Capture\n";

                switch (index)
                {
                    case 0:
                        newText += "\nBehold the bag unil the time runs out!";
                        break;
                    case 1:
                        newText += "\nArrow keys: Move";
                        break;
                    case 2:
                        newText += "\nD-pad/Left Stick: Move";
                        break;
                    default: break;

                }
                break;
            case "Gridroom Paint":
                miniGameTitle.text = "Gridroom Paint\n";
                switch (index)
                {
                    case 0:
                        newText += "\nPaint the most floor parts when the time runs out!";
                        break;
                    case 1:
                        newText += "\nArrow keys:  Change move direction";
                        break;
                    case 2:
                        newText += "\nD-pad/Left Stick:  Change move direction";
                        break;
                    default: break;

                }
                break;
            case "Splash Fun":

                miniGameTitle.text = "Splash Fun\n";
                switch (index)
                {
                    case 0:
                        newText += "\nThrow water or balloons on classmates!";
                        break;
                    case 1:
                        newText += "\nArrow keys: Move\nZ: Fire water\nX: Throw balloon";
                        break;
                    case 2:
                        newText += "\nD-pad/Left Stick: Move\nNorth button: Fire water\nEast/west button: Throw balloon";
                        break;
                    default: break;

                }
                break;
            case "Top Spin Slide":


                miniGameTitle.text = "Top Spin Slide\n";
                switch (index)
                {
                    case 0:
                        newText += "\nKnock the tops to earn points!";
                        break;
                    case 1:
                        newText += "\nSpace bar: Slide your top";
                        break;
                    case 2:
                        newText += "\nSouth button: Slide your top";
                        break;
                    default: break;

                }
                break;
            case "Pitch Battle":


                miniGameTitle.text = "Pitch Battle\n";
                 switch (index)
                {
                    case 0:
                        newText += "\nThrow balls on opponents!";
                        break;
                    case 1:
                        newText += "\nArrow keys: Select Target"
                                        + "\nHold space bar: Charge pitch" + "\nRelease space bar: Pitch";
                        break;
                    case 2:
                        newText += "\nD-pad/Left Stick: Select Target"
                                        + "\nHold South button: Charge pitch" + "\nRelease South button: Pitch";
                        break;
                    default: break;

                }
               

                break;
            case "Treasure Hunt":

                miniGameTitle.text = "Treasure Hunt\n" + "\nFind treasures inside backpacks!";
                switch (index)
                {
                    case 0:
                        newText += "\nFind treasures inside backpacks!";
                        break;
                    case 1:
                        newText += "\nArrow keys: Move"
                                        + "\nSpace bar: Open backpack";
                        break;
                    case 2:
                        newText += "\nD-pad/Left Stick: Move"
                                       + "\nSouth button: Open backpack";
                        break;
                    default: break;

                }
                break;
            default:
                break;
        }

        text.text = newText;
    }
}
