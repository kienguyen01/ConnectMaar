using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChangeSound : MonoBehaviour
{
    private Sprite soundOnImage;
    public Sprite soundOffImage;
    public Button button;
    private bool isSoundOn = true;
    public AudioSource audio;


    // 
    void Start()
    {
        soundOnImage = button.image.sprite;
    }

    // 
    void Update()
    {
        
    }
    public void ButtonClicked()
    {
        if (isSoundOn)
        {
            button.image.sprite = soundOffImage;
            isSoundOn = false;
           audio.mute = true;
        }
        else
        {
            button.image.sprite = soundOnImage;
            isSoundOn = true;
            audio.mute = false;
        }
    }
}
