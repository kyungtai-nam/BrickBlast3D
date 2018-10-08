using UnityEngine;
using System;
using System.Collections;

public class Block : MonoBehaviour {
	public enum E_STATE
	{
		Idle = 0,
		Drop,
		Total
	}

	const float DROP_DELAY = 0.8f; // 떨어질때 시간
	const float DROP_DIST = 1f; // shift 될 때 이동 거리
	const float vel = 5f; // 틱당 이동 속도
	// readonly Vector3 dir = Vector3.down;
	readonly Vector3 dir = Vector3.left;

	GFSM<E_STATE> m_fsm = null;
	TextMesh m_TextMesh = null;
	Transform m_tm = null; // cache transform

	Renderer m_render = null;

	ItemType.EType type = ItemType.EType.BLOCK; 

	public ItemType.EType eType {
		get {
			return type;
		}
	}

	// false일 때 bullet에 hit 안됨
	public bool isBlock = true;

	public bool useHP = true;
	int m_Hp = 0;

	public bool useTurn = false;
	public int turn = 0;

	// call by Item
	public Action cbTurn = null;
	public Action cbHitFX = null;
	public Action cbDestoryFX = null;

	DissolveFX m_dissolve = null;

	public DissolveFX dissolve {
		get {
			if (null == m_dissolve)
				m_dissolve = gameObject.AddComponent<DissolveFX> ();
			return m_dissolve;
		}
	}

	public Renderer render {
		get {
			if (null == m_render)
				m_render = GetComponent<Renderer> ();
			return m_render;
		}
	}

	public Transform tm
	{
		get
		{
			if( null == m_tm )
				m_tm = transform;
			return m_tm;
		}
	}

	public bool isIdle
	{
		get { return E_STATE.Idle == state; }
	}

	public GFSM<E_STATE> fsm
	{
		get
		{
			if( null == m_fsm )
			{
				m_fsm = new GFSM<E_STATE>();
				m_fsm.AddState(this, E_STATE.Idle);
				m_fsm.AddState(this, E_STATE.Drop);
			}
			return m_fsm;
		}
	}

	public E_STATE state
	{		
		get	{ return fsm.state; }
		set
		{
			enabled 	= true;
			fsm.state 	= value;
		}
	}

	public TextMesh textMesh {
		get {
			if (null == m_TextMesh) {
				m_TextMesh = transform.Find ("HP").GetComponent<TextMesh> ();
			}
			return m_TextMesh;
		}
	}

	public int hp {
		get { return m_Hp; }
		set {
			if (m_Hp == value)
				return;

			// Debug.Log ("Block:HP set()=" + m_Hp);
			if (!useHP) {
				HitFX();
				return;
			}


			// use HP
			m_Hp = value;
			if (0 < m_Hp) {
				HitFX();
				textMesh.text = value.ToString ();
				return;
			}

			DestoryFX();
			OnRemove ();
		}
	}

	// collision detected
	/*
	public void OnHit(int damage)
	{
		hp -= damage;
	}
	*/

	public void OnHitBall(int damage)
	{
		// turret!
	}
	/*
	public void OnHitBullet(int damage)
	{

	}
	*/

	// init hp
	public void ResetMaxHP(int maxHP)
	{
		m_Hp = maxHP;
		textMesh.text = m_Hp.ToString ();
	}

	void HitFX()
	{
		if (null != cbHitFX) {
			cbHitFX ();
			return;
		}

		SoundManager.Inst.Play ("Sound/Block-Hit");

		// TODO : flickering color
		GameObject go = GPoolManager.Inst.Add ("Prefab/VFX/BlockHitVFX", GameManager.Inst.tmGameRoot);
		go.transform.localPosition = tm.localPosition;
		go.SetActive (true);
	}

	void DestoryFX()
	{
		if (null != cbDestoryFX ) {
			cbDestoryFX();
			return;
		}

		SoundManager.Inst.Play ("Sound/Block-Destroy");

		GameObject go = GPoolManager.Inst.Add ("Prefab/VFX/BoxDestoryVFX", GameManager.Inst.tmGameRoot);
		go.transform.localPosition = tm.localPosition;
		go.SetActive (true);
	}
		


	public void Drop()
	{
		state = E_STATE.Drop;
	}

	void Update()
	{
		this.fsm.Update();
	}

	public void OnRemove()
	{
		GPoolManager.Inst.Delete (gameObject);
	}

	public void SetItemForDebug()
	{
		/*
		render.material = GameManager.Inst.matAddBall;
		textMesh.gameObject.SetActive (false);
		ResetMaxHP (1);
		eType = EType.ADD_BALL;
		*/
	}


