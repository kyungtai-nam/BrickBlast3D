using UnityEngine;
using System.Collections;

// 인게임 : 일시 정지 
public class RcUIPause : RcUI
{
	public bool isPaused = false;
	public System.Action cbResume = null;

	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Top);
		moveDelay = 0.1f;
	}

	public override void OnTouched()
	{		
		// Debug.Log ("Time.timeScale=" + Time.timeScale);

		if ( isPaused ) {
			Debug.Log ("RcUIPause:OnTouched() Hide -> Show");
			isPaused = false;

//			PlayManager.Inst.isPlaying = true;

			UIManager.Inst.uiPlayResume.Hide();
			UIManager.Inst.uiHome.Hide ();

//			UIManager.Inst.uiPause.gameObject.SetActive (true);		
//			UIManager.Inst.uiPause.Show();

			UIManager.Inst.goScreenLocker.SetActive(false);

			Time.timeScale = 1f;		

			if (null != cbResume)
				cbResume ();
			
		} else {
			Debug.Log ("RcUIPause:OnTouched() Show -> Hide");
			isPaused = true;

			Time.timeScale = 0f;

//			PlayManager.Inst.isPlaying = false;

			UIManager.Inst.goScreenLocker.SetActive(true);

			UIManager.Inst.uiPlayResume.gameObject.SetActive(true);
			UIManager.Inst.uiPlayResume.cbTouched = OnTouched;
			UIManager.Inst.uiPlayResume.Show ();

			UIManager.Inst.uiHome.gameObject.SetActive (true);
			UIManager.Inst.uiHome.Show ();

// 			UIManager.Inst.uiPause.Hide ();

			base.OnTouched ();
		}
	}	
}