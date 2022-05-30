using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

public class PlayerDeckHandler : MonoBehaviour
{
    //public SpriteRenderer spriteRenderer;
    //public Sprite newSprite;
    //spriteRenderer.sprite = newSprite;
    #region Buildmanager setup Singelton pattern
    // only 1 instance of BuildManager in scene that is easy to acsess
    // Dont duplicate this region 
    public static PlayerDeckHandler instance; //self reference
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
    #region props
    public Card enemyPrefab;
    private Card StartingCard;
    public List<Card> deck;
    public TextMeshProUGUI deckSizeText;

    public List<Card> manaDeck;
    public TextMeshProUGUI manaDeckSizeText;

    //public List<string> draftDeck;

    //public Transform[] DraftSlots;
    public Transform[] BattlefieldSlots;
    public bool[] availableBattlefieldSlots;


    public Transform[] cardSlots;
    public bool[] availableCardSlots;

    public List<GameObject> allInstantiatedObjects;

    public List<BattlefieldNodePlayerSide> battlefieldnodes;

    public List<Card> discardPile;
    public TextMeshProUGUI discardPileSizeText;
    private Animator camAnim;

    private int manaDeckCount;
    #endregion

    public bool cardIsFromManaDeck()
    {
        if (manaDeck.Count < manaDeckCount)
        {
            manaDeckCount--;
            return true;
        }
        return false;
    }
    private void Start()
    {
        camAnim = Camera.main.GetComponent<Animator>();
        manaDeckCount = manaDeck.Count;
        StartingCard = enemyPrefab;
        for (int i = 0; i < 4; i++)
        {
            //  StartingCard.GetRandomGeneratedCreature();

            var a = Instantiate(StartingCard);
            a.getSpecificCreature(13);
            a.gameObject.SetActive(false);
            allInstantiatedObjects.Add(a.gameObject);
            deck.Add(a);
        }
    }
    private void Update()
    {
        manaDeckSizeText.text = manaDeck.Count.ToString();
        deckSizeText.text = deck.Count.ToString();
        discardPileSizeText.text = discardPile.Count.ToString();
    }
    #region MoveCardToBattlefield
    public void moveCardtoBattlefield(Card card, int i)
    {
        // Make sure to enableBattlefieldPlacement(Card card).
        availableCardSlots[card.handIndex] = true;
        card.handIndex = i;
        card.transform.position = BattlefieldSlots[i].position;
        card.hasBeenPlayed = true;
        availableBattlefieldSlots[i] = false;
        resetBattlefieldSprites();
        checkforETBEffect(card);
    }

    void checkforETBEffect(Card card)
    {
        if(card.Effect1 == "clone")
        {
            for (int i = 0; i < cardSlots.Length; i++)
            {
                if(availableBattlefieldSlots[i] == true)
                {
                    Card Clone = Instantiate(card, BattlefieldSlots[i].transform.position, BattlefieldSlots[i].transform.rotation);
                    setValuesForTokenGeneratedCard(Clone, i);
                    Clone.Name = "Dupli-Kate";
                    Clone.CreatureType = "Clone";
                    break;
                }
            }
        }
        else if(card.Effect1 =="token")
        {
            for (int i = 0; i < cardSlots.Length; i++)
            {
                if (availableBattlefieldSlots[i] == true)
                {
                    Card  token  = Instantiate(card, BattlefieldSlots[i].transform.position, BattlefieldSlots[i].transform.rotation);
                    token.getSpecificCreature(7);
                    setValuesForTokenGeneratedCard(token, i);
                    token.Name = "Grandson";
                    token.CreatureType = "Clone";
                    break;
                }
            }
        }
    }

    private void setValuesForTokenGeneratedCard(Card token, int i)
    {
        token.transform.parent = GameObject.Find("CardsCanvas").transform;
        token.gameObject.transform.localScale = new Vector3(17.3f, 17.3f, 17.3f);
        token.Enemy = false;
        token.hasBeenPlayed = true;
        token.handIndex = i;
        token.tokenCard = true;
        availableBattlefieldSlots[i] = false;
        token.gameObject.SetActive(true);
    }
    private void resetBattlefieldSprites()
    {
        //Reset sprite nodes
        foreach (var item in battlefieldnodes)
        {
            item.PlayCard = false;
            item.resetSprite();
        }
    }

