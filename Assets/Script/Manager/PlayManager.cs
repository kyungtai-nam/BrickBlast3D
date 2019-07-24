using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 게임 플레이 자체 진행
public class PlayManager : GSingletonMono<PlayManager> 
{
	// screen size
	const int SIZE_X = 10;
	const int SIZE_Y = 10;

	const int sx = -SIZE_X / 2;
	const int sy = -SIZE_Y / 2; 
	const int dx = SIZE_X / 2;
	const int dy = SIZE_Y / 2;

	const int startY = dy - 1;
	const int endY = sy + 2; // bottom 
	const int endX = sx + 2; // isometric left + bottom

	const float BALL_SHOOTING_PERIOD = 0.1f;
	const float MAX_ELPASED_TIME = 1f;

	public System.Action cbGameResult = null;

	public enum E_STATE
	{
		Idle,
		BallMoving,
		BlockMoving,
		GameOver,
		Total
	}

	Vector3 dir = Vector3.zero; // look at

	public int step = 0; // block HP
	public int ballCount = 0; // need to make ball

	public int score = 0;
	public Ball rootBall = null;

	List<Ball> listBall = new List<Ball>();
	public List<Block> listBlock = new List<Block>();
	List<GameObject> listWall = new List<GameObject>();
	List<int> listPos = new List<int> (); // block random pos

	GameObject floorWalls = null;
	GameObject walls = null;
	GameObject wallsCollider = null;
	StartPoint startPoint = null;
	GFSMCoroutine<E_STATE> m_fsm = null;

	public GFSMCoroutine<E_STATE> fsm
	{
		get
		{
			if( null == m_fsm )
			{
				// Debug.Log ("PlayManager:fsm init");

				m_fsm = new GFSMCoroutine<E_STATE>(this);
				m_fsm.AddState(E_STATE.Idle			, OnIdleState);
				m_fsm.AddState(E_STATE.BallMoving	, OnBallMovingState);
				m_fsm.AddState(E_STATE.BlockMoving	, OnBlockMovingState);
				m_fsm.AddState(E_STATE.GameOver		, OnGameOverState);
			}
			return m_fsm;
		}
	}

	public E_STATE stateDebug; // debug

	public E_STATE state
	{
		get { return fsm.state; }
		set { 
			stateDebug = value;
			fsm.state = value; 
		}
	}
		

	void ShootBall(Vector3 dir)
	{
		GameManager.Inst.tmCameraTopView.gameObject.SetActive (false);

		startPoint.gameObject.SetActive (false);

		this.dir = dir;
		this.state = E_STATE.BallMoving;	
	}

	IEnumerator OnIdleState(E_STATE eState)
	{
		// 준비
		dir = Vector3.zero;
	
		MakeStartPoint ();

		while(fsm.IsEqualState(eState))
		{	
			yield return null;
		}

		rootBall = null;

		// 가지고 있는 공 수 만큼 생성
		for(int n=listBall.Count; n<ballCount; n++)
		{
			listBall.Add(Ball.Create(listBall[0].tm.localPosition, OnReachedBall));
		}
	}

	#region Callback

	// 공 도착 : 첫번째 도착한 공인지 체크 하기 위함
	void OnReachedBall(Ball ball)
	{
		if( rootBall )
			return;

		ball.isRoot = true;
		rootBall = ball;

		ResetStartPoint ();
	}
	#endregion


	#region FSM

	float lastCollisionTime = 0f;

	public void CollisionBallWithBlock()
	{
		lastCollisionTime = 0f;
	}

	IEnumerator OnBallMovingState(E_STATE eState)
	{		
		Debug.Log ("PlayManager:OnBallMovingState()");

		// 공 이동 시작
		for(int i=0; i<listBall.Count; ++i)
		{
			listBall[i].dir = dir;
			listBall[i].state = Ball.E_STATE.Moving;
			yield return new WaitForSeconds(BALL_SHOOTING_PERIOD);
		}

		bool showMsg = false;
		float elpasedBall = 0f;

		while(fsm.IsEqualState(eState))
		{
			elpasedBall += Time.deltaTime;
			lastCollisionTime += Time.deltaTime;

			if (elpasedBall > ConfigManager.Inst.maxBallElpasedTime) {
				if (!showMsg) {
					FastForward ();
					// UIManager.Inst.PopupMessage (Localization.Get("move_faster"), 1f);
					showMsg = true;
				}
			}

			if (lastCollisionTime > ConfigManager.Inst.maxNoCollBlock) {
				lastCollisionTime = 0f;

				UIManager.Inst.PopupMessage (Localization.Get("roll_ball"), 1f);
				RollBall (); 
			}

			if( true == IsEndOfBallMoving() ) 
				break;
			yield return null;
		}

		NormalForawrd ();
		state = E_STATE.BlockMoving;
	}

