using Kirurobo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowAligner : MonoBehaviour
{
    public RectTransform canvasVerticalOffset;

    void Start()
    {
#if UNITY_EDITOR
        return;
#endif
        // Ensure UniWindowController is enabled
        if (!UniWindowController.current.enabled)
        {
            UniWindowController.current.enabled = true;
        }

        // Get screen size
        Vector2Int screenSize = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);

        // Get window size
        Vector2 windowSize = UniWindowController.current.clientSize;

        // Calculate target position for the window to be aligned to the right
        Vector2Int targetPosition = new (screenSize.x - (int)windowSize.x, (int)windowSize.y / 2);

        // Move the window
        UniWindowController.current.windowPosition = targetPosition;

        if (canvasVerticalOffset != null)
            canvasVerticalOffset.offsetMax = new Vector2(canvasVerticalOffset.offsetMax.x, (canvasVerticalOffset.rect.height - 950) / 1) ;
    }
}
