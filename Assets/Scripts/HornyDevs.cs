using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornyDevs : MonoBehaviour
{
    public List<SpriteRenderer> spriteRenderer;
    public List<Sprite> sprites;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        var rand = sprites[Random.Range(0, sprites.Count)];
        foreach (var item in spriteRenderer)
        {
            item.sprite = rand;
        }
    }
}
