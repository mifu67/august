using UnityEngine;

public class ResetCursor : MonoBehaviour
{
    private void Awake()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
