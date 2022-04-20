using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;


    [HideInInspector]
    public PlayerGameData gameData;

    [HideInInspector]
    public List<PlayerState> playerStates;

    

    public 


     void Start()
    {
        //TextMeshProUGUI text = GameObject.Find("TutorialMessgae").GetComponent<TextMeshProUGUI>();

/*        Debug.Log("THIS IS TRUE");
        text.text = "Welcome to the tutorial";*/

    }

    private void Update()
    {
        //text.text = "Welcome to the tutorial";

        for (int i = 0; i < popUps.Length; i++)
        {
            if(i == popUpIndex)
            {
                popUps[i].SetActive(true);
            }
            else
            {
                popUps[i].SetActive(false);
            }
        }


        if (popUpIndex == 0)
        {

        }
    }

}
