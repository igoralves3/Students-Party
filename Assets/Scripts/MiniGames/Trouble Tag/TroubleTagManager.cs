using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TroubleTagManager : MonoBehaviour
{
    public float time;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GameObject.Find("TimeLeft").GetComponent<TextMeshProUGUI>();
        time = 30.0f;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time<= 0)
        {
            time = 0;

            foreach (var p in GameObject.FindGameObjectsWithTag("Player"))
            {
                
                switch (p.GetComponent<TroubleTagPlayer>().PlayerNumber)
                {
                    case 1:
                        GameManager.p1.Rank = 1;
                        break;
                    case 2:
                        GameManager.p2.Rank = 1;
                        break;
                    case 3:
                        GameManager.p3.Rank = 1;
                        break;
                    case 4:
                        GameManager.p4.Rank = 1;
                        break;
                    default: break;
                }
            }

            SceneManager.LoadScene("MiniGameRanking");
        }

        AtualizaTexto();
    }

    void AtualizaTexto()
    {
        text.text = "Time: " + ((int)time).ToString();
    }
}
