using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliceInChooseCategoryPrefabScript : MonoBehaviour
{
    [Header("General")]
    public Image backgroundImage;
    public bool expanded;
    public GameObject legalResearchEtcText;

    [Header("Shrinked")]
    public Sprite shrinkedBackground;

    [Header("Expanded")]
    public Sprite expandedBackground;
    public GameObject customText;
    public GameObject todoText;

    [Header("Todo")]
    public GameObject todoTemplate;
    //public string[] todoItemImageURLs;
    //public string[] todoItemTexts;
    public GameObject chooseToDoCategory;
    public GameObject chooseTodoItems;

    public Sprite personalIcon;
    public Sprite workIcon;
    public Sprite educationIcon;

    public void ShrinkOrExpand()
    {
        customText.SetActive(!expanded);
        todoText.SetActive(!expanded);

        if (expanded)
        {
            backgroundImage.sprite = shrinkedBackground;
            backgroundImage.rectTransform.sizeDelta = new Vector2(backgroundImage.rectTransform.sizeDelta.x, 24);

            legalResearchEtcText.SetActive(true);

            for (int i = 1; i < todoTemplate.transform.parent.childCount; i++)
            {
                Destroy(todoTemplate.transform.parent.GetChild(i).gameObject);
            }

            chooseTodoItems.SetActive(false);
        }
        else
        {
            backgroundImage.sprite = expandedBackground;
            backgroundImage.rectTransform.sizeDelta = new Vector2(backgroundImage.rectTransform.sizeDelta.x, 72);
        }

        expanded = !expanded;
    }

    public void ShowWorkEducationPersonal()
    {
        customText.SetActive(false);
        todoText.SetActive(false);
        legalResearchEtcText.SetActive(false);
        chooseToDoCategory.SetActive(true);
    }

    public void CustomGoalSelected()
    {
        ShrinkOrExpand();
        legalResearchEtcText.GetComponent<TextMeshProUGUI>().text = "custom selected";
    }

    public void ShowChooseItems(string category)
    {
        chooseToDoCategory.SetActive(false);
        chooseTodoItems.SetActive(true);

        TaskCategory taskCategory;
        if (category == "Personal")
            taskCategory = TaskCategory.Personal;
        else if (category == "Work")
            taskCategory = TaskCategory.Work;
        else
            taskCategory = TaskCategory.Education;

        StartCoroutine(APICommunication.GetToDoByCategory(taskCategory, (response) =>
        {
            TaskJson json = JsonConvert.DeserializeObject<TaskJson>(response);
            if (json != null)
            {
                List<string> todoItems = new();

                foreach (var task in json.tasks)
                {
                    if (task.status == "NextUp")
                        todoItems.Add(task.content);
                    ///TODO: NEXT UP WILL BE MOVED TO IN PROGRESS AUTOMATICALLY
                    else if (task.status == "InProgress")
                        todoItems.Add(task.content);
                    else continue;
                }

                chooseTodoItems.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(chooseTodoItems.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta.x, todoItems.Count * todoTemplate.GetComponent<RectTransform>().sizeDelta.y + 10);
                for (int i = 0; i < todoItems.Count; i++)
                {
                    var temp = Instantiate(todoTemplate, todoTemplate.transform.parent);
                    temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(todoTemplate.GetComponent<RectTransform>().anchoredPosition.x, -i * todoTemplate.GetComponent<RectTransform>().sizeDelta.y + todoTemplate.GetComponent<RectTransform>().anchoredPosition.y);
                    if (category == "Personal")
                        temp.transform.GetChild(0).GetComponent<Image>().sprite = personalIcon;
                    else if (category == "Work")
                        temp.transform.GetChild(0).GetComponent<Image>().sprite = workIcon;
                    else if (category == "Education")
                        temp.transform.GetChild(0).GetComponent<Image>().sprite = educationIcon;
                    temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = todoItems[i];
                    foreach (var button in temp.GetComponentsInChildren<Button>())
                    {
                        var todoItem = todoItems[i];
                        button.onClick.AddListener(() =>
                        {
                            legalResearchEtcText.GetComponent<TextMeshProUGUI>().text = todoItem;
                            ShrinkOrExpand();
                        });
                    }
                    temp.SetActive(true);
                }
            }
        },
        null));
    }
}