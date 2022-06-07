using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckInteractionManager : MonoBehaviour
{
    #region Buildmanager setup Singelton pattern
    // only 1 instance of BuildManager in scene that is easy to acsess
    // Dont duplicate this region 
    public static DeckInteractionManager instance; //self reference
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
    public int view;
    Card GambleCard;
    bool gameActive;
    bool boostAttack;
    public Transform[] GambleSlots;
    private PlayerDeckHandler PlayerDeckHandler;
    int end;
    int GambleDifficulty;
    //Narator
    private Naration narator;
    //Duplicate Cards
    public List<Card> duplicatesList;
    public List<string> foundItem;
    //View
    GameObject sacc;
    GameObject gamble;
    GameObject displayDeckbtn;
    GameObject booster;
    GameObject combineBtn;
    // Start is called before the first frame update
    void Start()
    {
        PlayerDeckHandler = PlayerDeckHandler.instance;
        end = 0;
        GambleDifficulty = 15;
        var narators = FindObjectsOfType<Naration>();
        narator = narators.Where(x => x.tag == "narratorDeckInteractionView").FirstOrDefault();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SelectView()
    {
        Debug.Log(view);
        sacc = GameObject.Find("SacrificeBtn");
        gamble = GameObject.Find("GambleBtn2");
        booster = GameObject.Find("BoosterBtn");
        displayDeckbtn = GameObject.Find("DisplayDeckBtn");
        combineBtn = GameObject.Find("CombineBtn");
        if (view == 2)
        {
            gamble.gameObject.SetActive(true);
            displayDeckbtn.gameObject.SetActive(true);
            sacc.gameObject.SetActive(false);
            booster.gameObject.SetActive(false);
            combineBtn.gameObject.SetActive(false);
        }
        else if (view == 4)
        {
            gamble.gameObject.SetActive(false);
            sacc.gameObject.SetActive(true);
            displayDeckbtn.gameObject.SetActive(true);
            booster.gameObject.SetActive(false); 
            combineBtn.gameObject.SetActive(false);
        }
        else if (view == 6)
        {
            gamble.gameObject.SetActive(false);
            sacc.gameObject.SetActive(false);
            displayDeckbtn.gameObject.SetActive(false);
            booster.gameObject.SetActive(false);
            combineBtn.gameObject.SetActive(true);
        }
        else if (view == 7)
        {
            gamble.gameObject.SetActive(false);
            sacc.gameObject.SetActive(false);
            displayDeckbtn.gameObject.SetActive(false);
            booster.gameObject.SetActive(true); 
            combineBtn.gameObject.SetActive(false);
        }
    }
    private IEnumerator leave()
    {
        end = 0;
        foreach (var item in PlayerDeckHandler.deck)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in CardCollectionManager.instance.Collection)
        {
            item.gameObject.SetActive(false);
        }
        if (GambleCard)
        {
            GambleCard.handIndex = -1;
            GambleCard.gameObject.SetActive(false);
            PlayerDeckHandler.deck.Add(GambleCard);
        }
        GambleDifficulty = 5;
        GambleCard = null;
        gameActive = false;
        if (narator)
            narator.resetText();
        yield return new WaitForSeconds(2f);
        LevelLoader.instance.LoadNextLevel(3);
        yield return new WaitForSeconds(1f);
        gamble.gameObject.SetActive(true);
        sacc.gameObject.SetActive(true);
        booster.gameObject.SetActive(true);
        displayDeckbtn.gameObject.SetActive(true);
        combineBtn.gameObject.SetActive(true);
    }
    public void leaveView()
    {
        StartCoroutine(leave());
    }
    public void activatePowerupGamble()
    {
        var Gamble = FindObjectsOfType<Card>().Where(x => x.handIndex == 50).FirstOrDefault();
        PlayerDeckHandler.deck.Remove(Gamble);
        if (Gamble && end < 2)
        {
            int D20 = UnityEngine.Random.Range(1, 20);

            if (D20 < GambleDifficulty)
            {
                PlayerDeckHandler.instance.addCardToGraveyard(Gamble); // discardPile.Add(Gamble);
                Gamble.gameObject.SetActive(false);
                //GameObject.Destroy(Gamble.gameObject);
                narator.DieMotherFucker();
            }
            else if (D20 >= GambleDifficulty && boostAttack == true)
            {
                Gamble.Attack += 1;
                GambleCard = Gamble;
                if (end == 0)
                {
                    narator.surviveRitual0(Gamble);
                }
            }
            else if (D20 >= GambleDifficulty && boostAttack == false)
            {
                Gamble.Defense += 1;
                if (end == 0)
                {
                    narator.surviveRitual0(Gamble);
                }
                GambleCard = Gamble;
            }
            GambleDifficulty = 17;
            end++;
        }
    }
    public void displayDeck()
    {
        var narators = FindObjectsOfType<Naration>();
        narator = narators.Where(x => x.tag == "narratorDeckInteractionView").FirstOrDefault();
        var allInstantiatedObjects = PlayerDeckHandler.allInstantiatedObjects;
        var cards = PlayerDeckHandler.deck;
        Vector3 pos;
        pos.x = 0;
        pos.y = 0;
        pos.z = 0;
        if (!gameActive && cards.Count > 0)
        {
            foreach (var card in cards)
            {
                if (allInstantiatedObjects.Contains(card.gameObject))
                {
                    card.gameObject.transform.position = GambleSlots[0].transform.position + pos;
                    card.cardBehaviour = 1;
                    card.gameObject.SetActive(true);
                    pos.x += 7;
                }
                else
                {
                    // Handle Non instansiated Cards. Maybe just make sure everythingg is instansiated instead idk
                }

            }
            if (view == 2)
            {
                boostAttack = CHAOS();
            }
            else if (view == 4)
            {
                narator.sacrifcie(0);
            }

            gameActive = true;
        }
    }
    private bool CHAOS()
    {
        var d20 = UnityEngine.Random.Range(1, 20);
        if (d20 > 15)
        {
            narator.gambleLine("Attack");
            return true;
        }
        else
        {
            narator.gambleLine("Defense");
            return false;
        }
    }
    #region Sacrifice
    public void sacrificeCard()
    {
        StartCoroutine(sacrifice());
    }
    IEnumerator sacrifice()
    {
        yield return new WaitForSeconds(1f);
        var offerdCard = FindObjectsOfType<Card>().Where(x => x.handIndex == 50).FirstOrDefault();
        if (offerdCard)
        {
            PlayerDeckHandler.deck.Remove(offerdCard);
            PlayerDeckHandler.discardPile.Add(offerdCard);
            offerdCard.gameObject.SetActive(false);
            //GameObject.Destroy(offerdCard.gameObject);
            narator.sacrifcie(1);
            yield return new WaitForSeconds(4f);
            leaveView();
        }
    }
    #endregion
    #region Booster
    public Card BoosterCard;
    public void OpenBooster()
    {
        if (!gameActive)
        {
            var cardCollection = CardCollectionManager.instance.Collection;
            gameActive = true;
            Vector3 pos;
            pos.x = 0;
            pos.y = 0;
            pos.z = 0;
            for (int i = 0; i < 5; i++)
            {
                pos.x += 7;
                BoosterCard = Instantiate(BoosterCard);
                BoosterCard.gameObject.transform.position = GambleSlots[0].transform.position + pos;
               // BoosterCard.GambleCard = true;
                BoosterCard.gameObject.SetActive(true);
                cardCollection.Add(BoosterCard);
            }
            pos.y += 10;
            for (int i = 0; i < 5; i++)
            {
                BoosterCard = Instantiate(BoosterCard);
                BoosterCard.gameObject.transform.position = GambleSlots[0].transform.position + pos;
              //  BoosterCard.GambleCard = true;
                BoosterCard.gameObject.SetActive(true);
                pos.x -= 7;
                cardCollection.Add(BoosterCard);
            }
        }
    }
    #endregion
    #region Combine Duplicates 
    public void ShowDuplicates()
    {
        if(!gameActive)
        {
            gameActive = true;
        Vector3 pos;
        pos.y = 0;
        pos.x = 0;
        pos.z = 0;

        Vector3 posy;
        posy.y = 10;
        posy.x = 0;
        posy.z = 0;
        foreach (var item in PlayerDeckHandler.instance.discardPile)
        {
            //Determine if there are 2 items with same name 
            duplicatesList = PlayerDeckHandler.instance.discardPile.Where(x => x.Name == item.Name).ToList();
            if (duplicatesList.Count > 1)
            {
                //Find item once then add it to foundItem so its skipped on the next itteraitons.
                if (!foundItem.Contains(duplicatesList[1].Name))
                {
                    //Display on screen
                    foundItem.Add(item.Name);
                    duplicatesList[0].gameObject.SetActive(true);
                    duplicatesList[0].transform.position = GambleSlots[0].transform.position + posy;
                    duplicatesList[0].cardBehaviour =2;
                    posy.x += 10;

                    duplicatesList[1].gameObject.SetActive(true);
                    duplicatesList[1].gameObject.transform.position = GambleSlots[0].transform.position + pos;
                    duplicatesList[1].cardBehaviour = 2;
                    pos.x += 10;
                }
            }
        }
        duplicatesList.Clear();
        foundItem.Clear();
        }
        // rest happens in Card On mouse down, if(CombineCard) 
    }
    public Card makeTwoCardsOneCard(Card kate, Card dupliKate)
    {
        // For some reason removing duplikate from deck dosent seem to work 
        //#really dumb workaround, At this point there should be only 2 combinecards that also have same name. (Never mind that Duplikate should be the same as wtf[1] anyway......)
        var wtf = PlayerDeckHandler.instance.discardPile.Where(x => x.Name == kate.Name && x.cardBehaviour == 2).ToList();
        PlayerDeckHandler.instance.discardPile.Remove(wtf[0]);
        PlayerDeckHandler.instance.discardPile.Remove(wtf[1]);

        //Apply stats to megaKate
        // Attack & Defense
        kate.Attack += dupliKate.Attack;
        kate.Defense += dupliKate.Defense;
        kate.Name += dupliKate.Name;
        // Cmc stays the same or maybe PAY WITH LIFE? 
       // if (kate.Effect1 != null && dupliKate.Effect1 == null)
           // kate.Effect1 = kate.Effect1;
      if (kate.Effect1 == null && dupliKate.Effect1 != null)
            kate.Effect1 = dupliKate.Effect1;
     //   else if (kate.Effect1 != null && dupliKate.Effect1 != null)
            //Card have only one effect so idk what happens here TODO:figure this out.
            return kate;
    }
    #endregion
}
