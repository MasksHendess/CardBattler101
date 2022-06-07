using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class CardCollectionManager : MonoBehaviour
    {
        #region Buildmanager setup Singelton pattern
        // only 1 instance of BuildManager in scene that is easy to acsess
        // Dont duplicate this region 
        public static CardCollectionManager instance; //self reference
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
        public List<Card> Collection;
        #region Collection
        public void displayCollection()
        {
            foreach (var item in Collection)
            {
                Debug.Log(item.Name);
            }
        }
        public void addCardToCollection(Card card)
        {
            Collection.Add(card);
        }

        public void deleteCardFromCollection(Card card)
        {
            Collection.Remove(card);
        }

        public void POSTCard(Card card, string name)
        {
            var obj = Collection.FirstOrDefault(x => x.Name == name);
            if (obj != null) obj = card;
        }
        #endregion
        #region Collection & Deck
        public void addToDeckFromCollection(Card card)
        {
            Collection.Remove(card);
            PlayerDeckHandler.instance.deck.Add(card);
        }
        public void addToCollectionFromDeck(Card card)
        {
            PlayerDeckHandler.instance.deck.Remove(card);
            Collection.Add(card);
        }
        #endregion
    }
}
