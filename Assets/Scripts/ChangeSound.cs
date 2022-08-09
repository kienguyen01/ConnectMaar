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
    public AudioSource clearBtnAudio;
    public AudioSource endTurnBtnAudio;
    public AudioSource inventoryBtnSound;
    public AudioSource nodeBtnAudio;
    public AudioSource sliderBtnAudio;
    public AudioSource onHoverAudio;
    public AudioSource exitBtnAudio;



    // 
    void Start()
    {
        soundOnImage = button.image.sprite;
    }

    // 
    public void ButtonClicked()
    {
        if (isSoundOn)
        {
            button.image.sprite = soundOffImage;
            isSoundOn = false;
            clearBtnAudio.mute = true;
            endTurnBtnAudio.mute = true;
            nodeBtnAudio.mute = true;
            inventoryBtnSound.mute = true;
            sliderBtnAudio.mute = true;
            onHoverAudio.mute = true;
            exitBtnAudio.mute = true;
        }
        else
        {
            button.image.sprite = soundOnImage;
            isSoundOn = true;
            clearBtnAudio.mute = false;
            endTurnBtnAudio.mute = false;
            nodeBtnAudio.mute = false;
            inventoryBtnSound.mute = false;
            sliderBtnAudio.mute = false;
            onHoverAudio.mute = false;
            exitBtnAudio.mute = false;

        }
    }
}
