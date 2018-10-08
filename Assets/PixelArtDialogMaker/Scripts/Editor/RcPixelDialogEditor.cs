using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor (typeof(RcPixelDialog))]
public class RcPixelDialogEditor : Editor {
	
	public enum DialogSizePreset
	{
		NONE = 0,

		BUTTON_100x100,

		BUTTON_NORMAL,
		BUTTON_MIDDLE,
		BUTTON_LARGE,
		BUTTON_CLOSE,

		GROUP_TITLE,
		GROUP_SINGLE,
		GROUP_TEXT_NUM,
		GROUP_LIST,
		GROUP_POPUP,
		GROUP_BIG,


	}

	public enum DialogColorPreset
	{
		NONE = 0,

		// set 0 : simple
		WHITE_BG_BLACK_FONT,
		WHITE_OUTLINE_TRANS_BG,


		// set 1 
		BUTTON_RED,
		BUTTON_ORANGE,
		BUTTON_DARK_YELLOW,
		INNER_BEIGE,
		BG_BEIGE,

		GRAY_TITLE,
		BUTTON_CLOSE,

		BUTTON_GREEN,
		BUTTON_BROWN,

		// set 2
		GROUP_GREEN,
		BUTTON_FLAT_GREEN,
		BUTTON_BRIGHT_BROWN,
	
		// set 3
		GROUP_DARK_BROWN,
		BUTTON_BROWN_GREEN,
	
		// set 4
		INVENTORY_SLOT_BG,
		INVENTORY_SLOT,
		INVENTORY_BUTTON,


		// set 5
		BLACK_N_GRAY,

		// set 6
		BUTTON_ENABLE_BLUE,
		BUTTON_DISABLE_GRAY,

		// set 7 : shop
		YELLOW_BAND,
		ELEMENT_BG,
		ELEMENT_HOLDER_INNER,
		ELEMENT_HOLDER_OUTTER,

	}


	RcPixelDialog dlg;

	DialogColorPreset eColor;
	DialogSizePreset eSize;

	void OnEnable()
	{
		dlg = (RcPixelDialog)target;
		dlg.ApplyObject ();
		// EditorUtility.SetDirty(target);
	}


	void BeginUI()
	{
		EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
			GUILayout.Space(5f);
			GUILayout.BeginVertical();
	}

	void EndUI()
	{
			GUILayout.Space(10f);
			GUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();

		if (!GUI.changed)
			return;
		
		EditorUtility.SetDirty(target);
	}


	// TODO : procedural ui image
	// TODO : border size


