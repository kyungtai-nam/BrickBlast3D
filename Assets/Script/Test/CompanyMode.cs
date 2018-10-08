using UnityEngine;
using System.Collections;

public class CompanyMode : MonoBehaviour {
	void Awake()
	{
		gameObject.SetActive (!ConfigManager.Inst.CompanyMode);
	}
}
