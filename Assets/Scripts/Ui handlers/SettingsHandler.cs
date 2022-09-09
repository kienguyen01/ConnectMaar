using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsHandler : MonoBehaviour
{
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
    public void StartMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