	void RollBall()
	{
		for (int n = 0; n < listBall.Count; n++) {
			Vector3 dir = new Vector3(Random.Range(-1f,1f), 0f, Random.Range(-1f,1f));
			listBall[n].dir = dir.normalized;
		}
	}

	void VerticalRollBall()
	{
		for (int n = 0; n < listBall.Count; n++) {
			Vector3 dir = new Vector3 (0f, 0f, 1f);
			listBall[n].dir = dir.normalized;
		}
	}


	void FastForward()
	{
		Time.timeScale = ConfigManager.Inst.fastForwardTimeScale;
		UIManager.Inst.forwardIcon.SetActive (true);
	}

	void NormalForawrd()
	{
		Time.timeScale = 1f;
		UIManager.Inst.forwardIcon.SetActive (false);
	}

	// 공 이동 종료 검사
	bool IsEndOfBallMoving()
	{
		for (int n = 0; n < listBall.Count; n++) {
			if (!listBall [n].isIdle)
				return false;
		}
		return true;
	}

	// 블럭 이동 종료 검사
	bool IsEndOfBlockMoving()
	{
		for(int n=0;n<listBlock.Count;) {
			Block block = listBlock[n];

			// is destoied.
			if ( !block || !block.gameObject.activeSelf )
			{
				listBlock.RemoveAt(n);
				continue;
			}

			if ( !block.isIdle )
				return false;

			n++;
		}
		return true;
	}

	void ForceIdleOfBlocks()
	{
		for (int n = 0; n < listBlock.Count;) {
			Block block = listBlock [n];

			// is destoied.
			if (!block || !block.gameObject.activeSelf) {
				listBlock.RemoveAt (n);
				continue;
			}

			block.state = Block.E_STATE.Idle;
			n++;
		}
	}

	bool IsEndOfGame()
	{
		float min = dy;

		for (int n = 0; n < listBlock.Count; n++) {
			Block block = listBlock [n];

			if (!block.isBlock)
				continue;
			
			if (min > block.tm.localPosition.x)
				min = block.tm.localPosition.x;			
		}

		// Debug.Log ("IsReachBottom() min y=" + minY);

		// is end of game
		if (endX > min) {
			state = E_STATE.GameOver;
			return true;
		}

		return false;
	}


	float durationBlockMoving = 0f;

	IEnumerator OnBlockMovingState(E_STATE eState)
	{
		// 블럭 이동 -> 블럭 생성
		MakeBlock();
		AddStep ();
		ShiftBlock();

		durationBlockMoving = 0f;

		while(fsm.IsEqualState(eState))
		{
			if (IsEndOfBlockMoving ()) {
				if (IsEndOfGame ())
					state = E_STATE.GameOver;
				else
					state = E_STATE.Idle;
			}

			durationBlockMoving += Time.deltaTime;

			if ( 5f < durationBlockMoving) {				
				Debug.LogWarning ("Block Hang");
				// UIManager.Inst.PopupMessage ("Block HANG", 1f);
				ForceIdleOfBlocks ();
				state = E_STATE.Idle;
			}
			yield return null;
		}
	}


	IEnumerator OnGameOverState(E_STATE eState)
	{
		// DropOutBlock ();
		Debug.Log ("PlayManger:OnGameOverState");
		// UIManager.Inst.popupResult.gameObject.SetActive (true);

		if (null != cbGameResult) {
			cbGameResult ();
			fsm.Reset ();
		}

		yield return null;

		/*
		while(fsm.IsEqualState(eState))
		{
			Debug.Log ("PlayManger:OnGameOverState");
			yield return null;
		}
		*/
	}

	#endregion // #region FSM


	void MakeWallCollider()
	{
		// only view
		walls = GPoolManager.Inst.Add ("Prefab/Game/Walls", GameManager.Inst.tmGameRoot, true);
		walls.transform.localPosition = new Vector3 (0f, -1f, 0f);

		// only collider 
		wallsCollider = GPoolManager.Inst.Add ("Prefab/Game/WallsCollider", GameManager.Inst.tmGameRoot, true);
		wallsCollider.transform.localPosition = Vector3.zero;

		// TODO : sizeX, sizeY에 맞춰서 벽 충돌영역 조절 필요
	}
		
