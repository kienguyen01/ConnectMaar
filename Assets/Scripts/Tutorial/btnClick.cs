using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class btnClick : MonoBehaviour
{
    // Start is called before the first frame update
    //public bool buttonPressed;

    [SerializeField]
    public GameObject FirstPipe;

    public bool FirstPipeActive;

    private void Start()
    {
        Button button = GameObject.Find("firstBtn").GetComponent<Button>();

        gameObject.GetComponent<Button>().onClick.AddListener(setActive);
        FirstPipeActive = true;
    }

    private void setActive()
    {
        FirstPipeActive ^= true;
    }


    public void onItemClicked()
    {
        Button button = GameObject.Find("firstBtn").GetComponent<Button>();
        Debug.Log("FUCK THIS IT WORKS" + button.name);

    }
}
 