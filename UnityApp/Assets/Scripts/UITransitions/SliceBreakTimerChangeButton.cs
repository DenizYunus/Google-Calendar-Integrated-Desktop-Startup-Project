using TMPro;
using UnityEngine;

public class SliceBreakTimerChangeButton : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public int timerVal;
    public int maxTimerVal;
    public int minTimerVal;

    void Start()
    {

    }

    void Update()
    {

    }

    public void IncreaseTimer()
    {
        if (timerVal <= maxTimerVal - 5)
        {
            timerVal += 5;
            timerText.text = timerVal + " mins";
        }
    }

    public void DecreaseTimer()
    {
        if (timerVal >= minTimerVal + 5)
        {
            timerVal -= 5;
            timerText.text = timerVal + " mins";
        }
    }
}
