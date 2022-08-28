using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProcessDeepLinkMngr : MonoBehaviour
{
    public static ProcessDeepLinkMngr Instance { get; private set; }
    public string deeplinkURL;
    public string roomName = null;
    public bool active = false;

    

    private void Awake()
    {
        if (Instance == null)
        {
            //Debug.LogError("THIS DOES NOT WORK");
            Instance = this;
            Debug.Log("ProcessDeepLinkMng line 20: " + Instance);
            Application.deepLinkActivated += onDeepLinkActivated;

            if (!String.IsNullOrEmpty(Application.absoluteURL))
            {
                // cold start and Application.absoluteURL not null so process Deep Link
                onDeepLinkActivated(Application.absoluteURL);
                Debug.Log("AbsoluteURL: " + Application.absoluteURL);
            }
            // initialize DeepLink Manager global variable
            else deeplinkURL = "[None]";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void onDeepLinkActivated(string url)
    {
        active = true;
        // update DeepLink Manager global variable, so URL can be accessed from anywhere 
        deeplinkURL = url;
        Debug.Log("ProcessDeepLinkMng line 43: " +  deeplinkURL);

        //Decode the DeepLink url to determine action
        roomName = url.Split("?"[0])[1];
        Debug.Log("ProcessDeepLinkMng line 47: " + roomName);
        //Debug.Log($"Deep Link Scene:{roomName}");
       
        SceneManager.LoadScene("Lobby");
    }
}
