using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupHandler : MonoBehaviour
{
    public Canvas canvas;
    private bool isCanvasOpen = false;

    public void Popup()
    {
        if (isCanvasOpen == false)
        {
            isCanvasOpen = true;
            canvas.enabled = true;
        }
        else
        {
            isCanvasOpen = false;
            canvas.enabled = false;

        }
    }

    public bool isOpen()
    {
        return isCanvasOpen;
    }
}
