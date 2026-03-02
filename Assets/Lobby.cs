using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lobby : MonoBehaviour
{

   


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var p in GameManager.players)
        {
            var t = GameObject.Find("Status" + p.Number.ToString()).GetComponent<TextMeshProUGUI>();
            if (p.inputDevice != null) {

                t.text = "P"+ p.Number.ToString();
            }
            else
            {
                var b = GameObject.Find("P" + p.Number.ToString()).GetComponent<UnityEngine.UI.Button>();
                b.interactable = false;
                t.text = "COM";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
