using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 게임 오브젝트 생성, 삭제 관리
public class GameManager : GSingletonMono<GameManager>
{
	Transform m_tmGameRoot = null;

	public Transform tmGameRoot {
		get {
			if ( null == m_tmGameRoot )
				m_tmGameRoot = GameObject.Find("Game Root").transform;
			return m_tmGameRoot;
		}
	}

	Transform m_tmCamera = null;

	public Transform tmCamera {
		get {
			if ( null == m_tmCamera )
				m_tmCamera = tmGameRoot.Find("Main Camera").transform;
			return m_tmCamera;
		}
	}

	//Transform m_tmCameraTopView = null;

	//public Transform tmCameraTopView {
	//	get {
	//		if ( null == m_tmCameraTopView )
	//			m_tmCameraTopView = tmGameRoot.Find("TopView Camera").transform;
	//		return m_tmCameraTopView;
	//	}
	//}


	GameObject Create(string pathPrefab, Transform tmParent)
	{
		if (null == tmParent) {
			Debug.LogWarning (string.Format ("GameManager:CreateObject() {0} null == tmParent", pathPrefab));
		}
		GameObject 	go = GameObject.Instantiate(Resources.Load(pathPrefab)) as GameObject;
		Transform 	tm = go.transform;

		go.SetActive(false);

		tm.parent = tmParent;
		tm.localPosition 	= Vector3.zero;
		tm.localRotation 	= Quaternion.identity;
		tm.localScale 		= Vector3.one;
		return go;
	}

	GameObject CreateByPool(string pathPrefab, Transform tmParent)
	{
		if (null == tmParent) {
			Debug.LogWarning (string.Format ("GameManager:CreateObject() {0} null == tmParent", pathPrefab));
		}

		GameObject go = GPoolManager.Inst.Add (pathPrefab, tmParent);
		Transform 	tm = go.transform;
	
		tm.parent = tmParent;
		tm.localPosition 	= Vector3.zero;
		tm.localRotation 	= Quaternion.identity;
		tm.localScale 		= Vector3.one;
		return go;
	}



	// contents
	Transform m_tmWater = null;

	public Transform tmWater {
		get {
			if (null == m_tmWater)
				m_tmWater = tmGameRoot.Find ("Water").transform;
			return m_tmWater;
		}
	}

	GameObject m_goFloor = null;

	public GameObject goFloor {
		get {
			if (null == m_goFloor) {
				m_goFloor = Create ("Prefab/Game/Floor", tmGameRoot);

				Transform tm = m_goFloor.transform;
				tm.localPosition = new Vector3 (0f, -1.1f, 0f);
				tm.localScale = new Vector3 (10f, 1f, 10f);
			}
			return m_goFloor;
		}
	}

	GameObject m_goDemoBall = null;

	public GameObject demoBall {
		get {
			if (null == m_goDemoBall) {
				m_goDemoBall = Create ("Prefab/Demo/DemoBall", tmGameRoot);

				Transform tm = m_goDemoBall.transform;
				tm.localPosition = Vector3.zero;
				tm.localScale = Vector3.one;
			}

			return m_goDemoBall;
		}
	}

	Fireworks m_fireworks = null;

	public Fireworks fireworks {
		get {
			if (null == m_fireworks) {
				GameObject go = Create ("Prefab/Game/Fireworks", tmGameRoot);
				m_fireworks = go.GetComponent<Fireworks> ();

				Transform tm = m_fireworks.transform; 
				tm.localPosition = Vector3.zero;
				tm.localScale = Vector3.one;					
			}

			return m_fireworks;
		}
	}
}