using UnityEngine;

public class ItemTri : MonoBehaviour {	
	Block block = null; // only reference 

	void Awake()
	{
		block = GetComponent<Block> ();
		block.cbHitFX = HitFX;
		block.cbDestoryFX = DestoryFX;
	}

	public void ResetAngle()
	{
		Vector3 angle = transform.localEulerAngles;
		//angle.y = Random.Range (0, 2) == 0 ? 135f : 315f; 
		angle.y = Random.Range (0, 2) == 0 ? 90f : 270f; 
		transform.localEulerAngles = angle;
	}

	void HitFX()
	{
		block.turn = 0;
		SoundManager.Inst.Play ("Sound/Block-Hit");
	}

	void DestoryFX()
	{
		// N/A
	}

}