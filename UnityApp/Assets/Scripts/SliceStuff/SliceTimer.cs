using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SliceTimer : MonoBehaviour
{
    public List<TextMeshProUGUI> timerText;
    private float timeRemaining = 1800;

    bool timerRunning = false;

    public Action timerEnded;
    private Coroutine countdownCoroutine;
    private int pausedTimeRemaining;

    public void StartTimer(int timeInSeconds)
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        timerText.ForEach(x => x.text = FormatTime(timeRemaining));
        timeRemaining = timeInSeconds;
        timerRunning = true;
        countdownCoroutine = StartCoroutine(Countdown());
    }

    public void PauseTimer()
    {
        if (!timerRunning) return;
        timerRunning = false;
        pausedTimeRemaining = (int)timeRemaining;
    }

    public void ResumeTimer()
    {
        if (timerRunning) return;
        timeRemaining = pausedTimeRemaining;
        timerRunning = true;
        countdownCoroutine = StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (timeRemaining > 0 && timerRunning)
        {
            yield return new WaitForSeconds(1);
            timeRemaining--;
            timerText.ForEach(x => x.text = FormatTime(timeRemaining));
        }
        if (timeRemaining <= 0)
        {
            timerRunning = false;
            timerText.ForEach(x => x.text = "00:00:00");
            timerEnded.Invoke();
        }
    }
    
    private string FormatTime(float timeInSeconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(timeInSeconds);
        return string.Format("{0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
