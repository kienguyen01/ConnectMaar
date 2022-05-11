using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class InfoPopup : MonoBehaviour
{
    private Button exitBtn;
    public static InfoPopup Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        exitBtn = transform.Find("ExitBtn").GetComponent<Button>();
        Hide();
    }

    public void ShowText(Action exitAction)
    {
        gameObject.SetActive(true);
        exitBtn.onClick.AddListener(() =>
        {
            Hide();
            exitAction();
        });
    }


    private void Hide()
    {
        gameObject.SetActive(false);
    }
}


