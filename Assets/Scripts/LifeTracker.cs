using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeTracker : MonoBehaviour
{
    public int damageDelt = 0;
    public Text lifeCountText;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeCountText.text ="Life: "+ damageDelt.ToString();
        slider.value = damageDelt;
    }

    public void damagePlayerFace(int damagetaken)
    {
        damageDelt -= damagetaken;
        if(damageDelt <= -6)
        {
            Debug.Log("GAME OVER YOU LOSE");
            Time.timeScale = 0;
        }
    }

    public void damageEnemyFace(int amount)
    {
        // Inscryption Rules, first to 5 DMG wins
        damageDelt +=amount;
        if(damageDelt >= 6)
        {
            Debug.Log("GAME OVER YOU WIN");
            Time.timeScale = 0;
        }
    }
}
