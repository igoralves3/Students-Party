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
    }

    // Update is called once per frame
    void Update()
    {
        var newText = "";

        var scene = SceneManager.GetActiveScene();

        if (scene.name == "MiniGameRanking")
        {
            newText += "Mini-Game Results\n";

            newText += "\nP1: " + GameManager.p1.Rank.ToString();


            newText += "\nP2: " + GameManager.p2.Rank.ToString();


            newText += "\nP3: " + GameManager.p3.Rank.ToString();



            newText += "\nP4: " + GameManager.p4.Rank.ToString();



            Player[] players = { GameManager.p1, GameManager.p2, GameManager.p3, GameManager.p4 };
            bool draw = false;
            Player winner = GameManager.p1;
            for (int i = 1; i < 4; i++)
            {
                if (players[i].Rank == 1)
                {

                    winner = players[i];

                }

            }
            newText += "\nCongratulations, Player " + winner.Number;


            text.text = newText;
        }
    }
}
