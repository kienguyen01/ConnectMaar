using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public int Duration;

    [SerializeField] private Image uiFill;

    [HideInInspector]
    public bool isOver;

    private int remainingDuration;
    private List<Coroutine> routines;

    private void Start()
    {
        routines = new List<Coroutine>();
        Tick(Duration);
    }

    public void Tick(int Seconds)
    {
        foreach (Coroutine r in routines)
        {
            StopCoroutine(r);
        }

        if(Duration != 0)
        {
            isOver = false;
            remainingDuration = Seconds;
            routines.Add(StartCoroutine(UpdateTimer()));
        }
    }

    public void Tick()
    {
        Tick(Duration);
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration >= 0)
        {
            uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
            remainingDuration--;
            yield return new WaitForSeconds(1f);
        }
        OnEnd();
    }

    private void OnEnd()
    {
        isOver = true;
    }
}
