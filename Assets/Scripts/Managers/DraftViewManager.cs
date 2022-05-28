using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class DraftViewManager : MonoBehaviour
    {
        // Draft
        public List<string> draftDeck;
        public Card card;
        public Transform[] DraftSlots;
        private PlayerDeckHandler PlayerDeckHandler;
        // Abomination
        public Card Result;
        private Card OriginalResult;
        public TextMeshProUGUI CREATURESEXYNAME;
        int abominationCounter;
        //Narator
        private Naration narator;
        private void Start()
        {
            PlayerDeckHandler = PlayerDeckHandler.instance;
            var narators= FindObjectsOfType<Naration>();
            narator = narators.Where(x => x.tag == "narratorDraftView").FirstOrDefault();


            abominationCounter = 0;
            OriginalResult = Result;
        }
        public bool draftCardIsUnique()
        {
            if (draftDeck.Count() != draftDeck.Distinct().Count())
            {
                Debug.Log("DUPLICATE DETECTED");
                return false;
            }
            else
                return true;
        }
        public void DraftCard()
        {
            draftDeck.Clear();
            // Step1:Create 3 draft cards, Step2: Player picks 1 card and Adds it to Deck, Step3: Delete untwanted Cards. 
            // Card Script handles step 2-3 Onmousedown.
            for (int i = 0; i < DraftSlots.Length; i++)
            {
                card.draftCard = true;
                Card obj = Instantiate(card, DraftSlots[i].transform.position , DraftSlots[i].transform.rotation) as Card;
                PlayerDeckHandler.allInstantiatedObjects.Add(obj.gameObject);
            }
            //reset for next time
            card.draftCard = false;
            //}
        }
        public void abominationNode()
        {
            //Create 3 Cards
            // Let player pick 1 card
            // Store 1 cards Power and Defense
            // Let player pick 2nd card
            // Store 2nd card CMC
            //Let player pick 3d card
            // Store 3d card ability? dont have that yeeeeeet
            narator.abominationLine(0);
            for (int i = 0; i < DraftSlots.Length; i++)
            {
                    Card obj = Instantiate(card, DraftSlots[i].transform.position, DraftSlots[i].transform.rotation) as Card;
                obj.Item = "626";
                    PlayerDeckHandler.instance.allInstantiatedObjects.Add(obj.gameObject);

            }
            Vector3 pos;
            pos.x = -30;
            pos.y = 0;
            pos.z = 0;
            Result = Instantiate(Result, DraftSlots[0].transform.position - pos, DraftSlots[0].transform.rotation) as Card;
            PlayerDeckHandler.instance.allInstantiatedObjects.Add(Result.gameObject); 
        }

        public void createGirlfriend(Card card)
        {
            if(abominationCounter == 2)
            {
                Result.Effect1 = card.Effect1;
                if (CREATURESEXYNAME.text.Length > 1)
                    Result.Name = CREATURESEXYNAME.text;
                else
                    Result.Name = generateRandomName();
                Result.gameObject.SetActive(false);
                PlayerDeckHandler.deck.Add(Result);
                Result = OriginalResult;
            }else if(abominationCounter == 1)
            {
                Result.CastingCost = card.CastingCost;
            }
            else if(abominationCounter == 0)
            {
                Result.Attack = card.Attack;
                Result.Defense = card.Defense;
            }
            GameObject.Destroy(card.gameObject); 
            abominationCounter++; 
          
            narator.abominationLine(abominationCounter);
            if (abominationCounter == 3)
            {
                abominationCounter = 0;
            }
        }

        string generateRandomName()
        {
            var randomInt = UnityEngine.Random.Range(0, 10);
            string result = "";
            switch(randomInt)
            {
                case 0:
                    result = "CatyMcCatFace";
                    break;
                case 1:
                    result =  "GoatyMcGoatFace";
                    break;
                case 2:
                    result = "HorseyMcHorseFace";
                    break;
                case 3:
                    result =  "FacyMcFaceFace";
                    break;
                case 4:
                    result = "MonkeyMcMonkface";
                    break;
                case 5:
                    result = "OrchyMcOrcface";
                    break;
                case 6:
                    result = "YaceyMcYaceFace";
                    break;
                case 7:
                    result = "CardyMcCardFace";
                    break;
                case 8:
                    result = "StorkyMcStorkFace";
                    break;
                case 9:
                    result = "DoggieMcDogeFace";
                    break;
                case 10:
                    result = "ScrubyMcScrubFace";
                    break;
            }
            return result;
    }
    }
}

