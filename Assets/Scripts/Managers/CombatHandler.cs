using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class CombatHandler : MonoBehaviour
{
    PlayerDeckHandler gm;
    EnemyDeckHandler enemygm;
    public Transform[] BattlefieldSlots;
    public bool[] availableBattlefieldSlots;

    public Transform[] EnemySlots;
    public bool[] availableEnemyCardSlots;

    LifeTracker LifeTracker;

    Card playersCreature;
    Card enemyCreature;

    private void Start()
    {
        gm = FindObjectOfType<PlayerDeckHandler>();
        enemygm = FindObjectOfType<EnemyDeckHandler>();
        LifeTracker = FindObjectOfType<LifeTracker>();
    }
    private void Update()
    {
    }

    private void OnMouseDown()
    {
    }
    private void additionalEffect(Card attacker, Card defender)
    {
        if (attacker.Effect1 == "multiattack")
        {
            attacker.dealDamage(defender);
        }
        else if( attacker.Effect1 =="millstrike" && attacker.Enemy == false)
        {
            EnemyDeckHandler enemy = FindObjectOfType<EnemyDeckHandler>();
            var deck = enemy.enemyDeck;
            attacker.Millstrike(deck,attacker.Attack);
        }
        else if (attacker.Effect1 == "millstrike" && attacker.Enemy == true)
        {
            PlayerDeckHandler PDH = FindObjectOfType<PlayerDeckHandler>();
            var deck = PDH.deck;
            attacker.Millstrike(deck, attacker.Attack);
        }
    }
    public void attackStep()
    {
        // Combat as 2 step
        // Players creatures attack
        // enemy creatures attack
        availableBattlefieldSlots = gm.availableBattlefieldSlots;
        availableEnemyCardSlots = enemygm.availableEnemyCardSlots;

        // Player Creatures Attack 
        //Find Ocupied Battlefield Slots
        for (int i = 0; i < availableBattlefieldSlots.Length; i++)
        {
            if (availableBattlefieldSlots[i] == false)
            {
                //do attack animations or something
                 playersCreature = FindObjectsOfType<Card>().Where(x => x.handIndex == i && x.hasBeenPlayed == true && x.Enemy == false).FirstOrDefault();
                // Null Ref if land card
                if (playersCreature != null)
                {
                    playersCreature.attackAnimation(false);
                }
            }
            //Both player and enemy have creatures
            if (availableEnemyCardSlots[i] == false && availableBattlefieldSlots[i] == false)
            {
                 playersCreature = FindObjectsOfType<Card>().Where(x => x.handIndex == i && x.hasBeenPlayed == true && x.Enemy == false).FirstOrDefault();
                 enemyCreature = FindObjectsOfType<Card>().Where(x => x.handIndex == i && x.hasBeenPlayed == true && x.Enemy == true).FirstOrDefault();
                // Fight
                playersCreature.dealDamage(enemyCreature);
                additionalEffect(playersCreature, enemyCreature);
            }
            // Player have creature Enemy Dont
            else if (availableEnemyCardSlots[i] == true && availableBattlefieldSlots[i] == false)
            {
                // Should yield 1 result ( Players Creature ) since there is no opposing creature.
                playersCreature = FindObjectsOfType<Card>().Where(x => x.handIndex == i && x.hasBeenPlayed == true).FirstOrDefault();
                if (playersCreature != null)
                {
                    // Null ref if land card. eh lets keep it even if landcards is not a thing anymore
                    LifeTracker.damageEnemyFace(playersCreature.Attack);
                }
                if(playersCreature.Effect1 == "multiattack")
                {
                    LifeTracker.damageEnemyFace(playersCreature.Attack);
                }
            }
        }
        //------------------------
        // Enemy Creatures Attack
        for (int i = 0; i < availableEnemyCardSlots.Length; i++)
        {
            if (availableBattlefieldSlots[i] == false)
            {
                //do attack animations or something
                 playersCreature = FindObjectsOfType<Card>().Where(x => x.handIndex == i && x.hasBeenPlayed == true && x.Enemy ==true).FirstOrDefault();
                // Null Ref if land card
                if (playersCreature != null)
                {
                    playersCreature.attackAnimation(true);
                }
            }
            //Both player and enemy have creatures
            if (availableEnemyCardSlots[i] == false && availableBattlefieldSlots[i] == false)
            {
                playersCreature = FindObjectsOfType<Card>().Where(x => x.handIndex == i && x.hasBeenPlayed == true && x.Enemy == false).FirstOrDefault();
                 enemyCreature = FindObjectsOfType<Card>().Where(x => x.handIndex == i && x.hasBeenPlayed == true && x.Enemy == true).FirstOrDefault();
                // Fight
                enemyCreature.dealDamage(playersCreature);
                if (enemyCreature.Effect1 == "multiattack")
                {
                    enemyCreature.dealDamage(enemyCreature);
                }
                //  playersCreature.takeDamage(enemyCreature);
            }
            //// Enemy has creature player Dont
           else if (availableEnemyCardSlots[i] == false && availableBattlefieldSlots[i] == true)
            {
                // Should yield 1 result ( Players Creature ) since there is no opposing creature.
                playersCreature = FindObjectsOfType<Card>().Where(x => x.handIndex == i && x.hasBeenPlayed == true).FirstOrDefault();
                if (playersCreature != null)
                {
                    LifeTracker.damagePlayerFace(playersCreature.Attack);
                    if (playersCreature.Effect1 == "multiattack")
                    {
                        LifeTracker.damageEnemyFace(playersCreature.Attack);
                    }
                }
            }
        }
       
    }
}

