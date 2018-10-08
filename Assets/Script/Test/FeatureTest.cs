using UnityEngine;
using System.Collections;

public class FeatureTest : MonoBehaviour {

	void FixedUpdate()
	{
		if (0 != RcSceneManager.Inst.SceneName.CompareTo ("HomeScene")) {	
			gameObject.SetActive (false);
		}
	}
}
