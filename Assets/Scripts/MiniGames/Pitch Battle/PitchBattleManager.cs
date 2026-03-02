using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Linq;
using System.Reflection;

using UnityEngine.UI;

using TMPro;

public class PitchBattleManager : MonoBehaviour
{

    public PitchBattlePlayer p1, p2, p3, p4;
    public GameManager gm;

    private float timeLeft;

    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        p1 = GameObject.Find("Player1").GetComponent<PitchBattlePlayer>();
        p2 = GameObject.Find("Player2").GetComponent<PitchBattlePlayer>();
        p3 = GameObject.Find("Player3").GetComponent<PitchBattlePlayer>();
        p4 = GameObject.Find("Player4").GetComponent<PitchBattlePlayer>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        timeLeft = 60.0f;

        text = GameObject.Find("Placar").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            timeLeft = 0.0f;

            SortResults();

            SceneManager.LoadScene("MiniGameRanking");
        }
    }

    void OnGUI()
    {
        var newText = "";


        newText += "P1: " + ((int)p1.score).ToString();

        

        newText += " \t\tP2: " + ((int)p2.score).ToString();

       newText += " \t\tP3: " + ((int)p3.score).ToString();

        newText += " \t\tP4: " + ((int)p4.score).ToString();

         newText += " \t\tTime: " + ((int)timeLeft).ToString();


        text.text = newText;
    }


    void SortResults()
    {
        var jogadores = FindObjectsOfType<PitchBattlePlayer>()
          .OrderBy(j => j.score)
          .ToList();

        for (int i = 0; i < jogadores.Count; i++)
        {
            jogadores[i].Rank = 4 - i;
        }


        GameManager.p1.Rank = p1.Rank;
        GameManager.p2.Rank = p2.Rank;
        GameManager.p3.Rank = p3.Rank;
        GameManager.p4.Rank = p4.Rank;

    }
}
