using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSelect : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    public int SpriteNumber;
    public int SpriteType = 0;
    public string path = "";

    public bool off = false;
    void Start()
    {

        if (path != "")
        {
            sprites = LoadSprites(path);
        }
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
 
    }

    // Update is called once per frame
    void Update()
    {
        if (!off && path != "")
        {
            spriteRenderer.sprite = sprites[SpriteNumber];
        }
        else
        {
            spriteRenderer.sprite = null;
        }
    }

    
    public Sprite[] LoadSprites(string path)
    {
        Sprite[] Sprites;
        object[] loadedSprites = Resources.LoadAll("Sprites/" + path, typeof(Sprite));
        Sprites = new Sprite[loadedSprites.Length];
        //this

        for (int x = 0; x < loadedSprites.Length; x++)
        {
            Sprites[x] = (Sprite)loadedSprites[x];
        }

        return Sprites;
    }

    
}
