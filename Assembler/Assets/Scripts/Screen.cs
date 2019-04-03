using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen : MonoBehaviour
{
    Texture2D texture;
    public int textureSize = 64;
    int i;
    public Vector2Int drawingCursorPosition;
    public Image drawingCursor;
    void Start()
    {
        
        texture = new Texture2D(textureSize, textureSize);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, textureSize, textureSize), new Vector2(0.5f,0.5f));
        sprite.name = "newSprite";
        GetComponent<Image>().sprite = sprite;
        drawingCursorPosition = new Vector2Int(textureSize / 2, textureSize / 2);
    }

    private void Update()
    {
        
        //SetPixel(i, 30, Color.blue);
        i++;
    }
    public void DrawCursor()
    {
        drawingCursor.GetComponent<RectTransform>().localPosition = new Vector2(drawingCursorPosition.x - 32, drawingCursorPosition.y - 32);
    }
    public void SetPixel(int x, int y, Color color)
    {
        texture.SetPixel(x, y, color);
        texture.Apply();
    }
}
