using Core;
using Octopus.Client;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    private void Start()
    {
        PrintMessage("Start");

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => 
        {
            var dependencyStatus = task.Result;
            
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Init();
            }
            else
            {
                Debugger.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}",
                    dependencyStatus));
            }
        });
    }

    private void Init()
    {
        PrintMessage("Init");


        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        
        //var token = Firebase.Messaging.FirebaseMessaging.GetTokenAsync().Result;
    }

    private void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        PrintMessage("Received Registration Token: " + token.Token);
        
        if (PlayerPrefs.GetString("updateFirebase", "") == token.Token) return;
        
        PlayerPrefs.SetInt("newToken", 1);
        PlayerPrefs.SetString("updateFirebase", token.Token);
        PlayerPrefs.Save();
        
        GameSettings.SetValue(Constants.FcmTokenKey, token.Token);
    }

    private void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) 
    {
        PrintMessage("-------------------------notification--------------------------");
        PrintMessage("Received a new message from: " + e.Message.From);
        
        var notification = e.Message.Notification;
        
        if (notification != null) 
        {
            PrintMessage("title: " + notification.Title);
            PrintMessage("body: " + notification.Body);
            
            var android = notification.Android;
            if (android != null)
            {
                PrintMessage("android channel_id: " + android.ChannelId);
            }
        }
        else
        {
            PrintMessage("notification == null");
        }
        
        if (e.Message.From.Length > 0)
        {
            PrintMessage("from: " + e.Message.From);
        }
        if (e.Message.Link != null)
        {
            PrintMessage("link: " + e.Message.Link.ToString());
        }
        
        if (e.Message.Data.Count > 0) 
        {
            PrintMessage("data:");

            foreach (var (key, value) in e.Message.Data) 
            {
                PrintMessage(" - " + key + ": " + value);

                GameSettings.SetValue(key, value);
            }
            
            StartCoroutine(SenderRequest.Send(new TrackRequest(),null));
        }
        
        PrintMessage("-------------------------_______________--------------------------");
    }
    
    public void OnDestroy()
    {
        Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
        Firebase.Messaging.FirebaseMessaging.TokenReceived -= OnTokenReceived;
    }
    
    private void PrintMessage(string message)
    {
        //Debugger.Log($"@@@ FirebaseInit ->: {message}", new Color(0.9f, 0.1f, 0.3f));
    }
}
