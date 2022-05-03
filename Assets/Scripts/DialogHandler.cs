using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class DialogHandler : MonoBehaviour
{
    [SerializeField] Button showMessageBtn;

    void Update()
    {
        showMessageBtn.onClick.RemoveAllListeners(); //avoiding leaks
        showMessageBtn.onClick.AddListener(onShowMessageBtnClicked);
    }
    void onShowMessageBtnClicked()
    {
        QuestionDialogUI.Instance.ShowQuestion("Are you sure you want to quit? ", () =>
        {
            Application.Quit();
            EditorApplication.ExitPlaymode();
        }, () => {

        });
    }
}
