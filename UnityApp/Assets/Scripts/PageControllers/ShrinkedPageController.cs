using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using UnityEngine;
using static MainScreenPageController;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
using System.Runtime.InteropServices;
#endif

public class ShrinkedPageController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenChatGPT()
    {
        Application.OpenURL("https://chat.openai.com");
    }

    public void ImmediateMeeting()
    {
        StartCoroutine(APICommunication.CreateImmediateMeeting((_, r) => { Application.OpenURL(JsonConvert.DeserializeObject<GoogleMeetingLinkJSONClass>(r).googleMeetLink); }, null));
    }

    public void JoinUpcomingMeeting()
    {
        StartCoroutine(APICommunication.GetUpcoming(1, (json) =>
        {
            List<UpcomingEvent> upcomingEvents = new();

            List<UpcomingEventJSON> upcomingEventsText = JsonConvert.DeserializeObject<List<UpcomingEventJSON>>(json);
            foreach (UpcomingEventJSON upcoming in upcomingEventsText)
            {
                UpcomingEvent newEvent = new()
                {
                    name = upcoming.name,
                    meetingLink = upcoming.meetingLink,
                    description = upcoming.description,
                    startAt = DateTime.Parse(upcoming.startAt),
                    endAt = DateTime.Parse(upcoming.endAt)
                };
                upcomingEvents.Add(newEvent);
            }

            upcomingEvents.RemoveAll(e => e.startAt.Day != DateTime.Now.Day);

            if (upcomingEvents.Count <= 0)
            {
                return;
            }

            if ((upcomingEvents[0].startAt - DateTime.Now).TotalMinutes < 60)
            {
                Application.OpenURL(upcomingEvents[0].meetingLink);
            }
        }));
    }

    public void CreateDoc()
    {
        Application.OpenURL("https://docs.google.com/document/create");
    }

    public void OpenGmailInbox()
    {
        Application.OpenURL("https://mail.google.com/mail/");
    }

    public void CreateNewMail()
    {
        Application.OpenURL("https://mail.google.com/mail/#inbox?compose=new");
    }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

    private const byte VK_MEDIA_PLAY_PAUSE = 0xB3;
    private const int KEYEVENTF_EXTENDEDKEY = 0x0001;
    private const int KEYEVENTF_KEYUP = 0x0002;
#endif

    public void PlayPauseMedia()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENDEDKEY, 0);
        keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
#endif
    }
}

public class GoogleMeetingLinkJSONClass
{
    public string googleMeetLink { get; set; }
}