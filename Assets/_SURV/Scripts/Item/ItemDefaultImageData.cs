using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "SURV/ItemDefaultImageData", fileName = "ItemDefaultImageData")]
public class ItemDefaultImageData : ScriptableObject
{
	[SerializeField] private Sprite[]			itemImages;

	private static ItemDefaultImageData instance;
	
	public static ItemDefaultImageData Instance
	{
		get
		{
			if(instance == null )
			{
				instance = Resources.Load<ItemDefaultImageData>("ItemDefaultImageData");
			}
			return instance;
		}
	}

	public Sprite GetSprite(ItemType itemType)
	{
		int index = (int)itemType;
		if ( index > itemImages.Length - 1)
		{
			Debug.LogError($"スプライト取得エラー {itemType.ToString()}");
			return null;
		}
		return itemImages[index];
	}
}
