using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen : MonoBehaviour
{
    Texture2D texture;
    Vector2 textureSize;
    public Vector2Int drawingCursorPosition;
    public GameObject drawingCursor;
    void Start()
    {
        textureSize.x = GetComponent<RectTransform>().rect.width;
        textureSize.y = GetComponent<RectTransform>().rect.height;

        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);



        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, textureSize.x, textureSize.y), Vector2.zero);

        sprite.name = "newSprite";
        GetComponent<Image>().sprite = sprite;
        ClearScreen();
    }

    public void ClearScreen()
    {
        for (int i = 0; i < textureSize.x; i++)
        {
            for (int j = 0; j < textureSize.y; j++)
            {
                texture.SetPixel(i, j, Color.white);
            }
        }
        texture.Apply();
    }

    public void DrawCursor()
    {
        drawingCursor.GetComponent<RectTransform>().localPosition = new Vector2(drawingCursorPosition.x, drawingCursorPosition.y);
    }

    public void DrawPixel(int r, int g, int b)
    {
        Color color = new Color(r, g, b);
        SetPixel(drawingCursorPosition.x, drawingCursorPosition.y, color);
    }

    public void SetPixel(int x, int y, Color color)
    {
        texture.SetPixel(x, y, color);
        texture.Apply();
    }
}
