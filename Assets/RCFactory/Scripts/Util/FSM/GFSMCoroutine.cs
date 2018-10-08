using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GFSMCoroutine<T>
{
	public delegate IEnumerator FuncState(T state);

	private Dictionary<T, FuncState>	m_hashState 		= new Dictionary<T, FuncState>();
	private T 							m_tState 			= default(T);
	private MonoBehaviour				m_monoBehaviour		= null;
	private bool						m_IsCoroutine		= false;

	public T state
	{
		get { return m_tState; }
		set
		{
			if( false == m_IsCoroutine || !IComparer.Equals(m_tState, value) )
			{
				m_tState = value;
				if( 0 < m_hashState.Count )
					_NextState();
			}
		}
	}

	public GFSMCoroutine(MonoBehaviour monoBehaviour)
	{
		m_monoBehaviour = monoBehaviour;
	}

	~GFSMCoroutine()
	{
		m_hashState.Clear();
	}

	public void Reset()
	{
		// 초기화.

		m_monoBehaviour.StopAllCoroutines();
		m_tState 		= default(T);
		m_IsCoroutine 	= false;
	}

	public void AddState(T tState, FuncState onState)
	{
		// 상태 추가.
		#if UNITY_EDITOR
		if( m_hashState.ContainsKey(tState) )
		{
			Debug.LogError("GFSMCoroutine::AddState() duplication state:"+tState.ToString());
			return;
		}
		#endif				
		m_hashState.Add(tState, onState);
	}

	public void DeleteState(T tState)
	{
		// 상태 삭제.

		m_hashState.Remove(tState);
	}

	public bool IsEqualState(T tState)
	{
		return IComparer.Equals(m_tState, tState);
	}

	private void _NextState()
	{
		#if UNITY_EDITOR
		if( !m_hashState.ContainsKey(m_tState) )
		{
			Debug.LogError("GFSMCoroutine::state change error="+m_tState.ToString());
			return;
		}
		#endif
		m_IsCoroutine = true;
		m_monoBehaviour.StartCoroutine(m_hashState[m_tState](m_tState));
	}
}


