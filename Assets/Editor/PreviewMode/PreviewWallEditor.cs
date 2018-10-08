using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor (typeof(PreviewWall))]
public class PreviewWallEditor : Editor {

	int SIZE_X = 10;
	int SIZE_Y = 10;

	int sx;
	int sy; 
	int dx;
	int dy;


	void Begin()
	{
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
	}

	void End()
	{
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	public override void OnInspectorGUI()
	{
		Begin ();
		GUILayout.Label("X ");
		int.TryParse(GUILayout.TextField(SIZE_X.ToString()), out SIZE_X);
		End ();

		Begin ();
		GUILayout.Label("Y ");
		int.TryParse (GUILayout.TextField (SIZE_Y.ToString ()), out SIZE_Y);				
		End ();


		Begin ();
		if (GUILayout.Button ("Make Wall")) {
			onMakeWall ();
		}
			
		if (GUILayout.Button ("Clear")) {
			onClear ();
		}
		End ();


		Begin ();
		if (GUILayout.Button ("Make Block")) {
			cal ();
			onMakeBlock ();
			onMakeBall ();
		}
		End ();
	}


	void cal()
	{
		sx = -SIZE_X / 2;
		sy = -SIZE_Y / 2; 
		dx = SIZE_X / 2;
		dy = SIZE_Y / 2;
	}
		
	void onMakeWall()
	{
		Debug.Log ("PreviewWallEditor:onMake()");

		Transform tmRoot = Selection.activeGameObject.transform;

		cal ();

		bool isWall = true;

		for (int y = sy; y <= dy; y++) {
			for (int x = sx; x <= dx; x++) {

				bool isSpawn = false;

				// top, bottom
				if (y == sy || y == dy) {
					isSpawn = true;
				}

				// left, right
				else if ( x == sx || x == dx ) {
					isSpawn = true;
				}

				isWall = !isWall;

				if (!isSpawn)
					continue;

				GameObject go = null;

				//if ( isWall ) 
					go = (GameObject)PrefabUtility.InstantiatePrefab (Resources.Load ("Prefab/Game/Wall"));
				//else
				//	go = (GameObject)PrefabUtility.InstantiatePrefab (Resources.Load ("Prefab/Game/Block"));


				go.transform.parent = tmRoot;
				go.transform.localPosition = new Vector3 (x, y, 0f);
				go.transform.localEulerAngles = Vector3.zero;
			}
		}
	}

	void onMakeBlock () {
		Transform tmRoot = Selection.activeGameObject.transform;

		int step = 1;
		// float y = sy + 0.9f; // bottom y point + start point height + ball height ??
		for(int y = dy-2; y < dy; y++ )  
		{			
			for (int x = sx+1; x < dx; ) {
				bool isSpawn = Random.Range (0, 2) == 0 ? true : false;

				if (!isSpawn) {
					x++;
					continue;
				}

				GameObject go = (GameObject)PrefabUtility.InstantiatePrefab (Resources.Load ("Prefab/Game/Block"));
				go.transform.parent = tmRoot;
				go.transform.localPosition = new Vector3 (x, y, 0f);
				go.transform.localEulerAngles = Vector3.zero;


				Block block = go.GetComponent<Block> ();
				block.ResetMaxHP (step);

				bool isItem = Random.Range (0, 2) == 0 ? true : false;
				if (isItem)
					block.SetItemForDebug ();

				x++;
			}

			step+=12;
		}
	}

	void onMakeBall()
	{
		Transform tmRoot = Selection.activeGameObject.transform;

		float y = sy + 0.9f; // bottom y point + start point height + ball height ??

		// ball
		{			
			GameObject go = (GameObject)PrefabUtility.InstantiatePrefab (Resources.Load ("Prefab/Game/Ball"));
			go.transform.parent = tmRoot;
			go.transform.localPosition = new Vector3 (0f, y, 0f);
			go.transform.localEulerAngles = Vector3.zero;
		}


		// start point
		{
			GameObject go = (GameObject)PrefabUtility.InstantiatePrefab (Resources.Load ("Prefab/Game/StartPoint"));
			go.transform.parent = tmRoot;
			go.transform.localPosition = new Vector3 (0f, y - 0.5f, 0f);
			go.transform.localEulerAngles = Vector3.zero;
		}
	}

	void onClear()
	{
		Debug.Log ("PreviewWallEditor:onClear()");

		Transform tmRoot = Selection.activeGameObject.transform;

		/*
		int cnt = tmRoot.childCount;
		for (int n = cnt - 1; n >= 0; n--) {
			Object.DestroyImmediate(tmRoot.GetChild (n).gameObject);
		}
		*/
		while(tmRoot.childCount>0){
			Object.DestroyImmediate(tmRoot.GetChild(0).gameObject);
		}
	}
}
