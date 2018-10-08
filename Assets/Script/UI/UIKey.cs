using UnityEngine;
using System.Collections;

public class UIKey : MonoBehaviour {	
	UILabel lbCount;

	TweenScale twScale = null;	
	RcScrollInt sn;

	void Awake()
	{		
		lbCount = transform.Find("Count").GetComponent<UILabel>();
		twScale = GetComponent<TweenScale> ();

		sn = gameObject.AddComponent<RcScrollInt>();
		sn.Init(lbCount, "{0}", UpdateVFX);
	}

	void Start()
	{
		sn.SetInt(RcSaveData.Inst.key, true);
	}

	void FixedUpdate()
	{
		sn.SetInt(RcSaveData.Inst.key);

	}

	void UpdateVFX()
	{
		twScale.ResetToBeginning();
		twScale.PlayForward();
	}
}
