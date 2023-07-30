using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddEventPageController : MonoBehaviour
{
    public static AddEventPageController Instance;
    bool isInitializing = true;

    string eventName = "";
    List<string> collaborators = new();
    List<string> collaboratorIDs = new();

    public ClockSystem clock;
    public CalendarUIObject calendar;

    public DateTime day;
    public DateTime startTime;
    public DateTime endTime;

    public TMP_InputField eventNameInputField;
    public TMP_InputField addCollaboratorInputField;
    public TextMeshProUGUI errorText;

    public Image avatarIconImage;

    public GameObject collaboratorProfilePicturesContainer;

    public GameObject submitButton;


    void OnEnable()
    {
        if (isInitializing)
        {
            if (Instance == null || Instance == this)
            {
                Instance = this;
                eventNameInputField.onEndEdit.AddListener((_eventName) => { eventName = _eventName; });
                addCollaboratorInputField.onEndEdit.AddListener(OnCollaboratorEnter);
            }
            else Destroy(this);
            isInitializing = false;
        }
        else
        {
            if (StoreAndCommunication.Instance.profilePicture != null && avatarIconImage.sprite == null)
                avatarIconImage.sprite = Sprite.Create(StoreAndCommunication.Instance.profilePicture, new Rect(0, 0, StoreAndCommunication.Instance.profilePicture.width, StoreAndCommunication.Instance.profilePicture.height), new Vector2(0.5f, 0.5f));
            calendar.SelectDay(DateTime.Today);
        }
    }

    private void OnCollaboratorEnter(string collaboratorUsername)
    {
        collaborators.Add(collaboratorUsername);
        addCollaboratorInputField.text = "";


        StartCoroutine(APICommunication.GetByUsername(collaboratorUsername, (usernameProfileJSON) =>
        {
            collaboratorIDs.Add(JsonConvert.DeserializeObject<ProfilePictureAndIDClass>(usernameProfileJSON).id);
            if (!string.IsNullOrEmpty(JsonConvert.DeserializeObject<ProfilePictureAndIDClass>(usernameProfileJSON).profilePictureURL))
                StartCoroutine(APICommunication.DownloadTexture(JsonConvert.DeserializeObject<ProfilePictureAndIDClass>(usernameProfileJSON).profilePictureURL, (tex) =>
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        GameObject collabImage = new();
                        collabImage.AddComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        collabImage.transform.SetParent(collaboratorProfilePicturesContainer.transform);
                        collabImage.GetComponent<RectTransform>().sizeDelta = new Vector2(28, 28);
                    });
                }));
        }));

        Debug.Log(collaboratorUsername);
    }

    public class ProfilePictureAndIDClass
    {
        public string id { get; set; }
        public string profilePictureURL { get; set; }
    }

    public void AddEvent()
    {
        if (string.IsNullOrEmpty(eventName))
        {
            errorText.text = "Event Name can't be blank.";
            return;
        }
        if (day == null || day.Year == 1)
        {
            errorText.text = "Hey, don't forget to choose the day.";
            return;
        }
        startTime = new DateTime(day.Year, day.Month, day.Day, startTime.Hour, startTime.Minute, startTime.Second);
        endTime = new DateTime(day.Year, day.Month, day.Day, endTime.Hour, endTime.Minute, endTime.Second);
        if (endTime <= startTime)
        {
            errorText.text = "End Time cannot be before than Start Time.";
            return;
        }
        submitButton.SetActive(false);
        StartCoroutine(APICommunication.AddEvent(eventName, collaboratorIDs.ToArray(), startTime, endTime, (statusCode, responseText) =>
        {
            if (statusCode == 200)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => ResetPage());
            }
            else Debug.Log(responseText);
        }, (err) => Debug.Log(err)));
    }

    void ResetPage()
    {
        eventNameInputField.text = "";
        collaboratorIDs.Clear();
        collaborators.Clear();
        for (int i = 0; i < collaboratorProfilePicturesContainer.transform.childCount; i++)
        {
            Destroy(collaboratorProfilePicturesContainer.transform.GetChild(i).gameObject);
        }
        AddedEventPageController.Instance.LoadPage(eventName, startTime);
        calendar.SelectDay(DateTime.Today);

        submitButton.SetActive(true);
        clock.SwitchToStartTimeSelection();
        AllPagesController.Instance.MoveTab(AllPagesController.TabName.ExpandedAddedEvent);
    }

    void Update()
    {

    }
}