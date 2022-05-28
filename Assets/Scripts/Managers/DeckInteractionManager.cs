using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckInteractionManager : MonoBehaviour
{
    Card GambleCard;
    bool gameActive;
    bool boostAttack; 
    public Transform[] GambleSlots;
    private PlayerDeckHandler PlayerDeckHandler;   
    //Narator
    private Naration narator;
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
    public void leaveGambleView()
    {
        end = 0;
        foreach (var item in PlayerDeckHandler.deck)
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
    }
    int end;
    int GambleDifficulty;
    public void activatePowerupGamble()
    {
        var Gamble = FindObjectsOfType<Card>().Where(x => x.handIndex == 50).FirstOrDefault();
        PlayerDeckHandler.deck.Remove(Gamble);
        if (Gamble && end < 2)
        {
            int D20 = UnityEngine.Random.Range(1, 20);

            if (D20 < GambleDifficulty)
            {
                GameObject.Destroy(Gamble.gameObject);
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
                    card.GambleCard = true;
                    card.gameObject.SetActive(true);
                    pos.x += 7;
                }
                else
                {
                    // Handle Non instansiated Cards. Maybe just make sure everythingg is instansiated instead idk
                    //  PlayerDeckHandler.deck.Remove(card);
                    //Card New = Instantiate(card, GambleSlots[0].transform.position + pos, GambleSlots[0].transform.rotation);
                    //NewCard = true;
                    //allInstantiatedObjects.Add(New.gameObject);
                    //New.GambleCard = true;
                    //New.gameObject.SetActive(true);
                }

            }
            boostAttack = CHAOS();
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
}
