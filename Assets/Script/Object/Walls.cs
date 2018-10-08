using UnityEngine;

public class Walls : MonoBehaviour {

	public bool isShow = true;

	void Awake()
	{					
		if ( isShow && !ConfigManager.Inst.CompanyMode)
			return;	

		DisableMeshRenderer ();
	}
		
	// 벽 충돌영역에는 별도의 메쉬 렌더링을 하지 않음
	void DisableMeshRenderer()
	{
		MeshRenderer mr = GetComponent<MeshRenderer>();			
		mr.enabled = false;
	}

}