using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGoalsManager : MonoBehaviour
{
    public GameObject goalTemplate;

    int goalCount = 1;

    public void AddGoal()
    {
        var temp = Instantiate(goalTemplate, goalTemplate.transform.parent);
        temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(goalTemplate.GetComponent<RectTransform>().anchoredPosition.x, - goalCount * goalTemplate.GetComponent<RectTransform>().sizeDelta.y + goalTemplate.GetComponent<RectTransform>().anchoredPosition.y);
        goalCount++;
        temp.SetActive(true);

        temp.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(temp.transform.parent.GetComponent<RectTransform>().sizeDelta.x, goalCount * goalTemplate.GetComponent<RectTransform>().sizeDelta.y + 10);
    }
}