	public override void OnInspectorGUI()
	{	
		// 모든 타이틀 필드에 대해서 적용됨 
		EditorGUIUtility.labelWidth = 80f;

		// title
		var style = new GUIStyle(GUI.skin.button);
		style.normal.textColor = Color.red;
		style.fontSize = 15;
		GUILayout.Label ("Pixel Art Dialog Maker", style);

		BeginUI ();



		GUILayout.Space(5f);
			
		// TODO : link game object
		// dlg.lbText = (UILabel)EditorGUILayout.ObjectField ("Text", dlg.lbText, typeof(UILabel), true);



		GUILayout.Space(5f);
		EditorGUI.BeginChangeCheck();
		dlg.startDepth = Mathf.Clamp(EditorGUILayout.IntField("Start Depth", dlg.startDepth, GUILayout.Width(200f)), -65535, 65535);
		if (EditorGUI.EndChangeCheck())
		{
			Debug.Log ("Apply Depth");
			dlg.ApplyDepth ();
		}

		// preset size
		GUILayout.Space(15f);
		EditorGUI.BeginChangeCheck();
		eSize = (DialogSizePreset)EditorGUILayout.EnumPopup("Preset Size", eSize, GUILayout.Width(200));
		if (EditorGUI.EndChangeCheck())
		{
			OnApplySize();
		}

		EditorGUI.BeginChangeCheck();
		GUILayout.Space(5f);
		dlg.width = Mathf.Clamp(EditorGUILayout.IntField("Width", dlg.width, GUILayout.Width(200f)), 1, 5000);			
		GUILayout.Space(5f);
		dlg.height = Mathf.Clamp(EditorGUILayout.IntField("Height", dlg.height, GUILayout.Width(200f)), 1, 5000);
			
		if (EditorGUI.EndChangeCheck())
		{
			eSize = DialogSizePreset.NONE;
			OnApplySize();
		}



		GUILayout.Space(10f);
		EditorGUI.BeginChangeCheck();
			dlg.lbText.text = EditorGUILayout.TextField ("Text", dlg.lbText.text);
			dlg.lbText.fontSize = EditorGUILayout.IntField ("Text Size", dlg.lbText.fontSize);
		if (EditorGUI.EndChangeCheck())
		{
			OnApplyFont ();
		}


		// prev preset color
		GUILayout.Space(15f);
		EditorGUILayout.LabelField ("Prev Preset", dlg.strPrevColor);

		// preset color
		//GUILayout.Space(15f);
		EditorGUI.BeginChangeCheck();
		eColor = (DialogColorPreset)EditorGUILayout.EnumPopup("Preset Color", eColor, GUILayout.Width(200f));

		if (EditorGUI.EndChangeCheck())
		{
			OnApplyColor();
		}



		EditorGUI.BeginChangeCheck();

		// outer
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		dlg.isOuter = EditorGUILayout.Toggle("Outer", dlg.isOuter, GUILayout.Width(100f));
		dlg.outer = EditorGUILayout.ColorField(dlg.outer, GUILayout.Width(100f));
		GUILayout.Space(5f);
		GUILayout.EndHorizontal();

		// outline
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		dlg.isOutline = EditorGUILayout.Toggle("Outline", dlg.isOutline, GUILayout.Width(100f));
		dlg.outline = EditorGUILayout.ColorField(dlg.outline, GUILayout.Width(100f));
		GUILayout.Space(5f);
		GUILayout.EndHorizontal();

		// shadow
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		dlg.isShadow = EditorGUILayout.Toggle("Shadow", dlg.isShadow, GUILayout.Width(100f));
		dlg.shadow = EditorGUILayout.ColorField(dlg.shadow, GUILayout.Width(100f));
		GUILayout.Space(5f);
		GUILayout.EndHorizontal();

		// highlight
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		dlg.isHighlight = EditorGUILayout.Toggle("Highlight", dlg.isHighlight, GUILayout.Width(100f));
		dlg.highlight = EditorGUILayout.ColorField(dlg.highlight, GUILayout.Width(100f));
		GUILayout.Space(5f);
		GUILayout.EndHorizontal();

		// backgroundDark
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		dlg.isBgDark = EditorGUILayout.Toggle("BgDark", dlg.isBgDark, GUILayout.Width(100f));
		dlg.bgDark = EditorGUILayout.ColorField(dlg.bgDark, GUILayout.Width(100f));
		GUILayout.Space(5f);
		GUILayout.EndHorizontal();

		// background
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		dlg.isBg = EditorGUILayout.Toggle("Bg", dlg.isBg, GUILayout.Width(100f));
		dlg.bg = EditorGUILayout.ColorField(dlg.bg, GUILayout.Width(100f));
		GUILayout.Space(5f);
		GUILayout.EndHorizontal();

		// font
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		dlg.isFont = EditorGUILayout.Toggle("Font", dlg.isFont, GUILayout.Width(100f));
		dlg.font = EditorGUILayout.ColorField(dlg.font, GUILayout.Width(100f));
		GUILayout.Space(5f);
		GUILayout.EndHorizontal();

		// fontShadow
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		dlg.isFontShadow = EditorGUILayout.Toggle("FontShadow", dlg.isFontShadow, GUILayout.Width(100f));
		dlg.fontShadow = EditorGUILayout.ColorField(dlg.fontShadow, GUILayout.Width(100f));
		GUILayout.Space(5f);
		GUILayout.EndHorizontal();


		// touch guard
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		dlg.isTouchGuard = EditorGUILayout.Toggle("Touch Guard", dlg.isTouchGuard, GUILayout.Width(100f));
		dlg.touchGuardBG = EditorGUILayout.ColorField(dlg.touchGuardBG, GUILayout.Width(100f));
		GUILayout.Space(5f);
		GUILayout.EndHorizontal();


		if (EditorGUI.EndChangeCheck())
		{
			eColor = DialogColorPreset.NONE;
			OnChangeColor ();				
		}


		EndUI();
	}


