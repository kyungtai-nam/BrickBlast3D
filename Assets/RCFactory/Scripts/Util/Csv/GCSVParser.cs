using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
// Ex>

	public class CSVTest : GCSVParser
	{
		public CSVTest()
		{
			SetCallback(OnCallback);
		}

		public void OnCallback(Dictionary<string, string> hashValue)
		{
			int n1 = this.GetItem_Int(hashValue, "n1");
			int n2 = this.GetItem_Int(hashValue, "n2");
		}
	}

	...
	...

	CSVTest csv = new CSVTest();
	csv.LoadString(strText);
*/

public class GCSVParser
{
	public delegate void FuncNode(Dictionary<string, string> hashValue);

	private string[] 							m_strFields		= null;
	private List<Dictionary<string, string>>	m_listData 		= null;
	private FuncNode 							m_OnCallback	= null;
	private string 								m_strFile		= "";
	private bool 								m_IsLoaded		= false;

	public string file
	{
		get { return m_strFile; }
		set { m_strFile = value; }
	}

	public bool isLoaded
	{
		get { return m_IsLoaded; }
	}

	public GCSVParser()
	{
		m_listData = new List<Dictionary<string, string>>();
	}

	public void SetCallback(FuncNode funcCallback)
	{
		// 함수 설정.
		
		m_OnCallback = funcCallback;
	}

	public virtual void Release()
	{
		m_IsLoaded = false;
	}

	public void LoadString(string strCSV)
	{
		Release();

		string[] strLines = strCSV.Split('\n');
		if( 0 < strLines.Length )
		{
			m_strFields = strLines[0].Split(new char[]{',','\t'});
			for(int i=1; i<strLines.Length; ++i)
			{
				strLines[i] = strLines[i].Trim();
				if( string.IsNullOrEmpty(strLines[i]) )
				   continue;

				Dictionary<string, string> 	hashValue 	= new Dictionary<string, string>();
				string[] 					strValue 	= strLines[i].Split(new char[]{',','\t'});
				int 						nSelValue	= 0;

				for(int j=0; j<m_strFields.Length; ++j)
				{
					if( nSelValue < strValue.Length )
					{
						string strKey = m_strFields[j].Trim();
						string strVal = strValue[nSelValue].Trim();

						if( 0 < strVal.Length && '"' == strVal[0] )
						{
							strVal = "";
							for(int k=nSelValue; k<strValue.Length; ++k)
							{
								strVal += strValue[k].Trim();
								++nSelValue;

								if( 0 < strVal.Length && '"' == strVal[strVal.Length-1] )
									break;

								strVal += ",";
							}
						}
						else
							++nSelValue;
#if UNITY_EDITOR
						if( true == hashValue.ContainsKey(strKey) )
							Debug.LogError("[GCSVParser::LoadString] hashValue duplication key : "+strKey);
#endif
						hashValue.Add(strKey, strVal);
					}
					else
					{
#if UNITY_EDITOR
						Debug.LogError("[GCSVParser::LoadString] j > strValue.Length-1");
#endif
					}
				}

				if( null == m_OnCallback )
					m_listData.Add(hashValue);
				else
				{
					m_OnCallback(hashValue);
					hashValue.Clear();
				}
			}
		}
		m_IsLoaded = true;
	}

	private static void _OnDefaultNode(Dictionary<string, string> hashValue)
	{
		// 기본 함수.
	}

#region Get Value	
	protected int GetItem_Int(Dictionary<string, string> hashValue, string strItem)
	{
		string strValue;
		if( true == hashValue.TryGetValue(strItem, out strValue) )
			return GValueParse.IntParse(strValue);
		return 0;
	}
	
	protected float GetItem_Float(Dictionary<string, string> hashValue, string strItem)
	{
		string strValue;
		if( true == hashValue.TryGetValue(strItem, out strValue) )
			return GValueParse.FloatParse(strValue);
		return 0f;
	}
	
	protected System.Int16 GetItem_Int16(Dictionary<string, string> hashValue, string strItem)
	{
		string strValue;
		if( true == hashValue.TryGetValue(strItem, out strValue) )
			return GValueParse.Int16Parse(strValue);
		return 0;
	}
	
	protected System.Int32 GetItem_Int32(Dictionary<string, string> hashValue, string strItem)
	{
		string strValue;
		if( true == hashValue.TryGetValue(strItem, out strValue) )
			return GValueParse.IntParse(strValue);
		return 0;
	}
	
	protected System.UInt16 GetItem_UInt16(Dictionary<string, string> hashValue, string strItem)
	{
		string strValue;
		if( true == hashValue.TryGetValue(strItem, out strValue) )
			return GValueParse.UInt16Parse(strValue);
		return 0;
	}
	
	protected System.UInt32 GetItem_UInt32(Dictionary<string, string> hashValue, string strItem)
	{
		string strValue;
		if( true == hashValue.TryGetValue(strItem, out strValue) )
			return GValueParse.UInt32Parse(strValue);
		return 0;
	}
	
	protected string GetItem_Str(Dictionary<string, string> hashValue, string strItem)
	{
		string strValue;
		if( true == hashValue.TryGetValue(strItem, out strValue) )
			return strValue;
		return "";
	}
#endregion
}

