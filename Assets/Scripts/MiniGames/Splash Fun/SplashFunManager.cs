using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashFunManager : MonoBehaviour
{
    public SplashFunPlayer p1, p2, p3, p4;

    public float timeLeft;

    public GameManager gm;

    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        p1 = GameObject.Find("Player1").GetComponent<SplashFunPlayer>();
        p2 = GameObject.Find("Player2").GetComponent<SplashFunPlayer>();
        p3 = GameObject.Find("Player3").GetComponent<SplashFunPlayer>();
        p4 = GameObject.Find("Player4").GetComponent<SplashFunPlayer>();

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
            AtualizaTexto();

            Debug.Log(text.text);

            SortResults();

            SceneManager.LoadScene("MiniGameRanking");
        }AtualizaTexto();
    }

    void AtualizaTexto()
    {
        var newText = "";


        newText += "P1: " + ((int)p1.score).ToString();

        newText += " B1: " + ((int)p1.balloonsLeft).ToString();


        newText += " \t\tP2: " + ((int)p2.score).ToString();

        newText += " B2: " + ((int)p2.balloonsLeft).ToString();

        newText += "\nP3: " + ((int)p3.score).ToString();

        newText += " B3: " + ((int)p3.balloonsLeft).ToString();

        newText += " \t\tP4: " + ((int)p4.score).ToString();
        
        newText += " B4: " + ((int)p4.balloonsLeft).ToString();

        newText += " \t\tTime: " + ((int)timeLeft).ToString();

  
        text.text = newText;
    }

    void SortResults()
    {

        var jogadores = new List<SplashFunPlayer>() { p1, p2, p3, p4 };
        //.OrderByDescending(j => j.score)
        //.ToList();
        jogadores.Sort((a, b) => b.score.CompareTo(a.score));


        int posicao = 0;
        int contador = 0;
        int? pontosAnterior = null;

        foreach (var jogador in jogadores)
        {
            contador++;

            if (pontosAnterior == null || jogador.score != pontosAnterior)
            {
                posicao = contador;

            }
            jogador.Rank = posicao;


            Console.WriteLine($"{posicao} - {jogador.PlayerNumber} - {jogador.score}");

            pontosAnterior = jogador.score;
        }

        GameManager.p1.Rank = jogadores.Find(p => p.PlayerNumber == 1).Rank;
        GameManager.p2.Rank = jogadores.Find(p => p.PlayerNumber == 2).Rank;
        GameManager.p3.Rank = jogadores.Find(p => p.PlayerNumber == 3).Rank;
        GameManager.p4.Rank = jogadores.Find(p => p.PlayerNumber == 4).Rank;

    }
}
