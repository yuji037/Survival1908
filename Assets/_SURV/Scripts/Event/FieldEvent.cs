using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEvent : MonoBehaviour
{
	[SerializeField] private string		eventName		= default;
	[SerializeField] private bool		destroyOnEnd	= true;

	protected FieldEventUnit eventUnit;

	public EventBeginTrigger BeginTrigger{
		get	{
			return eventUnit.BeginTrigger;
		}
	}

	public bool ReadyToBegin { get { return eventUnit.ReadyToBegin; } }
	public bool IsInProgress { get { return eventUnit.IsInProgress; } }

	private void Start()
	{
		// FieldEventManagerは事前に生成済みのはずなので
		Initialize();
	}

	protected virtual void Initialize()
	{
		eventUnit = FieldEventManager.Instance.GetEventUnit(eventName);
	}

	public void Begin()
	{
		Next();
	}

	/// <summary>
	/// イベント進行
	/// </summary>
	public virtual async void Next()
	{
		bool success = await eventUnit.Next();
		if (false == success)
		{
			OnEnd();
		}
	}

	public async void Skip()
	{
		await eventUnit.Skip();
		OnEnd();
	}

	private void OnEnd()
	{
		UIIngame.Instance.SwitchUI(0);
		IngameCoordinator.Instance.SetBattleActive(true);
		if (destroyOnEnd)
		{
			FieldEventManager.Instance.UnregisterEventCanProceed(this);
			Dispose();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var localPlayer = collision.gameObject.GetComponent<LocalPlayer>();
		if ( localPlayer == null ) { return; }

		FieldEventManager.Instance.RegisterEventCanProceed(this);

		if ( BeginTrigger == EventBeginTrigger.EnterArea
		&&   ReadyToBegin )
		{
			// イベント開始
			FieldEventManager.Instance.EventCanProceed.Begin();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		var localPlayer = collision.gameObject.GetComponent<LocalPlayer>();
		if ( localPlayer == null ) { return; }

		if ( BeginTrigger == EventBeginTrigger.DecideButton)
		{
			FieldEventManager.Instance.UnregisterEventCanProceed(this);
		}
	}

	protected virtual void Dispose()
	{
		Destroy(gameObject);
	}
}
