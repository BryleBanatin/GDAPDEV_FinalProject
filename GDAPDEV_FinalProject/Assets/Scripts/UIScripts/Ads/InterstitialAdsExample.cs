using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAdsExample : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iosAdunitId = "Interstitial_iOS";
    string _adUnitID;

    private void Awake()
    {
        _adUnitID = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iosAdunitId :
            _androidAdUnitId;
    }

    void Start()
    {
        WaitsForAdsManagerInitialized();
    }
    
     
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"{placementId}, has been loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {

        Debug.Log($"Unity Ads Initialization failed: {error.ToString()} - {message}");
    }


    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing ad unit {placementId}: {error.ToString()} - {message} ");
    }

    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) { }


    public void LoadAd()
    {
        //Important! Only load when the Advertisements class has been initialized. Otherwise, loading an Ad will result to an error.
        Debug.Log("Loading ad:" + _adUnitID);
    }

    public void ShowAd()
    {
        // Shows the ad. If the ad is not loaded, it will be displayed.
        Debug.Log("Showing Add: " + _adUnitID);
        Advertisement.Show(_adUnitID, this);
        LoadAd();
    }

    IEnumerator WaitsForAdsManagerInitialized()
    {
        yield return new WaitUntil(() => Advertisement.isInitialized);
        LoadAd();
    }
   
}
