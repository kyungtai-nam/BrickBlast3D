using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
	GGoogleAnalyticsManager.Inst.Log("Storm Bow", "Home");
*/

public class GGoogleAnalyticsManager : GSingletonMono<GGoogleAnalyticsManager> 
{
 /* kyungtai : NOT USED
	public Object GAv4 { get; set; }

	// Use this for initialization
	//void Start()
	protected override void Awake()
	{
		// Debug.LogWarning ("GA:Awake()");

		if( null == GoogleAnalyticsV4.instance )
		{
			GoogleAnalyticsV4 ga = GameObject.FindObjectOfType<GoogleAnalyticsV4>();
			if( null != ga )
				GoogleAnalyticsV4.instance = ga;
		}

		if( null == GoogleAnalyticsV4.instance )
			GameObject.Instantiate(this.GAv4);

		GoogleAnalyticsV4.instance.StartSession();
	}

	void OnApplicationQuit()
	{
		if( null == GoogleAnalyticsV4.instance )
			return;

		GoogleAnalyticsV4.instance.StopSession();
	} 

	public void Log(string title, string screenName)
	{
		if (null == GoogleAnalyticsV4.instance) {
			Debug.LogWarningFormat ("GA:Log null == GoogleAnalyticsV4.instance title={0},scene={1}", title, screenName);
			return;
		}

//		Debug.LogWarningFormat ("GA:Log title={0},scene={1}", title, screenName);

		GoogleAnalyticsV4.instance.LogScreen(title);
		GoogleAnalyticsV4.instance.LogScreen(new AppViewHitBuilder().SetScreenName(screenName));
	}
    */
}

