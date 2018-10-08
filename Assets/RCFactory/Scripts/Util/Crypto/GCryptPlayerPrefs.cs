using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GCryptPlayerPrefs
{
    static public string 						aes_key 	= "1$a3p9E#pAp2^ei04@!bnCF47(-@932T";
	static private System.Text.StringBuilder	m_sbConvert	= new System.Text.StringBuilder();


    static public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    static public void DeleteKey(string _key)
    {
        PlayerPrefs.DeleteKey(GCrypt.AES_encrypt(_key, GCryptPlayerPrefs.aes_key));
    }

    static public void Save()
    {
        PlayerPrefs.Save();
    }



    static public string ConvertObj2Str(Dictionary<int, GCryptValue<int>> hashList)
    {
        // 오브젝트 -> 문자로 변환.

        m_sbConvert.Length = 0;
        foreach( KeyValuePair<int, GCryptValue<int>> it in hashList )
        {
            if( 0 < m_sbConvert.Length )
                m_sbConvert.Append("@");

            m_sbConvert.Append(it.Key);
            m_sbConvert.Append("|");
            m_sbConvert.Append(it.Value.value);
        }
        return m_sbConvert.ToString();
    }

    static public void ConvertStr2Obj(Dictionary<int, GCryptValue<int>> hashList, string strData)
    {
        // 문자 -> 오브젝트로 변환.

        string[] strItem = strData.Split('@');

        hashList.Clear();
        for(int i=0; i<strItem.Length; ++i)
        {
            string[] strValue = strItem[i].Split('|');
            hashList.Add(GValueParse.IntParse(strValue[0]), new GCryptValue<int>(GValueParse.IntParse(strValue[1])));
        }
    }




    static public string GetString(string _key, string _defaultValue)
    {
        string _keyEncrypt = GCrypt.AES_encrypt(_key, GCryptPlayerPrefs.aes_key);
        if( false == PlayerPrefs.HasKey(_keyEncrypt) )
            return _defaultValue;

        string value        = PlayerPrefs.GetString(_keyEncrypt);
        string valueDecrypt = GCrypt.AES_decrypt(value, GCryptPlayerPrefs.aes_key);

        if( !string.IsNullOrEmpty(valueDecrypt) )
            return valueDecrypt;
        else
            return _defaultValue;
    }


    static public bool GetBool(string _key, bool _defaultValue)
    {       
        string _keyEncrypt = GCrypt.AES_encrypt(_key, GCryptPlayerPrefs.aes_key);
        if( false == PlayerPrefs.HasKey(_keyEncrypt) )
            return _defaultValue;

        string value        = PlayerPrefs.GetString(_keyEncrypt);
        string valueDecrypt = GCrypt.AES_decrypt(value, GCryptPlayerPrefs.aes_key);

        if( !string.IsNullOrEmpty(valueDecrypt) )
        {
            int result = 0;
            if( true == int.TryParse(valueDecrypt, out result) )
                return (1 == result)?true:false;
            else
                return _defaultValue;
        }
        else
            return _defaultValue;
    }

    static public int GetInt(string _key, int _defaultValue)
    {       
        string _keyEncrypt = GCrypt.AES_encrypt(_key, GCryptPlayerPrefs.aes_key);
        if( false == PlayerPrefs.HasKey(_keyEncrypt) )
            return _defaultValue;

        string value        = PlayerPrefs.GetString(_keyEncrypt);
        string valueDecrypt = GCrypt.AES_decrypt(value, GCryptPlayerPrefs.aes_key);

        if( !string.IsNullOrEmpty(valueDecrypt) )
        {
            int result = 0;
            if( true == int.TryParse(valueDecrypt, out result) )
                return result;
            else
                return _defaultValue;
        }
        else
            return _defaultValue;
    }

    static public float GetFloat(string _key, float _defaultValue)
    {
        string _keyEncrypt = GCrypt.AES_encrypt(_key, GCryptPlayerPrefs.aes_key);
        if( false == PlayerPrefs.HasKey(_keyEncrypt) )
            return _defaultValue;

        string value        = PlayerPrefs.GetString(_keyEncrypt);
        string valueDecrypt = GCrypt.AES_decrypt(value, GCryptPlayerPrefs.aes_key);

        if( !string.IsNullOrEmpty(valueDecrypt) )
        {
            float result = 0f;
            if( true == float.TryParse(valueDecrypt, out result) )
                return result;
            else
                return _defaultValue;
        }
        else
            return _defaultValue;
    }


	static public T[] GetArray<T>(string _key)
	{
		string value = GetString(_key, "");
		return (string.IsNullOrEmpty (value)) ? null : JsonHelper.FromJson<T> (value);
	}

	/*
    static public IDictionary GetDictionary(string _key)
    {		
        string value = GetString(_key, "");
		return (string.IsNullOrEmpty(value))?null:(IDictionary)JsonUtility.FromJson(value);
    }
    */
    
    








    static public void SetString(string _key, string _value)
    {
        PlayerPrefs.SetString(
            GCrypt.AES_encrypt(_key, GCryptPlayerPrefs.aes_key), 
            GCrypt.AES_encrypt(_value, GCryptPlayerPrefs.aes_key));
    }

    static public void SetBool(string _key, bool _value)
    {
        PlayerPrefs.SetString(GCrypt.AES_encrypt(_key, GCryptPlayerPrefs.aes_key), GCrypt.AES_encrypt(((true == _value)?1:0).ToString(), GCryptPlayerPrefs.aes_key));
    }
		
    static public void SetInt(string _key, int _value)
    {
        PlayerPrefs.SetString(GCrypt.AES_encrypt(_key, GCryptPlayerPrefs.aes_key), GCrypt.AES_encrypt(_value.ToString(), GCryptPlayerPrefs.aes_key));
    }

    static public void SetFloat(string _key, float _value)
    {
        PlayerPrefs.SetString(GCrypt.AES_encrypt(_key, GCryptPlayerPrefs.aes_key), GCrypt.AES_encrypt(_value.ToString(), GCryptPlayerPrefs.aes_key));
    }

	static public void SetArray<T>(string _key, T[] _value)
	{
		string data = JsonHelper.ToJson<T>(_value);
		SetString(_key, (null == _value)?"":data);
	}

	/*
    static public void SetList(string _key, IList _value)
    {
		string data = JsonUtility.ToJson (_value);
		SetString(_key, (null == _value)?"":data);
    }
	*/
	/*
    static public void SetDictionary(string _key, IDictionary _value)
    {
		string data = JsonUtility.ToJson (_value);
		SetString(_key, (null == _value)?"":data);
    }
    */


}

