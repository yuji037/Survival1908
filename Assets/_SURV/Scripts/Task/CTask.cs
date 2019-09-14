using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTask {

	public		int 	CalledCount;
    public      bool    IsEnd;
    protected   float   m_fStartTime;

	public void OnInitialize(){
		m_fStartTime = Time.time;
	}

    public virtual void OnStart()
    {
		
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnEnd()
    {

    }

	protected Coroutine StartCorutine(IEnumerator coroutine){
		return CGameCoordinator.Instance.StartCoroutine(coroutine);
	}
}