    public void checkInstance(Card card)
    {
        if (!allInstantiatedObjects.Contains(card.gameObject))
        {
            // gör ny obj för randomCard duger inte för Unity, unity tycker randomCard är samma all the time.
            // Eller ja obj represnterar ett separat objekt de e d som ska hända anyway
            Card obj = Instantiate(card, cardSlots[card.handIndex].transform.position, cardSlots[card.handIndex].transform.rotation);
            allInstantiatedObjects.Add(obj.gameObject);
        }
    }
    public void enableBattlefieldPlacement(Card card)
    {
        camAnim.SetTrigger("shake");
        // Find available Nodes
        for (int i = 0; i < availableBattlefieldSlots.Length; i++)
        {
            if (availableBattlefieldSlots[i] == true)
            {
                // make node available for card placement.
                battlefieldnodes[i].makeAvailableForCardPlacement(card);
            }
        }
    }
    #endregion
    #region CardDraw&DeckInteraction
    public void DrawCard()
    {
        if (deck.Count <= 0)
        {
            Debug.Log("GAME OVER YOU LOSE");
            Time.timeScale = 0;
        }

        if (deck.Count >= 1 && checkCardsInHand() < 9)
        {
            camAnim.SetTrigger("shake");
            var nar = FindObjectOfType<Naration>();
            nar.NiceTopDeck();
            Card randomCard = deck[Random.Range(0, deck.Count)];
            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    randomCard.gameObject.SetActive(true);
                    randomCard.handIndex = i;
                    randomCard.transform.position = cardSlots[i].position;
                    randomCard.Enemy = false;
                    randomCard.hasBeenPlayed = false;
                    deck.Remove(randomCard);
                  //  Debug.Log(randomCard.gameObject);
                    if (!allInstantiatedObjects.Contains(randomCard.gameObject))
                    {
                        // gör ny obj för randomCard duger inte för Unity, unity tycker randomCard är samma all the time.
                        // Eller ja obj represnterar ett separat objekt de e d som ska hända anyway
                        Card obj = Instantiate(randomCard, cardSlots[i].transform.position, cardSlots[i].transform.rotation);
                        allInstantiatedObjects.Add(obj.gameObject);

                    }
                    availableCardSlots[i] = false;
                    //Debug.Log(allInstantiatedObjects.Count + "< - that many gameobjs");
                    //foreach (var item in allInstantiatedObjects)
                    //{
                    //    Debug.Log("item instansiated:" +item.id);
                    //}
                    return;
                }
            }
        }
    }
    public void DrawManaCard()
    {
        if (manaDeck.Count <= 0)
        {
            Debug.Log("You out of mana");
        }

        if (manaDeck.Count >= 1 && checkCardsInHand() < 9)
        {
            camAnim.SetTrigger("shake");
            Card randomCard = manaDeck[Random.Range(0, manaDeck.Count)];
            //LandCard randomCard = manaDeck[Random.Range(0, manaDeck.Count)];
            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    randomCard.gameObject.SetActive(true);
                    randomCard.handIndex = i;
                    randomCard.transform.position = cardSlots[i].transform.position;
                    randomCard.Enemy = false;
                    randomCard.hasBeenPlayed = false;
                    manaDeck.Remove(randomCard);
                    Instantiate(randomCard);
                    availableCardSlots[i] = false;
                    return;
                }
            }
        }
    }
    public void Shuffle()
    {
        if (discardPile.Count >= 1)
        {
            foreach (Card card in discardPile)
            {
                deck.Add(card);
            }
            discardPile.Clear();
        }
    }
    public void ReturnCardsToDeck()
    {
        //check bfSlots and hand for Cards
        returnCardsAtSlotsToDeck(availableCardSlots);
        returnCardsAtSlotsToDeck(availableBattlefieldSlots);
      //restore manadeck
        while(manaDeck.Count < 10)
        {
            enemyPrefab.CreatureType = "Land";
            manaDeck.Add(enemyPrefab);
            enemyPrefab.CreatureType = "";
        }
        manaDeckCount = 10;

        //Empty Graveyard
        foreach (var item in discardPile)
        {
            deck.Add(item);
        }
        discardPile.Clear();
    }
    private void returnCardsAtSlotsToDeck(bool[] Slots)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i] == false)
            {
                Card card = FindObjectsOfType<Card>().Where(x => x.handIndex == i).FirstOrDefault();
                card.handIndex = -1;
                card.hasBeenPlayed = false;

                if (card.CreatureType != "Land" && card.CreatureType != "Clone" && card.tokenCard == false)
                {
                    deck.Add(card);
                }
                else if (card.CreatureType == "Land")
                {
                    manaDeck.Add(card);
                }
                else if (card.CreatureType == "Clone" || card.tokenCard == true)
                {
                    // Clones
                    GameObject.Destroy(card.gameObject);
                }
                card.gameObject.SetActive(false);
                Slots[i] = true;
            }
        }
    }
    private int checkCardsInHand()
    {
        int result = 0;
        foreach (var item in availableCardSlots)
        {
            if (item == false)
            {
                result++;
            }
        }
        return result;
    }
    #endregion
}
