using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.TradeView
{
    public class TradeViewManager : MonoBehaviour
    {
        #region Buildmanager setup Singelton pattern
        // only 1 instance of BuildManager in scene that is easy to acsess
        // Dont duplicate this region 
        public static TradeViewManager instance; //self reference
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
        public List<Card> graveyard;
        public Transform[] spawnPositions;
        public  int view;

        private void Start()
        {
        }
        public void SelectView()
        {

        }
        #region TradeGYcard
        public void displayGraveyard()
        {
            StartCoroutine(graveyardNode());
        }
        IEnumerator graveyardNode()
        {
            graveyard = PlayerDeckHandler.instance.discardPile;
            var narators = FindObjectsOfType<Naration>();
            var narator = narators.Where(x => x.tag == "narratorTrade").FirstOrDefault();
            showList(graveyard, 0,3);
            narator.Graveyard(0);
            yield return new WaitForSeconds(3f);
            showList(PlayerDeckHandler.instance.deck, 1, 2);
            narator.Graveyard(1);
        }
        private void showList(List<Card> list, int index, int behaviour)
        {
            Vector3 pos;
            pos.x = 0;
            pos.y = 0;
            pos.z = 0;
            foreach (Card card in list)
            {
                if (PlayerDeckHandler.instance.allInstantiatedObjects.Contains(card.gameObject))
                {
                    card.gameObject.transform.position = spawnPositions[index].transform.position + pos;
                    card.cardBehaviour = behaviour;
                    card.gameObject.SetActive(true);
                    pos.x += 7;
                }
            }
        }
        IEnumerator raiseDead(List<GameObject> graveyardCard)
        {
           Card deadCard = graveyardCard[0].gameObject.GetComponent<Card>();
           Card aliveCard = graveyardCard[1].gameObject.GetComponent<Card>();

            //Assign cards correctly regardless of what order player selected them
            foreach (var item in graveyardCard)
            {
                Card behaviorRefrence = item.gameObject.GetComponent <Card>() ;
                if (behaviorRefrence.cardBehaviour == 3)
                   deadCard = item.gameObject.GetComponent<Card>();
                else if( behaviorRefrence.cardBehaviour == 2) 
                    aliveCard = item.gameObject.GetComponent<Card>();
            }
             
            
            var narators = FindObjectsOfType<Naration>();
            var narator = narators.Where(x => x.tag == "narratorTrade").FirstOrDefault();

            deadCard.gameObject.tag= "Untagged";
            aliveCard.gameObject.tag= "Untagged";
            PlayerDeckHandler.instance.removeCardFromDeck(aliveCard); // deck.Remove(aliveCard);
            PlayerDeckHandler.instance.removeCardFromGraveyard(deadCard);// discardPile.Remove(deadCard);
            yield return new WaitForSeconds(1f);
            deadCard.gameObject.transform.position = spawnPositions[2].transform.position;
            aliveCard.gameObject.transform.position = spawnPositions[3].transform.position;
            yield return new WaitForSeconds(1f);
            PlayerDeckHandler.instance.addCardToGraveyard(aliveCard);// discardPile.Add(aliveCard);
            deadCard.CreatureType = "Undead";
            narator.Graveyard(2);
            yield return new WaitForSeconds(3f);
            PlayerDeckHandler.instance.addCardToDeck(deadCard);// deck.Add(deadCard);
            LevelLoader.instance.LoadNextLevel(3);
            graveyardCard.Clear();
        }
        public void theThing(Card card, int cardBehaviour)
        {
            card.transform.position = TradeViewManager.instance.spawnPositions[cardBehaviour].transform.position;
            card.gameObject.tag = "GraveyardCard";
            List<GameObject> graveyardCard = GameObject.FindGameObjectsWithTag("GraveyardCard").ToList();
            if(graveyardCard.Count == 2)
            {
                StartCoroutine(raiseDead(graveyardCard));
            }
        }
        #endregion
    }
}
