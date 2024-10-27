using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    
    public static AdManager instance;

    private InterstitialAd _interstitialAd;
    private const string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
  
   
    private RewardedAd _rewardedAd;
    private const string _adRewardUnitId = "ca-app-pub-3940256099942544/5224354917";
  


    private void Awake()
    {
        
        if(instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
          
        }

        else
        {

            Destroy(gameObject);

        }
   
    }

    private void Start()
    {
       
        MobileAds.Initialize((InitializationStatus initStatus) => { });

        LoadRewardedAd();

    }

    public void LoadInterstitialAd()
    {

        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        var adRequest = new AdRequest();


            InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {

                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;

            });

        RegisterEventHandlers(_interstitialAd);

    }

    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {

        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };

        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");

        };

        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };

        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };

        interstitialAd.OnAdFullScreenContentClosed += () =>
        {

            Debug.Log("Interstitial ad full screen content closed.");
            LoadInterstitialAd();

        };

        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            LoadInterstitialAd();
        };
    }

    public void LoadRewardedAd()
    {
        
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

       
        var adRequest = new AdRequest();

        
        RewardedAd.Load(_adRewardUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
            
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
            });
    }

    public void ShowRewardedAd()
    {
       
        const string rewardMsg = "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";


        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                GivePlayerReward();
            });
            RegisterReloadHandler(_rewardedAd);
        }

        else
        {

           StartCoroutine(MainMenuPanelManager.instance.OpenAndClosePanel("ErrorPanel"));

        }
    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
       
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");
          
            LoadRewardedAd();
        };
       
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
     
            LoadRewardedAd();
        };
    }

    private void GivePlayerReward()
    {

        PlayerPrefs.SetFloat("Coin", PlayerPrefs.GetFloat("Coin") + 1000);

    }

}
