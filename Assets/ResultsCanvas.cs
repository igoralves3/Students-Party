using UnityEngine;

using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsCanvas : MonoBehaviour
{

    public TextMeshProUGUI text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GameObject.Find("TextResults").GetComponent<TextMeshProUGUI>();
        SetResults();
    }

    // Update is called once per frame
    void SetResults()
    {
        var newText = "";

        
            newText += "Mini-Game Results\n";

            newText += "\nP1: " + GameManager.p1.Rank.ToString();


            newText += "\nP2: " + GameManager.p2.Rank.ToString();


            newText += "\nP3: " + GameManager.p3.Rank.ToString();



            newText += "\nP4: " + GameManager.p4.Rank.ToString();


            int winners = 0;
            Player[] players = { GameManager.p1, GameManager.p2, GameManager.p3, GameManager.p4 };
            bool draw = false;
            Player winner = GameManager.p1;
            for (int i = 0; i < 4; i++)
            {
                if (players[i].Rank == 1)
                {

                    winner = players[i];
                    winners++;
                }

            }
            if (winners == 1) {
                newText += "\nCongratulations, Player " + winner.Number;
            }

            text.text = newText;
        
    }
}
