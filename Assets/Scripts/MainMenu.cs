using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using TMPro;

public class MainMenu : MonoBehaviour
{
   public void ExitButton(){
       Application.Quit();
       Debug.Log("Game closed!");
   }
   public void StartGame(){
       SceneManager.LoadScene("Lobby");
   }

    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void OnLanguageChange(TMP_Dropdown dropdown)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[dropdown.value];
    }
}
