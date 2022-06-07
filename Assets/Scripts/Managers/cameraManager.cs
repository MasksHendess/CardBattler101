using Assets.Scripts.Managers;
using Assets.Scripts.TradeView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cameraManager : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public Camera main;
    public GameObject DraftView;
   // public GameObject StartView;
    public GameObject GameView;
    public GameObject MapView;
    public Canvas DraftViewCanvas;
    public Canvas GameViewCanvas;
    public Canvas TradeViewCanvas;
    public Canvas DeckInteractionViewCanvas;
    public Canvas MapViewCanvas;
    public Canvas PauseViewCanvas;

    public List<Canvas> AllCanvases;
    #region Buildmanager setup Singelton pattern
    // only 1 instance of BuildManager in scene that is easy to acsess
    // Dont duplicate this region 
    public static cameraManager instance; //self reference
    private void Awake()
    {
        //check if instance already exisist
        if (instance != null)
        {
            Debug.LogError("More than one camManager in scene");
            return;
        }

        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseViewCanvas.enabled = false;
       // PauseViewCanvas.gameObject.SetActive(false);
        // pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseViewCanvas.enabled = true; 
      //  PauseViewCanvas.gameObject.SetActive(true);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void Exit()
        {
        Environment.Exit(0);
        Application.Quit(0);
    }
    public Animator transition;
    public void changeView(Canvas eneabledCanvas, GameObject cameraposition)
    {
        foreach (var canvas in AllCanvases)
        {
            canvas.enabled = false;
            canvas.gameObject.SetActive(false);
        }
        eneabledCanvas.gameObject.SetActive(true);
        eneabledCanvas.enabled = true;
        main.gameObject.transform.position = cameraposition.transform.position;
    }
    public void goToMapScreen()
    {
        changeView(MapViewCanvas, MapView);
        PlayerMovement.instance.freeze = false;
    }
    public void goToDeckInteractionScreen(int view)
    {
        changeView(DeckInteractionViewCanvas, MapView);
        DeckInteractionManager.instance.view = view;
    }
    public void goToDraftScreen(int view)
    {
        changeView(DraftViewCanvas, GameView);
        DraftViewManager.instance.view = view;
    }
    public void goToTradeScreen(int view)
    {
        changeView(TradeViewCanvas, GameView);
        TradeViewManager.instance.view = view;
    }
    public void goToGameScreen()
    {
        changeView(GameViewCanvas, GameView);
        PlayerDeckHandler.instance.DrawStartingHand();
        CombatHandler.instance.gameState = gameState.DRAWPHASE;
    }
    public void changeCamera()
    {
        var endMarker1 = GameObject.FindGameObjectsWithTag("menu").FirstOrDefault();
        Debug.Log(endMarker1);
        main.gameObject.transform.position =  endMarker1.transform.position;

        SceneManager.LoadScene("Untitled");
    }
}
