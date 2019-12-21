using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CSingleton<T> where T : new()
{
    private static T m_instance;
    public static T Instance{
        get{
            if (m_instance == null) {
                m_instance = new T();
            }
            return m_instance;
        }
    }

	protected CSingleton()
	{
		if(m_instance != null )
		{
			Debug.LogError("2個目が作られました。" + typeof(T).Name);
		}
	}
}
