using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

using TMPro;

public class CoconutClashManager : MonoBehaviour
{
    public CoconutClashPlayer p1, p2, p3, p4;

    public float timeLeft;

    public GameManager gm;

    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        p1 = GameObject.Find("Player1").GetComponent<CoconutClashPlayer>();
        p2 = GameObject.Find("Player2").GetComponent<CoconutClashPlayer>();
        p3 = GameObject.Find("Player3").GetComponent<CoconutClashPlayer>();
        p4 = GameObject.Find("Player4").GetComponent<CoconutClashPlayer>();

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        timeLeft = 30.0f;

        text = GameObject.Find("Placar").GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            timeLeft = 0;


            SortResults();

            SceneManager.LoadScene("MiniGameRanking");
        }
    }

    void OnGUI()
    {

        var newText = "";

        //GUI.color = Color.cyan;

        //GUI.Label(new Rect(10, 10, 100, 20), "P1: " + p1.score.ToString());

        newText += "\nP1: " + p1.score.ToString();

        //GUI.color = Color.red;

        //GUI.Label(new Rect(10, 20, 100, 20), "P2: " + p2.score.ToString());

        newText += "\nP2: " + p2.score.ToString();

        //GUI.color = Color.green;

        //GUI.Label(new Rect(10, 30, 100, 20), "P3: " + p3.score.ToString());

        newText += "\nP3: " + p3.score.ToString();

        //GUI.color = Color.yellow;

        //GUI.Label(new Rect(10, 40, 100, 20), "P4: " + p4.score.ToString());

        newText += "\nP4: " + p4.score.ToString();

        //GUI.color = Color.black;

        //GUI.Label(new Rect(10, 50, 100, 20), "Time: " + ((int) timeLeft).ToString());

        newText += "\nTime: " + ((int)timeLeft).ToString();

        text.text = newText;
    }

    void SortResults()
    {
        var jogadores = FindObjectsOfType<CoconutClashPlayer>()
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
