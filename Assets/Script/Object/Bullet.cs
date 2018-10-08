using UnityEngine;

public class Bullet : MonoBehaviour {	
	public enum E_STATE
	{
		Moving = 0,
		End, 
		Total
	}

	public float VEL_DEFAULT = 10f; // 기본 이동속도

	public Vector3 dir = Vector3.zero;
	float vel = 0f;
	// int m_Hp = 1; // TODO : not used
	Transform m_tm = null;
	SphereCollider m_coll = null;
	float moveTotal = 0f; // 전체 이동량 
	GFSM<E_STATE> m_fsm = null;

	public E_STATE stateDebug = E_STATE.Total;

	public E_STATE state
	{		
		get	{ return this.fsm.state; }
		set
		{
			this.enabled 	= true;
			this.fsm.state 	= value;

			stateDebug = value;
		}
	}


	public GFSM<E_STATE> fsm
	{
		get
		{
			if( null == m_fsm )
			{
				m_fsm = new GFSM<E_STATE>();
				m_fsm.AddState(this, E_STATE.Moving);
				m_fsm.AddState (this, E_STATE.End);
			}
			return m_fsm;
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

	public Transform tm {
		get { 
			if (null == m_tm) {
				m_tm = transform;
			}
			return m_tm;
		}	
	}

	static public Bullet Create(Vector3 pos, Vector3 dir)
	{
		// Debug.LogFormat("Bullet:Create() pos={0} dir={1} id={2}", pos, dir);
		GameObject go = GPoolManager.Inst.Add ("Prefab/Game/Bullet", GameManager.Inst.tmGameRoot);
		go.transform.localPosition = pos;

		Bullet bullet = go.GetComponent<Bullet> ();
		bullet.dir = dir;
		bullet.vel = 0f;
		bullet.fsm.Reset ();
		bullet.state = E_STATE.Moving;
		// bullet.m_Hp = 1;// TODO : not used

		go.gameObject.SetActive (true);
		return bullet;
	}


	// sling
	public bool Raycasting(Vector3 dir, out RaycastHit hit, float maxDist, int layerMask)
	{
		return Physics.SphereCast (tm.localPosition + coll.center, coll.radius, dir, out hit, maxDist, layerMask);
	}

	// collision
	public bool Raycasting(Vector3 pos, Vector3 dir, out RaycastHit hit, float maxDist, int layerMask)
	{
		return Physics.SphereCast (pos + coll.center, coll.radius, dir, out hit, maxDist, layerMask);
	}




	// 중복 충돌 제거 및 위치 보정
	void OnTriggerStay(Collider other)
	{				
		// 충돌 처리 중복으로 들어오는거 거르기
		if( m_colliderEnter == other )
			return;

		// 블록만 
		if( other.gameObject.layer != LAYER_MASK_BLOCK ) 
			return;

		// 이동중 아니면 해당사항 없음 
		if (E_STATE.Moving != this.state)
			return;

		Vector3 vOldDir = dir;
		dir	= GetReflectedDir(m_vRayNormal, false);
		m_colliderEnter	= other;

		// 충돌된 지점 위치 보정
		RaycastHit 	hit;
		if( true == Raycasting(-vOldDir, out hit, Mathf.Infinity, 1<<LAYER_MASK_BLOCK) )
		{
			if (true == Raycasting(hit.point, vOldDir, out hit, Mathf.Infinity, 1 << LAYER_MASK_BLOCK))
				tm.localPosition = hit.point;
		}

		IsEndOfMoving(other);
	}



	Vector3 GetReflectedDir(Vector3 vNormal, bool IsEnableY)
	{
		Vector3 vP	= dir.normalized;
		Vector3 vR	= Vector3.Reflect(vP, vNormal);
		return vR;
	}

	// 이동 종료 검사
	bool IsEndOfMoving(Collider c)
	{
		WallCollider wall = c.GetComponent<WallCollider>();
		if ( wall ) {
			// Debug.Log ("Bullet:IsEndOFMoveing() wall id=" + gameObject.GetInstanceID());
			state = E_STATE.End;
			return true;
		}

		// block decrease HP
		Block block = c.GetComponent<Block> ();
		if( block )
		{
			block.hp -= 1;
			//block.OnHit (1);
			PlayManager.Inst.score++;

			// Debug.Log ("Bullet:IsEndOFMoveing() block id=" + gameObject.GetInstanceID());
			state = E_STATE.End;
			return true;
		}

		// Debug.Log ("Bullet:IsEndOfMoving() not found");
		return false;
	}


	void DestoryFX()
	{	
		// Debug.Log ("Bullet:DestoryFX()");
		// TODO : change vfx
		/*
		GameObject go = GPoolManager.Inst.Add ("Prefab/VFX/ArrowHit2", GameManager.Inst.tmGameRoot);
		go.transform.localPosition = tm.localPosition;
		go.SetActive (true);
		*/
	}

	void OnRemove ()
	{
		// Debug.Log ("Bullet:OnRemove() id=" + gameObject.GetInstanceID());
		GPoolManager.Inst.Delete(gameObject);
	}


	#region FSM

	int LAYER_MASK_BLOCK = 0;

	void Awake()
	{
		LAYER_MASK_BLOCK = LayerMask.NameToLayer("Block");
	}

	void Update()
	{
		fsm.Update();
	}

	void OnMovingEnter()
	{
		// Debug.Log ("Ball:OnMovingEnter()");
		// 이동 상태 시작
		vel = VEL_DEFAULT;
		moveTotal = Mathf.Infinity; // ?? what is this

		m_colliderEnter = null;
		m_colliderLast = null;
		m_vRayNormal = Vector3.zero;
		m_fRayTime = 0f;
	}

	// what is these?
	private Collider		m_colliderEnter		= null;
	private Collider		m_colliderLast		= null;

	private Vector3			m_vRayNormal		= Vector3.zero;
	private float 			m_fRayTime			= 0f;

	public const float MOVEMENT_EPSILON = 0.0001f;
	public const float BALL_INIT_POS = 0.001f;

	// change position
	void OnMovingUpdate()
	{
		// Debug.Log ("Bullet:OnMovingUpdate()");

		Vector3 nextPos = tm.localPosition;
		float dt = Time.smoothDeltaTime;
		float moveTick = Mathf.Min(moveTotal, vel * dt);

		RaycastHit hit;

		while (this.enabled || 0f < moveTotal) {
			
			if (E_STATE.Moving != state) {
				// Debug.LogWarning ("Bullet:OnMovingUpdate() E_STATE.Moving != state, moveTick=" + moveTick);
				break;			
			}

			// Debug.Log ("Bullet:OnMovingUpdate() moveTick=" + moveTick);

			bool isHit = Raycasting(nextPos,
				dir, 
				out hit, 
				Mathf.Infinity, 
				1<<LAYER_MASK_BLOCK); // checked vel * dir

			// no hit!
			if (!isHit) {
				if ( m_colliderLast ) 
				{
					dir	= GetReflectedDir(m_vRayNormal, false);
					m_colliderLast 	= null;
					m_fRayTime		= 0f;
					continue;
				}

				m_fRayTime += dt;
				if (Time.fixedDeltaTime <= m_fRayTime)
					dir = -dir;
				break;
			}

			// Debug.LogWarning ("Ball Hit");

			// hit! 
			m_vRayNormal 	= hit.normal;
			m_colliderLast	= hit.collider;

			if (MOVEMENT_EPSILON >= moveTick)
				break;

			float hitDist = hit.distance;
			if (moveTick <= hitDist) {
				nextPos += dir * moveTick;
				moveTotal -= moveTick;
				moveTick = 0f;
				continue;
			}

			if( true == IsEndOfMoving(hit.collider) )
				hitDist -= BALL_INIT_POS;

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

/*
	void OnMovingExit()
	{
		// Debug.Log ("Bullet:OnMovingExit() id=" + gameObject.GetInstanceID());
//		hp--;
	}
*/
	void OnEndEnter()
	{
		// Debug.LogFormat("Bullet:OnEndEnter() id={0} hp={1}",gameObject.GetInstanceID(), hp);
		// DestoryFX();
		OnRemove ();
	}

	#endregion
}