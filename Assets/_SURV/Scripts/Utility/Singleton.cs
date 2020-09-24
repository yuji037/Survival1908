using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> where T : new()
{
    private static T instance;
    public static T Instance{
        get{
            if (instance == null) {
                instance = new T();
            }
            return instance;
        }
    }

	protected Singleton()
	{
		if(instance != null )
		{
			Debug.LogError("2個目が作られました。" + typeof(T).Name);
		}
	}
}
