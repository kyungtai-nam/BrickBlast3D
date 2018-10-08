using UnityEngine;
using System.Collections;

public class IntroScene : MonoBehaviour {
	
	void Awake()
	{
		RcSceneManager.Inst.ChangeScene("IntroScene");

		Camera.main.backgroundColor = ConfigManager.Inst.clrCamBlack;
		/*
		FxPro fx = null;

		fx.VignettingIntensity = 0f;
		fx.VignettingIntensity = 0.797f;
		*/
	}

	IEnumerator Start()
	{
		// yield return new WaitForSeconds (0.1f);
		for (int n = 0; n <= 3; n++) {
			yield return new WaitForEndOfFrame ();
		}

		RcSceneManager.Inst.ChangeScene("HomeScene");
	}
}
