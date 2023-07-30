using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    public SliceTimer sliceTimer;
    SliceStatus sliceStatus = SliceStatus.Idle;

    int sliceTime;
    int restTime;

    public GameObject sliceTimerObject;

    bool running = false;

    public SliceStatus SliceStatus
    {
        get { return sliceStatus; }
        set
        {
            sliceStatus = value;
            if (sliceStatus == SliceStatus.Slice)
            {
                sliceTimer.StartTimer(sliceTime);
            }
            if (SliceStatus == SliceStatus.Rest)
            {
                sliceTimer.StartTimer(restTime);
            }
        }
    }

    public void PauseResumeButton()
    {
        if (running)
            PauseSlice();
        else ResumeSlice();
    }

    public void StartSlice(int _sliceTime, int _restTime)
    {
        sliceTime = _sliceTime;
        restTime = _restTime;
        sliceTimerObject.SetActive(true);
        SliceStatus = SliceStatus.Slice;
        running = true;
    }

    public void CancelSlice()
    {
        SliceStatus = SliceStatus.Idle;
        sliceTimerObject.SetActive(false);
        running = false;
    }

    public void PauseSlice()
    {
        sliceTimer.PauseTimer();
        running = false;
    }

    public void ResumeSlice()
    {
        sliceTimer.ResumeTimer();
        running = true;
    }

    public void TimerEnded()
    {
        if (sliceStatus == SliceStatus.Slice)
            SliceStatus = SliceStatus.Rest;
        else if (sliceStatus == SliceStatus.Rest)
            SliceStatus = SliceStatus.Slice;
    }

    void Start()
    {
        if (Instance != null)
            Destroy(this);
        else Instance = this;
        sliceTimer.timerEnded = TimerEnded;
    }
}

public enum SliceStatus
{
    Idle,
    Slice,
    Rest
}