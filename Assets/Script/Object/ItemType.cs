using UnityEngine;

public class ItemType {

	public enum EType {
		BLOCK = 0, // 일반 블럭
		
		BALL, // 공 추가

		TRI, /*
		TRI_LB, // 삼각형 왼쪽 밑 
		TRI_LU, // 삼각형 왼쪽 위 
		TRI_RB, // 삼각형 오른쪽 밑
		TRI_RU, // 삼각형 오른쪽 위
		*/

		ROTATION_TRI, // 회전형 삼각형

		TURRET, 
		/*
		TURRET_VERT, // 수직 미사일 발사 터렛
		TURRET_HORI, // 수평 미사일 발사 터렛
		TURRET_4WAY, // 4방 미사일 발사 터렛
		*/

		ERASER, // axis damage!!

		TOTAL
	}
}
