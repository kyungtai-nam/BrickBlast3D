using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor (typeof(RcObjectGroup))]
public class RcObjectGroupEditor : Editor
{
	int specing = 5;

	bool applyY = false;
	bool applyZ = false;

	public override void OnInspectorGUI()
    {
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();

		if (GUILayout.Button ("Zero")) {
			onZero ();
		}

		GUILayout.EndHorizontal();
		GUILayout.EndVertical();


		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();


		if ( GUILayout.Button("Sort") ) {
			onSort(); 
		}
			
		if ( GUILayout.Button("Ordered Sort") ) {
		 	onOrderedSort(); 
		}

		GUILayout.Label("Specing X :");
		int.TryParse(GUILayout.TextField(specing.ToString()), out specing);

		GUILayout.EndHorizontal();
		GUILayout.EndVertical();



		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();

		applyY = GUILayout.Toggle(applyY, "apply Y");
		applyZ = GUILayout.Toggle(applyZ, "apply Z");

		GUILayout.EndHorizontal();
		GUILayout.EndVertical();


    }

	void onZero()
	{
		foreach (GameObject go in Selection.gameObjects) {
			for (int n = 0; n < go.transform.childCount; n++) {				
				Transform tm = go.transform.GetChild(n);

				Vector3 pos = tm.localPosition;
				pos.x = 0f;

				if ( applyY ) 
					pos.y = 0f;

				if ( applyZ )
					pos.z = 0f;

				tm.localPosition = pos;
			}
		}
	}

	void onSort()
	{
		foreach (GameObject go in Selection.gameObjects) {
			Debug.Log ("Selection go = " + go.name);

			for (int n = 0; n < go.transform.childCount; n++) {
				Transform tm = go.transform.GetChild (n);

				Vector3 pos = tm.localPosition;
				pos.x += (float)specing * n;

				if ( applyY ) 
					pos.y = 0f;

				if ( applyZ )
					pos.z = 0f;

				tm.localPosition = pos;
			}
			/*
			for ( int n=begin_no;n<=end_no;n++)
			{
				Transform tm = go.transform.Find (string.Format(strNameFmt, n));
				if (null == tm)
					continue;

				Vector3 pos = tm.localPosition;
				pos.x += 5.0f * (n-1);
				pos.y = 0f;
				pos.z = 0f;

				tm.localPosition = pos;
			}
			*/
		}	
	}

	void onOrderedSort()
    {
		foreach (GameObject go in Selection.gameObjects) {
			Debug.Log ("Selection go = " + go.name);

			for (int n = 0; n < go.transform.childCount; n++) {
				Transform tm = go.transform.GetChild (n);

				//String.tm.name;
				string[] splitName = tm.name.Split ('-');

				int seq = 0;
				int.TryParse (splitName [1], out seq);

				Vector3 pos = tm.localPosition;
				pos.x += (float)specing * (seq-1);

				if ( applyY ) 
					pos.y = 0f;

				if ( applyZ )
					pos.z = 0f;

				tm.localPosition = pos;
			}
			/*
			for ( int n=begin_no;n<=end_no;n++)
			{
				Transform tm = go.transform.Find (string.Format(strNameFmt, n));
				if (null == tm)
					continue;

				Vector3 pos = tm.localPosition;
				pos.x += 5.0f * (n-1);
				pos.y = 0f;
				pos.z = 0f;

				tm.localPosition = pos;
			}
			*/
		}	
    }
}