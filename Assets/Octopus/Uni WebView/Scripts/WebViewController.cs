using System.Globalization;
using System.Linq;
using Core;
using Octopus.ExternalAppIntegration;
using Octopus.Client;
using Octopus.VerifyInternet;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Android;

public class WebViewController : MonoBehaviour
{
    [SerializeField, Header("Reference RectTransform")] private RectTransform _referenceRectTransform;
    
    [SerializeField, Header("Reload start page")] private bool canReload;
    
    private UniWebView _webView;

    private string _url;
    
    private bool _isVisible;

    private string UrlB
    {
        get
        {
            if(!GameSettings.HasKey(Constants.IsFirstRunWebView))
            {
                GameSettings.SetFirstWebView();
                
                return GameSettings.GetValue(Constants.ReceiveUrl, "");
            }
            else
            {
                var url = GameSettings.GetValue(Constants.StartUrl, "");
                
                if(!GameSettings.HasKey(Constants.StartUrl))
                    return GameSettings.GetValue(Constants.ReceiveUrl, "");
                
                if (!GameSettings.HasKey(Constants.LastUrl))
                {
                    GameSettings.SetValue(Constants.LastUrl, url);
                }
                else
                {
                    //Only start offer url
                    //url = GameSettings.GetValue(Constants.LastUrl, "");
                }

                return url;
            }
        }
        set 
        {
            if(!GameSettings.HasKey(Constants.StartUrl))
            {
                GameSettings.SetValue(Constants.StartUrl, value);
            }
            
            GameSettings.SetValue(Constants.LastUrl, value);
        }
    }
    
    private void Start()
    {
        InitializeWebView();
    }

    private void OnInitialize(bool? isConnection)
    {
        PrintMessage("### OnInitialize");
        
        CheckConnection(isConnection);
    }
    
    private void CheckConnection(bool? isConnection)
    {
        PrintMessage($"### CheckConnection: isConnection={isConnection}");
        
        if (isConnection != true) return;
        
        if(ConnectivityManager.Instance)
            ConnectivityManager.Instance.OnChangedInternetConnection.AddListener(OnInitialize);
            
        InitializeWebView();
    }

    private void InitializeWebView()
    {
        PrintMessage("### Initialize Webview");

        CreateWebView();

        LoadWebView();
    }

    private void CreateWebView()
    {
        PrintMessage("### Create WebView");
        
        if (_webView != null)
            return;

        UniWebView.SetAllowAutoPlay(true);
        UniWebView.SetAllowInlinePlay(true);
        UniWebView.SetJavaScriptEnabled(true);
        UniWebView.SetAllowJavaScriptOpenWindow(true);
        UniWebView.SetAllowUniversalAccessFromFileURLs(true);

        var webViewGameObject = new GameObject("UniWebView");
        _webView = webViewGameObject.AddComponent<UniWebView>();

        _webView.EmbeddedToolbar.Hide();
        _webView.SetAllowFileAccess(true);
        _webView.SetAllowFileAccessFromFileURLs(true);
        _webView.SetSupportMultipleWindows(true, true);
        _webView.SetCalloutEnabled(true);
        _webView.SetBackButtonEnabled(true);
        _webView.SetAllowBackForwardNavigationGestures(true);
        _webView.SetZoomEnabled(true);
        _webView.SetAcceptThirdPartyCookies(true);

        _webView.OnOrientationChanged += (view, orientation) => { SetFrame();};
        //_webView.RegisterOnRequestMediaCapturePermission(permission => UniWebViewMediaCapturePermissionDecision.Prompt);
        _webView.RegisterOnRequestMediaCapturePermission((permission) =>
        {
            PrintMessage($"### RegisterOnRequestMediaCapturePermission: request={permission.Resources}");
            PrintMessage($"### RegisterOnRequestMediaCapturePermission: request={permission.Host}");

            string[] expected = {"VIDEO"};

            if (permission.Resources.SequenceEqual(expected))
            {
                if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
                {
                    Permission.RequestUserPermission(Permission.Camera);

                    return UniWebViewMediaCapturePermissionDecision.Prompt;
                }


                return UniWebViewMediaCapturePermissionDecision.Grant;
            }

            return UniWebViewMediaCapturePermissionDecision.Grant;
        });

        SetUserAgent();

        RegisterShouldHandleRequest();

        SupportMultipleWindows();

        ShouldClose();

        SetFrame();
        
        Subscribe();
    }

    private void ShouldClose()
    {
        _webView.OnShouldClose += (view) =>
        {
            if (GameSettings.GetValue(Constants.LastUrl) != GameSettings.GetValue(Constants.StartUrl))
            {
                _webView.Load(GameSettings.GetValue(Constants.StartUrl));
            }
            else
            {
                if(canReload)
                {
                    _webView.Reload();
                }
            }

            return false;
        };
    }
    
    private void SetFrame()
    {
        PrintMessage($"@@@ OnOrientationChanged");
        
        if (_referenceRectTransform)
        {
            _webView.ReferenceRectTransform = _referenceRectTransform;
        }
        else
        {
            _webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        }
    }

