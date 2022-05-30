using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Assets.Scripts;

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

    CreatureEffects creatureEffects;

    private void Start()
    {
        gm = FindObjectOfType<PlayerDeckHandler>();
        enemygm = FindObjectOfType<EnemyDeckHandler>();
        LifeTracker = FindObjectOfType<LifeTracker>();
        creatureEffects = FindObjectOfType<CreatureEffects>();
    }
    private void Update()
    {
    }

    private void OnMouseDown()
    {
    }
    //private void additionalEffect(Card attacker, Card defender = null)
    //{
    //    if (attacker.Effect1 == "multiattack" && defender != null)
    //    {
    //        attacker.dealDamage(defender);
    //    }
    //    else if (attacker.Effect1 == "multiattack" && defender == null)
    //    {
    //        if (attacker.Enemy == true)
    //            LifeTracker.damagePlayerFace(attacker.Attack);
    //        else
    //            LifeTracker.damageEnemyFace(attacker.Attack);
    //    }
    //    else if (attacker.Effect1 == "millstrike" && attacker.Enemy == false)
    //    {
    //        EnemyDeckHandler enemy = FindObjectOfType<EnemyDeckHandler>();
    //        var deck = enemy.enemyDeck;
    //        creatureEffects.Millstrike(deck, attacker.Attack);
    //    }
    //    else if (attacker.Effect1 == "millstrike" && attacker.Enemy == true)
    //    {
    //        PlayerDeckHandler PDH = FindObjectOfType<PlayerDeckHandler>();
    //        var deck = PDH.deck;
    //        creatureEffects.Millstrike(deck, attacker.Attack);
    //    }
    //}
    private void executeAttack(bool[] availableAttackerSlots, bool[] availableDefenderSlots, bool isEnemy)
    {
        // one side attacks the other
        //Find Ocupied Battlefield Slots
        for (int i = 0; i < availableAttackerSlots.Length; i++)
        {
            if (availableAttackerSlots[i] == false)
            {
                //do attack animations or something
                playersCreature = FindObjectsOfType<Card>().Where(x => x.handIndex == i && x.hasBeenPlayed == true && x.Enemy == false).FirstOrDefault();
                // Null Ref if land card
                if (playersCreature != null)
                {
                    playersCreature.attackAnimation(false);
                }
            }
            //Both sides have creatures
            if (availableDefenderSlots[i] == false && availableAttackerSlots[i] == false)
            {
                // get creatures on Slot[i]
                playersCreature = FindObjectsOfType<Card>().Where(x => x.handIndex == i && x.hasBeenPlayed == true && x.Enemy == false).FirstOrDefault();
                enemyCreature = FindObjectsOfType<Card>().Where(x => x.handIndex == i && x.hasBeenPlayed == true && x.Enemy == true).FirstOrDefault();
                // Fight
                if (isEnemy == false)
                {
                    playersCreature.dealDamage(enemyCreature);
                    creatureEffects.checkForattackEffect(playersCreature, enemyCreature);
                }
                else
                {
                    enemyCreature.dealDamage(playersCreature);
                   creatureEffects.checkForattackEffect(enemyCreature, playersCreature);
                }
            }
            // Attacker have creature Defender Doesnt
            else if (availableDefenderSlots[i] == true && availableAttackerSlots[i] == false)
            {
                // Should yield 1 result since there is only one creature with handIndex i
                playersCreature = FindObjectsOfType<Card>().Where(x => x.handIndex == i && x.hasBeenPlayed == true).FirstOrDefault();
                if (playersCreature != null)
                {
                    if (isEnemy == false)
                    {
                        LifeTracker.damageEnemyFace(playersCreature.Attack);
                    }
                    else
                    {
                        LifeTracker.damagePlayerFace(playersCreature.Attack);
                    }
                    creatureEffects.checkForattackEffect(playersCreature, enemyCreature);
                }
            }
        }
    }
    public void triggerAttack()
    {
        availableBattlefieldSlots = gm.availableBattlefieldSlots;
        availableEnemyCardSlots = enemygm.availableEnemyCardSlots;
        // Combat as 2 step
        // Players creatures attack
        executeAttack(availableBattlefieldSlots, availableEnemyCardSlots, false);
        // enemy creatures attack
        executeAttack(availableEnemyCardSlots, availableBattlefieldSlots, true);
    }
}

