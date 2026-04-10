using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AppsFlyerSDK;

namespace Init
{
    public class AppsflyerInit : MonoBehaviour, IAppsFlyerConversionData
    {
        [SerializeField] private string _devKey;
        [SerializeField] private Text _text;

        private string advertisingId = "Не найдено";

        void Awake()
        {
            /*Application.RequestAdvertisingIdentifierAsync(
                (string adId, bool trackingEnabled, string error) =>
                {
                    advertisingId = string.IsNullOrEmpty(error) ? adId : $"Ошибка: {error}";
                    Debug.Log("Advertising ID: " + advertisingId);
                }
            );*/
        }

        void Start()
        {
            AppsFlyer.initSDK(_devKey, "", this);
            AppsFlyer.startSDK();
        }

        public void onConversionDataSuccess(string conversionData)
        {
            Dictionary<string, object> dataDict = AppsFlyer.CallbackStringToDictionary(conversionData);

            string creativeId = dataDict.TryGetValue("creative_id", out var value) ? value.ToString() : "Не найдено";
            string adCampaignId = dataDict.ContainsKey("ad_campaign_id")
                ? dataDict["ad_campaign_id"].ToString()
                : "Не найдено";
            string source = dataDict.ContainsKey("source") ? dataDict["source"].ToString() : "Не найдено";
            string subId1 = dataDict.ContainsKey("sub_id_1") ? dataDict["sub_id_1"].ToString() : "Не найдено";
            string subId2 = dataDict.ContainsKey("sub_id_2") ? dataDict["sub_id_2"].ToString() : "Не найдено";
            string subId3 = dataDict.ContainsKey("sub_id_3") ? dataDict["sub_id_3"].ToString() : "Не найдено";
            string appId = dataDict.ContainsKey("appid") ? dataDict["appid"].ToString() : "Не найдено";
            string campaignName = dataDict.ContainsKey("campaign_name")
                ? dataDict["campaign_name"].ToString()
                : "Не найдено";
            string campaignId = dataDict.ContainsKey("campaign_id") ? dataDict["campaign_id"].ToString() : "Не найдено";

            string formattedString =
                $"external_id={advertisingId}&creative_id={creativeId}&ad_campaign_id={adCampaignId}&source={source}&sub_id_1={subId1}&sub_id_2={subId2}&sub_id_3={subId3}&appid={appId}&campaign_name={campaignName}&campaign_id={campaignId}";

            _text.text = formattedString;
            Debug.Log(formattedString);
        }

        public void onConversionDataFail(string error)
        {
            _text.text = $"Ошибка атрибуции: {error}";
        }

        public void onAppOpenAttribution(string attributionData)
        {
        }

        public void onAppOpenAttributionFailure(string error)
        {
        }
    }
}