	void OnApplySize() 
	{
		/*
		button
			64x64 - small button
			204x64 - large button

			66x71 - small button2
			120x68 - middle button
		/*

		/*
		group
			420 x 145 - 2 row
			430 x 126 - 3 row
			420 x 60  - 1 row
		*/

		switch (eSize) {
		case DialogSizePreset.NONE:
			dlg.ApplySize (dlg.width, dlg.height);
			break;

		case DialogSizePreset.BUTTON_NORMAL: dlg.ApplySize(64,64); break;
		case DialogSizePreset.BUTTON_MIDDLE: dlg.ApplySize(120,68); break;
		case DialogSizePreset.BUTTON_LARGE: dlg.ApplySize(200,68); break;
		case DialogSizePreset.BUTTON_CLOSE: dlg.ApplySize(32,32); break;

		case DialogSizePreset.GROUP_TITLE: dlg.ApplySize(220,60); break;
		case DialogSizePreset.GROUP_SINGLE: dlg.ApplySize(400,160); break;
		case DialogSizePreset.GROUP_TEXT_NUM: dlg.ApplySize(400,64); break;
		case DialogSizePreset.GROUP_LIST: dlg.ApplySize(400,240); break;
		case DialogSizePreset.GROUP_POPUP: dlg.ApplySize(400,400); break;
		case DialogSizePreset.GROUP_BIG: dlg.ApplySize(500,500); break;

		case DialogSizePreset.BUTTON_100x100: dlg.ApplySize(100,100); break;			
		}

		dlg.ApplySize(dlg.width, dlg.height);
		Debug.Log (string.Format("OnApplySize() size={0}x{1}"
			, dlg.width, dlg.height));
	}


	void OnChangeColor()
	{
		dlg.ApplyColor ();
		Debug.Log ("OnChangeColor() eColor="+eColor);
	}

