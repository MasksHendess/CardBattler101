using Assets.Scripts;
using Assets.Scripts.Managers;
using Assets.Scripts.TradeView;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    #region props
    PlayerDeckHandler gm;
    DraftViewManager DraftHandler;
    //Text
    public TextMeshPro AttackDefenseText;
    public TextMeshPro NameCMCText;
    //Sprites
    public SpriteRenderer spriteRenderer;
    SpriteManager spriteMan;
    // Props
    public string Name;
    public int CastingCost;
    public string CreatureType;
    public string Item;
    // Text box
    public string Effect1;
    CreatureEffects effects;
    //Power Toughness
    public int Attack;
    public int Defense;

    private int startingAttack;
    private int startingDefense;

    //-- Meta shit
    public int cardBehaviour; // Assign a behavior to Card depending on where the Card is instansiated & what canvas is active 
    public bool Enemy;
    public bool tokenCard;
    public bool hasBeenPlayed;
    public int handIndex;

    #endregion 
    private void Start()
    {
        gm = FindObjectOfType<PlayerDeckHandler>();
        DraftHandler = FindObjectOfType<DraftViewManager>();
        spriteMan = FindObjectOfType<SpriteManager>();
        effects = FindObjectOfType<CreatureEffects>();
        //Determine if Land or Creature
        var isLandCard = gm.cardIsFromManaDeck();
        if (isLandCard == true)
        {
            createCard(spriteMan.getCardSprites(3), "LandShark", 0, 1, 0, "Land", "Add 1 Mana");
        }
        else if (tokenCard == true)
        {
            //Handles Token generated cards. ( This Check is here so that else statement won't trigger and override the token creature)
        }
        else
        {
            generateRandomCreatureCard(Random.Range(0, 14));
            // getSpecificCreature(13);
        }
        //  Effect1="clone";
        //Keep track of starting values
        startingAttack = Attack;
        startingDefense = Defense;
        // Handles Draft Cards
        if (cameraManager.instance.DraftViewCanvas.enabled && cardBehaviour == 1)
        {
            //Generate unique Draft Cards.
            DraftHandler.draftDeck.Add(this.Name);
            bool draftCardIsUnique = DraftHandler.draftCardIsUnique();
            //Reroll card values if duplicate.
            while (draftCardIsUnique == false)
            {
                DraftHandler.draftDeck.Remove(this.Name);
                generateRandomCreatureCard(Random.Range(0, 14));
                draftCardIsUnique = DraftHandler.draftCardIsUnique();
            }

            if (DraftHandler.draftDeck.Count > 3)
                DraftHandler.draftDeck.Clear();
        }
        //Apply new Card to Canvas
        var canvas = GameObject.FindGameObjectsWithTag("mainCanvas").FirstOrDefault();
        GameObject canvasGameObject = canvas.gameObject;
        //Make the new GameObject child of the Canvas
        this.transform.parent = canvasGameObject.transform;
    }
    private void Update()
    {
        AttackDefenseText.text = "A: " + Attack.ToString() + " D: " + Defense.ToString();
        NameCMCText.text = Name + "   CMC:" + CastingCost;
    }
    private void OnMouseDown()
    {
        #region GameView
        if (cameraManager.instance.GameViewCanvas.enabled)
            moveToBattleField();
        #endregion
        #region draftview 
        // Handles Reverse Looting
        else if (cameraManager.instance.DraftViewCanvas.enabled && cardBehaviour == 4)
        {
            cardBehaviour = 0;
            MoveToDiscardPile();  //Step2: Move picked card (this) to GY.
            putUnwantedCardsInDeck();// rest goes into Decku.
            DraftViewManager.instance.leaveView();
        }
        // Handles Looting
        else if (cameraManager.instance.DraftViewCanvas.enabled && cardBehaviour == 3)
        {
            cardBehaviour = 0;
            moveToDeck();  //Step2: Move picked card (this) to deck.
            putUnwantedCardsInGraveyard();// rest goes into graveyard.
            DraftViewManager.instance.leaveView();
        }
        //Handles Mutate Cards
        else if (cameraManager.instance.DraftViewCanvas.enabled && cardBehaviour == 2)//(MutateCard)
        {
            DraftHandler.createGirlfriend(this);
        }
        //Handles DraftedCards
        else if (cameraManager.instance.DraftViewCanvas.enabled && cardBehaviour == 1)//(draftCard)
        {
            // Step 1: handled by PlayerDeckHandler DraftCard()
            //Step 2: Add To Deck
            cardBehaviour = 0;
            moveToDeck();
            //Step 3: Delete unwanted Cards
            removeUnwantedCards();
            DraftViewManager.instance.leaveView();
        }
        #endregion
        #region deckinteractionview  
        // Handle GambleCard
        else if (cameraManager.instance.DeckInteractionViewCanvas.enabled && cardBehaviour == 1)//(GambleCard)
        {
            gameObject.transform.position = DeckInteractionManager.instance.GambleSlots[1].transform.position;
            handIndex = 50;
            var list = GameObject.FindObjectsOfType<Card>();
            foreach (var item in list)
            {
                if (cardBehaviour == 1)
                {
                    cardBehaviour = 0;
                }
            }
        }
        //Handle Combine Card
        else if (cameraManager.instance.DeckInteractionViewCanvas.enabled && cardBehaviour == 2)
        {
            var card = GameObject.FindObjectsOfType<Card>().Where(x => x.Name == this.Name && x.cardBehaviour == 2).ToList();
            //make this card this card + card1
            DeckInteractionManager.instance.makeTwoCardsOneCard(this, card[1]);
            cardBehaviour = 0;
            moveToDeck();
            var list = GameObject.FindObjectsOfType<Card>().Where(x => x.cardBehaviour == 2).ToList();
            foreach (var item in list)
            {
                item.cardBehaviour = 0;
                item.gameObject.SetActive(false);

            }
        }
        // Dont need to handle Sacrifice node because it should execute the same behaviour as a gamblecard untill its button interaction  
        #endregion
        #region Tradeview
        else if (cameraManager.instance.TradeViewCanvas.enabled && cardBehaviour == 3 ||cameraManager.instance.TradeViewCanvas.enabled && cardBehaviour == 2)
        {
            TradeViewManager.instance.theThing(this, cardBehaviour);
            var collections = GameObject.FindObjectsOfType<Card>();
            var collection = collections.Where(x => x.cardBehaviour == cardBehaviour);
            foreach (var item in collection)
            {
                if (item != this)
                    item.gameObject.SetActive(false);
            }
        }
        #endregion
    }
    private void putUnwantedCardsInDeck()
    {
        var list = GameObject.FindObjectsOfType<Card>();
        foreach (var item in list)
        {
            if (item.cardBehaviour != 0)
            {
                PlayerDeckHandler.instance.addCardToDeck(item);
            }
        }
    }
    private void putUnwantedCardsInGraveyard()
    {
        var list = GameObject.FindObjectsOfType<Card>();
        foreach (var item in list)
        {
            if (item.cardBehaviour != 0)
            {
                PlayerDeckHandler.instance.addCardToGraveyard(item);
            }
        }
    }
    private void removeUnwantedCards()
    {
        var list = GameObject.FindObjectsOfType<Card>();
        foreach (var item in list)
        {
            if (item.cardBehaviour != 0)
            {
                GameObject.Destroy(item.gameObject);
            }
        }
    }
    #region CombatBehaviour
    public void attackAnimation(bool enemy)
    {
        if (!enemy)
        {
            Up();
            Invoke("Back", 1f);
        }
        else if (enemy)
        {
            Back();
            Invoke("Up", 1f);
        }
    }
    private void Up()
    {
        transform.position += Vector3.up * 3f;
    }
    private void Back()
    {
        transform.position += Vector3.down * 3f;
    }
    public void dealDamage(Card target)
    {
        target.Defense -= Attack;

        effects.checkForattackEffect(this, target);
        if (target.Defense <= 0)
        {
            if (target.Enemy == true)
            {
                killEnemyTarget(target);
            }
            else if (target.Enemy == false)
            {
                KillPlayerTarget(target);
            }
            effects.applyOnKillTriggers(this);
        }
    }
    private void killEnemyTarget(Card target)
    {
        var enemy = FindObjectOfType<EnemyDeckHandler>();
        enemy.availableEnemyCardSlots[handIndex] = true;
        GameObject.Destroy(target.gameObject);
    }
    #endregion
    #region MoveToNewPlace
    private void moveToDeck()
    {
        //Instantiate(effect, transform.position, Quaternion.identity);
        gm.deck.Add(this);
        gameObject.SetActive(false);
    }
    private void KillPlayerTarget(Card target)
    {
        gm.availableBattlefieldSlots[target.handIndex] = true;
        //Instantiate(effect, transform.position, Quaternion.identity);
        target.Attack = target.startingAttack;
        target.Defense = target.startingDefense;
        gm.discardPile.Add(target);
        target.gameObject.SetActive(false);
    }

    private void moveToBattleField()
    {
        // Handles Cards in hand
        if (!hasBeenPlayed && CombatHandler.instance.gameState == gameState.PLAYERTURN)
        {
            //Find Land Cards on Battlefield
            List<Card> availableManaz = GameObject.FindObjectsOfType<Card>().ToList();
            availableManaz = availableManaz.Where(x => x.handIndex > -1 && x.hasBeenPlayed == true && x.CreatureType == "Land").ToList();

            // Check if player has enough mana to cast spell
            if (CastingCost <= availableManaz.Count)
            {
                for (int i = 0; i < CastingCost; i++)
                {
                    // Delete Manas equal to Casting Cost
                    //Todo maybe let player chose which manas dies? RN it just deletes from 0 to CastingCost
                    gm.availableBattlefieldSlots[availableManaz[i].handIndex] = true;
                    availableManaz[i].gameObject.SetActive(false);
                    GameObject.Destroy(availableManaz[i].gameObject);
                }
                // Enable Card to be put on the Battlefield (Player choses which lane this will ocupy)
                gm.enableBattlefieldPlacement(this);
            }
        }
    }
    private void MoveToDiscardPile()
    {
        gm.availableBattlefieldSlots[handIndex] = true;
        //Instantiate(effect, transform.position, Quaternion.identity);
        this.Attack = startingAttack;
        this.Defense = startingDefense;
        gm.addCardToGraveyard(this);
    }
    #endregion
    #region CreateCreature
    public void GetRandomGeneratedCreature()
    {
        generateRandomCreatureCard(Random.Range(0, 14));
    }

    public void getSpecificCreature(int i)
    {
        generateRandomCreatureCard(i);
    }
    private void generateRandomCreatureCard(int randomnumber)
    {
        // Gör Hosts och Augments
        // Hosts: creatures som vill ha abillities  2 be stronk
        // Augments: Creatures som har abillites men kassa stats
        if (spriteMan == null)
        {
            spriteMan = FindObjectOfType<SpriteManager>();
        }
        switch (randomnumber)
        {
            case 0:
                createCard(spriteMan.getCardSprites(5), "Gargaton", 7, 7, 5, "Big dumb dumb");
                break;
            case 1:
                createCard(spriteMan.getCardSprites(4), "Big Bertha", 4, 4, 3, "Your Ex");
                break;
            case 2:
                createCard(spriteMan.getCardSprites(6), "Valkyrie", 3, 2, 2, "Bear"); //Host
                break;
            case 3:
                createCard(spriteMan.getCardSprites(7), "Hellborn", 1, 1, 0, "Devil");
                break;
            case 4:
                createCard(spriteMan.getCardSprites(9), "Adder", 1, 1, 2, "Snake", "poison"); // Deathtouch, augment
                break;
            case 5:
                createCard(spriteMan.getCardSprites(3), "Bacon", 1, 6, 2, "Mutated-Pig");
                break;
            case 6:
                createCard(spriteMan.getCardSprites(0), "Hellhound", 2, 1, 2, "Dog");
                break;
            case 7:
                createCard(spriteMan.getCardSprites(2), "Draugr", 1, 1, 1, "Undead");
                break;
            case 8:
                createCard(spriteMan.getCardSprites(1), "Stitchling", 3, 1, 2, "Zombee");
                break;
            case 9:
                createCard(spriteMan.getCardSprites(8), "Ice Spider", 1, 4, 2, "Gigant Spider", "ice");// on attack apply frozen solid on opponent preventing it from attacking for one turn
                break;
            case 10:
                createCard(spriteMan.getCardSprites(7), "Piplup", 1, 1, 1, "Poggermon", "millstrike");
                break;
            case 11:
                createCard(spriteMan.getCardSprites(6), "Kate", 1, 1, 2, "Poggermon", "clone"); // ETB: create a token copy of itself on first available slot
                break;
            case 12:
                createCard(spriteMan.getCardSprites(5), "DireWolf", 2, 3, 4, "Wolf", "multiattack");
                break;
            case 13:
                createCard(spriteMan.getCardSprites(4), "Gravedigger", 1, 1, 2, "SmollDog", "token"); //ETB: Create a 1/1 token on first avialable slot
                break;
            case 14:
                createCard(spriteMan.getCardSprites(3), "Changling", 1, 1, 2, "Changling", "random"); //
                break;
            case 15:
                createCard(spriteMan.getCardSprites(2), "Vampire Spawn", 1, 2, 2, "Vampire", "bloodthirst"); //+1ATTK On kill
                break;
            default:
                createCard(spriteMan.getCardSprites(1), "ShitstickNicksdick", 4, 4, 4, "Elephant", "Trample");
                break;
        }
    }
    private void createCard(Sprite sprite, string name, int attack, int defense, int castingcost, string creaturetype, string effect = "")
    {
        // Card card = new Card();
        spriteRenderer.sprite = sprite;
        Attack = attack;
        Defense = defense;
        CastingCost = castingcost;
        Name = name;
        CreatureType = creaturetype;
        //  Trigger1 = trigger;
        Effect1 = effect;
    }
    #endregion
}