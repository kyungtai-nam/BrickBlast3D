using UnityEngine;

public class ItemTriRot : MonoBehaviour {
	Block block = null; // only reference 

	public float degree_per_hit = 10f;

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

	public void RotateClock()
	{
		Vector3 angle = transform.localEulerAngles;
		angle.y += degree_per_hit;
		transform.localEulerAngles = angle;
	}

	void HitFX()
	{
		block.turn = 0;
		RotateClock();

		SoundManager.Inst.Play ("Sound/Block-Hit");
	}

	void DestoryFX()
	{	
		// N/A
	}
}
