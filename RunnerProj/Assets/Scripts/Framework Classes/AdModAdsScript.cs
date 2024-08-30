using UnityEngine;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine.UI;
using System;
using Runner.UImanager;
using Runner.PlayerCharacter;
using Runner.AdLoadingUI;

namespace Runner.AdMod
{

    public class AdModAdsScript : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private AdLoadingUIScript _adLoading;

        //  public TextMeshProUGUI totalCoinsTxt;

        public const string appId = "ca-app-pub-1385093244148841~5602672977";// "ca-app-pub-3940256099942544~3347511713";


#if UNITY_ANDROID
        //const string  bannerId = "ca-app-pub-1385093244148841/2952458907";
        //const string interId = "ca-app-pub-3940256099942544/1033173712";
        const string rewardedId = "ca-app-pub-3940256099942544/5224354917";
        //const string nativeId = "ca-app-pub-3940256099942544/2247696110";

#elif UNITY_IPHONE
    //const string bannerId = "ca-app-pub-3940256099942544/2934735716";
    //const string interId = "ca-app-pub-3940256099942544/4411468910";
    const string rewardedId = "ca-app-pub-3940256099942544/1712485313";
    //const string nativeId = "ca-app-pub-3940256099942544/3986624511";

#endif

        BannerView bannerView;
        //  InterstitialAd interstitialAd;
        RewardedAd rewardedAd;
        // NativeAd nativeAd;


        private void Start()
        {
            MobileAds.RaiseAdEventsOnUnityMainThread = true;
            MobileAds.Initialize(initStatus =>
            {
                Debug.Log("Ads initialized");
            });
        }


        #region Rewarded

        public void LoadRewardedAd()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }

            var adRequest = new AdRequest();
            adRequest.Keywords.Add("unity-admob-sample");

            RewardedAd.Load(rewardedId, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.Log(error);
                    return;
                }

                rewardedAd = ad;
                RewardedAdEvents(rewardedAd);
            });
        }
        private void Update()
        {
            if (rewardedAd != null)
                if (rewardedAd.CanShowAd())
                {
                    ShowRewardedAd();
                }
        }
        public void ShowRewardedAd()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Show((Reward reward) =>
                {
                    _adLoading.HideCanvas();
                    _player.StateMachine.ChangeState(_player.RunningState);
                });
            }
        }

        public void RewardedAdEvents(RewardedAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(("Rewarded ad paid {0} {1}." +
                    adValue.Value +
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Rewarded ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("Rewarded ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad full screen content closed.");
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);
            };
        }

        #endregion

    }

}