    private void RegisterShouldHandleRequest()
    {
        _webView.RegisterShouldHandleRequest(request => {

            PrintMessage($"@@@ 👁️RegisterShouldHandleRequest: request.Url={request.Url}");
            
            if (IsBlockedUrl(request.Url)) 
            {
                PrintMessage($"### 🔒Blocked download files: {request.Url}");
                return false; 
            }
            
            if (request.Url.StartsWith("intent://"))
            {
                OpenIntent(request.Url);
                
                return false;
            }
            
            if (request.Url.StartsWith("http://") || request.Url.StartsWith("https://"))
            {
                PrintMessage($"### 🔗 OpenURL: {request.Url}");
                
                return true;
            }

            PrintMessage($"### 🧃 Application OpenURL: {request.Url}");
            
            Application.OpenURL(request.Url);
            
            return false;

            //ExternalAppLauncher.Instance.RunExternalApp(request.Url);
            
            //return !ExternalAppLauncher.Instance.IsOpeningOtherApp;
        });
    }

    private void OpenIntent(string intentUrl)
    {
        try
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject =
                new AndroidJavaObject("android.content.Intent", intentClass.GetStatic<string>("ACTION_VIEW"));

            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", intentUrl);

            intentObject.Call<AndroidJavaObject>("setData", uriObject);

            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            currentActivity.Call("startActivity", intentObject);
        }
        catch (System.Exception e)
        {
            PrintMessage($"Not Found intent: " + e.Message);
        }
    }

    private void SupportMultipleWindows()
    {
        _webView.OnMultipleWindowOpened += (view, windowId) => {
            PrintMessage($"🪟 @@@ OnMultipleWindowOpened");
            PrintMessage($"        view.Url {view.Url}");
            PrintMessage($"        A new window with identifier '{windowId}' is opened");
        };
        
        _webView.OnMultipleWindowClosed += (view, windowId) => {
            PrintMessage($"🪟 @@@ OnMultipleWindowClosed");
            PrintMessage($"        view.Url {view.Url}");
            PrintMessage($"        A new window with identifier '{windowId}' is closed");
        };
    }

    private void Subscribe()
    {
        PrintMessage($"📥Subscribe");
        
        _webView.OnPageStarted += OnPageStarted;
            
        _webView.OnPageFinished += OnPageFinished;

        _webView.OnLoadingErrorReceived += OnLoadingErrorReceived;
    }

    private void SetUserAgent()
    { 
        var agent = _webView.GetUserAgent();
        
        agent = ClearAgent(agent);
        
        agent = Regex.Replace(agent, @"Version/\d+\.\d+", "");
        
        _webView.SetUserAgent(agent);
    }
    
    private string ClearAgent(string agent)
    {
        return agent.Replace("; wv", "");
    }
    
    private void UnSubscribe()
    {
        PrintMessage($"📤UnSubscribe");
        
        _webView.OnPageStarted -= OnPageStarted;
        
        _webView.OnPageFinished -= OnPageFinished;
        
        _webView.OnLoadingErrorReceived -= OnLoadingErrorReceived;
            
        _webView.UnregisterShouldHandleRequest();
    }
    
    private void LoadWebView()
    {
        PrintMessage($"LoadUrl: _webView = {_webView}");

        _url = UrlB;
       
        AddPermissionTrustDomain("forms.kycaid.com");
        
        _webView.Load(_url);
    }
    
    private void AddPermissionTrustDomain(string domain)
    {
        _webView.AddPermissionTrustDomain(domain);
    }

    private void OnLoadingErrorReceived(UniWebView view, int errorCode, string errorMessage, UniWebViewNativeResultPayload payload)
    {
        PrintMessage($"### 💀OnLoadingErrorReceived: errorCode={errorCode}, _webView.Url={_webView.Url}, errorMessage={errorMessage}");
        
        ShowWebView();
    }
    
    private void OnPageStarted(UniWebView view, string url)
    {
        PrintMessage($"### 🎬OnPageStarted UniWebView: url={url}, _webView.Url={_webView.Url}");

        CultureInfo ci = new CultureInfo("en-US");
        
        if (!url.StartsWith("http", true, ci) && !url.StartsWith("about:blank", true, ci))
        {
            Application.OpenURL(url);
            
            if (_webView.CanGoBack) 
                _webView.GoBack();
        }
    }
    
    private void OnPageFinished(UniWebView view, int statusCode, string url)
    {
        PrintMessage($"### 🖱️OnPageFinished: IsOpeningOtherApp={ExternalAppLauncher.Instance.IsOpeningOtherApp}");
        
        if (ExternalAppLauncher.Instance.IsOpeningOtherApp) return;
        
        PrintMessage($"### 🏁OnPageFinished: url={url} / _webView.Url={_webView.Url}");

        //GameSettings.CheckStartUrl(url);

        if(url != "about:blank")
        {
            _url = url;

            UrlB = url;
        }
            
        ShowWebView();
    }

    private void HideWebView()
    {
        if(_webView == null) return;
        
        if (!_isVisible) return;

        _isVisible = false;
        
        _webView.Hide();
        
        if(ConnectivityManager.Instance)
            ConnectivityManager.Instance.CheckErrorReceived();
        
        if(ConnectivityManager.Instance)
            ConnectivityManager.Instance.OnChangedInternetConnection.AddListener(CheckConnection);
    }

    private void ShowWebView()
    {
        if(_webView == null) return;
        
        if (_isVisible) return;

        _isVisible = true;
        
        _webView.Show();
    }
    
    private bool IsBlockedUrl(string url)
    {
        string[] blockedExtensions = { ".zip", ".apk", ".pdf", ".exe", ".aab", ".bin" };
        
        foreach (var ext in blockedExtensions)
        {
            if (url.EndsWith(ext))
                return true;
        }
        return false;
    }

    private void PrintMessage(string message)
    {
        //Debugger.Log($"@@@ WebViewController ->: {message}", new Color(0.2f, 0.9f, 0.2f));
    }
}
