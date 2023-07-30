using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CalendarUIObject : MonoBehaviour
{
    public TextMeshProUGUI titleText;  // Drag your Text component here
    public TextMeshProUGUI dateText;
    public Button leftButton; // Drag your Button component here
    public Button rightButton; // Drag your Button component here
    public GameObject dayButtonPrefab; // Drag your day button prefab here
    public Transform dayButtonContainer; // Drag the parent object of the day buttons here
    public Button cancelButton; // Drag your Button component here
    public Button okButton; // Drag your Button component here

    private DateTime selectedDate = DateTime.Now;

    private void Start()
    {
        leftButton.onClick.AddListener(() =>
        {
            selectedDate = selectedDate.AddMonths(-1);
            UpdateCalendar();
        });

        rightButton.onClick.AddListener(() =>
        {
            selectedDate = selectedDate.AddMonths(1);
            UpdateCalendar();
        });

        UpdateCalendar();
    }

    private void UpdateCalendar()
    {
        titleText.text = selectedDate.ToString("MMMM yyyy");

        // Clear old day buttons
        foreach (Transform child in dayButtonContainer)
        {
            Destroy(child.gameObject);
        }

        DateTime startDate = new DateTime(selectedDate.Year, selectedDate.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);

        // Determine the first day of this month
        int leadingDays = ((int)startDate.DayOfWeek) % 7;

        // Get last month's date for leading days
        DateTime previousMonthDate = startDate.AddDays(-leadingDays);

        // Add the leading days from the last month
        for (int i = 0; i < leadingDays; i++)
        {
            DateTime dayDate = previousMonthDate.AddDays(i);
            Button dayButton = CreateDayButton(dayDate.Day.ToString(), false);
        }

        // Create the days of the month
        for (int i = 0; i < daysInMonth; i++)
        {
            DateTime dayDate = startDate.AddDays(i);
            Button dayButton = CreateDayButton(dayDate.Day.ToString(), true);
            dayButton.onClick.AddListener(() => { SelectDay(dayDate); });
        }

        // Optional: Add trailing days from the next month for a complete grid
        int trailingDays = (7 - ((daysInMonth + leadingDays) % 7)) % 7;
        DateTime nextMonthDate = startDate.AddMonths(1);

        for (int i = 0; i < trailingDays; i++)
        {
            DateTime dayDate = nextMonthDate.AddDays(i);
            Button dayButton = CreateDayButton(dayDate.Day.ToString(), false);
        }
    }

    private Button CreateDayButton(string dayNumber, bool isActiveMonth)
    {
        Button dayButton = Instantiate(dayButtonPrefab, dayButtonContainer).GetComponent<Button>();
        TextMeshProUGUI dayText = dayButton.GetComponentInChildren<TextMeshProUGUI>();
        dayText.text = dayNumber;

        // You might want to visually distinguish between active and inactive days
        dayText.color = isActiveMonth ? new Color(13 / 255f, 74 / 255f, 160 / 255f, 1) : Color.gray;

        if (!isActiveMonth)
            dayButton.GetComponent<TextOnHoverChange>().defaultColor = Color.gray;

        return dayButton;
    }




    public void SelectDay(DateTime day)
    {
        selectedDate = day;
        titleText.text = selectedDate.ToString("MMMM dd, yyyy");
        dateText.text = titleText.text;
        AddEventPageController.Instance.day = selectedDate;
    }
}
