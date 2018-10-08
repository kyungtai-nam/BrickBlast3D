using UnityEngine;
using System.Collections;

public class UIVersion : MonoBehaviour {
	UILabel lb;

	void Awake()
	{
		lb = GetComponent<UILabel>();
		lb.text = string.Format("[i]v {0}[/i]", Application.version);
	}
}
