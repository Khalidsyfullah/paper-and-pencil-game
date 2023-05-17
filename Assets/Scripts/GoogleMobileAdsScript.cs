using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class GoogleMobileAdsScript : MonoBehaviour
{
    static string adUnitIdInterestitial = "ca-app-pub-3387668599125624/3714154112";
    static string adUnitIdInterestitialTest = "ca-app-pub-3940256099942544/1033173712";
    static string adUnitIdRewarded = "ca-app-pub-3387668599125624/7431690239";
    static string adUnitIdRewardedTest = "ca-app-pub-3940256099942544/5224354917";
    int number = 1;
    private static InterstitialAd interstitial;
    private static RewardedAd rewardedAd;

    static int retries = 1, retriesRewarded = 1;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => {
            LoadInterstitialAd();
            LoadRewardedAd();
        });

        if (number == 1)
        {
            adUnitIdRewarded = adUnitIdRewardedTest;
            adUnitIdInterestitial = adUnitIdInterestitialTest;
        }
    }


    public static void LoadRewardedAd()
    {
        
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        var adRequest = new AdRequest.Builder().Build();
        RewardedAd.Load(adUnitIdRewarded, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    if(retriesRewarded < 5)
                    {
                        LoadRewardedAd();
                        retriesRewarded++;
                    }
                    return;
                }

                rewardedAd = ad;
                ad.OnAdFullScreenContentClosed += () =>
                {
                    LoadRewardedAd();
                };
               
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    LoadRewardedAd();
                };


            });
    }


    public static bool ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                
            });
            return true;
        }
        return false;
    }





    public static void LoadInterstitialAd()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
            interstitial = null;
        }


        var adRequest = new AdRequest.Builder().Build();
        
        InterstitialAd.Load(adUnitIdInterestitial, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    if(retries < 5)
                    {
                        LoadInterstitialAd();
                        retries++;
                    }
                    return;
                }

                interstitial = ad;

                ad.OnAdFullScreenContentClosed += () =>
                {
                    LoadInterstitialAd();
                };


                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    LoadInterstitialAd();
                };
            });

    }

    public static bool ShowAd()
    {
        if (interstitial != null && interstitial.CanShowAd())
        {
            interstitial.Show();
            return true;
        }
        return false;
        
    }



}
