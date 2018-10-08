using UnityEngine;
using System.Collections;


// 사운드 온오프
public class RcUISound : RcUI
{
	Transform tmOn;
	Transform tmOff;

	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Left);
		moveDelay = 0.3f;

		tmOn = transform.Find ("On");
		tmOff = transform.Find ("Off");

		ShowSoundSprite (RcSaveData.Inst.sound);
	}

	public override void OnTouched()
	{
		Debug.Log ("RcUISound:OnTouched()");

		RcSaveData.Inst.sound = !RcSaveData.Inst.sound;
		ShowSoundSprite (RcSaveData.Inst.sound);

		RcSaveData.Inst.SaveSound (true);
	}


	void ShowSoundSprite(bool isOn)
	{
		tmOn.gameObject.SetActive (isOn);
		tmOff.gameObject.SetActive (!isOn);
	}
}