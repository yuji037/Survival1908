using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupEvent : FieldEvent, ISpawnable
{
	private ItemCountUnit[] items;
	private Spawner spawner;

	public void SetItems(ItemCountUnit[] items)
	{
		this.items = items;
	}

	protected override void Initialize()
	{
		var lines = new List<string[]>();
		var itemGetEventWord = string.Empty;
		for (int i = 0; i < items.Length; ++i)
		{
			var item = items[i];
			itemGetEventWord += $"{item.itemID}:{item.count}";
			if(i < items.Length - 1)
			{
				// 次がある
				itemGetEventWord += "#";
			}
		}
		lines.Add(new string[] { "ItemGetEvent", "1", "3", itemGetEventWord });
		lines.Add(new string[] { "", "", "1", GetItemDialogText(items) });
		eventUnit = new FieldEventUnit(lines);
	}

	private string GetItemDialogText(ItemCountUnit[] items)
	{
		var str = string.Empty;
		for (int i = 0; i < items.Length; ++i)
		{
			var item = items[i];
			var itemName = ItemDataMan.Instance.GetItemStatusById(item.itemID).Name;
			str += $"{itemName}を{item.count}個";
			if (i < items.Length - 1)
			{
				// 次がある
				str += "、";
			}
		}
		str += "手に入れた。";
		return str;
	}

	public void SetSpawner(Spawner spawner)
	{
		this.spawner = spawner;
	}

	protected override void Dispose()
	{
		spawner.UnregisterDeadObject(this.gameObject);
		base.Dispose();
	}
}
