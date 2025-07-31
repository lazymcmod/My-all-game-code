using UnityEngine;

public class AimCursorCanvas : MonoBehaviour
{
    public GameObject aimImage; // Crosshair image (UI element on Canvas)

    private bool isCursorLocked = true;

    void Start()
    {
        LockCursor();
    }

    void Update()
    {
        // Escape se unlock
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }

        // Left click se dobara lock
        if (Input.GetMouseButtonDown(0) && !isCursorLocked)
        {
            LockCursor();
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        aimImage.SetActive(true); // Crosshair show
        isCursorLocked = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        aimImage.SetActive(false); // Crosshair hide (optional)
        isCursorLocked = false;
    }
}
