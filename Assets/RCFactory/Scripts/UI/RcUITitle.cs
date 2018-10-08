using UnityEngine;
using System.Collections;


// 타이틀 텍스트
public class RcUITitle : RcUI
{
//	public string strTitle = "Storm Bow";

	UILabel lbTitle;

	protected override void Awake()
	{		
		base.Awake ();
		// replace to UILocalize

		// lbTitle = GetComponent<UILabel>();

		// TODO : to read manifest 
		/*
		string title = Global.PRJ_TITLE_KR;
		if (SystemLanguage.Korean != Application.systemLanguage)
			title = Global.PRJ_TITLE_EN;
		*/
		// 폰에서 유니티 프로젝트의 설정값을 읽어온다. string.xml에서 읽어오지 않음!
		// Debug.Log ("RcUITitle:Awake() productName=" + Application.productName);


		// lbTitle.text = string.Format("[i]{0}[/i]", Localization.Get("title"));

	}

}