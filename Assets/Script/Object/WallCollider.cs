using UnityEngine;
using System.Collections;

public class WallCollider : MonoBehaviour {
	public enum EDir
	{
		Top = 0,
		Bottom,
		Left,
		Right,
		Total
	}

	public 	EDir dir;

	// 충돌시 정지하는 영역인가? (시작지점이 있는 바닥의 경우 check)
	public bool isStopArea;
}