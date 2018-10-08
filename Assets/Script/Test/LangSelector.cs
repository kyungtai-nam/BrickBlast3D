using UnityEngine;
using System.Collections;

public class LangSelector : MonoBehaviour {

	string[] langs = new string[]{
		"Korean",
		"English",
		"French",
		"Italian",
		"Dutch",
		"Spanish",
		"Japanese",
		"ChineseSimplified",
		"ChineseTraditional",
		"Portuguese",
		"Russian",
		"Turkish",
		"Arabic"};

	public UIGrid grid;

	void Awake()
	{
		MakeLangButtons ();
	}

	public void SetClickEvent(UIEventTrigger trigger, EventDelegate dele, object value, object value2=null)
	{
		// EventDelegate dele = new EventDelegate(this, func);

		if ( null != value ) 
			dele.parameters[0].value = value;

		if ( null != value2 )
			dele.parameters[1].value = value2;

		EventDelegate.Set(trigger.onClick, dele);
	}
		
	void MakeLangButtons()
	{
		for (int n = 0; n < langs.Length; n++) {
			GameObject go = GPoolManager.Inst.Add ("Prefab/UI/UILangSelector", grid.transform, true);

			Transform tm = go.transform;
			tm.localScale = Vector3.one;

			UILabel lb = tm.Find ("Text").GetComponent<UILabel> ();
			lb.text = langs [n];

			UIEventTrigger trig = go.GetComponent<UIEventTrigger> ();
			// SetClickEvent(trig, new EventDelegate(this, "On" + langs[n]), null);
			SetClickEvent(trig, new EventDelegate(this, "OnSelect"), langs[n]);
		}

		grid.repositionNow = true;
	}


	public void OnSelect(string lang)
	{
		Debug.Log ("LangSelector:OnSelect()=" + lang);
		Localization.language = lang; 
	}

	public void OnKorean()
	{
		Localization.language ="Korean"; 
	}

	public void OnEnglish()
	{
		Localization.language ="English"; 
	}

	public void OnFrench()
	{
		Localization.language ="French"; 
	}

	public void OnItalian()
	{
		Localization.language ="Italian"; 
	}

	public void OnDutch()
	{
		Localization.language ="Dutch"; 
	}

	public void OnSpanish()
	{
		Localization.language ="Spanish"; 
	}

	public void OnJapanese()
	{
		Localization.language ="Japanese"; 
	}

	public void OnChineseSimplified()
	{
		Localization.language ="ChineseSimplified"; 
	}

	public void OnChineseTraditional()
	{
		Localization.language ="ChineseTraditional"; 
	}

	public void OnPortuguese()
	{
		Localization.language ="Portuguese"; 
	}

	public void OnRussian()
	{
		Localization.language ="Russian"; 
	}

	public void OnTurkish()
	{
		Localization.language ="Turkish"; 
	}

	public void OnArabic()
	{
		Localization.language ="Arabic"; 
	}
}
