using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSlider : MonoBehaviour
{
    public GameObject panelInventory;

    public void ShowInventoryPanel()
    {
        if(panelInventory != null)
        {
            Animator animator = panelInventory.GetComponent<Animator>();
            if(animator != null)
            {
                bool isOpen = animator.GetBool("show");
                animator.SetBool("show", !isOpen);
            }
        }
    }

}
