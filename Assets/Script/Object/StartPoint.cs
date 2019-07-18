using UnityEngine;
using System.Collections;

public class StartPoint : MonoBehaviour {
	public enum E_STATE
	{
		Idle = 0, 
		Moving,
		Total
	}

	/*
		상태 체크해서 공이 움직이지 않을 때 
		슬링샷을 통해 공을 발사 
	*/


	public bool DEBUG_LOG = false;
	public float TOO_SHORT_DISTANCE = 10f;


	public bool ANGLE_GUARD = true;
	public float ANGLE_RANGE = 70f;
	public float ANGLE_CANCEL_RANGE = 80f;

	GuideLine m_guideLine = null;

	GuideLine guideLine {
		get {
			if (null == m_guideLine) {
				GameObject go = GPoolManager.Inst.Add ("Prefab/Game/GuideLine", GameManager.Inst.tmGameRoot, true);

				Transform tm = go.transform;
				tm.localPosition = Vector3.zero;

				m_guideLine = go.GetComponent<GuideLine> ();
				m_guideLine.CleanUp ();
				m_guideLine.DEBUG_LOG = DEBUG_LOG;
			}

			return m_guideLine;
		}
	}

	public void Release()
	{
		GPoolManager.Inst.Delete(guideLine.gameObject);
	}

	public System.Action<Vector3> cbShoot = null;

	/*
	void Start()
	{
		MakeGuideLine ();
		cbShoot = Shoot;
	}
	*/


	void OnEnable()
	{
		src = Vector2.zero;
		dst = Vector2.zero;
	}

	void Shoot(Vector3 dir)
	{
		float angle = Vector3.Angle (Vector3.right, dir);
		Debug.LogFormat("StartPoint:Shoot() dir={0} angle={1}", dir, angle);
	}

	bool IsTooShort()
	{
		float d = Vector2.Distance (src, dst);

		if (d < TOO_SHORT_DISTANCE) {
			if ( DEBUG_LOG ) 
				Debug.LogWarning ("StartPoint:IsTooShort() too short=" + d); 
			return true;
		}

		if ( DEBUG_LOG ) 
			Debug.Log ("StartPoint:IsTooShort() right distance=" + d); 
		return false;
	}

	public bool isReady = true;

	void Update()
	{		
		#if UNITY_EDITOR
			MouseProcess();
			KeyProcess();
		#else
			TouchProcess ();
		#endif
	}

	bool BeginTouch(Vector2 pos)
	{
		if (Mathf.Approximately (src.magnitude, 0f)) {
			if ( DEBUG_LOG ) 
				Debug.Log ("StartPoint:BeginTouch() set start=" + pos);

//			CameraUtil.Convert3DtoUI (GameManager.Inst.tmCamera, UIManager.Inst.cameraUI, pos);

			UIManager.Inst.touchBeginPoint.SetActive (true);
			UIManager.Inst.touchBeginPoint.transform.position = UICamera.currentCamera.ScreenToWorldPoint (pos);

			src = pos;
            dst = pos + Vector2.right;
            isCancel = true;
            //dst = pos; //revert
            
            guideLine.Make(transform.localPosition, MakeDir(Vector2.zero));
            
			//guideLine.Make (transform.localPosition, MakeDir(), GuideLineDist);
			return true;
		}
		return false;
	}


	bool isCancel = false;

	void DragTouch(Vector2 pos)
	{
		float d = Vector2.Distance (src, pos);

		UIManager.Inst.touchDragPoint.SetActive (true);
		UIManager.Inst.touchDragPoint.transform.position = UICamera.currentCamera.ScreenToWorldPoint (pos);

		if (d < TOO_SHORT_DISTANCE) {
			guideLine.CleanUp ();
			isCancel = true;
			return;
		}

		guideLine.Make (transform.localPosition, MakeDir(dst));

		Vector3 v = MakeDir (pos);
		float angle = Vector3.Angle (Vector3.right, v.normalized);
		//float angle = Vector3.Angle (Vector3.up, v.normalized);

		if (DEBUG_LOG) {
			Debug.LogFormat ("StartPoint:DragTouch() src={0},dst={1},angle={2},d={3}"
				, src, dst, angle, d);
		}

		if (ANGLE_GUARD) {
			// shoot cancel

			if (ANGLE_CANCEL_RANGE <= angle) {
                guideLine.CleanUp ();
				isCancel = true;
				return;
			}

			// range guard
			if (ANGLE_RANGE < angle) {
				return;
			} 
		}

		isCancel = false;
		dst = pos;
	}

	void EndTouch(Vector2 pos)
	{
		// dst = pos;
		UIManager.Inst.touchBeginPoint.SetActive (false);
		UIManager.Inst.touchDragPoint.SetActive (false);	


		guideLine.CleanUp ();

		if (isCancel || IsTooShort ()) {
			src = Vector2.zero;
			dst = Vector2.zero;
			return;
		}

		SoundManager.Inst.Play ("Sound/StartPoint-Aim");

		cbShoot (MakeDir(dst));
		src = Vector2.zero;
		dst = Vector2.zero;
	}

	Vector3 MakeDir(Vector2 tmp)
	{
		Vector3 pos = new Vector3 (src.y - tmp.y, 0f, tmp.x - src.x);
		//Vector3 pos = new Vector3 (src.x - tmp.x, 0f, src.y - tmp.y);
		// pos = Quaternion.Euler(0f, 45f, 0f) * pos;
		return pos.normalized;
	}

	Vector2 src = Vector2.zero;
	Vector2 dst = Vector2.zero;

	void TouchProcess()
	{
		if (0 >= Input.touchCount)
			return;

		Vector2 touch2 = Input.GetTouch (0).position; // 터치한 위치

		if(Input.GetTouch(0).phase == TouchPhase.Began)
		{
			BeginTouch (touch2);
		}
		else if(Input.GetTouch(0).phase == TouchPhase.Moved)    // 터치하고 움직이믄 발생한다.
		{
			DragTouch (touch2);
		}
		else if(Input.GetTouch(0).phase == TouchPhase.Ended)    // 터치 따악 떼면 발생한다.
		{
			EndTouch (touch2);
		}
	}

	void MouseProcess()
	{
		Vector2 touch2 = Input.mousePosition;    // 터치한 위치

		if (Input.GetMouseButton (0)) { 
			if (!BeginTouch (touch2)) {
				DragTouch (touch2);
			}
		}

		if ( Input.GetMouseButtonUp(0) ){
			EndTouch (touch2);
		}
	}


	Vector2 touchKey = new Vector2(1f, -100f);

	void KeyProcess()
	{
		if (Input.GetKey ("up")) {
			BeginTouch (new Vector2(100f, 100f));
			touchKey = new Vector2 (100f, -100f);

			DragTouch (touchKey);
			return;
		}

		if (Input.GetKey ("left")) {
			touchKey.x += 1f;
			touchKey.y += 1f;
			DragTouch (touchKey);
			return;
		}

		if (Input.GetKey ("right")) {
			touchKey.x -= 1f;
			touchKey.y -= 1f;
			DragTouch (touchKey);
			return;
		}
			
		if ( Input.GetKeyDown(KeyCode.Space) ){
			EndTouch (touchKey);
			return;
		}	
	}
}