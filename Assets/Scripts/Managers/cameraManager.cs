using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Canvas DeckInteractionViewCanvas;
    public Canvas MapViewCanvas;
    public Canvas PauseViewCanvas;

    public List<Canvas> AllCanvases;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
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
        Application.Quit(0);
    }
    public void changeView(Canvas eneabledCanvas, GameObject cameraposition)
    {
        foreach (var canvas in AllCanvases)
        {
            canvas.enabled = false;
            canvas.gameObject.SetActive(false);
        }
        eneabledCanvas.gameObject.SetActive(true);
        eneabledCanvas.enabled = true;
     //   main.gameObject.transform.position = cameraposition.transform.position;
    }
    public void goToMapScreen()
    {
        changeView(MapViewCanvas, MapView);
    }
    public void goToDeckInteractionScreen()
    {
        changeView(DeckInteractionViewCanvas, MapView);
    }
    public void goToDraftScreen()
    {
        changeView(DraftViewCanvas, GameView);
    }
    //public void goToStartScreen()
    //{
    //    changeView(startViewCanvas, StartView);
    //}
    public void goToGameScreen()
    {
        changeView(GameViewCanvas, GameView);
    }
    public void changeCamera()
    {
        var endMarker1 = GameObject.FindGameObjectsWithTag("menu").FirstOrDefault();
        Debug.Log(endMarker1);
        main.gameObject.transform.position =  endMarker1.transform.position;

        SceneManager.LoadScene("Untitled");
    }
}
