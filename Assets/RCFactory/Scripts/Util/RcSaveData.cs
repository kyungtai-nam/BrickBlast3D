using UnityEngine;
using System.Collections;
using VoxelBusters.NativePlugins;

public class RcSaveData : GSingletonMono<RcSaveData> 
{	
	// public int version;

	// resources
	public int gold = 0;
	public int gem = 0;
	public int key = 0;

	// option
	public bool sound = true;
	public bool rated = false;

	// statistics
	public int bestScore = 0;
	public int lastScore = 0;
		
	public bool showAd = true;

 	string timeReward = null;

	public bool Load()
	{
		// version = GCryptPlayerPrefs.GetString("version", Application.version);

		gold = GCryptPlayerPrefs.GetInt ("gold", 1000);
		gem = GCryptPlayerPrefs.GetInt ("gem", 0);

		sound = GCryptPlayerPrefs.GetBool ("sound", true);
		rated = GCryptPlayerPrefs.GetBool ("rated", false);

		bestScore = GCryptPlayerPrefs.GetInt ("bestScore", 0);
		lastScore = GCryptPlayerPrefs.GetInt ("lastScore", 0);

		showAd = GCryptPlayerPrefs.GetBool ("showAd", true);
		timeReward = GCryptPlayerPrefs.GetString("timeReward", null);

//		CollectionManager.Inst.Load ();
		return false;
	}

	public void Save()
	{
		// version = Application.version;
		// GCryptPlayerPrefs.SetString("version", version);

		GCryptPlayerPrefs.SetInt ("gold", gold);
		SaveGem();

		SaveSound();
		GCryptPlayerPrefs.SetBool ("rated", rated);

		GCryptPlayerPrefs.SetInt ("bestScore", bestScore);
		GCryptPlayerPrefs.SetInt ("lastScore", lastScore);

		SaveAd();
		SaveTimeReward();

//		CollectionManager.Inst.Save ();

		GCryptPlayerPrefs.Save ();
	}

	public void SaveSound(bool forceSave=false)
	{
		GCryptPlayerPrefs.SetBool ("sound", sound);

		if ( forceSave ) 
			GCryptPlayerPrefs.Save ();
	}

	public void SaveAd(bool forceSave=false)
	{
		GCryptPlayerPrefs.SetBool ("showAd", showAd);

		if ( forceSave ) 
			GCryptPlayerPrefs.Save ();

	
		showAd = GCryptPlayerPrefs.GetBool ("showAd", true);
		Debug.Log ("RcSaveData:SaveAd() showAd=" + showAd);
	}

	public void SaveTimeReward()
	{
		if (string.IsNullOrEmpty (timeReward))
			return;
		
		GCryptPlayerPrefs.SetString("timeReward", timeReward);	
	}

	public void SaveGem()
	{
		GCryptPlayerPrefs.SetInt ("gem", gem);
	}


	public string TimeReward
	{
		get {
			return timeReward;		
		}
		set {
			timeReward = value;
		}
	}



	public bool CheckBestScore()
	{
		if (lastScore < bestScore)
			return false;

		bestScore = lastScore;
		return true;
	}
}