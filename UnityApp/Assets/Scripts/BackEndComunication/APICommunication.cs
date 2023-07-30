using Assets.Scripts;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class APICommunication : MonoBehaviour
{

    public static async Task GoogleLogin(string authorizationCode)
    {

        Dictionary<string, object> parameters = new ()
        {
            {
                "AuthCode", authorizationCode
            }
        };

        await SendPostRequestWithParameter(APIUrls.GoogleLogin, parameters, (_, jsonResponse) => StoreAndCommunication.Instance.UpdateInfoFromJSON(jsonResponse)) ;
    }

    public static IEnumerator GetToDoByCategory(TaskCategory category, Action<string> onSuccess, Action<string> onError = null)
    {
        string url;
        if (category == TaskCategory.Personal)
            url = $"{APIUrls.GetToDoByCategory}?Category=Personal";
        else if (category == TaskCategory.Work)
            url = $"{APIUrls.GetToDoByCategory}?Category=Work";
        else
            url = $"{APIUrls.GetToDoByCategory}?Category=Education";
        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("Authorization", $"Bearer {StoreAndCommunication.Instance.AccessToken}");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.result);
            onError?.Invoke(request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;

            onSuccess?.Invoke(responseText);
        }
        request.Dispose();
    }

    public static IEnumerator AddToDoByCategory(string name, TaskCategory category, Action<int, string> onSuccess, Action<string> onError)
    {
        string url = APIUrls.AddToDoByCategory;

        string categoryString;
        if (category == TaskCategory.Personal)
            categoryString = "Personal";
        else if (category == TaskCategory.Work)
            categoryString = "Work";
        else
            categoryString = "Education";

        url += "?Category=" + categoryString;

        Dictionary<string, object> data = new ()
        {
            { "task", name }
        };

        //if (deadline != null)
        //    data.Add("deadLine", deadline?.ToString("o"));

        Task task = SendPostRequestWithParameter(url, data, onSuccess, onError);

        yield return new WaitUntil(() => task.IsCompleted);
    }

    public static IEnumerator SetToDoStatus(string id, TaskCategory category, TaskStatus status, Action<int, string> onSuccess, Action<string> onError)
    {
        string url = APIUrls.SetToDoStatus;

        string categoryString;
        if (category == TaskCategory.Personal)
            categoryString = "Personal";
        else if (category == TaskCategory.Work)
            categoryString = "Work";
        else
            categoryString = "Education";

        string statusString = status.ToString();

        url += "/" + id + "?category=" + categoryString + "&status=" + statusString;

        //if (deadline != null)
        //    data.Add("deadLine", deadline?.ToString("o"));

        Task task = SendPostRequestWithParameter(url, null, onSuccess, onError);

        yield return new WaitUntil(() => task.IsCompleted);
    }

    public static IEnumerator SetDeadlineOfCategory(TaskCategory category, DateTime deadline, Action<int, string> onSuccess, Action<string> onError)
    {
        string url = APIUrls.SetToDoCategoryDeadline;

        string categoryString = category.ToString();

        url += "?Category=" + categoryString;

        Dictionary<string, object> data = new()
        {
            { "deadline", deadline.ToString("o") }
        };

        //if (deadline != null)
        //    data.Add("deadLine", deadline?.ToString("o"));

        Task task = SendPostRequestWithParameter(url, data, onSuccess, onError);

        yield return new WaitUntil(() => task.IsCompleted);
    }

    public static IEnumerator ClearCompletedInCategory(string taskCategory, Action<int, string> onSuccess, Action<string> onError)
    {
        string url = $"{APIUrls.ClearToDoCategory}?Category={taskCategory}";
        UnityWebRequest request = UnityWebRequest.Delete(url);

        request.SetRequestHeader("Authorization", $"Bearer {StoreAndCommunication.Instance.AccessToken}");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.result);
            onError?.Invoke(request.error);
        }
        else
        {
            string responseText = request.result.ToString();

            onSuccess?.Invoke((int)request.responseCode, responseText);
        }
        request.Dispose();
    }

    public static IEnumerator GetUpcoming(int count, Action<string> onSuccess, Action<string> onError = null)
    {
        string url = $"{APIUrls.GetUpcomingList}?EventCount={count}";

        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("Authorization", $"Bearer {StoreAndCommunication.Instance.AccessToken}");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.result);
            onError?.Invoke(request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;

            onSuccess?.Invoke(responseText);
        }
    }

    public static IEnumerator AddEvent(string name, string[] collaborators, DateTime startAt, DateTime endAt, Action<int, string> onSuccess, Action<string> onError)
    {
        string url = APIUrls.AddEventPage;

        if (collaborators.Length > 0) url += "?";

        for (int i = 0; i < collaborators.Length; i++)
        {
            if (i > 0) url += "&";
            url += $"Collaborators%5B{i}%5D={collaborators[i]}";
        }

        Task task = SendPostRequestWithParameter(url, new Dictionary<string, object>()
        {
            {"name", name },
            {"startAt", startAt.ToString("o") },
            {"endAt", endAt.ToString("o") },
            {"description", "" }
        }, onSuccess, onError);

        yield return new WaitUntil(() => task.IsCompleted);
    }

    public static IEnumerator CreateImmediateMeeting(Action<int, string> onSuccess, Action<string> onError)
    {
        string url = APIUrls.CreateImmediateMeeting;

        Task task = SendPostRequestWithParameter(url, null, onSuccess, onError);

        yield return new WaitUntil(() => task.IsCompleted);
    }

    public static IEnumerator GetByUsername(string userName, Action<string> onSuccess, Action<string> onError = null)
    {
        string url = $"{APIUrls.GetByUsername}?UserName={userName}";

        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("Authorization", $"Bearer {StoreAndCommunication.Instance.AccessToken}");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.result);
            onError?.Invoke(request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;

            onSuccess?.Invoke(responseText);
        }
    }

    public static IEnumerator DownloadTexture(string url, Action<Texture2D> onDownloadComplete)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("err");
        }

        var operation = request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("err");
        }

        while (!operation.isDone)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            var handler = (DownloadHandlerTexture)request.downloadHandler;
            var texture = handler.texture;

            UnityMainThreadDispatcher.Instance().Enqueue(() => onDownloadComplete(texture));
        }
    }

    public static async Task SendPostRequestWithParameter(string url, Dictionary<string, object> parameters, Action<int, string> onSuccess = null, Action<string> onError = null)
    {
        UnityWebRequest request = UnityWebRequest.Post(url, "");
        request.SetRequestHeader("Content-Type", "application/json");
        if (!string.IsNullOrEmpty(StoreAndCommunication.Instance.AccessToken))
            request.SetRequestHeader("Authorization", $"Bearer {StoreAndCommunication.Instance.AccessToken}");

        if (parameters != null)
        {
            string json = JsonConvert.SerializeObject(parameters);

            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }
        request.downloadHandler = new DownloadHandlerBuffer();


        AsyncOperation operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Delay(100);
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error + " " + request.downloadHandler.text);
            onError.Invoke(request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            onSuccess.Invoke((int)request.responseCode, responseText);
            Debug.Log(responseText);
            //return responseText;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}