using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class changeText : MonoBehaviour
{
    private TMP_Text tutorialTitle;
    private TMP_Text tutorialBody;
    void Start()
    {

        tutorialTitle = GameObject.Find("txtTitle").GetComponent<TMP_Text>();
        tutorialBody = GameObject.Find("txtBody").GetComponent<TMP_Text>();

        //change content
        tutorialTitle.text = "CHANGE";
        tutorialBody.text = "This";
    }

}
