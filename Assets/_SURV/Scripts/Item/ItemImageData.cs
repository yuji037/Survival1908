using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "SURV/ItemImageData", fileName = "ItemImageData" )]
public class ItemImageData : ScriptableObject
{
	[SerializeField] private Sprite[]			itemImages;

	private static ItemImageData instance;
	
	public static ItemImageData Instance
	{
		get
		{
			if(instance == null )
			{
				instance = Resources.Load<ItemImageData>("ItemImageData");
			}
			return instance;
		}
	}

	private Sprite GetSprite(ItemType itemType)
	{
		int index = (int)itemType;
		if ( index > itemImages.Length - 1)
		{
			Debug.LogError($"スプライト取得エラー {itemType.ToString()}");
			return null;
		}
		return itemImages[index];
	}

	public Sprite GetSprite(string itemID)
	{
		var itemStatus = ItemDataMan.Instance.GetItemStatusById(itemID);
		return GetSprite(itemStatus);
	}

	public Sprite GetSprite(ItemStatus itemStatus)
	{
		if ( int.TryParse(itemStatus.IconIndex, out var index) )
		{
			return itemImages[index];
		}
		return GetSprite(itemStatus.Type);
	}
}
