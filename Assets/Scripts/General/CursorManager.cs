using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;

    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
    }
}