	void MakeFloorBlock()
	{
		GameObject go = GPoolManager.Inst.Add ("Prefab/Game/FloorWalls", GameManager.Inst.tmGameRoot, true);
		go.transform.localPosition = new Vector3 (0, -1f, 0);
	}


	// only top view
	void MakeWall()
	{
		for (int y = sy; y <= dy; y++) {
			for (int x = sx; x <= dx; x++) {

				bool isSpawn = false;

				// top, bottom
				if (y == sy || y == dy) {
					isSpawn = true;
				}

				// left, right
				else if ( x == sx || x == dx ) {
					isSpawn = true;
				}

				if (!isSpawn)
					continue;

				GameObject go = GPoolManager.Inst.Add ("Prefab/Game/Wall", GameManager.Inst.tmGameRoot, true);
				// before block depth
				go.transform.localPosition = new Vector3 (y, 0f, x);
				listWall.Add (go);
			}
		}
	}


	void ResetStartPoint()
	{
		if (null == rootBall)
			return;
		
		startPoint.transform.localPosition = rootBall.tm.localPosition;
	}

	void MakeStartPoint()
	{
		if (null == startPoint) {
			GameObject go = GPoolManager.Inst.Add ("Prefab/Game/StartPoint", GameManager.Inst.tmGameRoot, false);
			startPoint = go.GetComponent<StartPoint> ();
			startPoint.cbShoot = ShootBall;
		}

		startPoint.gameObject.SetActive (true);
		ResetStartPoint ();

		GameManager.Inst.tmCameraTopView.gameObject.SetActive (true);
	}

	void MakeBall()
	{
		float v = sx + 0.6f; // bottom y point + start point height + ball height ??
		Ball ball = Ball.Create(new Vector3 (v, 0f, 0f), OnReachedBall);

		listBall.Add(ball);
	
		if (null == rootBall)
			rootBall = ball;
	}

/*
	// debug
	void MakeTurret()
	{
		int y = dy;

		for (int x = sx+1; x < dx; x++)
			listBlock.Add (Block.Create (new Vector3 (y, 0f, x), step, ItemType.EType.TURRET));		
	}

	// debug
	void MakeCustomTotalBlock()
	{
		int y = dy;
		int x = sx + 1;

		listBlock.Add (Block.Create (new Vector3 (x++, y, 0f), step, ItemType.EType.BLOCK));
		listBlock.Add (Block.Create (new Vector3 (x++, y, 0f), step, ItemType.EType.TRI));
		listBlock.Add (Block.Create (new Vector3 (x++, y, 0f), step, ItemType.EType.TRI));

		listBlock.Add (Block.Create (new Vector3 (x++, y, 0f), step, ItemType.EType.BLOCK));
		listBlock.Add (Block.Create (new Vector3 (x++, y, 0f), step, ItemType.EType.BLOCK));
		//		x++;

		if ( step%2 == 1 ) 
			listBlock.Add (Block.Create (new Vector3 (x++, y, 0f), step, ItemType.EType.TURRET));
		else
			listBlock.Add (Block.Create (new Vector3 (x++, y, 0f), step, ItemType.EType.BLOCK));

		//		x++;
		listBlock.Add (Block.Create (new Vector3 (x++, y, 0f), step, ItemType.EType.BLOCK));
		listBlock.Add (Block.Create (new Vector3 (x++, y, 0f), step, ItemType.EType.BLOCK));
		listBlock.Add (Block.Create (new Vector3 (x++, y, 0f), step, ItemType.EType.BLOCK));

		step++;
	}

	// debug
	void MakeCustomBlock()
	{
		int y = dy;
		int x = sx + 1;

		x++;
		x++;
		x++;

		listBlock.Add (Block.Create (new Vector3 (x++, y, 0f), step, ItemType.EType.BLOCK));

		if ( step%2 == 1 ) 
			listBlock.Add (Block.Create (new Vector3 (x++, y, 0f), step, ItemType.EType.TURRET));
		else
			listBlock.Add (Block.Create (new Vector3 (x++, y, 0f), step, ItemType.EType.BLOCK));
		
//		x++;
		listBlock.Add (Block.Create (new Vector3 (x++, y, 0f), step, ItemType.EType.BLOCK));
		x++;
		x++;

		step++;
	}
*/
	// debug
	void MakeTriangle()
	{
		int y = dy;

		for (int x = sx + 1; x < dx; x += 2)
			listBlock.Add (Block.Create (new Vector3 (y, 0f, x), step, ItemType.EType.TRI));
	}

