using UnityEngine;

public class ItemBall : MonoBehaviour {	

	// only reference 
	Block block = null;

	void Awake()
	{
		block = GetComponent<Block> ();
		block.cbHitFX = HitFX;
		block.cbDestoryFX = DestoryFX;
	}
		
	void OnTurn()
	{

	}

	void OnRemove()
	{
		
	}


	void HitFX()
	{
		// N/A
	}

	void DestoryFX()
	{
		Affect ();

		SoundManager.Inst.Play ("Sound/ItemBall-Destory");

		GameObject go = GPoolManager.Inst.Add ("Prefab/VFX/BoxDestoryVFX", GameManager.Inst.tmGameRoot);
		go.transform.localPosition = transform.localPosition;
		go.SetActive (true);		
	}

	void Affect()
	{
		// Debug.Log ("ItemBall:Affect()");
		PlayManager.Inst.ballCount++;

		if (ConfigManager.Inst.maxBallCount < PlayManager.Inst.ballCount)
			PlayManager.Inst.ballCount = ConfigManager.Inst.maxBallCount;
	}
}