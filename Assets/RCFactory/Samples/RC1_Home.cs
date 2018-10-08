using UnityEngine;
using System.Collections;

public class RC1_Home : MonoBehaviour 
{
	public Transform uiRoot;

	void Awake()
	{
		uiRoot = transform.Find ("UI Root");
	}

	public void Alert(string msg)
	{
		GameObject obj = GPoolManager.Inst.Add("Prefab/AlertLabel", uiRoot);
		UIAlertLabel lb = obj.GetComponent<UIAlertLabel> ();
		lb.Init (msg);

		obj.SetActive (true);
	}
		
	public void OnTimeReward()
	{
		Alert ("OnTimeReward()");
	}

	public void OnSetting()
	{
		Alert ("OnSetting()");
	}


	public void OnPlay()
	{
		Alert ("OnPlay()");
	}

	// AOS 

	public void OnShare()
	{
		Alert ("OnShare()");
		// AppShare.Inst.shareText("Title", "Desc", "https://play.google.com/store/apps/details?id=com.retrocellstudio.dodgecat");
	}

	public void OnRate()
	{
		Alert ("OnRate()");

	}


	public void OnSign()
	{
		Alert ("OnSign()");

	}
		

	public void OnAchievement()
	{
		Alert ("OnAchievement()");

	}


	public void OnLeaderboard()
	{
		Alert ("OnLeaderboard()");
	}

	public void OnSnapshot()
	{
		Alert ("OnSnapshot()");
	}

	public void OnSound()
	{
		Alert ("OnSound()");
	}

}
