using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using TMPro;
using Google.Cloud.Translation.V2;

public class MainMenu : MonoBehaviour
{
    public TMP_Dropdown DropDownBox;

    private bool LoadedPrefs;

    private void Start()
    {
        LoadedPrefs = false;
    }

    private void Update()
    {
        if(!LoadedPrefs && LocalizationSettings.AvailableLocales.Locales.Count > 1)
        {
            if (PlayerPrefs.GetString("Selected_Language") == LanguageCodes.Dutch)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                DropDownBox.value = 0;
            }
            else if (PlayerPrefs.GetString("Selected_Language") == LanguageCodes.English)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                DropDownBox.value = 1;
            }

            LoadedPrefs = true;
        }
    }

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
        PlayerPrefs.SetString("Selected_Language", (dropdown.value == 0) ? LanguageCodes.Dutch : LanguageCodes.English);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[dropdown.value];
    }
}