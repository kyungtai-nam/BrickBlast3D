using UnityEngine;

public class Ball : MonoBehaviour 
{
	public enum E_STATE
	{
		Idle = 0, 
		Moving,
		Sort,
		Total
	}

	public float VEL_DEFAULT = 10f; // 기본 이동속도
	public float VEL_SORT = 15f; // 정렬시 이동속도
	public Vector3 dir { get; set; } // look at
	public float vel = 0f; // velocity
	public bool isRoot = false;

	public delegate void func(Ball ball);
	public func onMovingEnd { get; set; } // event callback

	Transform m_tm = null; // cache transform
	SphereCollider m_coll = null;
	GFSM<E_STATE> m_fsm = null;

	float moveTotal = 0f; // 전체 이동량 


	public E_STATE state
	{		
		get	{ return this.fsm.state; }
		set
		{
			this.enabled 	= true;
			this.fsm.state 	= value;
		}
	}
		
	public bool isIdle
	{
		get { return E_STATE.Idle == this.state; }
	}

	public GFSM<E_STATE> fsm
	{
		get
		{
			if( null == m_fsm )
			{
				m_fsm = new GFSM<E_STATE>();
				m_fsm.AddState(this, E_STATE.Idle);
				m_fsm.AddState(this, E_STATE.Moving);
				m_fsm.AddState(this, E_STATE.Sort);
			}
			return m_fsm;
		}
	}

	public Transform tm
	{
		get
		{
			if( null == m_tm )
				m_tm = this.transform;
			return m_tm;
		}
	}

	public SphereCollider coll
	{
		get {
			if (null == m_coll)
				m_coll = GetComponent<SphereCollider> ();
			return m_coll;
		}
	}

	/*
	void Awake()
	{
		Debug.LogErrorFormat ("Ball Center={0},Rad={1}", coll.center, coll.radius);
	}
	*/

	void Update()
	{
		fsm.Update();
	}


	public void Reset()
	{
		fsm.Reset ();
	}

	// sling
	public bool Raycasting(Vector3 dir, out RaycastHit hit, float maxDist, int layerMask)
	{
		return Physics.SphereCast (tm.localPosition, coll.radius, dir, out hit, maxDist, layerMask);
	}

	// collision
	public bool Raycasting(Vector3 pos, Vector3 dir, out RaycastHit hit, float maxDist, int layerMask)
	{
		return Physics.SphereCast (pos, coll.radius, dir, out hit, maxDist, layerMask);
	}



	static public Ball Create(Vector3 pos, func callback)
	{
		GameObject go = GPoolManager.Inst.Add ("Prefab/Game/Ball", GameManager.Inst.tmGameRoot);

		Ball ball = go.GetComponent<Ball> ();

		ball.state = E_STATE.Idle;
		ball.dir = Vector3.zero;
		ball.vel = 0f;
		ball.onMovingEnd = callback;

		go.transform.localPosition = pos;
		// go.transform.localScale = Vector3.one * 0.3f;
		go.gameObject.SetActive (true);
		return ball;
	}


	bool IsBlockOrItem(int layer)
	{
		if (layer == PlayManager.Inst.LAYER_MASK_BLOCK)
			return true;

		if (layer == PlayManager.Inst.LAYER_MASK_ITEM)
			return true;

		return false;
	}

	// 중복 충돌 제거 및 위치 보정
	void OnTriggerStay(Collider other)
	{				
		// 충돌 처리 중복으로 들어오는거 거르기
		if( m_colliderEnter == other )
			return;

		// 블록만 
		if( !IsBlockOrItem(other.gameObject.layer) ) 
			return;
		
		// 이동중 아니면 해당사항 없음 
		if (E_STATE.Moving != this.state)
			return;

		Vector3 vOldDir = dir;
		dir	= GetReflectedDir(m_vRayNormal, false);
		m_colliderEnter	= other;


		// 충돌된 지점 위치 보정
		RaycastHit 	hit;
		if( true == Raycasting(-vOldDir, out hit, Mathf.Infinity, PlayManager.Inst.LAYER_MASK_BLOCK_ITEM) )
		{
			if (true == Raycasting(hit.point, vOldDir, out hit, Mathf.Infinity, PlayManager.Inst.LAYER_MASK_BLOCK_ITEM))
				tm.localPosition = hit.point;
		}

		IsEndOfMoving(other);
	}


