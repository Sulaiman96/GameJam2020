using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    enum MouseStates { Valid, Invalid };

    MouseStates currentMouseState;

    [SerializeField] private Texture2D Crosshair = null;
    [SerializeField] private Texture2D invalidCrosshair = null;
    [SerializeField, Range(-1, 1)] private float aimAngleThreshold = -0.2f;

    private Vector2 cursorOffset;
    // Start is called before the first frame update
    void Start()
    {
        cursorOffset = new Vector2(16, 16);
        currentMouseState = MouseStates.Valid;
        if (Crosshair)
            Cursor.SetCursor(Crosshair, cursorOffset, CursorMode.Auto);
    }

    public bool GetMouseLocation(ref Vector3 targetLocation)
    {
        Plane PlayerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDist = 0.0f;
        if (PlayerPlane.Raycast(ray, out hitDist))
        {
            targetLocation = ray.GetPoint(hitDist);
            return true;
        }

        return false;
    }

    public bool IsValidAimDirection(Vector3 mousePosition)
    {
        Vector3 direction = mousePosition - transform.position;
        float dotValue = Vector3.Dot(transform.forward.normalized, direction.normalized);

        // Return false if mouse is behind the player
        if (dotValue < aimAngleThreshold)
            return false;

        return true;
    }

    public void SetCursorTexture(Vector3 mouseLocation)
    {
        bool validMouseAim = IsValidAimDirection(mouseLocation);

        if (validMouseAim && currentMouseState != MouseStates.Valid)
        {
            if (!Crosshair)
                return;

            Cursor.SetCursor(Crosshair, cursorOffset, CursorMode.Auto);
            currentMouseState = MouseStates.Valid;
        }
        else if (!validMouseAim && currentMouseState != MouseStates.Invalid)
        {
            if (!invalidCrosshair)
                return;

            Cursor.SetCursor(invalidCrosshair, cursorOffset, CursorMode.Auto);
            currentMouseState = MouseStates.Invalid;
        }
    }
}
