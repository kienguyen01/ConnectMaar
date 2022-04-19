using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCanvasScript : MonoBehaviour
{
    public Transform transformCanvas;
    // Start is called before the first frame update
    void Start()
    {
        transformCanvas.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
