
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class TopSpinSlideManager : MonoBehaviour
{

    public TopSpinSlidePlayer p1, p2, p3, p4;
    public SliderTop sliderTop;

    public float timeLeft;

    public GameManager gm;

    private int frames;

    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        p1 = GameObject.Find("Player1").GetComponent<TopSpinSlidePlayer>();
        p2 = GameObject.Find("Player2").GetComponent<TopSpinSlidePlayer>();
        p3 = GameObject.Find("Player3").GetComponent<TopSpinSlidePlayer>();
        p4 = GameObject.Find("Player4").GetComponent<TopSpinSlidePlayer>();

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        frames = 0;
        timeLeft = 30.0f;

        text = GameObject.Find("Placar").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var d = Time.deltaTime;
        timeLeft -= d;
        if (timeLeft <= 0)
        {
            timeLeft = 0;


            SortResults();

            SceneManager.LoadScene("MiniGameRanking");
        }
        frames++;
       
        if (frames >= 300) {
            frames = 0;
            var r = Random.Range(0f, 100f);
            
                var nextX = 0f;
                var nextY = 0f;
                var b = Random.Range(0f, 10f);

                if (b <= 5f)
                {
                    nextX = -10f;

                }
                else
                {
                    nextX = 10f;

                }
                nextY = Random.Range(0f, 6f) - 4f;

                var t = Instantiate(sliderTop, new Vector3(nextX, nextY, 0f), Quaternion.identity);
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

        var jogadores = FindObjectsOfType<TopSpinSlidePlayer>()
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
