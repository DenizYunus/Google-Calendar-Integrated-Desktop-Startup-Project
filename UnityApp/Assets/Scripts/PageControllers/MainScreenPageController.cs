using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenPageController : MonoBehaviour
{
    [Header("Upcoming Next")]
    public TextMeshProUGUI upcomingEventNameText;
    public TextMeshProUGUI upcomingEventTimeText;
    public Button upcomingEventJoinButton;
    public Slider upcomingEventRemainingTimeSlider;
    public TextMeshProUGUI upcomingEventRemainingTimeText;

    [Header("Upcoming 1")]
    public TextMeshProUGUI upcomingEvent1NameText;
    public TextMeshProUGUI upcomingEvent1TimeText;

    [Header("Upcoming 2")]
    public TextMeshProUGUI upcomingEvent2NameText;
    public TextMeshProUGUI upcomingEvent2TimeText;

    [Header("Upcoming 3")]
    public TextMeshProUGUI upcomingEvent3NameText;
    public TextMeshProUGUI upcomingEvent3TimeText;

    bool isInitializing = true;

    void OnEnable()
    {
        if (isInitializing)
        {
            isInitializing = false;
            return;
        }

        StartCoroutine(APICommunication.GetUpcoming(4, FillUpcomings, null));
    }

    public void GoToSlicesPage()
    {
        if (NotificationManager.Instance.SliceStatus != SliceStatus.Idle)
        {
            AllPagesController.Instance.MoveTab(AllPagesController.TabName.ExpandedSlicedIn);
        } else AllPagesController.Instance.MoveTab(AllPagesController.TabName.ExpandedSliceIn);
    }

    void FillUpcomings(string json)
    {
        List<UpcomingEvent> upcomingEvents = new();

        List<UpcomingEventJSON> upcomingEventsText = JsonConvert.DeserializeObject<List<UpcomingEventJSON>>(json);
        foreach (UpcomingEventJSON upcoming in upcomingEventsText)
        {
            UpcomingEvent newEvent = new();
            newEvent.name = upcoming.name;
            newEvent.meetingLink = upcoming.meetingLink;
            newEvent.description = upcoming.description;
            newEvent.startAt = DateTime.Parse(upcoming.startAt);
            newEvent.endAt = DateTime.Parse(upcoming.endAt);
            upcomingEvents.Add(newEvent);
        }

        upcomingEvents.RemoveAll(e => e.startAt.Day != DateTime.Now.Day);

        if (upcomingEvents.Count <= 0)
        {
            upcomingEventNameText.text = "No Upcoming Meeting!";
            upcomingEventTimeText.text = "";
            upcomingEventRemainingTimeText.text = "";
            upcomingEventRemainingTimeSlider.value = 0;
            upcomingEventJoinButton.onClick.RemoveAllListeners();
            upcomingEvent1NameText.text = "It's so quite here :)";
            upcomingEvent1TimeText.text = "ssh...";
            upcomingEvent2NameText.text = "It's so quite here :)";
            upcomingEvent2TimeText.text = "ssh...";
            upcomingEvent3NameText.text = "It's so quite here :)";
            upcomingEvent3TimeText.text = "ssh...";
            return;
        }


        if ((upcomingEvents[0].startAt - DateTime.Now).TotalMinutes < 60)
        {
            upcomingEvents = upcomingEvents.OrderBy(e => e.startAt).ToList();
            upcomingEventNameText.text = upcomingEvents[0].name;
            upcomingEventTimeText.text = upcomingEvents[0].startAt.ToString("h.mm tt") + " - " + upcomingEvents[0].endAt.ToString("h.mm tt");
            upcomingEventJoinButton.onClick.RemoveAllListeners();
            upcomingEventJoinButton.onClick.AddListener(() => Application.OpenURL(upcomingEvents[0].meetingLink));
            upcomingEventRemainingTimeSlider.value = 1 - ((float)((upcomingEvents[0].startAt - DateTime.Now)).TotalMinutes / 60f);
            upcomingEventRemainingTimeText.text = (int)((upcomingEvents[0].startAt - DateTime.Now)).TotalMinutes + " Min Left";
            if (upcomingEvents.Count >= 2)
            {
                upcomingEvent1NameText.text = upcomingEvents[1].name;
                upcomingEvent1TimeText.text = upcomingEvents[1].startAt.ToString("h.mm tt") + " - " + upcomingEvents[1].endAt.ToString("h.mm tt");
                if (upcomingEvents.Count >= 3)
                {
                    upcomingEvent2NameText.text = upcomingEvents[2].name;
                    upcomingEvent2TimeText.text = upcomingEvents[2].startAt.ToString("h.mm tt") + " - " + upcomingEvents[2].endAt.ToString("h.mm tt");
                    if (upcomingEvents.Count >= 4)
                    {
                        upcomingEvent3NameText.text = upcomingEvents[3].name;
                        upcomingEvent3TimeText.text = upcomingEvents[3].startAt.ToString("h.mm tt") + " - " + upcomingEvents[3].endAt.ToString("h.mm tt");

                    }
                    else
                    {
                        upcomingEvent3NameText.text = "It's so quite here :)";
                        upcomingEvent3TimeText.text = "ssh...";
                    }
                }
                else
                {
                    upcomingEvent2NameText.text = "It's so quite here :)";
                    upcomingEvent2TimeText.text = "ssh...";
                    upcomingEvent3NameText.text = "It's so quite here :)";
                    upcomingEvent3TimeText.text = "ssh...";
                }
            }
            else
            {
                upcomingEvent1NameText.text = "It's so quite here :)";
                upcomingEvent1TimeText.text = "ssh...";
                upcomingEvent2NameText.text = "It's so quite here :)";
                upcomingEvent2TimeText.text = "ssh...";
                upcomingEvent3NameText.text = "It's so quite here :)";
                upcomingEvent3TimeText.text = "ssh...";
            }
        }
        else
        {
            upcomingEvents = upcomingEvents.OrderBy(e => e.startAt).ToList();

            upcomingEventNameText.text = "No Upcoming Meeting!";
            upcomingEventTimeText.text = "";
            upcomingEventRemainingTimeText.text = "";
            upcomingEventRemainingTimeSlider.value = 0;
            upcomingEventJoinButton.onClick.RemoveAllListeners();
            //upcomingEventTimeText.text = upcomingEvents[0].startAt.ToString("h.mm tt") + " - " + upcomingEvents[0].endAt.ToString("h.mm tt");
            if (upcomingEvents.Count >= 1)
            {
                upcomingEvent1NameText.text = upcomingEvents[0].name;
                upcomingEvent1TimeText.text = upcomingEvents[0].startAt.ToString("h.mm tt") + " - " + upcomingEvents[0].endAt.ToString("h.mm tt");
                if (upcomingEvents.Count >= 2)
                {
                    upcomingEvent2NameText.text = upcomingEvents[1].name;
                    upcomingEvent2TimeText.text = upcomingEvents[1].startAt.ToString("h.mm tt") + " - " + upcomingEvents[1].endAt.ToString("h.mm tt");
                    if (upcomingEvents.Count >= 3)
                    {
                        upcomingEvent3NameText.text = upcomingEvents[2].name;
                        upcomingEvent3TimeText.text = upcomingEvents[2].startAt.ToString("h.mm tt") + " - " + upcomingEvents[2].endAt.ToString("h.mm tt");

                    }
                    else
                    {
                        upcomingEvent3NameText.text = "It's so quite here :)";
                        upcomingEvent3TimeText.text = "ssh...";
                    }
                }
                else
                {
                    upcomingEvent2NameText.text = "It's so quite here :)";
                    upcomingEvent2TimeText.text = "ssh...";
                    upcomingEvent3NameText.text = "It's so quite here :)";
                    upcomingEvent3TimeText.text = "ssh...";
                }
            }
            else
            {
                upcomingEvent1NameText.text = "It's so quite here :)";
                upcomingEvent1TimeText.text = "ssh...";
                upcomingEvent2NameText.text = "It's so quite here :)";
                upcomingEvent2TimeText.text = "ssh...";
                upcomingEvent3NameText.text = "It's so quite here :)";
                upcomingEvent3TimeText.text = "ssh...";
            }
        }
    }

    public class UpcomingEventJSON
    {
        public string name { get; set; }
        public string description { get; set; }
        public string meetingLink { get; set; }
        public string startAt { get; set; }
        public string endAt { get; set; }
    }

    public class UpcomingEvent
    {
        public string name { get; set; }
        public string description { get; set; }
        public string meetingLink { get; set; }
        public DateTime startAt { get; set; }
        public DateTime endAt { get; set; }
    }

    void Update()
    {

    }
}
