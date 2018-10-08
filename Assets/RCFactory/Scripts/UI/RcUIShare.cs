using UnityEngine;
using System.Collections;

// 공유 버튼
public class RcUIShare : RcUI
{	
	// string message_kr = "[{0}] 구글 플레이에서 다운 받으세요!";
	// string message_en = "[{0}] Anroid App On Google Play!";

	// string url = "https://play.google.com/store/apps/details?id=com.retrocellstudio.chestrpg";


	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Right);
		moveDelay = 0.3f;
	}

	public override void OnTouched()
	{
		Debug.Log ("RcUIShare:OnTouched()");

		string show_url = Global.SHARE_URL;

		switch (Application.systemLanguage){
		case SystemLanguage.Korean:
		case SystemLanguage.English:
		case SystemLanguage.French:
		case SystemLanguage.Italian:
		case SystemLanguage.Dutch:
		case SystemLanguage.Spanish:
		case SystemLanguage.Japanese:
		// case SystemLanguage.ChineseSimplified:
		// case SystemLanguage.ChineseTraditional:
		case SystemLanguage.Portuguese:
		case SystemLanguage.Russian:
		case SystemLanguage.Turkish:
		case SystemLanguage.Arabic:
			show_url += "&hl=" + Localization.Get ("url_postfix");
			break;

		default :
			break;
		}

		string show_message = string.Format("[{0}] {1}", Localization.Get("title"), Localization.Get("share"));

		RcShare.Inst.ShareURL(show_message, show_url);

		base.OnTouched ();
	}
}