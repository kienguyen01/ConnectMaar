using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupHander : MonoBehaviour
{
    public GameObject gameObject;
    public Animator animator;

    public void PopUp()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("pop");
       
    }
}
