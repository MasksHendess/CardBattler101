using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Naration : MonoBehaviour
{
    public TextMeshProUGUI Text;
    string inputText;
    bool isWriting;
    // Start is called before the first frame update
    void Start()
    {
        isWriting = false;
        Text.text = "";
    }


    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator MyIEnumerator()
    {
        isWriting = true;
        Text.text = "";
        char[] textArray = inputText.ToCharArray();
        foreach (char singleLetter in textArray)
        {
            yield return new WaitForSeconds(0.01f);
            Text.text += singleLetter;
        }
        isWriting = false;
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
    #region DeckInteractionView
    public void sacrifcie(int index)
    {
        if(!isWriting)
        { 
            if(index == 0)
        inputText = "Cards... Cards... I hate cards... give me... Card... I will... cut it... ";
           else if(index == 1)
          inputText = "That Card... Is of intrest... Only to... Historians. I am... Satisfied.";
            StartCoroutine("MyIEnumerator");
        }
    }
    public void surviveRitual0(Card card)
    {
        inputText = card.Name + " survived the ritual. It became stronger than before. ";
     StartCoroutine("tauntEnum"); 
    }
    public void gambleLine(string stat)
    {
        inputText = "Select a card to be judged. I will evalute its " +stat+" to determine how dissapointing it is.";
        StartCoroutine("MyIEnumerator");
    }
    #endregion
    #region draftview
    public void LoadMutateLevelFail()
    {
        if (!isWriting)
        {
                inputText = "... You don't have enough cards...";
            StartCoroutine("MyIEnumerator");
        }
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
                inputText = "Huh. I wouldn't have picked that... Which one has the DANKEST ABILLITY?";
                break;
            case 3:
                inputText = "YOUR CHILD IS BORN! ...Wow so ugly... Let's put that right into your deck... ";
                break;
            default:
                inputText = "Mr Sexxxis Pizza. Our special to day is Sexeroni! (its like peperoni but sexy)";
                break;
        }
        StartCoroutine("MyIEnumerator");
    }
    #endregion
    public void Graveyard(int i)
    {
        switch (i)
        {
            case 0:
                inputText = "Remember these cards? They all fought and perished for you...";
                break;
            case 1:
                inputText = "A Card may earn a resurection... For a price. Sacrifice... Required...";
                break;
            case 2:
                inputText = "A fitting sacrifice... Very well you may put that one back in your deck I suppose. ";
                break;
            default:
                inputText = "All that for a drop of blood...";
                break;
        }
        StartCoroutine("MyIEnumerator");
    }
    #region npc
    public void NpcHelloWorld(InteractionObject npc)
    {
        if(!isWriting)
        { 
        inputText = "Hello my name is " + npc.name +".";
        StartCoroutine("MyIEnumerator");
        }
    }

    public void npcTriggerEvent(int index)
    {
        if (!isWriting)
        {
            switch (index)
            {
                case 0:
                    inputText = "ITS TIME TO D-D-D-DUEL!!!!!";
                    break;
                case 1:
                    inputText = "You activate the Drafting Machine...";
                    break;
                case 2:
                    inputText = "You activate the 5T4T B00ZTR Machine";
                    break;
                case 3:
                    inputText = "STONKS!";
                    break;
                case 4:
                    inputText = "I'll help you get rid of cards...";
                    break;
                case 5:
                    inputText = "Three corpses... they shall rise as one...";
                    break;
                case 6:
                    inputText = "Two corpses... They shall rise as one...";
                    break;
                case 7:
                    inputText = "I sell packs. Do you wish to buy packs...";
                    break;
                case 8:
                    inputText = "Dying is mearly a setback...";
                    break;
                case 9:
                    inputText = "Three creatures... Starving... Save one?";
                    break;
                case 10:
                    inputText = "Three creatures... kungfu fighting... which one wasn't as fast as lightning?";
                    break;
                default:
                    inputText = "It's not about the money. It's about sending a message.";
                    break;
            }
            StartCoroutine("MyIEnumerator");
        }
    }
    #endregion
    public void resetText()
    {
        inputText = "";
        StartCoroutine("MyIEnumerator");
    }
}
