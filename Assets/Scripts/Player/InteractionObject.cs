using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    public bool talks;
    public string message;
    private Naration narrator;
    public cameraManager camMan;
    // Start is called before the first frame update
    void Start()
    {
        camMan = FindObjectOfType<cameraManager>(); 
        var narators = FindObjectsOfType<Naration>();
        narrator = narators.Where(x => x.tag == "narratorMap").FirstOrDefault();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Talk()
    {
        narrator.NpcHelloWorld(this);
    }

    public void triggerBattle()
    { 
        narrator.npcTriggerBattle();
        camMan.goToGameScreen();
    }

    private bool yesNoDialogue()
    {
        //Dialogue
        // >Yes
        // No
        if (true)
            return true;
        else
            return false;
    }
}
