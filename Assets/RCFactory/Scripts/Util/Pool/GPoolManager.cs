using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 	Create> 
 
 		GameObject go = GPoolManager.Inst.Add("Prefab/AlertLabel", holder.transform);
		Transform tm = go.transform;
		tm.parent = holder.transform;
		tm.localPosition = Vector3.zero;
		tm.localScale = Vector3.one;
		go.SetActive(true);
	
		UIAlertLabel al = go.GetComponent<UIAlertLabel>();
		al.Init (msg);

	Delete>
		GPoolManager.Inst.Delete(gameObject);
*/ 
 


public class GPoolManager : GSingletonMono<GPoolManager>
{
	private Dictionary<string, GPool> m_hashPool = new Dictionary<string, GPool>();

	public GameObject Add(string strResID, Transform tmParent, bool show=false)
	{
		// 추가.
		GPool pool = null;
		if( false == m_hashPool.TryGetValue(strResID, out pool) )
		{
			pool = new GPool();
			m_hashPool.Add(strResID, pool);
		}
			
		return pool.Add(strResID, tmParent, show);
	}

	public void PreLoad(string strResID, int nCount)
	{
		// 불러오기.

		GPool pool = null;
		if( false == m_hashPool.TryGetValue(strResID, out pool) )
		{
			pool = new GPool();
			m_hashPool.Add(strResID, pool);
		}

		for(int i=0; i<nCount; ++i)
			pool.PreLoad(strResID, this.transform);
	}

	public void Delete(GameObject go)
	{
		// 삭제.

		string 	strResID 	= go.GetComponent<GPoolResID>().id;
		GPool 	pool 		= null;

		if (true == m_hashPool.TryGetValue (strResID, out pool))
			pool.Delete (go, this.transform);
		else {
			Debug.LogError ("GPoolManager:Delete() go=" + go.name);
		}
	}
}

