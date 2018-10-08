using UnityEngine;
using System.Collections;

public class UIGold : MonoBehaviour {	
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
		sn.SetInt(RcSaveData.Inst.gold, true);
	}

	void FixedUpdate()
	{
		sn.SetInt(RcSaveData.Inst.gold);

	}

	void UpdateVFX()
	{
		twScale.ResetToBeginning();
		twScale.PlayForward();
	}
}
