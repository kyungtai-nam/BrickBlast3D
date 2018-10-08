using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GFSM<T>
{
	public delegate void FuncState();

	public class State
	{
		public FuncState 	onEnter		= OnDefault;
		public FuncState 	onUpdate	= OnDefault;
		public FuncState 	onExit		= OnDefault;

		static public void OnDefault()
		{
		}
	}

	private Dictionary<T, State>	m_hashState 			= new Dictionary<T, State>();
	private T 						m_tState 				= default(T);
	private State 					m_stateCurrent 			= m_stateDefault;
	private static State			m_stateDefault 			= new State();
	private T						m_tNextState			= default(T);
	private bool 					m_IsInit				= true;
	private int 					m_nRenderedFrameCount 	= 0;

	public T state
	{
		get { return m_tState; }
		set { m_tState = value; }
	}

	public State currentState
	{
		get { return m_stateCurrent; }
	}

	~GFSM()
	{
		m_hashState.Clear();
	}

	public void Reset()
	{
		m_stateCurrent 	= m_stateDefault;
		m_IsInit		= true;
	}

	public void Update()
	{
		if( true == m_IsInit || !IComparer.Equals(m_tState, m_tNextState) )
		{
			m_stateCurrent.onExit();
			#if UNITY_EDITOR
			if( !m_hashState.ContainsKey(m_tState) )
			{
				Debug.LogError("GFSM::state change error="+m_tState.ToString());
				return;
			}
			#endif            
			m_tNextState	= m_tState;
			m_stateCurrent 	= m_hashState[m_tState];			

			m_stateCurrent.onEnter();

			m_IsInit 				= false;
			m_nRenderedFrameCount 	= Time.renderedFrameCount;
		}
		else
		{
			if( m_nRenderedFrameCount < Time.renderedFrameCount )
				m_stateCurrent.onUpdate();
		}
	}

	public void AddState(MonoBehaviour tClass, T tState)
	{
		// 상태 추가.

		AddState(tState, 
			_FindMethod(tClass, tState, "Enter"), 
			_FindMethod(tClass, tState, "Update"), 
			_FindMethod(tClass, tState, "Exit"));
	}

	public void AddState(T tState, FuncState onEnter, FuncState onUpdate, FuncState onExit)
	{
		// 상태 추가.

		#if UNITY_EDITOR
		if( m_hashState.ContainsKey(tState) )
		{
			Debug.LogError("GFSM::AddState() duplication state:"+tState.ToString());
			return;
		}
		#endif
		State state = new State();

		state.onEnter 	= (null == onEnter)?State.OnDefault:onEnter;
		state.onUpdate 	= (null == onUpdate)?State.OnDefault:onUpdate;
		state.onExit 	= (null == onExit)?State.OnDefault:onExit;

		m_hashState.Add(tState, state);
	}

	public void DeleteState(T tState)
	{
		// 상태 삭제.

		m_hashState.Remove(tState);
	}

	private FuncState _FindMethod(MonoBehaviour tClass, T tState, string strState)
	{
		// 함수 검색.

		string 							strMethod 	= "On"+tState.ToString()+strState;
		System.Reflection.MethodInfo 	mi 			= tClass.GetType().GetMethod(strMethod, 
			System.Reflection.BindingFlags.Public |
			System.Reflection.BindingFlags.NonPublic | 
			System.Reflection.BindingFlags.Instance);

		if( null != mi )
			return (FuncState)System.Delegate.CreateDelegate(typeof(FuncState), tClass, mi, false);
		else
			return null;
	}
}

