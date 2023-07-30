using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskItemObject : MonoBehaviour
{
    public Sprite nextUpIcon;
    public Sprite inProgressIcon;

    TextMeshProUGUI taskNameText;
    Image statusIcon;
    Button taskNameButton;
    Button statusIconButton;

    public string toDoName;
    public string toDoId;
    public TaskCategory category;
    public TaskStatus status;

    public void UseParameters(string _id, string _taskName, TaskCategory _category, TaskStatus _status = TaskStatus.NextUp)
    {
        taskNameText = GetComponentInChildren<TextMeshProUGUI>(true);
        statusIcon = GetComponentInChildren<Image>(true);
        taskNameButton = taskNameText.gameObject.GetComponent<Button>();
        statusIconButton = statusIcon.gameObject.GetComponent<Button>();
        taskNameText.text = _taskName;
        toDoName = _taskName;
        category = _category;
        toDoId = _id;

        Status = _status;
    }

    public TaskStatus Status
    {
        get { return status; }
        set
        {
            status = value;
            switch (status)
            {
                case TaskStatus.NextUp:
                    if (category == TaskCategory.Personal)
                        gameObject.transform.SetParent(ToDoPageController.Instance.personalNextUpContainer.transform);
                    else if (category == TaskCategory.Work)
                        gameObject.transform.SetParent(ToDoPageController.Instance.workNextUpContainer.transform);
                    else if (category == TaskCategory.Education)
                        gameObject.transform.SetParent(ToDoPageController.Instance.educationNextUpContainer.transform);
                    statusIcon.sprite = nextUpIcon;
                    taskNameText.fontStyle = FontStyles.Normal;
                    taskNameButton.onClick.RemoveAllListeners();
                    taskNameButton.onClick.AddListener(() => ChangeStatus(TaskStatus.InProgress));
                    statusIconButton.onClick.RemoveAllListeners();
                    statusIconButton.onClick.AddListener(() => ChangeStatus(TaskStatus.InProgress));
                    break;
                case TaskStatus.InProgress:
                    if (category == TaskCategory.Personal)
                        gameObject.transform.SetParent(ToDoPageController.Instance.personalInProgressContainer.transform);
                    else if (category == TaskCategory.Work)
                        gameObject.transform.SetParent(ToDoPageController.Instance.workInProgressContainer.transform);
                    else if (category == TaskCategory.Education)
                        gameObject.transform.SetParent(ToDoPageController.Instance.educationInProgressContainer.transform);
                    statusIcon.sprite = inProgressIcon;
                    taskNameText.fontStyle = FontStyles.Normal;
                    taskNameButton.onClick.RemoveAllListeners();
                    taskNameButton.onClick.AddListener(() => ChangeStatus(TaskStatus.Completed));
                    statusIconButton.onClick.RemoveAllListeners();
                    statusIconButton.onClick.AddListener(() => ChangeStatus(TaskStatus.NextUp));
                    break;
                case TaskStatus.Completed:
                    if (category == TaskCategory.Personal)
                        gameObject.transform.SetParent(ToDoPageController.Instance.personalCompletedContainer.transform);
                    else if (category == TaskCategory.Work)
                        gameObject.transform.SetParent(ToDoPageController.Instance.workCompletedContainer.transform);
                    else if (category == TaskCategory.Education)
                        gameObject.transform.SetParent(ToDoPageController.Instance.educationCompletedContainer.transform);
                    statusIcon.sprite = inProgressIcon;
                    taskNameText.fontStyle = FontStyles.Strikethrough;
                    taskNameButton.onClick.RemoveAllListeners();
                    taskNameButton.onClick.AddListener(() => ChangeStatus(TaskStatus.InProgress));
                    statusIconButton.onClick.RemoveAllListeners();
                    statusIconButton.onClick.AddListener(() => ChangeStatus(TaskStatus.NextUp));
                    break;
            }
        }
    }

    public void ChangeStatus(TaskStatus _status)
    {
        switch (_status)
        {
            case TaskStatus.NextUp:
                StartCoroutine(APICommunication.SetToDoStatus(toDoId, category, _status, (_, _) =>
                {
                    if (category == TaskCategory.Personal)
                        gameObject.transform.SetParent(ToDoPageController.Instance.personalNextUpContainer.transform);
                    else if (category == TaskCategory.Work)
                        gameObject.transform.SetParent(ToDoPageController.Instance.workNextUpContainer.transform);
                    else if (category == TaskCategory.Education)
                        gameObject.transform.SetParent(ToDoPageController.Instance.educationNextUpContainer.transform);
                    statusIcon.sprite = nextUpIcon;
                    taskNameText.fontStyle = FontStyles.Normal;
                    taskNameButton.onClick.RemoveAllListeners();
                    taskNameButton.onClick.AddListener(() => Status = TaskStatus.InProgress);
                    statusIconButton.onClick.RemoveAllListeners();
                    statusIconButton.onClick.AddListener(() => Status = TaskStatus.InProgress);
                }, null));
                break;
            case TaskStatus.InProgress:
                StartCoroutine(APICommunication.SetToDoStatus(toDoId, category, _status, (_, _) =>
                {
                    if (category == TaskCategory.Personal)
                        gameObject.transform.SetParent(ToDoPageController.Instance.personalInProgressContainer.transform);
                    else if (category == TaskCategory.Work)
                        gameObject.transform.SetParent(ToDoPageController.Instance.workInProgressContainer.transform);
                    else if (category == TaskCategory.Education)
                        gameObject.transform.SetParent(ToDoPageController.Instance.educationInProgressContainer.transform);
                    statusIcon.sprite = inProgressIcon;
                    taskNameText.fontStyle = FontStyles.Normal;
                    taskNameButton.onClick.RemoveAllListeners();
                    taskNameButton.onClick.AddListener(() => Status = TaskStatus.Completed);
                    statusIconButton.onClick.RemoveAllListeners();
                    statusIconButton.onClick.AddListener(() => Status = TaskStatus.NextUp);
                }, null));
                break;
            case TaskStatus.Completed:
                StartCoroutine(APICommunication.SetToDoStatus(toDoId, category, _status, (_, _) =>
                {
                    if (category == TaskCategory.Personal)
                        gameObject.transform.SetParent(ToDoPageController.Instance.personalCompletedContainer.transform);
                    else if (category == TaskCategory.Work)
                        gameObject.transform.SetParent(ToDoPageController.Instance.workCompletedContainer.transform);
                    else if (category == TaskCategory.Education)
                        gameObject.transform.SetParent(ToDoPageController.Instance.educationCompletedContainer.transform);
                    statusIcon.sprite = inProgressIcon;
                    taskNameText.fontStyle = FontStyles.Strikethrough;
                    taskNameButton.onClick.RemoveAllListeners();
                    taskNameButton.onClick.AddListener(() => Status = TaskStatus.InProgress);
                    statusIconButton.onClick.RemoveAllListeners();
                    statusIconButton.onClick.AddListener(() => Status = TaskStatus.NextUp);
                }, null));
                break;
        }
    }
}

public enum TaskStatus
{
    NextUp,
    InProgress,
    Completed
}

public enum TaskCategory
{
    Personal,
    Work,
    Education
}