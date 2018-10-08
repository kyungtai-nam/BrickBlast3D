using UnityEngine;

public class UIBallCount : MonoBehaviour {
	TextMesh textMesh = null;

	int cacheCount = 0;

	void Awake()
	{
		textMesh = GetComponent<TextMesh> ();
	}

	void FixedUpdate()
	{
		if (cacheCount == PlayManager.Inst.ballCount)
			return;
		
		cacheCount = PlayManager.Inst.ballCount;
		textMesh.text = cacheCount.ToString ();
	}
}