using UnityEngine;
using UnityEngine.UI;

public class CrosshairFollowMouse : MonoBehaviour
{
    private RectTransform crosshairRect;

    void Start()
    {
        crosshairRect = GetComponent<RectTransform>();
        Cursor.visible = true; // Optional: Show cursor
    }

    void Update()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(),
            Input.mousePosition,
            null,
            out mousePos
        );

        crosshairRect.anchoredPosition = mousePos;
    }
}
