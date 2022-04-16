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
    float currentEmission, maxEmission = 50; //setting it to 100

    private void Start()
    {
        currentEmission = maxEmission; //setting it automatically to 100% level;
    }
    private void Update()
    {
        playerText.text = currentEmission + "%"; //displaying the percentage
        //check if currentEmission > maxEmission, then currentemission = maxEmission (currentEmission !> 100)
        if (currentEmission > maxEmission) 
        {
            currentEmission = maxEmission;
        }
       
        lerpSpeed = 3f * Time.deltaTime; //can be changed to increase or decrease lerp speed
        EmissionLevelFiller();
    }
    void EmissionLevelFiller()
    {
        //playerEmissionbar.fillAmount = Mathf.Lerp(emissionBar.fillAmount,currentEmission / maxEmission, lerpSpeed); // should be between 0 and 1 
    }


    //functions for decreasing and increasing emission Levels
    //for decreasing, method takes in a parameter
    //check if currentEmission > 0, subtract currentEmission -= emissionPoints(parameter)
    //for increasing, method takes in a parameter
    //check if currentEmission < maxEmission, add currentEmission += emissionPoints(parameter)

}
