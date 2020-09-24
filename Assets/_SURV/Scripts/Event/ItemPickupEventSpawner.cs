using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapUtility;

public class ItemPickupEventSpawner : Spawner
{
	[SerializeField] private ItemRateUnit[] itemRateUnits = default;

	protected override void OnSpawned(GameObject newObj)
	{
		var ev = newObj.GetComponent<ItemPickupEvent>();
		var itemCountList = new List<ItemCountUnit>();
		var totalRate = 0f;
		foreach(var unit in itemRateUnits)
		{
			totalRate += unit.dropRatePercent;
		}
		var random = Random.Range(0f, totalRate);
		totalRate = 0f;
		ItemCountUnit hitUnit = null;
		foreach(var unit in itemRateUnits)
		{
			totalRate += unit.dropRatePercent;
			if(random < totalRate)
			{
				hitUnit = unit.itemCountUnit;
				break;
			}
		}

		var count = Mathf.RoundToInt(hitUnit.count * Random.Range(0.5f, 1.5f));

		// 少なくとも1個はゲット
		if(count == 0) { count = 1; }

		var newCountUnit = new ItemCountUnit();
		newCountUnit.itemID = hitUnit.itemID;
		newCountUnit.count = count;

		// NOTE: EnemyのDropとは違う抽選内容
		itemCountList.Add(hitUnit);

		ev.SetItems(itemCountList.ToArray());
	}
}
