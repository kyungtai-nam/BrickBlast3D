using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GCSVManager
{
	private struct S_Load
	{
		public GCSVParser	csv;
		public string 		strFile;
	}

	private List<S_Load> 	m_listLoad = null;
	private int 			m_nSelLoad = 0;

	public GCSVManager()
	{
		m_listLoad = new List<S_Load>(); 
	}

	public void Add(GCSVParser csv, string strFile)
	{
		// 추가.

		S_Load stData = new S_Load();

		stData.csv	 	= csv;
		stData.strFile 	= strFile;

		m_listLoad.Add(stData);
	}

	public bool Load()
	{
		// 불러오기.

		while(m_listLoad.Count > m_nSelLoad)
		{
			if( false == m_listLoad[m_nSelLoad].csv.isLoaded )
			{
				_Load(m_listLoad[m_nSelLoad++]);
				return true;
			}
			else
				++m_nSelLoad;
		}
		m_nSelLoad = 0;
		return false;
	}

	public bool Load(GCSVParser csv)
	{
		// 불러오기.

		if( false == csv.isLoaded )
		{
			for(int i=m_listLoad.Count; i>0;)
			{
				S_Load stData = m_listLoad[--i];
				if( csv == stData.csv )
				{
					_Load(stData);
					return true;
				}
			}
		}
		return false;
	}

	private void _Load(S_Load stData)
	{
		// 불러오기.

		stData.csv.file = stData.strFile;

		TextAsset ta = Resources.Load(stData.strFile, typeof(TextAsset)) as TextAsset;
		if( null != ta )
			stData.csv.LoadString(ta.text);
		else
			stData.csv.Release();

		Resources.UnloadAsset(ta);
		ta = null;
	}
}

