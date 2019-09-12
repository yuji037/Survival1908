using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CSingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T m_instance;
	public static T Instance {
		get {
			return m_instance;
		}
	}

	protected virtual void Awake(){
        if(m_instance == this as T)
        {
            return;
        }
		if (m_instance != null) {
			Debug.LogError("シングルトンだが２つ目が存在します。" + typeof(T).Name);
		}

        //Debug.Log(typeof(T).Name + "初期化");
		m_instance = this as T;
	}
}
