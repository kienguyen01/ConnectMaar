using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupHandler : MonoBehaviour
{
    public Canvas canvas;
    public bool isCanvasOpen = false;
    public void Popup()
    {
        if (isCanvasOpen == false)
        {
            isCanvasOpen = true;
            canvas.enabled = true;
        }
        else if (isCanvasOpen == true)
        {
            isCanvasOpen = false;
            canvas.enabled = false;

        }

    }
}
