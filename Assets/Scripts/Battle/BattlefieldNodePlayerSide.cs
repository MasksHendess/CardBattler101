using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldNodePlayerSide : MonoBehaviour
{
    public bool PlayCard;
    public int id;

    private SpriteRenderer spriteRenderer;
    private List<Sprite> sprites;

    Card card;
    // Start is called before the first frame update
    void Start()
    {
        PlayCard = false;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        sprites = SpriteManager.instance.getBattlefieldSprites();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void makeAvailableForCardPlacement(Card Card)
    {
        card = Card;
        PlayCard = true;
        spriteRenderer.sprite = sprites[1];
    }
    private void OnMouseDown()
    {
            if(PlayCard)
            PlayerDeckHandler.instance.moveCardtoBattlefield(card, id);
    }

    public void resetSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }
}
