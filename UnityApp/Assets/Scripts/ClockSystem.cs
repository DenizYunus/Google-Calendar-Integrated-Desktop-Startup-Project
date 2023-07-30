using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClockSystem : MonoBehaviour
{
    public TextMeshProUGUI startEndText;
    public TextMeshProUGUI timeText; // Single text for both start and end time
    public TextMeshProUGUI mainTimeText;
    public Button amButton;
    public Button pmButton;
    public Button nextButton;
    public Button backButton;
    public Button selectMinutesButton;
    public Button changeHourButton;

    public Button[] numberButtons; // Assign your 12 number buttons here
    public Image lineImage;

    private int selectedHour = 12;
    private int selectedMinute = 0;
    private bool isSelectingMinutes = false;
    private bool isSelectingEndTime = false;
    private bool isPM = false;
    private TextMeshProUGUI currentTimeText;

    private void Start()
    {
        amButton.onClick.AddListener(() => { isPM = false; UpdateTime(); pmButton.GetComponent<TextMeshProUGUI>().color = new Color(126f / 255f, 95f / 255f, 250f / 255f, 165f / 255f); amButton.GetComponent<TextMeshProUGUI>().color = new Color(126f / 255f, 95f / 255f, 250f / 255f, 255f / 255f); });
        pmButton.onClick.AddListener(() => { isPM = true; UpdateTime(); amButton.GetComponent<TextMeshProUGUI>().color = new Color(126f / 255f, 95f / 255f, 250f / 255f, 165f / 255f); pmButton.GetComponent<TextMeshProUGUI>().color = new Color(126f / 255f, 95f / 255f, 250f / 255f, 255f / 255f); });
        selectMinutesButton.onClick.AddListener(SwitchToMinuteSelection);
        nextButton.onClick.AddListener(SwitchToEndTimeSelection);
        backButton.onClick.AddListener(SwitchToStartTimeSelection);
        changeHourButton.onClick.AddListener(SwitchToHourSelection);
        for (int i = 0; i < 12; i++)
        {
            int hour = (i + 1) % 12; // Assign the hours from 1 to 12
            numberButtons[i].onClick.AddListener(() => { SelectNumber(hour); });
        }
        currentTimeText = timeText;
        UpdateTime();
    }


    private string startTime = "";
    private string endTime = "";
    private void UpdateTime()
    {
        string time = selectedHour.ToString("D2") + ":" + selectedMinute.ToString("D2");// + (isPM ? " PM" : " AM");
        startEndText.text = isSelectingEndTime ? "End" : "Start";
        currentTimeText.text = time;

        string mainTime = selectedHour.ToString("D2") + ":" + selectedMinute.ToString("D2") + (isPM ? " PM" : " AM");

        if (isSelectingEndTime)
        {
            endTime = mainTime;
            mainTimeText.text = "Time: " + startTime + " - " + endTime;
            AddEventPageController.Instance.endTime = new System.DateTime(1, 1, 1, (selectedHour % 12) + (isPM ? 12 : 0), selectedMinute, 0);

        }
        else
        {
            startTime = mainTime;
            mainTimeText.text = "Time: " + startTime;
            AddEventPageController.Instance.startTime = new System.DateTime(1, 1, 1, (selectedHour % 12) + (isPM ? 12 : 0), selectedMinute, 0);
        }
    }

    private void SelectNumber(int number)
    {
        if (isSelectingMinutes)
        {
            selectedMinute = number * 5; // Each number represents 5 minutes
        }
        else
        {
            selectedHour = number == 0 ? 12 : number; // If number is 0, then it's 12 o'clock
        }
        UpdateTime();

        // Update the line to point towards the selected number
        float angle = GetClockAngle(number);
        lineImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private float GetClockAngle(int number)
    {
        // Convert the number to an angle, with 12 being at 90 degrees
        float anglePerNumber = 360f / 12f;
        float angle = 90f - (anglePerNumber * number);

        // Clamp the angle to the range [-180, 180]
        if (angle < -180f) angle += 360f;
        if (angle > 180f) angle -= 360f;

        return angle;
    }


    private void SwitchToHourSelection()
    {
        isSelectingMinutes = false;
        changeHourButton.gameObject.SetActive(false); // Hide the "Change Hour" button
        selectMinutesButton.gameObject.SetActive(true); // Show the "Select Minutes" button
        SelectNumber(selectedHour); // Re-select the current hour
        // You might want to change the UI to reflect that we're now selecting hours
    }

    private void SwitchToMinuteSelection()
    {
        isSelectingMinutes = true;
        selectMinutesButton.gameObject.SetActive(false); // Hide the "Select Minutes" button
        changeHourButton.gameObject.SetActive(true); // Show the "Change Hour" button
        SelectNumber(selectedMinute / 5); // Select the current minute
    }

    private void SwitchToEndTimeSelection()
    {
        isSelectingEndTime = true;
        backButton.gameObject.SetActive(true);
        nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ready To Go!";
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() => { AddEventPageController.Instance.AddEvent(); });
        ResetTime();
        // You might want to change the UI to reflect that we're now selecting the end time
    }

    public void SwitchToStartTimeSelection()
    {
        isSelectingEndTime = false;
        backButton.gameObject.SetActive(false);
        nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next!";
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(SwitchToEndTimeSelection);
        ResetTime();
        // You might want to change the UI to reflect that we're now selecting the start time
    }

    public void ResetTime()
    {
        float angle = GetClockAngle(0);
        lineImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        selectedHour = 12;
        selectedMinute = 0;
        isPM = false;
        isSelectingMinutes = false;
        selectMinutesButton.gameObject.SetActive(true);
        changeHourButton.gameObject.SetActive(false);
        UpdateTime();
    }
}
