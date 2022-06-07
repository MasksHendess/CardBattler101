using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Assets.Scripts;
using UnityEngine.UI;
using Assets.Scripts.Managers;

//public enum gameState { START,DRAW, PLAYERTURN, ENEMYTURN, WON, LOST }
public class CombatHandler : MonoBehaviour
{
    #region Buildmanager setup Singelton pattern
    // only 1 instance of BuildManager in scene that is easy to acsess
    // Dont duplicate this region 
    public static CombatHandler instance; //self reference
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
    public gameState gameState;
    PlayerDeckHandler gm;
    EnemyDeckHandler enemygm;
    public Transform[] BattlefieldSlots;
    public bool[] availableBattlefieldSlots;

    public Transform[] EnemySlots;
    public bool[] availableEnemyCardSlots;

    //Life Tracker
    public int damageDelt = 0;
    public Text lifeCountText;
    public Slider slider;

    Card playersCreature;
    Card enemyCreature;

    CreatureEffects creatureEffects;

    private void Start()
    { 
        gameState = gameState.START;
        gm = FindObjectOfType<PlayerDeckHandler>();
        enemygm = FindObjectOfType<EnemyDeckHandler>();
      //  LifeTracker = FindObjectOfType<LifeTracker>();
        creatureEffects = FindObjectOfType<CreatureEffects>();
    }
    void Update()
    {
        lifeCountText.text = "Life: " + damageDelt.ToString();
        slider.value = damageDelt;
    }
    public void dealDamage(int damagetaken, string attacker)
    {
        if(attacker == "enemy")
        { 
        damageDelt -= damagetaken;
        }
        else if (attacker == "player")
        {
            damageDelt += damagetaken;
        }

        if (damageDelt <= -10)
        {
            gameState = gameState.LOST;
            Debug.Log("GAME OVER YOU LOSE");
            Time.timeScale = 0;
        }

        if (damageDelt >= 10)
        {
            gameState = gameState.WON;
            PlayerDeckHandler.instance.ReturnCardsToDeck();
            damageDelt = 0;
            LevelLoader.instance.LoadNextLevel(3);
        }
    }
    private void attackAnimation(bool[] availableAttackerSlots, bool isEnemy, int i)
    {
        //do attack animations or something
        if (availableAttackerSlots[i] == false && isEnemy == false)
        {
            playersCreature = FindObjectsOfType<Card>().Where(x => x.handIndex == i && x.hasBeenPlayed == true && x.Enemy == false).FirstOrDefault();
            // Null Ref if land card
            if (playersCreature != null)
            {
                playersCreature.attackAnimation(false);
            }
        }
        else if (availableAttackerSlots[i] == false && isEnemy == true)
        {
            playersCreature = FindObjectsOfType<Card>().Where(x => x.handIndex == i && x.hasBeenPlayed == true && x.Enemy == true).FirstOrDefault();
            // Null Ref if land card
            if (playersCreature != null)
            {
                playersCreature.attackAnimation(true);
            }
        }
    }
    private void executeAttack(bool[] availableAttackerSlots, bool[] availableDefenderSlots, bool isEnemy)
    {
        // one side attacks the other
        //Find Ocupied Battlefield Slots
        for (int i = 0; i < availableAttackerSlots.Length; i++)
        {
            attackAnimation(availableAttackerSlots, isEnemy, i);
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
                        dealDamage(playersCreature.Attack, "player");
                        //LifeTracker.damageEnemyFace(playersCreature.Attack);
                    }
                    else
                    {
                        dealDamage(playersCreature.Attack, "enemy");
                        //LifeTracker.damagePlayerFace(playersCreature.Attack);
                    }
                    creatureEffects.checkForattackEffect(playersCreature, enemyCreature);
                }
            }
        }
    }
    public void triggerAttack()
    {
        if(gameState == gameState.PLAYERTURN)
        {
            gameState = gameState.ENEMYTURN;
        availableBattlefieldSlots = gm.availableBattlefieldSlots;
        availableEnemyCardSlots = enemygm.availableEnemyCardSlots;
       
        StartCoroutine(combatStep());
        }
        gameState = gameState.DRAWPHASE;
    }

    IEnumerator combatStep ()
    {
        //Player attacks
        executeAttack(availableBattlefieldSlots, availableEnemyCardSlots, false);
        yield return new WaitForSeconds(1f);
        // Generate Enemy Creatures
        //for (int i = 0; i < availableEnemyCardSlots.Length; i++)
        //{
        //    EnemyDeckHandler.instance.DrawCardEnemy();
        //}
        //yield return new WaitForSeconds(1f);
        // Enemy Attacks
        executeAttack(availableEnemyCardSlots, availableBattlefieldSlots, true);
    }
}

