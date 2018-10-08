using UnityEngine;
using System.Collections.Generic;

// for slingshot
public class GuideLine : MonoBehaviour {
	public float DOT_SPECING = 0.5f;
	public bool DEBUG_LOG = false;
	public float REFLECTION_LINE_LEN = 5f;

	const float BALL_SIZE = 0.3f;

	public enum EType {
		PassThru, // 충돌체크 없이
		OnlyWall, // 벽과만 충돌 체크 하며 길이만 각도에 적용
		ReachFirst, // 블록과 충돌체크 하며 길이만 각도에 적용
		FirstColl, // 블록과 충돌체크 하며 충돌 지점 까지 그림
		SecondColl, // 블록과 충돌체크 하며 충돌 지점과 반사 지점까지 그림
		Total	
	}

	public EType type = EType.FirstColl;


	Transform m_tm = null;

	public Transform tm {
		get {
			if ( null == m_tm ) 
				m_tm = this.transform;
			return m_tm;
		}
	}

	List<GameObject> m_list = new List<GameObject>();

	// error
	/*
	void OnDisable()
	{
		CleanUp();
	}
	*/


	void MakeBoxLine(Vector3 src, Vector3 dst, bool toggle=false)
	{
		float dist = Vector3.Distance (src, dst);

		int cnt = Mathf.FloorToInt(dist / DOT_SPECING);
		cnt = Mathf.Min (cnt, 20);

		for(int n=0;n<cnt;n++)
		{
			Vector3 pos = Vector3.Lerp (src, dst, (float)n / (float)cnt);		
			MakeBox (pos, toggle, n == 0);	
		}
	}

	void MakeBox(Vector3 pos, bool toggle=false, bool zoom=false)
	{
		string prefab = toggle ? "Prefab/Game/GuideDot" : "Prefab/Game/GuideDot2";
		GameObject goDot = GPoolManager.Inst.Add(prefab, tm, true);
		Transform tmDot = goDot.transform;
		tmDot.localPosition = pos;
		m_list.Add(goDot);
	}
	 


	public void CleanUp()
	{
//		if (!PlayerManager.IsValid ())
//			return;
		
		m_list.ReleaseToPool ();
	}	






	public void Make(Vector3 pos, Vector3 dir) {
		gameObject.SetActive (true);
		tm.localPosition = Vector3.zero;

		CleanUp ();

		switch (type) {
		case EType.PassThru:
			DrawPassThru (pos, dir);
			return;
		case EType.OnlyWall:
			DrawOnlyWall (pos, dir);
			return;
		case EType.ReachFirst:
			DrawReachFirst (pos, dir);
			return;
		case EType.FirstColl:
			DrawFirstColl (pos, dir);
			return;
		case EType.SecondColl:
			DrawSecondColl (pos, dir);
			return;
		}
	}

	void DrawPassThru(Vector3 pos, Vector3 dir) {
		MakeBoxLine (pos, pos + dir * REFLECTION_LINE_LEN, false);
	}

	void DrawOnlyWall(Vector3 pos, Vector3 dir) {
		RaycastHit hit;

		if (Physics.SphereCast (pos, BALL_SIZE, dir, out hit, Mathf.Infinity, 1 << PlayManager.Inst.LAYER_MASK_WALL)) {
			Vector3 vr = Vector3.Reflect (dir, hit.normal);

			if (DEBUG_LOG) {
				Debug.LogFormat ("GuideLine:Make() ref src={0},dst={1},dir={2},vr={3}"
					, pos, hit.point, dir, vr);
			}

			float dist = Vector3.Distance (pos, hit.point) + 1f;
			MakeBoxLine (pos, pos + dir * dist, false);

		} else {
			if (DEBUG_LOG)
				Debug.LogError ("GuideLine:Make() NOT COLL");

			MakeBoxLine (pos, pos + dir * REFLECTION_LINE_LEN, true);
		}
	}

	void DrawReachFirst(Vector3 pos, Vector3 dir) {
		RaycastHit hit;

		if (Physics.SphereCast (pos, BALL_SIZE, dir, out hit, Mathf.Infinity, PlayManager.Inst.LAYER_MASK_BLOCK_ITEM)) {
			Vector3 vr = Vector3.Reflect (dir, hit.normal);

			if (DEBUG_LOG) {
				Debug.LogFormat ("GuideLine:Make() ref src={0},dst={1},dir={2},vr={3}"
					, pos, hit.point, dir, vr);
			}

			float dist = Vector3.Distance (pos, hit.point) + 1f;
			MakeBoxLine (pos, pos + dir * dist, false);

		} else {
			if (DEBUG_LOG)
				Debug.LogError ("GuideLine:Make() NOT COLL");

			MakeBoxLine (pos, pos + dir * REFLECTION_LINE_LEN, true);
		}
	}

	void DrawFirstColl(Vector3 pos, Vector3 dir) {
		RaycastHit hit;

		if (Physics.SphereCast (pos, BALL_SIZE, dir, out hit, Mathf.Infinity, PlayManager.Inst.LAYER_MASK_BLOCK_ITEM)) {
			// if (Physics.Raycast (pos, dir, out hit, Mathf.Infinity, PlayManager.Inst.LAYER_MASK_BLOCK_ITEM)) {
			Vector3 vr = Vector3.Reflect (dir, hit.normal);

			if (DEBUG_LOG) {
				Debug.LogFormat ("GuideLine:Make() ref src={0},dst={1},dir={2},vr={3}"
					, pos, hit.point, dir, vr);
			}

			// before collision 
			MakeBoxLine (pos, hit.point, true);

		} else {
			if (DEBUG_LOG)
				Debug.LogError ("GuideLine:Make() NOT COLL");

			MakeBoxLine (pos, pos + dir * REFLECTION_LINE_LEN, false);
		}
	}

	void DrawSecondColl(Vector3 pos, Vector3 dir) {
		RaycastHit hit;

		if (Physics.SphereCast (pos, BALL_SIZE, dir, out hit, Mathf.Infinity, PlayManager.Inst.LAYER_MASK_BLOCK_ITEM)) {
			// if (Physics.Raycast (pos, dir, out hit, Mathf.Infinity, PlayManager.Inst.LAYER_MASK_BLOCK_ITEM)) {
			Vector3 vr = Vector3.Reflect (dir, hit.normal);

			if (DEBUG_LOG) {
				Debug.LogFormat ("GuideLine:Make() ref src={0},dst={1},dir={2},vr={3}"
					, pos, hit.point, dir, vr);
			}

			// before collision 
			MakeBoxLine (pos, hit.point, true);


			Vector3 pos2nd = hit.point;
			if (Physics.SphereCast (pos2nd, BALL_SIZE, vr.normalized, out hit, Mathf.Infinity, PlayManager.Inst.LAYER_MASK_BLOCK_ITEM)) {
				vr = Vector3.Reflect (dir, hit.normal);	

				MakeBoxLine (pos2nd, hit.point, true);
			} else {
				if (DEBUG_LOG)
					Debug.LogError ("GuideLine:Make() 2nd NOT COLL");

				MakeBoxLine (hit.point, hit.point + vr * REFLECTION_LINE_LEN, false);
			}

		} else {
			if (DEBUG_LOG)
				Debug.LogError ("GuideLine:Make() NOT COLL");

			MakeBoxLine (pos, pos + dir * REFLECTION_LINE_LEN, false);
		}
	}

}
