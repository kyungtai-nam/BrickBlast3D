using UnityEngine;
using System.Collections;

// 애니메이션에서 완료 이벤트 처리용 
public class RcAniEvent : MonoBehaviour 
{
	public System.Action cbAniStart = null;
	public System.Action cbAniEnd = null;
	
	// 애니메이션 시작시 연결해놨다면 호출 
	public void OnAniStart()
	{
		if ( null != cbAniStart )
			cbAniStart();
	}

	// 애니메이션 종료시 연결해놨다면 호출
	public void OnAniEnd()
	{
		if ( null != cbAniEnd )
			cbAniEnd();
	}
}