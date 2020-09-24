using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInstantMessage : SingletonMonoBehaviour<UIInstantMessage>
{
	[SerializeField] private GameObject messageTemp = default;
	[SerializeField] private int countMax = 5;

	private List<MessageUnit> units = new List<MessageUnit>();
	private LinkedList<string> reserveMessages = new LinkedList<string>();

	private void Start()
	{
		messageTemp.SetActive(false);
	}

	private void Update()
	{
		foreach(var unit in units)
		{
			unit.Tick();
		}
		CheckReserveMessage();
	}

	public void RequestMessage(string message)
	{
		var unit = FindUsableUnit();
		if(unit == null)
		{
			reserveMessages.AddLast(message);
			return;
		}
		unit.Activate(message);
	}

	private void CheckReserveMessage()
	{
		if(reserveMessages.Count <= 0) { return; }
		var unit = FindUsableUnit();
		if(unit == null) { return; }
		var message = reserveMessages.First.Value;
		reserveMessages.RemoveFirst();
		unit.Activate(message);
	}

	private MessageUnit FindUsableUnit()
	{
		foreach(var unit in units)
		{
			if(unit.dispRemainTime <= 0f)
			{
				return unit;
			}
		}
		if (units.Count < countMax)
		{
			var newObj = Instantiate(messageTemp, transform);
			var newUnit = new MessageUnit(newObj);
			units.Add(newUnit);
			return newUnit;
		}
		return null;
	}

	public class MessageUnit
	{
		private static float FADE_OUT_TIME = 1f;

		public float dispRemainTime;
		private GameObject gameObject;
		private Text text;
		private Graphic[] graphics;

		public MessageUnit(GameObject obj)
		{
			gameObject = obj;
			text = obj.GetComponentInChildren<Text>();
			graphics = obj.GetComponentsInChildren<Graphic>();
		}

		public void Tick()
		{
			if (!gameObject.activeSelf) { return; }

			dispRemainTime -= Time.unscaledDeltaTime;

			if (dispRemainTime <= 0f)
			{
				dispRemainTime = 0f;
				gameObject.SetActive(false);
			}
			else if (dispRemainTime < FADE_OUT_TIME)
			{
				SetColorAlpha(dispRemainTime / FADE_OUT_TIME);
			}
		}

		public void Activate(string message)
		{
			text.text = message;
			gameObject.SetActive(true);
			gameObject.transform.SetSiblingIndex(0);
			SetColorAlpha(1f);
			dispRemainTime = 5f;
		}

		private void SetColorAlpha(float alpha)
		{
			foreach(var g in graphics)
			{
				var c = g.color;
				c.a = alpha;
				g.color = c;
			}
		}
		
	}
}
