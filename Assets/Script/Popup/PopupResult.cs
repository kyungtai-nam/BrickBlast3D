using UnityEngine;
using System.Collections;

public class PopupResult : MonoBehaviour {

	public void OnRetry()
	{
		Debug.Log("PopupResult:OnRetry()");
	
		PlayManager.Inst.Release ();
		PlayManager.Inst.Init ();

		gameObject.SetActive (false);
	}

	public void OnHome()
	{
		Debug.Log("PopupResult:OnHome()");

		// TODO : go to home
		gameObject.SetActive (false);
	}
}
