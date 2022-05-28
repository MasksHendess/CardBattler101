using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    #region Buildmanager setup Singelton pattern
    // only 1 instance of BuildManager in scene that is easy to acsess
    // Dont duplicate this region 
    public static SpriteManager instance; //self reference
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
    public List<Sprite> Battlefieldsprites;
    public List<Sprite> CardSprites;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Sprite> getBattlefieldSprites()
    {
        return Battlefieldsprites;
    }

    public Sprite getCardSprites(int index)
    {
        if (CardSprites[index] != null)
            return CardSprites[index];
        else
        {
            Debug.Log("Sprite Error!");
            return CardSprites[CardSprites.Count];
        }
    }
}
