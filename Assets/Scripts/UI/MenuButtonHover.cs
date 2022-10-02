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

    // Animation

    private Dictionary<GameObject, Vector3> ButtonStartPositions = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Vector3> TargetEndPositions = new Dictionary<GameObject, Vector3>();

    public Vector3 GetStartPosition(GameObject Button)
    {
        return ButtonStartPositions.ContainsKey(Button) ? ButtonStartPositions[Button] : Button.transform.position;
    }

    public void OnPointerHover(GameObject Button)
    {
        Cursor.SetCursor(Texture2D.redTexture, Vector2.zero, CursorMode.Auto);
        Vector3 Position = GetStartPosition(Button);
        Position.x += HoverIndent;
        StartCoroutine(Animation(Button, Position));
    }

    public void OnPointerExit(GameObject Button)
    {
        Cursor.SetCursor(Texture2D.whiteTexture, Vector2.zero, CursorMode.Auto);
        StartCoroutine(Animation(Button, GetStartPosition(Button)));
    }

    private IEnumerator Animation(GameObject Button, Vector3 EndPos)
    {

        TextMeshProUGUI Text = Button.GetComponentInChildren<TextMeshProUGUI>();
        TargetEndPositions[Button] = EndPos;

        if (!ButtonStartPositions.ContainsKey(Button))
        {
            ButtonStartPositions[Button] = Text.transform.position;
        }

        float ElapsedTime = 0;
        float WaitTime = .2f;
 
        while (ElapsedTime < WaitTime && TargetEndPositions[Button] == EndPos)
        {
            Text.transform.position = Vector3.Lerp(Button.transform.position, EndPos, (ElapsedTime / WaitTime));
            ElapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log((ElapsedTime < WaitTime).ToString() + ", " +  (TargetEndPositions[Button] == EndPos).ToString());
        yield return null;

    }

}
