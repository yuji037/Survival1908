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

    public static void Init(){
        if (m_instance != null)
            return;

        m_instance = new T();
    }

    public static void Reflesh()
    {
        m_instance = new T();
    }
}