	// depreated : Only test
	public static Block CreateRandom(Vector3 pos, int maxHP)
	{
		int v = UnityEngine.Random.Range (0, 100);

		ItemType.EType type = ItemType.EType.BLOCK;

		// 75 block, 20 ball, 10 tri, 5 turret
		if (v < 5)
			type = ItemType.EType.TURRET;
		/*
		else if (v < 15)
			type = ItemType.EType.TRI;
		else if (v < 35)
			type = ItemType.EType.ROTATION_TRI;	
		*/	
		else if (v < 45)
			type = ItemType.EType.BALL;		

		return Block.Create (pos, maxHP, type);
	}

	public static Block CreateUniformRandom(Vector3 pos, int maxHP )
	{
		int n = UnityEngine.Random.Range (0, (int)ItemType.EType.TOTAL);
		ItemType.EType type = (ItemType.EType)n;
		return Block.Create (pos, maxHP, type);
	}


	const int MAX_TURN = 7; // 바닥 바로 앞 -1

	void SetBlock(int maxHP)
	{
		ResetMaxHP (maxHP);
		isBlock = true;
		useHP = true;
		useTurn = false;	
	}

	void SetItemBall()
	{
		ResetMaxHP (1);
		isBlock = false;
		useHP = true;
		useTurn = true;
		turn = MAX_TURN;
	}

	void SetItemOnlyTurn()
	{
		isBlock = false;

		useHP = false;
		useTurn = true;
		turn = MAX_TURN;
		//turn = UnityEngine.Random.Range (2, MAX_TURN); // 2~8
	}


	// call by play manager. (self create)
	public static Block Create(Vector3 pos, int maxHP, ItemType.EType type)
	{
		GameObject go = null;
		Block block = null;

		switch (type) {
		case ItemType.EType.BLOCK:
			go = GPoolManager.Inst.Add ("Prefab/Game/Block", GameManager.Inst.tmGameRoot);
			block = go.GetComponent<Block> ();
			block.SetBlock (maxHP);
			break;

		case ItemType.EType.BALL:
			go = GPoolManager.Inst.Add ("Prefab/Game/ItemBall", GameManager.Inst.tmGameRoot);
			block = go.GetComponent<Block> ();
			block.SetItemBall ();
			break;

		case ItemType.EType.TRI:
			{
				go = GPoolManager.Inst.Add ("Prefab/Game/ItemTri", GameManager.Inst.tmGameRoot);
				block = go.GetComponent<Block> ();
				block.SetItemOnlyTurn ();

				ItemTri tri = go.GetComponent<ItemTri> ();
				// tri.RotateClock90 (UnityEngine.Random.Range (1, 4));
				tri.ResetAngle();

			}
			break;

		case ItemType.EType.ROTATION_TRI:
			{
				go = GPoolManager.Inst.Add ("Prefab/Game/ItemTriRot", GameManager.Inst.tmGameRoot);
				block = go.GetComponent<Block> ();
				block.SetItemOnlyTurn ();

				ItemTriRot triRot = go.GetComponent<ItemTriRot> ();
				triRot.ResetAngle ();
			}
			break;

		case ItemType.EType.TURRET:
			go = GPoolManager.Inst.Add ("Prefab/Game/ItemTurret", GameManager.Inst.tmGameRoot);
			block = go.GetComponent<Block> ();
			block.SetItemOnlyTurn ();
			break;

		case ItemType.EType.ERASER:
			go = GPoolManager.Inst.Add ("Prefab/Game/ItemEraser", GameManager.Inst.tmGameRoot);
			block = go.GetComponent<Block> ();
			block.SetItemOnlyTurn ();

			ItemEraser item = go.GetComponent<ItemEraser> ();
			item.Init (UnityEngine.Random.Range (0, 2) == 0);
			break;

		default:
			go = GPoolManager.Inst.Add ("Prefab/Game/Block", GameManager.Inst.tmGameRoot);
			block = go.GetComponent<Block> ();
			block.ResetMaxHP (maxHP);
			block.isBlock = true;
			break;
		}

		block.tm.localPosition = pos;
		block.type = type;
		block.dissolve.Reset ();

		block.fsm.Reset ();
		block.state = Block.E_STATE.Idle;

		go.SetActive (true);
		return block;	
	}


	#region FSM

	// drop 
	Vector3 src = Vector3.zero;
	Vector3 dst = Vector3.zero;
	float progress = 0f;

	void OnDropEnter()
	{
		src = tm.localPosition;
		dst = tm.localPosition + (dir * DROP_DIST);
		progress = 0f;
	}

	// 떨어지는 상태 업데이트
	void OnDropUpdate()
	{
		progress += vel * Time.smoothDeltaTime;
		float ratio = progress / DROP_DELAY; 
		Vector3 nextPos = Vector3.Lerp (src, dst, ratio);

		tm.localPosition = nextPos;

		if ( 1.0f <= ratio )
			state = E_STATE.Idle;
	}

	void OnDropExit()
	{
		if (!useTurn)
			return;
		
		if (0 < --turn)
			return;

		DestoryFX();
		dissolve.Play (OnRemove);
		// OnRemove ();
	}
	#endregion // #region FSM
}