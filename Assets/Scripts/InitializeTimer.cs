using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeTimer : MonoBehaviour
{
    [SerializeField] Timer timer;
    private void Start()
    {
        timer.SetDuration(10).Begin(); // initializing timer //duration amount to be adapted
    }
}
