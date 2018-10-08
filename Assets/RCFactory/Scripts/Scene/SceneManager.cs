using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : GSingletonMono<SceneManager> {

	Dictionary<string, SceneBase> dic = new Dictionary<string, SceneBase>();
	string nowScene = null;

	public bool Add(string sceneName, SceneBase sceneObject)
	{
		if (dic.ContainsKey (sceneName)) {
			Debug.LogError ("SceneManager:Add() dup sceneName=" + sceneName);
			return false;
		}

		dic.Add (sceneName, sceneObject);
		return true;
	}

	public bool Change(string sceneName)
	{
		if (null != nowScene) {
			dic [nowScene].SceneClose ();
		}

		if (!dic.ContainsKey (sceneName)) {
			Debug.LogError ("SceneManager:Change() not found scene=" + sceneName);
			return false;
		}

		dic [sceneName].SceneOpen ();
		return true;
	}
}
