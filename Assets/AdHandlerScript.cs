using UnityEngine;
using System.Collections;

#if !UNITY_WP8 
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;
#endif

#if UNITY_WP8 

#endif

public class AdHandlerScript : MonoBehaviour {

#if !UNITY_WP8 
	static private BannerView bannerView = null;
	static private InterstitialAd interstitial = null;
#endif

	static private bool adsEnabled = true;

	static public bool bannerVisible = false;

	// Use this for initialization
	void Start () {

		if (!adsEnabled)
			return;
#if !UNITY_WP8 
		if (bannerView == null) 
		{
				//admob
				CreateBanner ();
				bannerView.Hide ();

				//unityads
				if (Advertisement.isSupported) {
						Advertisement.allowPrecache = true;
						Advertisement.Initialize ("19490");
				} else {
						Debug.Log ("Platform not supported");
				}
		}
		if (interstitial == null) 
		{
			CreateInterstitial();
		}
#endif
	}

	// Update is called once per frame
	void Update () {
	
	}

	static public void hideBanner()
	{
		if (!adsEnabled)
			return;
#if !UNITY_WP8 
		bannerView.Hide();
		bannerVisible = false;
#endif
	}

	static public void showBanner()
	{
		if (!adsEnabled)
			return;
#if !UNITY_WP8 
		if (bannerView == null)
			CreateBanner();
		bannerView.Show();
		bannerVisible = true;
#endif
	}

	static public void showInterstitial()
	{
		if (!adsEnabled)
			return;
#if !UNITY_WP8 

		/*if (interstitial != null && interstitial.IsLoaded()) 
		{
			interstitial.Show();
		} */
#endif
	}

	static public void showVideoAd()
	{
		if (!adsEnabled)
			return;
#if !UNITY_WP8 
		// Show with default zone, pause engine and print result to debug log
		if (Advertisement.isReady ()) {
			Advertisement.Show (null, new ShowOptions {
				pause = true,
				resultCallback = result => {
					Debug.Log(result.ToString());
				}
			});
		}
#endif
	}
	static public void DestroyAds()
	{
		if (!adsEnabled)
			return;
#if !UNITY_WP8 

		if (bannerView != null)
			bannerView.Destroy();

		if (interstitial != null)
			interstitial.Destroy();
#endif
	}

	static private void CreateBanner()
	{
		if (!adsEnabled)
			return;
#if !UNITY_WP8 

		bannerView = new BannerView(
			"ca-app-pub-3481625071767526/8530747292", AdSize.Banner, AdPosition.Top);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the banner with the request.
		bannerView.LoadAd(request);
#endif
	}

	static private void CreateInterstitial()
	{
#if !UNITY_WP8 

		// Initialize an InterstitialAd.
		/*InterstitialAd interstitial = new InterstitialAd("ca-app-pub-3481625071767526/2872663294");
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);*/
#endif
	}

	void OnGUI () {

	}

}
