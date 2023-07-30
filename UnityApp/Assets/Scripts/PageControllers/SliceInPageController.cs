using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceInPageController : MonoBehaviour
{
    public SliceBreakTimerChangeButton sliceTime;
    public SliceBreakTimerChangeButton restTime;

    public void SliceIn()
    {
        NotificationManager.Instance.StartSlice(sliceTime.timerVal * 60, restTime.timerVal * 60);
    }
}