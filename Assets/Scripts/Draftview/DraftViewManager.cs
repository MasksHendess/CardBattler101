using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    // view
    // 1 Draft      behaviour 1
    //5 Mutate                2
    // 9 Looting              3
    // 10 reverse Looting     4
    public class DraftViewManager : MonoBehaviour
    {
        #region Buildmanager setup Singelton pattern
        // only 1 instance of BuildManager in scene that is easy to acsess
        // Dont duplicate this region 
        public static DraftViewManager instance; //self reference
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
        //View
        bool draftBtnPressed;
        public int view;
        GameObject draftBtn;
        GameObject mutateBtn;
        GameObject lootingBtn;
        GameObject reverseLootingBtn;
        private void Start()
        {
            PlayerDeckHandler = PlayerDeckHandler.instance;
            var narators = FindObjectsOfType<Naration>();
            narator = narators.Where(x => x.tag == "narratorDraftView").FirstOrDefault();


            abominationCounter = 0;
            OriginalResult = Result;
        }
        public void SelectView()
        {
            draftBtn = GameObject.Find("DraftButton");
            mutateBtn = GameObject.Find("FrankensteinButton");
            lootingBtn = GameObject.Find("lootingBtn");
            reverseLootingBtn = GameObject.Find("reverseLootingBtn");
            if (view == 1) // Draft
            {
                draftBtn.gameObject.SetActive(true);
                mutateBtn.gameObject.SetActive(false);
                lootingBtn.gameObject.SetActive(false);
                reverseLootingBtn.gameObject.SetActive(false);
            }
            else if (view == 5) // Frankensteiner
            {
                draftBtn.gameObject.SetActive(false);
                mutateBtn.gameObject.SetActive(true);
                lootingBtn.gameObject.SetActive(false);
                reverseLootingBtn.gameObject.SetActive(false);
            }
            else if (view == 9) // Looting
            {
                draftBtn.gameObject.SetActive(false);
                mutateBtn.gameObject.SetActive(false);
                lootingBtn.gameObject.SetActive(true);
                reverseLootingBtn.gameObject.SetActive(false);
            }
            else if (view == 10) //Reverse Looting
            {
                draftBtn.gameObject.SetActive(false);
                mutateBtn.gameObject.SetActive(false);
                lootingBtn.gameObject.SetActive(false);
                reverseLootingBtn.gameObject.SetActive(true);
            }
        }
        public void leaveView()
        {
            StartCoroutine(leave());
        }
        private IEnumerator leave()
        {
            if (narator != null)
                narator.resetText();

            yield return new WaitForSeconds(2f);
            LevelLoader.instance.LoadNextLevel(3);
            yield return new WaitForSeconds(1f);
            draftBtnPressed = false;
            draftBtn.gameObject.SetActive(true);
            mutateBtn.gameObject.SetActive(true);
            lootingBtn.gameObject.SetActive(true);
            reverseLootingBtn.gameObject.SetActive(true);
        }
        #region Draft
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
            if (!draftBtnPressed)
            {
                draftDeck.Clear();
                // Step1:Create 3 draft cards, Step2: Player picks 1 card and Adds it to Deck, Step3: Delete untwanted Cards. 
                // Card Script handles step 2-3 Onmousedown.
                for (int i = 0; i < DraftSlots.Length; i++)
                {
                    Card obj = Instantiate(card, DraftSlots[i].transform.position, DraftSlots[i].transform.rotation) as Card;
                    // set Cardbehaviour , handle draft, looting, reverse looting
                    if(view == 1)
                        obj.cardBehaviour = 1;
                    if (view == 9)
                        obj.cardBehaviour = 3;
                    else if (view == 10)
                        obj.cardBehaviour = 4;
                    obj.gameObject.SetActive(true);
                    PlayerDeckHandler.allInstantiatedObjects.Add(obj.gameObject);
                }
                draftBtnPressed = true;
            }
        }
        #endregion
        #region Mutate
        public void abominationNode()
        {
            if (!draftBtnPressed)
            {
                draftBtnPressed = true;
                var narators = FindObjectsOfType<Naration>();
                narator = narators.Where(x => x.tag == "narratorDraftView").FirstOrDefault();

                narator.abominationLine(0);

                //Get 3 Cards from deck
                for (int i = 0; i < DraftSlots.Length; i++)
                {
                    Card randomCard = PlayerDeckHandler.instance.getRandomCardFromGraveyard(); // discardPile[UnityEngine.Random.Range(0, PlayerDeckHandler.instance.discardPile.Count)];
                   // PlayerDeckHandler.instance.discardPile.Remove(randomCard);
                    
                    randomCard.cardBehaviour = 2;  // enables mutate card behaviour on mousedown for card 
                    randomCard.transform.position = DraftSlots[i].position;
                    // Create 3 Random Cards
                    //    Card obj = Instantiate(card, DraftSlots[i].transform.position, DraftSlots[i].transform.rotation) as Card;
                    //obj.Item = "626";
                    //    PlayerDeckHandler.instance.allInstantiatedObjects.Add(obj.gameObject);
                }

                // Create dummy to apply stats to. 
                Vector3 pos;
                pos.x = -30;
                pos.y = 0;
                pos.z = 0;
                Result = Instantiate(Result, DraftSlots[0].transform.position - pos, DraftSlots[0].transform.rotation) as Card;
                PlayerDeckHandler.instance.allInstantiatedObjects.Add(Result.gameObject);
            }
        }

        public void createGirlfriend(Card card)
        {
            // Let player pick 1 card. Values will be taken from that card and stored on Result
            //Determine what values to take from chosen card
            if (abominationCounter == 2)
            {
                // take Effect
                Result.Effect1 = card.Effect1;
                // Create Card Name
                if (CREATURESEXYNAME.text.Length > 1)
                    Result.Name = CREATURESEXYNAME.text;
                else
                    Result.Name = generateRandomName();
                // Add Result to Deck
                Result.CreatureType = "Undead";
                Result.gameObject.SetActive(false);
                PlayerDeckHandler.deck.Add(Result);
                Result = OriginalResult;
                leaveView();
            }
            else if (abominationCounter == 1)
            {
                // take CMC
                Result.CastingCost = card.CastingCost;
            }
            else if (abominationCounter == 0)
            {
                // take Attack and Defense
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
            switch (randomInt)
            {
                case 0:
                    result = "CatyMcCatFace";
                    break;
                case 1:
                    result = "GoatyMcGoatFace";
                    break;
                case 2:
                    result = "HorseyMcHorseFace";
                    break;
                case 3:
                    result = "FacyMcFaceFace";
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
        #endregion
        #region Looting & reverse Looting
        public void looting()
        {
            if(!draftBtnPressed)
            {
                draftBtnPressed = true; 
                draftDeck.Clear();
                // Create 3 cards  
                for (int i = 0; i < DraftSlots.Length; i++)
                {
                    Card obj = Instantiate(card, DraftSlots[i].transform.position, DraftSlots[i].transform.rotation) as Card;
                    if (view == 9)
                        obj.cardBehaviour = 3; // enables looting card behaviour on mousedown for card 
                    else if (view == 10)
                        obj.cardBehaviour = 4;

                    obj.gameObject.SetActive(true);
                    PlayerDeckHandler.allInstantiatedObjects.Add(obj.gameObject);
                }
                draftBtnPressed = true;
            }
        }
        #endregion
    }
}

