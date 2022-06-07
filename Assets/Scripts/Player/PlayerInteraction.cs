using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    GameObject currentInteractionObject;
    InteractionObject currentInteractionObjectScript;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentInteractionObject)
        {

            if (currentInteractionObjectScript.talks && currentInteractionObjectScript.message > -1)
            {
                currentInteractionObjectScript.triggerEvent(currentInteractionObjectScript.message);
            }
            else if(currentInteractionObjectScript.talks)
            {
                currentInteractionObjectScript.Talk();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("interactionObject"))
        {
            currentInteractionObject = other.gameObject;
            currentInteractionObjectScript = currentInteractionObject.GetComponent<InteractionObject>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentInteractionObject = null;
    }
}
