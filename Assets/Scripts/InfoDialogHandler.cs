using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class InfoDialogHandler : MonoBehaviour
{
    [SerializeField] Button showPopup;
    void Update()
    {
        showPopup.onClick.AddListener(onShowBtnClicked);
    }
    void onShowBtnClicked()
    {
        InfoPopup.Instance.ShowText(() => { });
    }

}
