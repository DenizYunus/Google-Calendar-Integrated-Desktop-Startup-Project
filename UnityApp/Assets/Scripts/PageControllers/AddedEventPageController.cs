using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddedEventPageController : MonoBehaviour
{
    public static AddedEventPageController Instance;
    bool isInitializing = true;

    public TextMeshProUGUI eventNameText;
    public TextMeshProUGUI startDateText;
    public TextMeshProUGUI dateText;

    public List<Image> avatarIconImage;

    void OnEnable()
    {
        if (isInitializing)
        {
            if (Instance == null || Instance == this)
                Instance = this;
            else Destroy(this);
            isInitializing = false;
        }
        else
        {
            avatarIconImage.ForEach(x => x.sprite = Sprite.Create(StoreAndCommunication.Instance.profilePicture, new Rect(0, 0, StoreAndCommunication.Instance.profilePicture.width, StoreAndCommunication.Instance.profilePicture.height), new Vector2(0.5f, 0.5f)));
        }
    }
    
    public void LoadPage(string eventName, DateTime startTime)
    {
        eventNameText.text = eventName;
        startDateText.text = startTime.ToString("hh:mm tt");
        dateText.text = startTime.ToString("MMMM d, yyyy");
    }

    void Update()
    {
        
    }
}
