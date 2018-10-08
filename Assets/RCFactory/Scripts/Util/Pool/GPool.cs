using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GPool
{
	private Queue<GameObject> m_queueFree = null;

	public GPool()
	{
		m_queueFree = new Queue<GameObject>();
	}

	public void Destroy()
	{
		// 모두 삭제.

		while(0 < m_queueFree.Count)
		{
			GameObject go = m_queueFree.Dequeue();
			if( null != go )
				GameObject.Destroy(go);
		}
	}

	public GameObject Add(string strResID, Transform tmParent, bool show)
	{
		// 큐에 있는 오브젝트 리턴 (없으면 생성).

		GameObject go = _Dequeue();
		if( null == go )
		{
			Object obj = Resources.Load(strResID);
			if( null == obj )
			{
#if UNITY_EDITOR
				Debug.LogError("GPool::Add not found = "+strResID);
#endif
				return null;
			}
			go = GameObject.Instantiate(obj) as GameObject;

			GPoolResID	poolID 	= go.AddComponent<GPoolResID>();		
			poolID.id 			= strResID;
		}

		Transform tm = go.transform;
		tm.parent			= tmParent;
		// tm.localPosition 	= Vector3.zero;
		// tm.localRotation 	= Quaternion.identity;
		// tm.localScale 		= Vector3.one;

		go.SetActive(show);
		return go;
	}

	public void PreLoad(string strResID, Transform tmParent)
	{
		// 불러오기.

		GameObject go = GameObject.Instantiate(Resources.Load(strResID)) as GameObject;
		
		GPoolResID	poolID 	= go.AddComponent<GPoolResID>();		
		poolID.id 			= strResID;
		go.transform.parent = tmParent;

		go.SetActive(false);
		m_queueFree.Enqueue(go);
	}

	public void Delete(GameObject go, Transform tmParent)
	{
		// 큐에 추가.

		go.transform.parent = tmParent;
		go.SetActive(false);
		m_queueFree.Enqueue(go);
	}

	private GameObject _Dequeue()
	{
		GameObject go = null;
		while( 0 < m_queueFree.Count )
		{
			go = m_queueFree.Dequeue();
			if( null != go )
				return go;
		}
		return null;
	}
}

