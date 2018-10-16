using UnityEngine;
using System.Collections;
public class AdColonySupportMgr : MonoBehaviour 
{
    /*
     * kyungtai : NOT USED
    static AdColonySupportMgr instance = null;
    public static AdColonySupportMgr GetInstance
    {
	        get
	        {
		            if (instance == null)
			            {
			                instance = FindObjectOfType(typeof(AdColonySupportMgr)) as AdColonySupportMgr;
			 
			                if (instance == null)
				                {
				                    Debug.Log("Nothing" + instance.ToString());
				                    return null;
				                }
			            }
		            return instance;
		        }
	    }
 
//    public string appId_Android = string.Empty;
//    public string zoneString_Android = string.Empty;
    public string appId_Ios = string.Empty;
    public string zoneString_Ios = string.Empty;
 
    string appId = string.Empty;
    string zoneString = string.Empty;
 
    void Start()
    {
	#if UNITY_ANDROID
		appId = Global.ADCOLONY_APP_ID;
		zoneString = Global.ADCOLONY_ZONE;
	#elif UNITY_IPHONE
		appId = appId_Ios;
		zoneString = zoneString_Ios;
	#endif
		AdColony.Configure("1.0", appId, zoneString);
	    AdColony.OnVideoStarted = OnVideoStarted;
	    AdColony.OnVideoFinished = OnVideoFinished;
	}
 
    void OnVideoStarted()
    {
		Debug.Log("Ad playing.");
	}
 
    void OnVideoFinished(bool ad_shown)
    {
		Debug.Log("Ad Finished.");
	}
     
    /// <summary>
    /// 동영상을 보여주는 함수
    /// </summary>
    public void ShowVideoAdmob()
	{
		if (AdColony.IsVideoAvailable(zoneString))
	    {
	        Debug.Log(this.gameObject.name + " triggered playing a video ad.");
	        AdColony.ShowVideoAd(zoneString);
	    }
		else
	    {
	        Debug.Log(this.gameObject.name + " tried to trigger playing an ad, but the video is not available yet.");
			UIManager.Inst.PopupMessage ("adcolony not available yet!");
	    }
    }
    */
}