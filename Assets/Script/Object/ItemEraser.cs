using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemEraser : MonoBehaviour {
	// only reference 
	Block block = null;
	bool axisZ = true;

	public GameObject goAxisZ;
	public GameObject goAxisY;

	void Awake()
	{
		// Debug.Log ("ItemEraser:Awake()");
		block = GetComponent<Block> ();
		block.cbHitFX = HitFX;
		block.cbDestoryFX = DestoryFX;
	}

	public void Init(bool axisZ = true)
	{
		goAxisZ.SetActive (axisZ);
		goAxisY.SetActive (!axisZ);
		this.axisZ = axisZ;
	}
		
	void HitFX()
	{
		SoundManager.Inst.Play ("Sound/ItemEraser-Hit");


		block.turn = 0;

		Affect ();

		string vfxName = null;
		if ( axisZ ) 
			vfxName = "Prefab/VFX/EraserAxisZ";
		else
			vfxName = "Prefab/VFX/EraserAxisX";
		
		GameObject go = GPoolManager.Inst.Add (vfxName, GameManager.Inst.tmGameRoot);

		Vector3 pos = Vector3.zero;

		if (axisZ)
			pos.x = transform.localPosition.x;
		else
			pos.z = transform.localPosition.z;
		
		go.transform.localPosition = pos;
		go.SetActive (true);
	}
		
	void DestoryFX()
	{	
		goAxisZ.SetActive (false);
		goAxisY.SetActive (false);
	}

	void Affect()
	{
		Debug.Log ("ItemEraser:Affect()");

		Vector3 pos = transform.localPosition;

		List<Block> listBlock = PlayManager.Inst.listBlock;

		for (int n = 0; n < listBlock.Count; ) {
			Block block = listBlock[n];
			// is destoied.
			if ( !block || !block.gameObject.activeSelf )
			{
				listBlock.RemoveAt(n);
				continue;
			}

			n++;

			if ( !block.isBlock )
				continue;
			
			float diff = 0;

			if (axisZ) 
				diff = pos.x - block.tm.localPosition.x;
			else 
				diff = pos.z - block.tm.localPosition.z;	
				
			if (Mathf.Abs (diff) > 0.1f)
				continue;
			
			block.hp -= 1;
		}
	}
}