	void OnApplyColor()
	{
		switch (eColor) {
		case DialogColorPreset.NONE:
			return;


		case DialogColorPreset.WHITE_BG_BLACK_FONT:
			dlg.SetClrGroup (
				new string[]{
					"", 	// outer 
					"", 	// outline
					"", 	// shadow
					"", 	// highlight
					"", 	// bgDark
					"ffffff", 	// bg
					"000000", 	// font
					"", 	// font shadow
				});
			break;


		case DialogColorPreset.WHITE_OUTLINE_TRANS_BG:
			dlg.SetClrGroup (
				new string[]{
					"", 	// outer 
					"ffffff", 	// outline
					"", 	// shadow
					"", 	// highlight
					"", 	// bgDark
					"4A4A4A40", 	// bg
					"ffffff", 	// font
					"", 	// font shadow
				});
			break;


		case DialogColorPreset.BUTTON_RED:
			dlg.SetClrGroup (
				new string[]{
					"ffffff", 	// outer 
					"000000", 	// outline
					"931c00", 	// shadow
					"ff7b48", 	// highlight
					"ff3d11", 	// bgDark
					"ff5311", 	// bg
					"ffffff", 	// font
					"000000", 	// font shadow
				});
			break;




		case DialogColorPreset.BUTTON_ORANGE: 
			dlg.SetClrGroup (
				new string[] {
					"ffffff", 	// outer 
					"ff5311", 	// outline
					"c03b1c", 	// shadow
					"ff7b48", 	// highlight
					"ff3d11", // bgDark
					"ff5311", 	// bg
					"ffffff", 	// font
					"000000", 	// font shadow
				});
			break;

		case DialogColorPreset.BUTTON_DARK_YELLOW:
			dlg.SetClrGroup (
				new string[] {
					"ffffff", 	// outer 
					"000000", 	// outline
					"7d5a00", 	// shadow
					"ffcc00", 	// highlight
					"c98000", 	// bgDark
					"ffa200", 	// bg
					"ffffff", 	// font
					"000000", 	// font shadow
				});			
			break;

		case DialogColorPreset.INNER_BEIGE:
			dlg.SetClrGroup (
				new string[] {
					"", 		// outer 
					"f9f0d1", 	// outline
					"e0ca9d", 	// shadow
					"b09050", 	// highlight
					"", 		// bgDark
					"d2b883", 	// bg
					"ffffff", 	// font
					"9a8760", 	// font shadow
				});
			break;


		case DialogColorPreset.BG_BEIGE:
			dlg.SetClrGroup (
				new string[]{
					"7e4a27", 	// outer 
					"654225", 	// outline
					"d7bc88", 	// shadow
					"fffcf2", 	// highlight
					"", 	// bgDark
					"faf0d0", 	// bg
					"735c47", 	// font
					"ffffff", 	// font shadow
				});
			break;


		case DialogColorPreset.GRAY_TITLE:
			dlg.SetClrGroup (
				new string[]{
					"", 	// outer 
					"fffefe", 	// outline
					"606060", 	// shadow
					"979797", 	// highlight
					"", 	// bgDark
					"7c7c7c", 	// bg
					"ffffff", 	// font
					"000000", 	// font shadow
				});
			break;


		case DialogColorPreset.BUTTON_GREEN:
			dlg.SetClrGroup (
				new string[]{
					"", 	// outer 
					"3a6d2f", 	// outline
					"358425", 	// shadow
					"63d54d", 	// highlight
					"", 	// bgDark
					"5fb14f", 	// bg
					"ffffff", 	// font
					"000000", 	// font shadow
				});
			break;



		case DialogColorPreset.BUTTON_BROWN:
			dlg.SetClrGroup (
				new string[]{
					"", 	// outer 
					"f9f0d1", 	// outline
					"b09050", 	// shadow
					"e0ca9d", 	// highlight
					"", 	// bgDark
					"d2b883", 	// bg
					"ffffff", 	// font
					"000000", 	// font shadow
				});
			break;

		case DialogColorPreset.BUTTON_CLOSE:
			dlg.SetClrGroup (
				new string[] {
					"", 	// outer 
					"830800", 	// outline
					"da4e4f", 	// shadow
					"ff7871", 	// highlight
					"", 	// bgDark
					"fb5449", 	// bg
					"f8f3eb", 	// font
					"af0400", 	// font shadow
				});
			break;

		case DialogColorPreset.GROUP_GREEN:
			dlg.SetClrGroup (
				new string[] {
					"", 	// outer 
					"213521", 	// outline
					"314529", 	// shadow
					"297519", 	// highlight
					"", 	// bgDark
					"3a5931", 	// bg
					"738e6b", 	// font
					"", 	// font shadow
				});
			break;


		case DialogColorPreset.BUTTON_FLAT_GREEN:
			dlg.SetClrGroup (
				new string[] {
					"", 	// outer 
					"ffffff", 	// outline
					"5a7921", 	// shadow
					"", 	// highlight
					"5a7919", 	// bgDark
					"739629", 	// bg
					"ffffff", 	// font
					"", 	// font shadow
				});
			break;

		case DialogColorPreset.BUTTON_BRIGHT_BROWN:
			dlg.SetClrGroup (
				new string[] {
					"7b2800", 	// outer 
					"fffbef", 	// outline
					"ce7900", 	// shadow
					"ffdf9c", 	// highlight
					"ffbe4a", 	// bgDark
					"ffce73", 	// bg
					"101010", 	// font
					"ffaa00", 	// font shadow
				});
			break;


		case DialogColorPreset.GROUP_DARK_BROWN:
			dlg.SetClrGroup (
				new string[] {
					"", 	// outer 
					"634110", 	// outline
					"", 	// shadow
					"", 	// highlight
					"422d08", 	// bgDark
					"312400", 	// bg
					"", 	// font
					"", 	// font shadow
				});
			break;

		
		case DialogColorPreset.BUTTON_BROWN_GREEN:
			dlg.SetClrGroup (
				new string[] {
					"", 	// outer 
					"e6be7b", 	// outline
					"213d00", 	// shadow
					"84b600", 	// highlight
					"5a7900", 	// bgDark
					"6b8e00", 	// bg
					"ffffff", 	// font
					"424900", 	// font shadow
				});
			break;


		case DialogColorPreset.INVENTORY_SLOT_BG:
			dlg.SetClrGroup (
				new string[]{
					"", 	// outer 
					"", 	// outline
					"", 	// shadow
					"", 	// highlight
					"", 	// bgDark
					"100c08", 	// bg
					"ffa25a", 	// font
					"291c19", 	// font shadow
				});
			break;


		case DialogColorPreset.INVENTORY_SLOT:
			dlg.SetClrGroup (
				new string[]{
					"422400", 	// outer 
					"a49e94", 	// outline
					"525152", 	// shadow
					"212010", 	// highlight
					"", 	// bgDark
					"3a2410", 	// bg
					"ffa25a", 	// font
					"291c19", 	// font shadow
				});
			break;

		case DialogColorPreset.INVENTORY_BUTTON:
			dlg.SetClrGroup (
				new string[]{
					"796432", 	// outer 
					"ffdf9c", 	// outline
					"422000", 	// shadow
					"683508", 	// highlight
					"A16900", 	// bgDark
					"c58100", 	// bg
					"ffffde", 	// font
					"644119", 	// font shadow
				});
			break;



		case DialogColorPreset.BLACK_N_GRAY:
			dlg.SetClrGroup (
				new string[]{
					"", 	// outer 
					"848079", 	// outline
					"848079", 	// shadow
					"", 	// highlight
					"", 	// bgDark
					"ffffff", 	// bg
					"6f6d68", 	// font
					"", 	// font shadow
				});
			break;



		case DialogColorPreset.BUTTON_ENABLE_BLUE:
			dlg.SetClrGroup (
				new string[]{
					"", 	// outer 
					"0d4b86", 	// outline
					"2083e3", 	// shadow
					"3bbcff", 	// highlight
					"", 	// bgDark
					"3b9eff", 	// bg
					"ffffff", 	// font
					"296eb1", 	// font shadow
				});
			break;

		case DialogColorPreset.BUTTON_DISABLE_GRAY:
			dlg.SetClrGroup (
				new string[]{
					"", 	// outer 
					"3d3d3d", 	// outline
					"7a7a7a", 	// shadow
					"cacaca", 	// highlight
					"", 	// bgDark
					"ababab", 	// bg
					"fefefe", 	// font
					"757575", 	// font shadow
				});
			break;


		case DialogColorPreset.YELLOW_BAND:
			dlg.SetClrGroup (
				new string[]{
					"3b3524", 	// outer 
					"db9b00", 	// outline
					"ffe400", 	// shadow
					"ffe400", 	// highlight
					"", 	// bgDark
					"ffc000", 	// bg
					"ffffff", 	// font
					"955e00", 	// font shadow
				});
			break;


		case DialogColorPreset.ELEMENT_BG:
			dlg.SetClrGroup (
				new string[]{
					"", 	// outer 
					"020202", 	// outline
					"3f361b", 	// shadow
					"776b48", 	// highlight
					"", 	// bgDark
					"514628", 	// bg
					"ffffff", 	// font
					"605841", 	// font shadow
				});
			break;


		case DialogColorPreset.ELEMENT_HOLDER_INNER:
			dlg.SetClrGroup (
				new string[]{
					"", 	// outer 
					"382913", 	// outline
					"4c4531", 	// shadow
					"", 	// highlight
					"", 	// bgDark
					"40351f", 	// bg
					"ffffff", 	// font
					"786c49", 	// font shadow
				});
			break;

		case DialogColorPreset.ELEMENT_HOLDER_OUTTER:
			dlg.SetClrGroup (
				new string[]{
					"", 	// outer 
					"6f6442", 	// outline
					"", 	// shadow
					"28220f", 	// highlight
					"", 	// bgDark
					"3f361b", 	// bg
					"ffffff", 	// font
					"2e2814", 	// font shadow
				});
			break;
				
		}


		dlg.ApplyColor (eColor.ToString());
		Debug.Log ("OnApplyColor() eColor="+eColor);
	}



	void OnApplyFont()
	{
		// dlg.lbText.text = "";
		// dlg.lbText.fontSize = 0;
	}

}