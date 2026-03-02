using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{

    public string Name;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {

        switch (Name)
        {
            case "1P":
                if (GameManager.connectedPlayers >= 1)
                {
                    GameManager.playersIngame = 1;
                }

                Debug.Log("Total de jogadores no jogo: " + GameManager.playersIngame);
                SceneManager.LoadScene("MiniGame Select");
                break;
          
            case "2P":
                if (GameManager.connectedPlayers >= 2)
                {
                    GameManager.playersIngame = 2;
                }

                Debug.Log("Total de jogadores no jogo: " + GameManager.playersIngame);
                SceneManager.LoadScene("MiniGame Select");
                break;
            case "3P":
                if (GameManager.connectedPlayers >= 3)
                {
                    GameManager.playersIngame = 3;
                }

                Debug.Log("Total de jogadores no jogo: " + GameManager.playersIngame);
                SceneManager.LoadScene("MiniGame Select");
                break;
            case "4P":
                if (GameManager.connectedPlayers >= 4)
                {
                    GameManager.playersIngame = 4;
                }

                Debug.Log("Total de jogadores no jogo: " + GameManager.playersIngame);
                SceneManager.LoadScene("MiniGame Select");
                break;
            default:
                break;
        }

        switch (Name)
        {
            case "Free Battle":
                SceneManager.LoadScene("PlayerSettings");
                //SceneManager.LoadScene("MiniGame Select");
                break;
            case "Start":
               
                SceneManager.LoadScene(GameManager.game);
                break;
            case "Back":
                SceneManager.LoadScene("TitleScreen");
                break;
            case "Return":
                GameManager.p1.Rank = 0;
                GameManager.p2.Rank = 0;
                GameManager.p3.Rank = 0;
                GameManager.p4.Rank = 0;
                SceneManager.LoadScene("MiniGame Select");
                break;
            default:
                break;
        }

        switch (Name)
        {
            case "Coconut Clash":
                GameManager.game = "Coconut Clash";
                SceneManager.LoadScene("MiniGameRules");
                break;

            case "Hallway Flag":
                GameManager.game = "Hallway Flag";
                SceneManager.LoadScene("MiniGameRules");
                break;
            case "Trouble Tag":
                GameManager.game = "Trouble Tag";
                SceneManager.LoadScene("MiniGameRules");
                break;
            case "Bag Capture":
                GameManager.game = "Bag Capture";
                SceneManager.LoadScene("MiniGameRules");
                break;
            case "Gridroom Paint":
                GameManager.game = "Gridroom Paint";
                SceneManager.LoadScene("MiniGameRules");
                break;
            case "Splash Fun":
                GameManager.game = "Splash Fun";
                SceneManager.LoadScene("MiniGameRules");
                break;
            case "Top Spin Slide":
                GameManager.game = "Top Spin Slide";
                SceneManager.LoadScene("MiniGameRules");
                break;
            case "Pitch Battle":
                GameManager.game = "Pitch Battle";
                SceneManager.LoadScene("MiniGameRules");
                break;
            case "Treasure Hunt":
                GameManager.game = "Treasure Hunt";
                SceneManager.LoadScene("MiniGameRules");
                break;
            default:
                break;
        }
    }
}
