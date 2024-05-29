using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SurvivalTimer : MonoBehaviour
{
    public float survivalTime = 0f;
    public bool isCounting = true;
    public TMP_Text timerText;

    void Update()
    {
        if (isCounting)
        {
            survivalTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            float time = survivalTime;
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
            timerText.text = timeString;
        }
    }

    public void StopTimer()
    {
        isCounting = false;
    }
}