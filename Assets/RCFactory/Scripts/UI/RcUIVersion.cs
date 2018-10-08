using UnityEngine;
using System.Collections;

public class RcUIVersion : RcUI
{
	UILabel lb;

	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Right);	
		moveDelay = 0.4f;	

		lb = GetComponent<UILabel>();
		lb.text = string.Format("[i]v {0}[/i]", Application.version);
	}
}