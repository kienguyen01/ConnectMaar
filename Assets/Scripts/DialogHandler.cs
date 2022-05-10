using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;

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
            //yes action
            //TODO: TEST THIS OUT //TO SEE IF GAME SESSION IS ENDED AFTER RUNNING THIS
            Application.Quit();
            Debug.Log("Quit game");
            //quit application
            EditorApplication.ExitPlaymode();
            //load main menu scene
            SceneManager.LoadScene("Menu");
        }, () => {
            //no action
        });
    }
}