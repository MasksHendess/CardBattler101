using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Naration : MonoBehaviour
{
   public TextMeshProUGUI Text;
    string inputText;
    // Start is called before the first frame update
    void Start()
    {
        Text.text = "";
    }


    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator MyIEnumerator()
    {
        Text.text = "";
        char[] textArray = inputText.ToCharArray();
        foreach (char singleLetter in textArray)
        {
            yield return new WaitForSeconds(0.01f);
            Text.text += singleLetter;
        }
    }
    IEnumerator tauntEnum()
    {
        Text.text = "";
        char[] textArray = inputText.ToCharArray();
        foreach (char singleLetter in textArray)
        {
            yield return new WaitForSeconds(0.01f);
            Text.text += singleLetter;
        }
        yield return new WaitForSeconds(1f);
        Text.text = "...";
        yield return new WaitForSeconds(1f);
        Text.text ="";
        inputText = "It has still not unlocked its true potential. Perhaps another Judgement will unleash its true powers..."; 
        textArray = inputText.ToCharArray();
        foreach (char singleLetter in textArray)
        {
            yield return new WaitForSeconds(0.01f);
            Text.text += singleLetter;
        }
    }
    public void helloWorld()
    {
        inputText = "Welcome to CardBattler101. Its time for some EMOTIONAL DAMAGE";
        StartCoroutine("MyIEnumerator");
    }

    public void DieMotherFucker()
    {
        inputText = "HA HA YES DIE TRASH!";
        StartCoroutine("MyIEnumerator");
    }

    public void NiceTopDeck()
    {
        inputText = "Nice Top Deck!";
        StartCoroutine("MyIEnumerator");
    }

    public void surviveRitual0(Card card)
    {
        inputText = card.Name + " survived the ritual. It became stronger than before. ";
     StartCoroutine("tauntEnum"); 
    }
    private void taunt()
    {
        StartCoroutine("MyIEnumerator");
    }
    public void gambleLine(string stat)
    {
        inputText = "Select a card to be judged. I will evalute its " +stat+" to determine how dissapointing it is.";
        StartCoroutine("MyIEnumerator");
    }
    public void abominationLine(int i)
    {
        switch(i)
        {
            case 0:
                inputText = "Before you lies three trash cards. Which one has the DANKEST POWER and DEFENSE stats?";
                break;
            case 1:
                inputText = "Very well. Which one has the DANKEST CASTING COST?";
                break;
            case 2:
                inputText = "Huh. I wouldn't have picked that...\n Which one has the DANKEST ABILLITY?";
                break;
            case 3:
                inputText = "YOUR CHILD IS BORN! ...\nWow so ugly... Let's put that right into your deck... ";
                break;
            default:
                inputText = "Mr Sexxxis Pizza. Our special to day is Sexeroni! (its like peperoni but sexy)";
                break;
        }
        StartCoroutine("MyIEnumerator");
    }
}
