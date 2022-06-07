using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    public bool talks;
    public int message;
    private Naration narrator;
    // Start is called before the first frame update
    void Start()
    { 
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
    //public void OnMouseDown()
    //{
    //    StartCoroutine(SuckABigFuckingDick());
    //}
    IEnumerator loadLevel()
    {
        narrator.npcTriggerEvent(message);
        PlayerMovement.instance.freeze = true;
        yield return new WaitForSeconds(2f);
        LevelLoader.instance.LoadNextLevel(message);
    }
    public void triggerEvent(int index)
    {
        StartCoroutine(loadLevel());
    }

}
