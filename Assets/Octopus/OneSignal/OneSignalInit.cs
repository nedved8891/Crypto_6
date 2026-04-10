using UnityEngine;
using OneSignalSDK;
using OneSignalSDK.Debug.Models;

public class OneSignalInit : MonoBehaviour
{
    async void Start()
    {
        // Enable verbose logging for debugging (remove in production)
        OneSignal.Debug.LogLevel = LogLevel.Verbose;
        // Initialize with your OneSignal App ID
        OneSignal.Initialize("7e4e1592-170b-4df4-9566-620fe2b15e41");
        // Use this method to prompt for push notifications.
        // We recommend removing this method after testing and instead use In-App Messages to prompt for notification permission.
        await OneSignal.Notifications.RequestPermissionAsync(true);
    }
}
