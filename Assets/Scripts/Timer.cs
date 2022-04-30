using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Timer UI references: ")]
    [SerializeField] private Image uiFillImage;
    [SerializeField] private TMP_Text uiText;

    public int Duration { get; private set; } //readonly
    private int remainingDuration;

    private void Awake()
    {
        ResetTimer();
    }
    private void ResetTimer()
    {
        uiText.text = "00:00";
        uiFillImage.fillAmount = 0f;
        Duration = remainingDuration = 0;
    }
    public Timer SetDuration(int seconds)
    {
        Duration = remainingDuration = seconds;
        return this;
    }
    public void Begin()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateTimer());
    }
    private IEnumerator UpdateTimer()
    {
        while(remainingDuration > 0)
        {
            UpdateUI(remainingDuration);
            remainingDuration--;
            yield return new WaitForSeconds(1f);
        }
        End();
    }
    private void UpdateUI(int seconds)
    {
        //uiText.text = string.Format($"{seconds / 60}:{seconds % 60}");
        uiText.text = string.Format("{0:D2}:{1:D2}", (seconds / 60), (seconds % 60)); //printing 2 digits
        uiFillImage.fillAmount = Mathf.InverseLerp(0, Duration, seconds);
    }
    public void End()
    {
        ResetTimer();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
