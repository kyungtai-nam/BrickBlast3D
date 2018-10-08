using UnityEngine;
using System.Collections;


/*
 usage>

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


	
 */
[RequireComponent(typeof(UILabel))]
public class UIAlertLabel : MonoBehaviour
{
	public TweenPosition twPos;
	public TweenAlpha twAlpha;
	public UILabel lb;

	public void Init(string msg)
	{
		Debug.Log ("UIAlertLabel:Init() msg=" + msg);

		lb.text = msg;

		twPos.ResetToBeginning ();
		twAlpha.ResetToBeginning ();

		twPos.PlayForward ();
		twAlpha.PlayForward ();
	}
		
	public void Done()
	{
		GPoolManager.Inst.Delete(gameObject);
	}
}

