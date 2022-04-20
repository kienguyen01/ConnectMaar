using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmissionLevelPlayer : MonoBehaviour
{
    public TMP_Text playerText;
    public Image playerEmissionbar;
    float lerpSpeed; //making sure increase and decrease is smooth
    float currentEmission, maxEmission; //setting it to 100

    private void Start()
    {
        maxEmission = 50; //setting it automatically to 100% level;
    }
    private void Update()
    {
        currentEmission = GameState.instance.playerStates[0].gameData.totalPoint;
        playerText.text = currentEmission + "%"; //displaying the percentage
        //check if currentEmission > maxEmission, then currentemission = maxEmission (currentEmission !> 100)
        if (currentEmission > maxEmission) 
        {
            currentEmission = maxEmission;
        }
       
        lerpSpeed = 3f * Time.deltaTime; //can be changed to increase or decrease lerp speed
        Debug.Log(playerEmissionbar.fillAmount);
        Debug.Log(currentEmission);
        Debug.Log(maxEmission);
        playerEmissionbar.fillAmount = Mathf.Lerp(playerEmissionbar.fillAmount, currentEmission / maxEmission, lerpSpeed);
        Debug.Log(playerEmissionbar.fillAmount);
        Debug.Log(currentEmission);
        Debug.Log(maxEmission);// should be between 0 and 1 
    }
    void EmissionLevelFiller()
    {

    }


    //functions for decreasing and increasing emission Levels
    //for decreasing, method takes in a parameter
    //check if currentEmission > 0, subtract currentEmission -= emissionPoints(parameter)
    //for increasing, method takes in a parameter
    //check if currentEmission < maxEmission, add currentEmission += emissionPoints(parameter)

}
