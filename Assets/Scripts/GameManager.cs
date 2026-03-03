using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int Number { get; set; }
    public bool isAI { get; set; }
    public int Rank { get; set; }
    public InputDevice inputDevice { get; set; }
}

public class GameManager : MonoBehaviour
{
    public static Player p1 = new Player() { Number = 1, isAI = false, Rank = 0};
    public static Player p2 = new Player() { Number = 2, isAI = true, Rank = 0 };
    public static Player p3 = new Player() { Number = 3, isAI = true, Rank = 0 };
    public static Player p4 = new Player() { Number = 4, isAI = true, Rank = 0 };

    public static Player[] players = { p1, p2, p3, p4 };
    public static int connectedPlayers = 0;
    public static int playersIngame = 0;

    public GUIStyle guis;

    public static string mode;
    public static string game;

    public TextMeshProUGUI text;

    public static PlayerInputManager playerInputManager;

    public static GameManager Instance;

    public void CheckControllers()
    {
        Debug.Log("Total de jogadores: " + connectedPlayers);

    }

    bool JaExisteJogadorComDispositivo(InputDevice device)
    {
        foreach (var player in players)
        {
            if (player.inputDevice == device)
                return true;
        }
        return false;
    }

    public void SetControllers()
    {
        var connectedStatus = 0;
           
                
                Debug.Log("Player 1 entrou com teclado");

                players[connectedStatus].inputDevice = Keyboard.current;
        connectedStatus++;
            

        foreach (var gamepad in Gamepad.all)
        {
           
            if (connectedPlayers < 4) {
                
                Debug.Log("Jogador entrou com controle: " + gamepad.displayName);

                players[connectedStatus].inputDevice = gamepad;
                connectedStatus++;
            }
              
        }
        connectedPlayers = connectedStatus;
    }

    void Awake()
    {
        if (Instance == null)
        {

            Instance = this;
            DontDestroyOnLoad(gameObject);

           
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(null);

            

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        SetControllers();
    }

    // Start is called before the first frame update
    void Start()
    {
        connectedPlayers = 0;

        
        SetControllers();
        CheckControllers();

        var scene = SceneManager.GetActiveScene();
        if (scene.name == "MiniGame Select")
        {
            mode = "Free Battle";
        }
       

            guis.fontSize = 40;
    }

    // Update is called once per frame
    void Update()
    {
       
        

    }


}
