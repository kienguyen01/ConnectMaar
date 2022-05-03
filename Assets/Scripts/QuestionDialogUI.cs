using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;



public class QuestionDialogUI : MonoBehaviour
{
    public static QuestionDialogUI Instance { get; private set; }
    private TextMeshProUGUI text_TMP;
    private Button yesBtn;
    private Button noBtn;

    private void Awake()
    {
        Instance = this;
        text_TMP = transform.Find("DialogText").GetComponent<TextMeshProUGUI>();
        yesBtn = transform.Find("YesButton").GetComponent<Button>();
        noBtn = transform.Find("NoButton").GetComponent<Button>();

        Hide();
    }

    public void ShowQuestion(string question, Action yesAction, Action noAction)
    {
        gameObject.SetActive(true);

        text_TMP.text = question;
        //yesBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(yesAction));
        //noBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(noAction));

        yesBtn.onClick.AddListener(() =>
        {
            Hide();
            yesAction();
        });
        noBtn.onClick.AddListener(() =>
        {
            Hide();
            noAction();
        });

    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
