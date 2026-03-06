
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HallwayFlagManager : MonoBehaviour
{
    public Camera mc;
    public GameObject hurdle;

    public HallwayFlagPlayer p1, p2, p3, p4;

    public static int studentsLeft = 4;
    public static bool finished = false;

    // Start is called before the first frame update
    void Start()
    {
        finished = false;
        p1 = GameObject.Find("Player1").GetComponent<HallwayFlagPlayer>();
        p2 = GameObject.Find("Player2").GetComponent<HallwayFlagPlayer>();
        p3 = GameObject.Find("Player3").GetComponent<HallwayFlagPlayer>();
        p4 = GameObject.Find("Player4").GetComponent<HallwayFlagPlayer>();

        mc = Camera.main;

        studentsLeft = 4;

        GeraObstaculos();
    }

    // Update is called once per frame
    void Update()
    {
        if (finished || studentsLeft == 0)
        {
            SortResults();

          

            SceneManager.LoadScene("MiniGameRanking");
        }
        else
        {

            mc.GetComponent<Transform>().position += Vector3.right * 4.0f * Time.deltaTime;
        }
    }


    void GeraObstaculos()
    {
        for (int i = 15; i < 65; i++)
        {
            if (Random.Range(0f,10f) >= 5f)
            {
                Instantiate(hurdle, new Vector3(i * 1.0f, 0-2f, 0), Quaternion.identity);
                i += 10;
            }
        }
    }

    void SortResults()
    {
        HallwayFlagPlayer[] players = { p1, p2, p3, p4 };
        
        GameManager.p1.Rank = p1.Rank;
        GameManager.p2.Rank = p2.Rank;
        GameManager.p3.Rank = p3.Rank;
        GameManager.p4.Rank = p4.Rank;

    }
}
