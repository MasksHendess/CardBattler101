using Assets.Scripts.Managers;
using Assets.Scripts.TradeView;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    #region Buildmanager setup Singelton pattern
    // only 1 instance of BuildManager in scene that is easy to acsess
    // Dont duplicate this region 
    public static LevelLoader instance; //self reference
    private void Awake()
    {
        //check if instance already exisist
        if (instance != null)
        {
            Debug.LogError("More than one LevelLoader in scene");
            return;
        }

        instance = this;
    }
    #endregion
    public Animator transition;
    public float transitionTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        transition.SetTrigger("Start");
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    LoadNextLevel();
        //}
    }

    public void LoadNextLevel(int index)
    {
       StartCoroutine(LoadLevel(index));
        
    }
    IEnumerator LoadLevel( int index)
    {
        transition.SetTrigger("End");
        yield return new WaitForSeconds(transitionTime);
        transition.SetTrigger("Start");
        switch (index)
        {
            case 0:
                cameraManager.instance.goToGameScreen();
                break;
            case 1:// Draft
                cameraManager.instance.goToDraftScreen(1);
                DraftViewManager.instance.SelectView(); 
                break;
            case 2: //Gamble
                cameraManager.instance.goToDeckInteractionScreen(2);
                DeckInteractionManager.instance.SelectView();
                break;
            case 3: 
                cameraManager.instance.goToMapScreen();
                break;
            case 4: // Sac
                cameraManager.instance.goToDeckInteractionScreen(4);
                DeckInteractionManager.instance.SelectView();
                break;
            case 5: // Mutate
                if(PlayerDeckHandler.instance.discardPile.Count >=3)
                { 
                cameraManager.instance.goToDraftScreen(5);
                DraftViewManager.instance.SelectView();
                }
                else
                {
                    var narators = FindObjectsOfType<Naration>();
                  var  narator = narators.Where(x => x.tag == "narratorMap").FirstOrDefault();
                    narator.LoadMutateLevelFail();
                    PlayerMovement.instance.freeze = false;
                }
                break;
            case 6: // Mycologist
                cameraManager.instance.goToDeckInteractionScreen(6);
                DeckInteractionManager.instance.SelectView();
                break;
            case 7: // B00ster
                cameraManager.instance.goToDeckInteractionScreen(7);
                DeckInteractionManager.instance.SelectView();
                break;
            case 8: // Graveyard Trade
                cameraManager.instance.goToTradeScreen(8);
                TradeViewManager.instance.SelectView();
                break;
            case 9: // Looter
                cameraManager.instance.goToDraftScreen(9);
                DraftViewManager.instance.SelectView();
                break;
            case 10: // Reverse Looter
                cameraManager.instance.goToDraftScreen(10);
                DraftViewManager.instance.SelectView();
                break;
        }
    }

}