	Vector3 GetReflectedDir(Vector3 vNormal, bool IsEnableY)
	{
		Vector3 vP	= dir.normalized;
		Vector3 vR	= Vector3.Reflect(vP, vNormal);

		// Debug.LogWarningFormat ("GetReflectedDir() dir={0},norm={1},refl={2}", dir, vNormal, vR);
		/*
		if( false == IsEnableY )
		{
			if( !Mathf.Approximately(vR.y, 0f) )
			{
				vR.z += vR.y;
				vR.y = 0f;
			}
		}
		*/
		return vR;
	}

	// 이동 종료 검사
	bool IsEndOfMoving(Collider c)
	{
		WallCollider wall = c.GetComponent<WallCollider>();
		if ( wall ) {
			if (!wall.isStopArea) {
				// 공 여러개일 때 너무 시끄러움
				// SoundManager.Inst.Play ("Sound/Block-Hit");	
				return false;
			}
			// reach bottom wall
			if (null != onMovingEnd)
				onMovingEnd (this);

			state = E_STATE.Sort;	
			return true;
		}

		// block decrease HP
		Block block = c.GetComponent<Block> ();
		if( block )
		{
			PlayManager.Inst.CollisionBallWithBlock ();

			// block.OnHit (1);
			block.hp -= 1;

			switch (block.eType) {
			case ItemType.EType.TRI:
			case ItemType.EType.ROTATION_TRI:
				break;

			default:
				PlayManager.Inst.score++;	
				break;
			}
		}
		return false;
	}



	#region FSM




	void OnMovingEnter()
	{
		// Debug.Log ("Ball:OnMovingEnter()");

		// 이동 상태 시작
		vel = VEL_DEFAULT;
		moveTotal = Mathf.Infinity; // ?? what is this
		isRoot = false;
	}

	// what is these?
	private Collider		m_colliderEnter		= null;
	private Collider		m_colliderLast		= null;

	private Vector3			m_vRayNormal		= Vector3.zero;
	private float 			m_fRayTime			= 0f;

	public const float MOVEMENT_EPSILON = 0.0000001f;
	public const float BALL_INIT_POS = 0.001f;

	// change position
	void OnMovingUpdate()
	{
		Vector3 nextPos = tm.localPosition;
		float dt = Time.smoothDeltaTime;
		float moveTick = Mathf.Min(moveTotal, vel * dt);

		RaycastHit hit;

		while (this.enabled || 0f < moveTotal) {			
			bool isHit = Raycasting(nextPos
				, dir
				, out hit
				, Mathf.Infinity
				, PlayManager.Inst.LAYER_MASK_BLOCK_ITEM); // checked vel * dir

			// no hit!
			if (!isHit) {
				if ( m_colliderLast ) 
				{
					dir	= GetReflectedDir(m_vRayNormal, false);
					m_colliderLast 	= null;
					m_fRayTime		= 0f;
					continue;
				}

				// 끼어 있는 상태로 봐야 하나?
				m_fRayTime += dt;
				if (Time.fixedDeltaTime <= m_fRayTime)
					dir = -dir;
				break;
			}

			// Debug.LogWarning ("Ball Hit");

			// hit! 
			m_vRayNormal 	= hit.normal;
			m_colliderLast	= hit.collider;

			if (0f >= moveTick)
			// if (MOVEMENT_EPSILON >= moveTick)
				break;

			float hitDist = hit.distance;
			if (moveTick <= hitDist) {
				nextPos += dir * moveTick;
				moveTotal -= moveTick;
				moveTick = 0f;
				continue;
			}

			if (true == IsEndOfMoving (hit.collider)) {
			//	hitDist -= BALL_INIT_POS;
			}

			nextPos += dir * hitDist;
			moveTotal -= hitDist;
			moveTick -= hitDist;

			dir	= GetReflectedDir(hit.normal, false);
			m_colliderEnter	= hit.collider;			
			continue;		
		}

//		m_fSpeed += this.accSpeed*fDeltaTime;
		tm.localPosition = nextPos;
	}



	// 정렬 상태 시작
	void OnSortEnter()
	{
		// Debug.Log("Ball:OnSortEnter()");

		if (isRoot) {
			state = E_STATE.Idle;
			return;
		}

		vel = VEL_SORT;
	}

	// 정렬 상태 업데이트
	void OnSortUpdate()
	{	
		Vector3 nextPos = tm.localPosition;
		Vector3 lookAt = PlayManager.Inst.rootBall.tm.localPosition - nextPos;

		float dist = lookAt.magnitude;
		float moveTick = Time.smoothDeltaTime * vel;

		if ( moveTick >= dist )
		{
			moveTick = dist;
			state = E_STATE.Idle;
		}

		nextPos += lookAt.normalized * moveTick;
		tm.localPosition = nextPos;
	}


#endregion

}