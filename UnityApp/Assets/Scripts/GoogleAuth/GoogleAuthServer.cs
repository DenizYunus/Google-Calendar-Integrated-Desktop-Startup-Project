using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

public class GoogleAuthServer : MonoBehaviour
{
    private HttpListener listener;
    private string redirectUri = "http://localhost:5000";
    public static GoogleAuthServer Instance;

    void Start()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    public GoogleAuthServer()
    {
        listener = new HttpListener();
        listener.Prefixes.Add(redirectUri + "/");
    }

    public async Task<string> GetAuthorizationCode()
    {
        listener.Start();

        HttpListenerContext context = await listener.GetContextAsync();
        HttpListenerRequest request = context.Request;

        // You can add error handling here - this example assumes that the request will contain a 'code' query parameter
        string code = Regex.Match(request.Url.Query, @"(?<=code=)[^&]+").Value;

        HttpListenerResponse response = context.Response;
        string responseString = "<html><body>You may now return to the application.</body></html>";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        System.IO.Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();

        listener.Stop();

        return code;
    }

    private void OnDestroy()
    {
        listener.Stop();
    }
}
