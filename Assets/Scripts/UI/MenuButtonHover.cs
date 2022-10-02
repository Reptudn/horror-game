using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButtonHover : MonoBehaviour
{

    [Range(1, 100)]
    public int HoverIndent = 20;

    public Texture2D PointerTexture;
    public Texture2D CursorTexture;

    public void OnPointerHover(GameObject Button)
    {
        RectTransform text = Button.GetComponentInChildren<RectTransform>();
        Vector3 position = text.position;
        position.x += HoverIndent;
        text.SetPositionAndRotation(position, text.rotation);
        Cursor.SetCursor(Texture2D.redTexture, Vector2.zero, CursorMode.Auto);
        //Button.GetComponentInChildren<TextMeshProUGUI>().SetText("Deez");
    }

    public void OnPointerExit(GameObject Button)
    {
        RectTransform text = Button.GetComponentInChildren<RectTransform>();
        Vector3 position = text.position;
        position.x -= HoverIndent;
        text.SetPositionAndRotation(position, text.rotation);
        Cursor.SetCursor(Texture2D.whiteTexture, Vector2.zero, CursorMode.Auto);
    }

}
