using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Linq;
using System.Reflection;

using UnityEngine.UI;

using TMPro;

public class BagCaptureManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public BagCapturePlayer p1, p2, p3, p4;
    public float timeLeft;

    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        p1 = GameObject.Find("Player1").GetComponent<BagCapturePlayer>();
        p2 = GameObject.Find("Player2").GetComponent<BagCapturePlayer>();
        p3 = GameObject.Find("Player3").GetComponent<BagCapturePlayer>();
        p4 = GameObject.Find("Player4").GetComponent<BagCapturePlayer>();

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        timeLeft = 60.0f;

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

      

        newText += "\nP1: " + ((int)p1.score).ToString();

      

        newText += "\nP2: " + ((int)p2.score).ToString();

       
        newText += "\nP3: " + ((int)p3.score).ToString();

       

        newText += "\nP4: " + ((int)p4.score).ToString();

       

        newText += "\nTime: " + ((int)timeLeft).ToString();

        text.text = newText;
    }

    void SortResults()
    {

        var bag = GameObject.Find("Bag").GetComponent<Bag>();

        bag.owner.GetComponent<BagCapturePlayer>().score = 99;

        var jogadores = FindObjectsOfType<BagCapturePlayer>()
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
