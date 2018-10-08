using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IsometricMover : MonoBehaviour {
	public Vector3 dir = Vector3.right;

	List<GameObject> list = new List<GameObject>();
	int step = 1;

	void Start() {
		step = 1;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			MakeBlock ();
			return;
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			ShiftBlock ();
			return;
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			Clear ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			MakeIso ();
		}
	}


	// screen size
	const int SIZE_X = 10;
	const int SIZE_Y = 10;

	const int sx = -SIZE_X / 2;
	const int sy = -SIZE_Y / 2; 
	const int dx = SIZE_X / 2;
	const int dy = SIZE_Y / 2;

	public bool axisXY_ = true;
	public bool axisX_Y = false;
	public bool axisY_X = false;

	public bool axis_XY = false;
	public bool axis_YX = false;

	public bool shift_x = true;
	public bool shift_y = false;
	public bool shift_z = false;

	public bool inv_shift_x = false;
	public bool inv_shift_y = false;
	public bool inv_shift_z = false;



	Vector2 cart2iso(int x, int y)
	{
		Vector2 iso = new Vector2 ();
		iso.x = x - y;
		iso.y = (x + y) * 0.5f; // div 2
		return iso;
	}

	Vector2 iso2cart(Vector2 iso)
	{
		Vector2 cart = new Vector2 ();
		cart.x = (2f * iso.y + iso.x) * 0.5f; // div 2
		cart.y = (2f * iso.y - iso.x) * 0.5f; // div 2
		return cart;
	}

	// for 2d
	void MakeIso()
	{
		for (int y = -1; y <= 1; y++) {
			for (int x = -1; x <= 1; x++) {
				Vector2 iso = cart2iso (x, y);
				list.Add (Block.Create (new Vector3 (iso.x, iso.y, 0f), 1, ItemType.EType.BLOCK).gameObject);
			}
		}
	}

	void MakeBlock()
	{
		int y = dy;

		for (int x = sx+1; x < dx; x++) {
			bool isSpawn = Random.Range (0, 2) == 0 ? true : false;

			if (!isSpawn)
				continue;

			if ( axisXY_ ) 
				list.Add(Block.CreateRandom(new Vector3 (x, y, 0f), step).gameObject);
			if ( axisX_Y ) 
				list.Add(Block.CreateRandom(new Vector3 (x, 0f, y), step).gameObject);
			if ( axisY_X )
				list.Add(Block.CreateRandom(new Vector3 (y, 0f, x), step).gameObject);
			if ( axis_XY ) 
				list.Add(Block.CreateRandom(new Vector3 (0f, x, y), step).gameObject);
			if ( axis_YX) 
				list.Add(Block.CreateRandom(new Vector3 (0f, y, x), step).gameObject);
		}

		step++;
	}

	void ShiftBlock()
	{
		for (int n = 0; n < list.Count; n++) {

			Vector3 pos = list [n].transform.localPosition;
			if ( shift_x ) 
				pos.x += 1f;
			if (inv_shift_x)
				pos.x -= 1f;

			if ( shift_y ) 
				pos.y += 1f;
			if (inv_shift_y)
				pos.y -= 1f;

			if ( shift_z ) 
				pos.z += 1f;
			if (inv_shift_z)
				pos.z -= 1f;		

			list [n].transform.localPosition = pos;
		}
	}

	void Clear()
	{
		list.ReleaseToPool();
	}
}
