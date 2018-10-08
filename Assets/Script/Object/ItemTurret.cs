using UnityEngine;

public class ItemTurret : MonoBehaviour {
	// only reference 
	Block block = null;

	void Awake()
	{
		Debug.Log ("ItemTurret:Awake()");

		block = GetComponent<Block> ();
		block.cbHitFX = HitFX;
		block.cbDestoryFX = DestoryFX;

	}

	void HitFX()
	{		
		Affect ();

		// TODO : flickering color
		GameObject go = GPoolManager.Inst.Add ("Prefab/VFX/ArrowHit2", GameManager.Inst.tmGameRoot);
		go.transform.localPosition = transform.localPosition;
		go.SetActive (true);
	}

	void DestoryFX()
	{	
		/*
		GameObject go = GPoolManager.Inst.Add ("Prefab/VFX/PickUnit", transform);
		go.SetActive (true);
		*/
	}

	// x,y axis
	//Vector3[] arrDir = new Vector3[]{Vector3.left, Vector3.right, Vector3.up, Vector3.down };

	// x, z axis
	Vector3[] arrDir = new Vector3[]{Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

	void Affect()
	{
		Debug.Log ("ItemTurret:Affect()");

		// one way!
		//int n = 3; // dn
		int n = 2; // up

		// create 4 dir bullets 
		for (n = 0; n < 4; n++) 
		{
			Vector3 pos = transform.localPosition;
			pos += arrDir [n] * 0.5f;
			Bullet.Create (pos, arrDir[n]);
			// Bullet bullet = 
		}
	}
}