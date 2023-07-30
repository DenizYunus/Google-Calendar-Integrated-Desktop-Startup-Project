using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ToDoPageController : MonoBehaviour
{
    public static ToDoPageController Instance;
    bool isInitializing = true;

    public GameObject personalNextUpContainer;
    public GameObject personalInProgressContainer;
    public GameObject personalCompletedContainer;

    public GameObject workNextUpContainer;
    public GameObject workInProgressContainer;
    public GameObject workCompletedContainer;

    public GameObject educationNextUpContainer;
    public GameObject educationInProgressContainer;
    public GameObject educationCompletedContainer;

    public TMP_InputField addPersonalInputField;
    public TMP_InputField addWorkInputField;
    public TMP_InputField addEducationInputField;

    public GameObject taskPrefab;

    void OnEnable()
    {
        if (isInitializing)
        {
            if (Instance == null || Instance == this)
            {
                Instance = this;
                addPersonalInputField.onEndEdit.AddListener((taskName) =>
                {
                    if (taskName == "") return;
                    StartCoroutine(APICommunication.AddToDoByCategory(taskName, TaskCategory.Personal, (_, id) =>
                    {
                        addPersonalInputField.text = "";
                        GameObject taskObject = Instantiate(taskPrefab);
                        taskObject.GetComponent<TaskItemObject>().UseParameters(id, taskName, TaskCategory.Personal, TaskStatus.NextUp);
                    }, null));
                });

                addWorkInputField.onEndEdit.AddListener((taskName) =>
                {
                    if (taskName == "") return;
                    StartCoroutine(APICommunication.AddToDoByCategory(taskName, TaskCategory.Work, (_, id) =>
                    {
                        addWorkInputField.text = "";
                        GameObject taskObject = Instantiate(taskPrefab);
                        taskObject.GetComponent<TaskItemObject>().UseParameters(id, taskName, TaskCategory.Work, TaskStatus.NextUp);
                    }, null));
                });

                addEducationInputField.onEndEdit.AddListener((taskName) =>
                {
                    if (taskName == "") return;
                    StartCoroutine(APICommunication.AddToDoByCategory(taskName, TaskCategory.Education, (_, id) =>
                    {
                        addEducationInputField.text = "";
                        GameObject taskObject = Instantiate(taskPrefab);
                        taskObject.GetComponent<TaskItemObject>().UseParameters(id, taskName, TaskCategory.Education, TaskStatus.NextUp);
                    }, null));
                });
            }
            else Destroy(this);
            isInitializing = false;
        }
        else
        {
            foreach (Transform childTransform in personalNextUpContainer.transform)
            {
                if (childTransform.name != "AddTask")
                {
                    Destroy(childTransform.gameObject);
                }
            }

            foreach (Transform childTransform in personalInProgressContainer.transform)
            {
                Destroy(childTransform.gameObject);
            }

            foreach (Transform childTransform in personalCompletedContainer.transform)
            {
                Destroy(childTransform.gameObject);
            }

            foreach (Transform childTransform in workNextUpContainer.transform)
            {
                if (childTransform.name != "AddTask")
                    Destroy(childTransform.gameObject);
            }

            foreach (Transform childTransform in workInProgressContainer.transform)
            {
                Destroy(childTransform.gameObject);
            }

            foreach (Transform childTransform in workCompletedContainer.transform)
            {
                Destroy(childTransform.gameObject);
            }

            foreach (Transform childTransform in educationNextUpContainer.transform)
            {
                if (childTransform.name != "AddTask")
                    Destroy(childTransform.gameObject);
            }

            foreach (Transform childTransform in educationInProgressContainer.transform)
            {
                Destroy(childTransform.gameObject);
            }

            foreach (Transform childTransform in educationCompletedContainer.transform)
            {
                Destroy(childTransform.gameObject);
            }
            //personalNextUpContainer.transform.GetComponentsInChildren<Transform>(true).ToList().ForEach(x => { if (x.name != "AddTask") Destroy(x.gameObject); });
            //personalInProgressContainer.transform.GetComponentsInChildren<Transform>(true).ToList().ForEach(x => { if (x.name != "AddTask") Destroy(x.gameObject); });
            //personalCompletedContainer.transform.GetComponentsInChildren<Transform>(true).ToList().ForEach(x => { if (x.name != "AddTask") Destroy(x.gameObject); });
            //workNextUpContainer.transform.GetComponentsInChildren<Transform>(true).ToList().ForEach(x => Destroy(x.gameObject));
            //workInProgressContainer.transform.GetComponentsInChildren<Transform>(true).ToList().ForEach(x => Destroy(x.gameObject));
            //workCompletedContainer.transform.GetComponentsInChildren<Transform>(true).ToList().ForEach(x => Destroy(x.gameObject));
            //educationNextUpContainer.transform.GetComponentsInChildren<Transform>(true).ToList().ForEach(x => Destroy(x.gameObject));
            //educationInProgressContainer.transform.GetComponentsInChildren<Transform>(true).ToList().ForEach(x => Destroy(x.gameObject));
            //educationCompletedContainer.transform.GetComponentsInChildren<Transform>(true).ToList().ForEach(x => Destroy(x.gameObject));

            List<TaskCategory> taskCategories = new() { TaskCategory.Personal, TaskCategory.Work, TaskCategory.Education };
            foreach (TaskCategory taskCategory in taskCategories)
            {
                StartCoroutine(APICommunication.GetToDoByCategory(taskCategory, (response) =>
                {
                    TaskJson json = JsonConvert.DeserializeObject<TaskJson>(response);
                    if (json != null)
                    {
                        foreach (var task in json.tasks)
                        {
                            TaskStatus _status;
                            if (task.status == "NextUp")
                                _status = TaskStatus.NextUp;
                            else if (task.status == "InProgress")
                                _status = TaskStatus.InProgress;
                            else
                                _status = TaskStatus.Completed;

                            GameObject taskObject = Instantiate(taskPrefab);
                            taskObject.GetComponent<TaskItemObject>().UseParameters(task.id, task.content, taskCategory, _status);
                        }
                    }
                },
                null));
            }
        }
    }

    public void ClearCompletedToDos(string taskCategory)
    {
        StartCoroutine(APICommunication.ClearCompletedInCategory(taskCategory, (_, _) => { OnEnable(); }, null));
    }

    void Update()
    {

    }
}

public class TaskJson
{
    public TaskJSONItem[] tasks { get; set; }
    public string deadLine { get; set; }
}

public class TaskJSONItem
{
    public string id { get; set; }
    public string content { get; set; }
    public string status { get; set; }
    public string createdAt { get; set; }
}