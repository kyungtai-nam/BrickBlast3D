using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneResult : MonoBehaviour
{
	bool isShow = false;
	List<ParticleSystem> list = new List<ParticleSystem>();

	void Awake()
	{
//		list.Add (GameManager.Inst.vfxResult2);
	}

	void Start()
	{
		Debug.Log ("SceneResult:Start()");	
		isShow = true;		
	}

	void FixedUpdate()
	{
		if (!isShow)
			return;

		for (int n = 0; n < list.Count; n++) {
			if (list [n].gameObject.activeSelf)
				continue;

			ShowVFX (list [n]);
		}
	}

	void ShowVFX(ParticleSystem ps)
	{	
		Vector3 pos = new Vector3 (Random.Range (-3f, 3f), Random.Range (-5f, 5f), Random.Range (-10f, -5f));
		ps.transform.localPosition = pos;
		ps.gameObject.SetActive (true);

		// Debug.Log ("SceneResult:ShowVFX() pos=" + pos);
	}

	/*
	void OnDisable()
	{
		Debug.Log ("SceneResult:OnDisable()");

		isShow = false;
		goVfx.SetActive (false);
	}
	*/
}