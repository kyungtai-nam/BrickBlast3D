using UnityEngine;
using System.Collections;

public class CameraUtil  : MonoBehaviour {

	public static Vector3 Convert3DtoUI(Camera cam3D, Camera cam2D, Vector3 objWorldPos)
	{
		/*
		Vector3 view = GameManager.Inst.MainCamera.WorldToScreenPoint (transform.position);
		Vector3 ui = UIManager.Inst.cameraUI.ScreenToWorldPoint (view);
		obj.transform.position = ui;
		*/
	
		Vector3 screen = cam3D.WorldToScreenPoint (objWorldPos);
		Vector3 worldPosInUI = cam2D.ScreenToWorldPoint (screen);

		return worldPosInUI;
	}

}
