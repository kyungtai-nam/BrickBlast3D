using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
	
public class RcDialogMaker : EditorWindow {

	[MenuItem ("RetroCell/Pixel Art Dialog Maker %g")]
	static void CreateInstance () {

		Transform tmParent = null;

		if (null != Selection.activeGameObject)
			tmParent = Selection.activeGameObject.transform;

		GameObject go = (GameObject)PrefabUtility.InstantiatePrefab (Resources.Load ("RcPixelDialog"));
		go.transform.position = Vector3.zero;

		if ( null != tmParent ) 
			go.transform.parent = tmParent;
	
		DisconnectObject (go);
		// AssetDatabase.Refresh();

		// random rename
		go.name = go.name + Random.Range (10, 100).ToString();

		Selection.activeGameObject = go;
	}

	const string TheTempPrefabPath = "Assets/rcpixeldialog_temp.prefab";

	static void DisconnectObject(GameObject go) {
		Object prefab = PrefabUtility.CreateEmptyPrefab(TheTempPrefabPath);
		PrefabUtility.ReplacePrefab(go, prefab, ReplacePrefabOptions.ConnectToPrefab);
		AssetDatabase.DeleteAsset(TheTempPrefabPath);
		PrefabUtility.DisconnectPrefabInstance(go);
	}

}