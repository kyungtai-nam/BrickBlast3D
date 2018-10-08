using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fireworks : MonoBehaviour {
	// ParticleSystem m_vfx = null;

	ParticleSystem vfx {
		get {
			GameObject go = GameObject.Instantiate (
				Resources.Load ("Prefab/VFX/CFXM4 Firework B")
				// , UIManager.Inst.tmRoot) as GameObject;
				, GameManager.Inst.tmGameRoot) as GameObject;

			/*
			Transform tm = go.transform;
			tm.localPosition = Vector3.zero;
			tm.localScale = Vector3.one;
			*/

			go.SetActive (false);
			ParticleSystem m_vfx = go.GetComponent<ParticleSystem> ();
			return m_vfx;
		}
	}

	bool isShow = false;
	List<ParticleSystem> list = new List<ParticleSystem>();


	public int count = 3;
	public float maxDelay = 0.4f;

	void Awake()
	{
		// list.Add (GameManager.Inst.vfxResult);

		for (int n = 0; n < count; n++) {			
			list.Add (vfx);
		}
	}

	void OnEnable()
	{
		Debug.Log ("Fireworks:OnEnable()");  
		isShow = true;    
	}

	void OnDisable()
	{
		Debug.Log ("Fireworks:OnDisable()");  
		isShow = false;
	}


	float delay = 0f;

	void FixedUpdate()
	{
		if (!isShow)
			return;

		for (int n = 0; n < list.Count; n++) {
			if (list [n].gameObject.activeSelf)
				continue;

			delay -= Time.deltaTime;

			if (0 < delay)
				continue;

			ShowVFX (list [n]);
			delay = Random.Range (0f, maxDelay);
		}
	}
		
	void ShowVFX(ParticleSystem ps)
	{ 
		Vector3 pos = new Vector3 (Random.Range (-10f, 10f), Random.Range (-10f, 10f), 0f);
		// Vector3 pos = new Vector3 (Random.Range (-1f, 1f), Random.Range (-1f, 1f), 0f);
		ps.transform.localPosition = pos;
		ps.gameObject.SetActive (true);
		// Debug.Log ("SceneResult:ShowVFX() pos=" + pos);
	}
}