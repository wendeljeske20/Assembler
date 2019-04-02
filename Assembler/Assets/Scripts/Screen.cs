using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen : MonoBehaviour
{
    Texture2D texture;
    int i;
    void Start()
    {
        int textureSize = 64;
        texture = new Texture2D(textureSize, textureSize);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, textureSize, textureSize), Vector2.zero);
        sprite.name = "newSprite";
        
        GetComponent<Image>().sprite = sprite;

        //for (int y = 0; y < texture.height; y++)
        //{
        //    for (int x = 0; x < texture.width; x++)
        //    {
        //        SetPixel(x, y, Color.white);
        //    }
        //}
        //SetPixel(20, 10, Color.red);
    }

    private void Update()
    {
        
        //SetPixel(i, 30, Color.red);
        //i++;
    }

    void SetPixel(int x, int y, Color color)
    {
        texture.SetPixel(x, y, color);
        texture.Apply();
    }
}
