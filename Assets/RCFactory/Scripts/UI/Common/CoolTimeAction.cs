using UnityEngine;
using System.Collections;

// 쿨타임 후 반복 호출
public class CoolTimeAction
{
	// 쿨타임 
	float coolTime = 1.0f;
	
	// 쿨타임 남은 시간
	float time = 0.0f;

	// 고유번호 
	int seq = 0;

	// 쿨 타임 지나면 발생하는 이벤트
	System.Action<int> onAction = null;

	bool repeat = true;
	bool called = false;

	public float CoolTime
	{
		get
		{
			return coolTime;
		}
		set
		{
			coolTime = value;
		}
	}

	// 초기화
	public void Init(int _seq, System.Action<int> _onAction, float _coolTime, bool repeat=false, bool called=true)
	{
		seq = _seq;
		onAction = _onAction;
		coolTime = _coolTime;

		time = 0.0f;
		this.repeat = repeat;
		this.called = called;
	}
	
	// 시간 초기화  
	public void Reset()
	{
		time = coolTime;
		called = false;
	}

	// 시간 체크해서 쿨타임 지났으면 실제 함수 호출
	public void OnAction()
	{
		// Debug.Log("OnAction=" + id + " time=" + time);

		// 한번만 호출 해야 한다
		if ( !repeat && called )
			return;

		time -= Time.deltaTime;

		if ( time >= 0f )
			return;

		// Debug.Log("OnAction=" + id);

		time = coolTime;

		if ( null == onAction )
			return;

		onAction(seq);
		called = true;
	}
}
