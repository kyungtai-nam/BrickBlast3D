using UnityEngine;
using System.Collections;

public class UIGem : MonoBehaviour {	
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
		sn.SetInt(RcSaveData.Inst.gem, true);
	}

	void FixedUpdate()
	{
		sn.SetInt(RcSaveData.Inst.gem);

	}

	void UpdateVFX()
	{
		twScale.ResetToBeginning();
		twScale.PlayForward();
	}
}
