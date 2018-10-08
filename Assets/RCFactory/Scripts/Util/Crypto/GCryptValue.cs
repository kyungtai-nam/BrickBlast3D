using UnityEngine;
using System.Collections;

public class GCryptValue<T>
{
	private GICryptValue<T>	m_ICryptValue = null;

	public GCryptValue()
	{
		_Init();
		value = default(T);
	}

	public GCryptValue(T _value)
	{
		_Init();
		value = _value;
	}

	public T value
	{
		get { return m_ICryptValue.get(); }
		set	{ m_ICryptValue.set(value);	}
	}

	private void _Init()
	{
		if( typeof(T) == typeof(bool) )
			m_ICryptValue = new GCryptValueBool() as GICryptValue<T>;
		else if( typeof(T) == typeof(System.Int16) )
			m_ICryptValue = new GCryptValueInt16() as GICryptValue<T>;
		else if( typeof(T) == typeof(System.Int32) )
			m_ICryptValue = new GCryptValueInt32() as GICryptValue<T>;
		else if( typeof(T) == typeof(System.Int64) )
			m_ICryptValue = new GCryptValueInt64() as GICryptValue<T>;
		else if( typeof(T) == typeof(System.UInt16) )
			m_ICryptValue = new GCryptValueUInt16() as GICryptValue<T>;
		else if( typeof(T) == typeof(System.UInt32) )
			m_ICryptValue = new GCryptValueUInt32() as GICryptValue<T>;
		else if( typeof(T) == typeof(System.UInt64) )
			m_ICryptValue = new GCryptValueUInt64() as GICryptValue<T>;
		else if( typeof(T) == typeof(float) )
			m_ICryptValue = new GCryptValueFloat() as GICryptValue<T>;
		else if( typeof(T) == typeof(double) )
			m_ICryptValue = new GCryptValueDouble() as GICryptValue<T>;
		else if( typeof(T) == typeof(string) )
			m_ICryptValue = new GCryptValueString() as GICryptValue<T>;
		else
			Debug.LogError("GCryptValue<T> error="+typeof(T).ToString());
	}
}