	int GetRandomBlockCount()
	{
		if (3 > step)
			return Random.Range (2, 4);

		return Random.Range (2, listPos.Count);
	}

	public void MakeBlock()
	{
		listPos.Shuffle ();

		int maxCount = GetRandomBlockCount();

		// uniq item in a row
		bool spawnItem = false;

		bool spawnBall = false;
		bool spawnTurret = true;

		bool spawnTri = true;
		bool spawnTriRot = false;

		bool spawnEraser = false;

		if (2 > step)
			spawnItem = true;

		bool isDouble = step % 20 == 0;
		int blockHP = step;

		if (isDouble) {
			maxCount = 3;
			blockHP *= 2;

			spawnTriRot = true;
			spawnEraser = true;
		}

		for (int n = 0; n < maxCount; n++) {

			// must a ball in a line
			if (!spawnBall) {
				spawnBall = true;
				listBlock.Add (Block.Create (new Vector3 (startY, 0, listPos[n]), step, ItemType.EType.BALL));
				continue;
			}

			if (step <= 3) {
				listBlock.Add (Block.Create (new Vector3 (startY, 0, listPos[n]), step, ItemType.EType.BLOCK));
				continue;
			}

			//if (!spawnItem) {
				
				if (!spawnTurret) {
					if (Random.Range (0, 100) <= 5) {
						spawnItem = spawnTurret = true;
						listBlock.Add (Block.Create (new Vector3 (startY, 0, listPos[n]), step, ItemType.EType.TURRET));
						continue;
					}
				}

				if (!spawnTri) {
					if (Random.Range (0, 100) <= 100) {
						spawnTri = true;
						listBlock.Add (Block.Create (new Vector3 (startY, 0, listPos[n]), step, ItemType.EType.TRI));
						continue;
					}
				}

				if (!spawnTriRot) {
					if (Random.Range (0, 100) <= 50) {
						spawnItem = spawnTriRot = true;
						listBlock.Add (Block.Create (new Vector3 (startY, 0, listPos[n]), step, ItemType.EType.ROTATION_TRI));
						continue;
					}
				}

				if (!spawnEraser) {
					if (Random.Range (0, 100) <= 50) {
						spawnItem = spawnEraser = true;
						listBlock.Add (Block.Create (new Vector3 (startY, 0, listPos[n]), step, ItemType.EType.ERASER));
						continue;
					}
				}
			// }

			listBlock.Add (Block.Create (new Vector3 (startY, 0, listPos[n]), blockHP, ItemType.EType.BLOCK));
		}
	}

	void AddScoreByStep()
	{
		// +5 / 10 step
		PlayManager.Inst.score += (step-1) * (step / 10) + 1 * 5;
	}

	public void AddStep()
	{
		AddScoreByStep ();
		step++;

		string ach_gid = null;

		if (5 == step)
			ach_gid = RcGameService.ACH_GID_NEWBIE;
		else if (20 == step)
			ach_gid = RcGameService.ACH_GID_BRONZE;
		else if (50 == step)
			ach_gid = RcGameService.ACH_GID_SILVER;
		else if (100 == step)
			ach_gid = RcGameService.ACH_GID_GOLD;
		else if (200 == step)
			ach_gid = RcGameService.ACH_GID_PLATINUM;		

		if (string.IsNullOrEmpty (ach_gid))
			return;
		
		RcGameService.Inst.SaveAchievement(ach_gid);
	}

	public void ShiftBlock()
	{
		for (int n = 0; n < listBlock.Count; ) {
			Block block = listBlock[n];

			// is destoied.
			if ( !block || !block.gameObject.activeSelf )
			{
				listBlock.RemoveAt(n);
				continue;
			}

			block.Drop();
			n++;
		}
	}
		
	public void DropOutBlock()
	{
		for (int n = 0; n < listBlock.Count; ) {
			Block block = listBlock[n];

			// is destoied.
			if ( !block || !block.gameObject.activeSelf )
			{
				listBlock.RemoveAt(n);
				continue;
			}

			block.hp = 0;
			n++;
		}
	}



