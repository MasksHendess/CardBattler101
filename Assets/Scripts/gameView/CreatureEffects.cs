using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CreatureEffects : MonoBehaviour
    {
        PlayerDeckHandler deckHandler;
         Transform[] BattlefieldSlots;
         bool[] availableBattlefieldSlots;


         Transform[] cardSlots;
         bool[] availableCardSlots;
       // LifeTracker LifeTracker;

        private void Start()
        {
            deckHandler = FindObjectOfType<PlayerDeckHandler>();
            //LifeTracker = FindObjectOfType<LifeTracker>();
        }

        private void applyCardSlots()
        {
            cardSlots = deckHandler.cardSlots;
            availableCardSlots = deckHandler.availableCardSlots;
        }
        private void applyBattlefieldSlots()
        {
            BattlefieldSlots = deckHandler.BattlefieldSlots;
            availableBattlefieldSlots = deckHandler.availableBattlefieldSlots;
        }
     
        // Kill Trigger
        public void applyOnKillTriggers(Card card)
        {
            applyBattlefieldSlots();
            applyCardSlots();
            if (card.Effect1 == "bloodthirst")
                bloodthirstTrigger(card);
        }   
        //Attack Trigger+---
        public void checkForattackEffect(Card Attacker, Card Defender)
        {
            applyBattlefieldSlots();
            applyCardSlots();
            //Defender effects
            if(Defender)
            { 
            if (Defender.Effect1 == "unsummon")
            {
                unsummon(Attacker);
            }
            else if (Defender.Effect1 == "porcupine")
            {
                porcupine(Attacker);
            }
            }
            //attacker Effects
            if (Attacker.Effect1 == "poison" && Attacker.Attack >= 1 && Defender)
            {
                poison(Defender);
            }
            else if (Attacker.Effect1 == "multiattack" && Defender != null)
            {
                multiattack(Attacker, Defender);
            }
            else if (Attacker.Effect1 == "multiattack" && Defender == null)
            {
                multiattack(Attacker);
            }
            else if (Attacker.Effect1 == "millstrike" && Attacker.Enemy == false)
            {
                EnemyDeckHandler enemy = FindObjectOfType<EnemyDeckHandler>();
                var deck = enemy.enemyDeck;
                Millstrike(deck, Attacker.Attack);
            }
            else if (Attacker.Effect1 == "millstrike" && Attacker.Enemy == true)
            {
                PlayerDeckHandler PDH = FindObjectOfType<PlayerDeckHandler>();
                var deck = PDH.deck;
                Millstrike(deck, Attacker.Attack);
            }
        }
        // Creature Effects
        void bloodthirstTrigger(Card card)
        {
                card.Attack += 1;
        }
        void unsummon(Card card)
        { // Attacker go to hand
            if (card.Enemy == true)
            {
                // go to enemy hand
            }
            else
            {
                bool positionFound = false;
                for (int i = 0; i < availableCardSlots.Length; i++)
                {
                    if(availableCardSlots[i] == true)
                    {
                        card.transform.position = cardSlots[i].transform.position;
                        break;
                    }
                }
                if(!positionFound)
                {
                    //Do nothing look happi :3
                }
                else
                {
                    card.hasBeenPlayed = false;
                    availableBattlefieldSlots[card.handIndex] = true;
                }
            }
        }
        void porcupine(Card Attacker)
        {
                // I Hate porcupines - Poe Melee Player
                Attacker.Defense -= 1;
        }
        void poison(Card Defender)
        {
                Defender.Defense = 0;
        }
        void multiattack(Card attacker, Card defender = null)
        {
            if (defender != null)
                attacker.dealDamage(defender);
            else if (defender == null && attacker.Enemy == false)
                CombatHandler.instance.dealDamage(attacker.Attack, "player");
            else if (defender == null && attacker.Enemy == true)
                CombatHandler.instance.dealDamage(attacker.Attack, "enemy");
        }
        public void Millstrike(List<Card> deck, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var rng = UnityEngine.Random.Range(0, deck.Count);
                deck.Remove(deck[rng]);
            }
        }
    }
}
