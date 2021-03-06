using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDeckHandler : MonoBehaviour
{
    #region Buildmanager setup Singelton pattern
    // only 1 instance of BuildManager in scene that is easy to acsess
    // Dont duplicate this region 
    public static EnemyDeckHandler instance; //self reference
    private void Awake()
    {
        //check if instance already exisist
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene");
            return;
        }

        instance = this;
    }
    #endregion
    public Card enemyPrefab;

    public List<Card> enemyDeck; 
    public TextMeshProUGUI deckSizeText;
    // public TextMeshProUGUI enemyDeckSizeText;

    public Transform[] EnemySlots;
    public bool[] availableEnemyCardSlots;
    // Start is called before the first frame update
    void Start()
    {
        addEnemiesToDeck();
    }

    // Update is called once per frame
    void Update()
    {
        deckSizeText.text = enemyDeck.Count.ToString();
    }

    public void addEnemiesToDeck()
    {
        for (int i = 0; i < 10; i++)
        {
            enemyDeck.Add(enemyPrefab);
        }
    }
    public void DrawCardEnemy()
    {
        if (enemyDeck.Count > 0)
        {
            for (int i = 0; i < availableEnemyCardSlots.Length; i++)
            {
                Card randomCard = enemyDeck[Random.Range(0, enemyDeck.Count)];
                if (availableEnemyCardSlots[i] == true)
                {
                    randomCard.gameObject.SetActive(true);
                    randomCard.handIndex = i;
                    randomCard.transform.position = EnemySlots[i].position;
                    randomCard.Enemy = true;
                    randomCard.hasBeenPlayed = true; // prevent player from attacking with enemies
                    enemyDeck.Remove(randomCard);
                    Instantiate(randomCard);
                    availableEnemyCardSlots[i] = false;
                    return;
                }
            }
        }
        else
        {
        }
    }
}