	// for debug
	IEnumerator ApplyDamageAllBlock(int damage, float delay=0.1f)
	{
		for (int n = 0; n < listBlock.Count; ) {
			Block block = listBlock[n];

			// is destoied.
			if ( !block || !block.gameObject.activeSelf )
			{
				listBlock.RemoveAt(n);
				continue;
			}

			block.hp -= damage;
			n++;

			yield return new WaitForSeconds (delay);
		}
	}
		

	public int LAYER_MASK_WALL = 0;
	public int LAYER_MASK_BALL = 0;
	public int LAYER_MASK_BLOCK = 0;
	public int LAYER_MASK_ITEM = 0;
	public int LAYER_MASK_BLOCK_ITEM = 0;

	protected override void Awake()
	{
		LAYER_MASK_BALL = LayerMask.NameToLayer("Ball");
		LAYER_MASK_BLOCK = LayerMask.NameToLayer("Block");
		LAYER_MASK_ITEM = LayerMask.NameToLayer ("Item");
		LAYER_MASK_WALL = LayerMask.NameToLayer ("Wall");
		LAYER_MASK_BLOCK_ITEM = 1 << LAYER_MASK_BLOCK | 1 << LAYER_MASK_ITEM | 1 << LAYER_MASK_WALL;

		for (int x = sx+1; x < dx; x++)
			listPos.Add (x);
	}

	public void Init(bool isNew=true)
	{		
		Release ();

		if (isNew) {
			score = 0;
			step = 1;
			ballCount = 1;
		}

		GameManager.Inst.goFloor.SetActive (true);
		MakeWallCollider ();
		MakeWall ();
		MakeFloorBlock ();

		MakeBall ();
		MakeStartPoint ();

		state = E_STATE.BlockMoving;
	}


	public void Release()
	{
		Debug.Log ("PlayManager:Release()");
		fsm.Reset ();

		GameManager.Inst.goFloor.SetActive (false);

		listBlock.ReleaseToPool ();
		listBall.ReleaseToPool ();
		listWall.ReleaseToPool ();

		if (floorWalls) {
			GPoolManager.Inst.Delete (floorWalls);
			floorWalls = null;
		}

		if (walls) {
			GPoolManager.Inst.Delete (walls);
			walls = null;
		}

		if ( wallsCollider ) {
			GPoolManager.Inst.Delete (wallsCollider);
			wallsCollider = null;
		}
			
		if (startPoint) {
			startPoint.Release ();
			GPoolManager.Inst.Delete (startPoint.gameObject);
			startPoint = null;
		}

		rootBall = null;
	}

#if UNITY_EDITOR



	// for debug
	IEnumerator ApplyDamageOnlyBlock(int damage, float delay=0.1f)
	{
		for (int n = 0; n < listBlock.Count; ) {
			Block block = listBlock[n];

			// is destoied.
			if ( !block || !block.gameObject.activeSelf )
			{
				listBlock.RemoveAt(n);
				continue;
			}

			if (!block.isBlock) {
				n++;	
				continue;
			}

			block.hp -= damage;
			n++;

			yield return new WaitForSeconds (delay);
		}
	}


	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			ballCount++;
			return;
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			ballCount--;
			return;
		}

		if (Input.GetKey (KeyCode.Alpha3)) {
			if ( state == E_STATE.Idle ) 
				state = E_STATE.BlockMoving;
			return;
		}

		if (Input.GetKeyDown (KeyCode.Alpha4)) { 
			ShiftBlock();
			return;
		}

		if (Input.GetKeyDown (KeyCode.Alpha5)) { 
			BreakRandomBlock ();
			return;
		}

		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			StartCoroutine(ApplyDamageAllBlock(1));
			return;
		}

		if (Input.GetKeyDown (KeyCode.Alpha8)) {
			StartCoroutine(ApplyDamageAllBlock(1000));
			return;
		}
			
		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			StartCoroutine(ApplyDamageOnlyBlock(1000));
			return;
		}

		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			state = E_STATE.GameOver;
			return;
		}

		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			UIManager.Inst.PopupMessage ("VERT!", 0.5f);
			VerticalRollBall ();
			return;
		}
	}
#endif


	void BreakRandomBlock() {
		Block block = listBlock.Random ();
		block.hp = 0;
	}
}