using UnityEngine;
using System.Collections;

public abstract class GICryptValue<T>
{
	protected byte[] 	m_szValue 	= null;
	protected byte 		m_szKey 	= 0;
#if UNITY_EDITOR
	protected T			m_value		= default(T);
#endif

	public abstract T 		get();
	public abstract void 	set(T value);

	protected byte[] _SetXOR()
	{
		byte[] szValue = new byte[m_szValue.Length/2];
		for(int i=0; i<m_szValue.Length; ++i)
		{
			m_szValue[i] ^= m_szKey;
			if( 1 == i%2 )
				szValue[i/2] = m_szValue[i];
		}
		return szValue;
	}

	protected void _SetValue(byte[] szValue)
	{
		m_szValue = new byte[szValue.Length*2];
		for(int i=0; i<m_szValue.Length; ++i)
		{
			if( 0 == i%2 )
				m_szValue[i] = (byte)UnityEngine.Random.Range(1, 255);
			else
				m_szValue[i] = szValue[i/2];
		}
	}
}

public class GCryptValueBool : GICryptValue<bool>
{	
	public override bool get()
	{
		bool value = System.BitConverter.ToBoolean(_SetXOR(), 0);
		set(value);
		return value;
	}
	
	public override void set(bool value)
	{
#if UNITY_EDITOR
		m_value		= value;
#endif
		byte[] szValue = System.BitConverter.GetBytes(value);
		_SetValue(szValue);
		m_szKey 	= (byte)UnityEngine.Random.Range(1, 255);

		_SetXOR();
	}
}

public class GCryptValueInt16 : GICryptValue<System.Int16>
{
	public override System.Int16 get()
	{
		System.Int16 value = System.BitConverter.ToInt16(_SetXOR(), 0);
		set(value);
		return value;
	}
	
	public override void set(System.Int16 value)
	{
#if UNITY_EDITOR
		m_value		= value;
#endif
		byte[] szValue = System.BitConverter.GetBytes(value);
		_SetValue(szValue);
		m_szKey 	= (byte)UnityEngine.Random.Range(1, 255);
		
		_SetXOR();
	}
}

public class GCryptValueInt32 : GICryptValue<System.Int32>
{
	public override System.Int32 get()
	{
		System.Int32 value = System.BitConverter.ToInt32(_SetXOR(), 0);
		set(value);
		return value;
	}
	
	public override void set(System.Int32 value)
	{
#if UNITY_EDITOR
		m_value		= value;
#endif
		byte[] szValue = System.BitConverter.GetBytes(value);
		_SetValue(szValue);
		m_szKey 	= (byte)UnityEngine.Random.Range(1, 255);
		
		_SetXOR();
	}
}

public class GCryptValueInt64 : GICryptValue<System.Int64>
{
	public override System.Int64 get()
	{
		System.Int64 value = System.BitConverter.ToInt64(_SetXOR(), 0);
		set(value);
		return value;
	}
	
	public override void set(System.Int64 value)
	{
#if UNITY_EDITOR
		m_value		= value;
#endif
		byte[] szValue = System.BitConverter.GetBytes(value);
		_SetValue(szValue);
		m_szKey 	= (byte)UnityEngine.Random.Range(1, 255);
		
		_SetXOR();
	}
}

public class GCryptValueUInt16 : GICryptValue<System.UInt16>
{
	public override System.UInt16 get()
	{
		System.UInt16 value = System.BitConverter.ToUInt16(_SetXOR(), 0);
		set(value);
		return value;
	}
	
	public override void set(System.UInt16 value)
	{
#if UNITY_EDITOR
		m_value		= value;
#endif
		byte[] szValue = System.BitConverter.GetBytes(value);
		_SetValue(szValue);
		m_szKey 	= (byte)UnityEngine.Random.Range(1, 255);
		
		_SetXOR();
	}
}

public class GCryptValueUInt32 : GICryptValue<System.UInt32>
{
	public override System.UInt32 get()
	{
		System.UInt32 value = System.BitConverter.ToUInt32(_SetXOR(), 0);
		set(value);
		return value;
	}
	
	public override void set(System.UInt32 value)
	{
#if UNITY_EDITOR
		m_value		= value;
#endif
		byte[] szValue = System.BitConverter.GetBytes(value);
		_SetValue(szValue);
		m_szKey 	= (byte)UnityEngine.Random.Range(1, 255);
		
		_SetXOR();
	}
}

public class GCryptValueUInt64 : GICryptValue<System.UInt64>
{
	public override System.UInt64 get()
	{
		System.UInt64 value = System.BitConverter.ToUInt64(_SetXOR(), 0);
		set(value);
		return value;
	}
	
	public override void set(System.UInt64 value)
	{
#if UNITY_EDITOR
		m_value		= value;
#endif
		byte[] szValue = System.BitConverter.GetBytes(value);
		_SetValue(szValue);
		m_szKey 	= (byte)UnityEngine.Random.Range(1, 255);
		
		_SetXOR();
	}
}

public class GCryptValueFloat : GICryptValue<float>
{
	public override float get()
	{
		float value = System.BitConverter.ToSingle(_SetXOR(), 0);
		set(value);
		return value;
	}
	
	public override void set(float value)
	{
#if UNITY_EDITOR
		m_value		= value;
#endif
		byte[] szValue = System.BitConverter.GetBytes(value);
		_SetValue(szValue);
		m_szKey 	= (byte)UnityEngine.Random.Range(1, 255);
		
		_SetXOR();
	}
}

public class GCryptValueDouble : GICryptValue<double>
{
	public override double get()
	{
		double value = System.BitConverter.ToSingle(_SetXOR(), 0);
		set(value);
		return value;
	}
	
	public override void set(double value)
	{
#if UNITY_EDITOR
		m_value		= value;
#endif
		byte[] szValue = System.BitConverter.GetBytes(value);
		_SetValue(szValue);
		m_szKey 	= (byte)UnityEngine.Random.Range(1, 255);
		
		_SetXOR();
	}
}

public class GCryptValueString : GICryptValue<string>
{
	public override string get()
	{
		if( null == m_szValue )
			return "";

		string value = System.Text.Encoding.Default.GetString(_SetXOR());
		set(value);
		return value;
	}
	
	public override void set(string value)
	{
		if( string.IsNullOrEmpty(value) )
		{
			m_szValue 	= null;
			m_szKey 	= 0;
			return;
		}

#if UNITY_EDITOR
		m_value		= value;
#endif
		byte[] szValue 	= System.Text.Encoding.Default.GetBytes(value);
		_SetValue(szValue);
		m_szKey 	= (byte)UnityEngine.Random.Range(1, 255);
		
		_SetXOR();
	}
}