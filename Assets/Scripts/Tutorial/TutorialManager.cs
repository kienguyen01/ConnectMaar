using System.Collections;
using System.Collections.Generic;
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


     void Awake()
    {
        
    }

    private void Update()
    {
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
