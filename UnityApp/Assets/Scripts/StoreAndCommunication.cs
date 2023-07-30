using Newtonsoft.Json;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static StoreAndCommunication;

public class StoreAndCommunication : MonoBehaviour
{
    public static StoreAndCommunication Instance;

    public string username;
    public string email;
    public string fullName;
    public string profilePictureURL;
    public Texture2D profilePicture;

    public string AccessToken = "";

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this);

        DontDestroyOnLoad(this);

        if (PlayerPrefs.HasKey("signedIn"))
        {
            if (PlayerPrefs.GetInt("signedIn") == 1)
            {
                if (PlayerPrefs.HasKey("profilePictureURL"))
                {
                    username = PlayerPrefs.GetString("username");
                    email = PlayerPrefs.GetString("email");
                    fullName = PlayerPrefs.GetString("fullname");
                    AccessToken = PlayerPrefs.GetString("accessToken");
                    profilePictureURL = PlayerPrefs.GetString("profilePictureURL");
                    StartCoroutine(DownloadProfilePicture(profilePictureURL));
                }
            }
        }
        
    }

    void Update()
    {
        
    }

    public void LogOut()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        AllPagesController.Instance.MoveTab(1);
    }

    void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetInt("signedIn", 1);
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("email", email);
        PlayerPrefs.SetString("fullname", fullName);
        PlayerPrefs.SetString("accessToken", AccessToken);
        PlayerPrefs.SetString("profilePictureURL", profilePictureURL);
        PlayerPrefs.Save();
    }

    public void UpdateInfoFromJSON(string json)
    {
        LoginInfo loginInfo = JsonConvert.DeserializeObject<LoginInfo>(json);

        email = loginInfo.emailAddress;
        username = loginInfo.userName;
        fullName = loginInfo.name;
        AccessToken = loginInfo.accessToken;
        profilePictureURL = loginInfo.profilePictureURL;

        StartCoroutine(DownloadProfilePicture(profilePictureURL));

        SaveToPlayerPrefs();
        Debug.Log(email + username + fullName + " " + AccessToken);
    }

    public IEnumerator DownloadProfilePicture(string url)
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
            //UnityMainThreadDispatcher.Instance().Enqueue(() => Debug.Log(operation.progress));
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

            UnityMainThreadDispatcher.Instance().Enqueue(() => profilePicture = texture);
        }
    }

    public class LoginInfo
    {
        public string emailAddress { get; set; }
        public string userName { get; set; }
        public string name { get; set; }
        public string profilePictureURL { get; set; }
        public string accessToken { get; set; }
    }
}