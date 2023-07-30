using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSignInGetAuthCode : MonoBehaviour
{
    private string clientId = "yourClientIdHere";
    private string redirectUri = "http://localhost:5000";
    private string code;

    public GameObject signInWithButton;
    public LoadingCircleFiller loader;

    public async void SignIn()
    {
        string authUrl =
            "https://accounts.google.com/o/oauth2/v2/auth?" +
            "client_id=" + clientId +
            "&response_type=code" +
            //"&scope=openid%20email%20profile" +
            "&scope=https://www.googleapis.com/auth/calendar%20https://www.googleapis.com/auth/userinfo.email" +
            "&redirect_uri=" + UnityWebRequest.EscapeURL(redirectUri) +
            "&access_type=offline" +
            "&prompt=consent";
            //"&state=unity";

        Application.OpenURL(authUrl);

        loader.transform.parent.gameObject.SetActive(true);
        signInWithButton.SetActive(false);
        loader.StartAnimation();
        code = await GoogleAuthServer.Instance.GetAuthorizationCode();
        await APICommunication.GoogleLogin(code);
        loader.StopAnimation();
        signInWithButton.SetActive(true);
        loader.transform.parent.gameObject.SetActive(false);
        AllPagesController.Instance.MoveTab(AllPagesController.TabName.ExpandedMainScreen);
    }

    void Update()
    {

    }
}
