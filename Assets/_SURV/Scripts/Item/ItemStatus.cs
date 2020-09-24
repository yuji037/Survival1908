using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

[Serializable]
public class Efficacy
{
    [SerializeField] private EfficacyType   type		= default;
	[SerializeField] private float			value		= default;
	[SerializeField] private float			duration	= default;
	
	public EfficacyType		Type	 { get => type; }
	public float			Value	 { get => value; }
	public float			Duration { get => duration; }
}

[Serializable]
public class ItemStatus
{
	[SerializeField] private string			name			= default;
	[SerializeField] private string			id				= default;
	[SerializeField] private ItemType		itemType		= default;
	[SerializeField] private string			iconIndex		= default;		// アイコンはitemType準拠でない場合のみここにIndexを入れる
    [SerializeField] private Efficacy[]     efficacys		= default;

	public string	Name			{ get => name;  }
	public string	ID				{ get => id; } 
	public ItemType Type			{ get => itemType; } 
	public string	IconIndex		{ get => iconIndex; } 
	public float	EffectValue1		{ get => efficacys[0].Value; }
	public IReadOnlyCollection<Efficacy> Efficacys { get => efficacys; }

	public void Use(){
        switch (itemType) {
            case ItemType.Food:
                SoundManager.Instance.Play("SE_Eat00");
				ExecuteEfficacys();
				break;
			case ItemType.Facility:
                break;
			case ItemType.Potion:
                SoundManager.Instance.Play("SE_Heal00"); 
				ExecuteEfficacys();
				break;
			default:
				Debug.LogError($"アイテム効果が設定されていません。ID:{id} Name:{name} Type:{itemType}");
				break;
        }
    }

	public void ExecuteEfficacys()
	{
		var localPlayerStatus = LocalPlayer.Instance.StatusModule as PartyCharaStatusModule;
		var localPlayerEfficacy = LocalPlayer.Instance.EfficacyModule as EfficacyModule;
		foreach (var eff in efficacys)
		{
			switch (eff.Type)
			{
				case EfficacyType.None:
					// 効果なし
					break;
				case EfficacyType.AddHP:
					localPlayerStatus.AddChangeHP(eff.Value);
					break;
				case EfficacyType.AddMP:
					localPlayerStatus.AddChangeMP(eff.Value);
					break;
				case EfficacyType.AddFood:
					localPlayerStatus.AddChangeFoodfullness(eff.Value);
					break;
				default:
					localPlayerEfficacy.AddEfficacy(eff.Type, eff.Value, 0f, eff.Duration);
					break;
			}
		}
	}

	public bool IsUsableType()
	{
		return itemType != ItemType.NoUse;
	}

	/// <summary>
	/// アイテム使用動詞を取得する（「食べる」「装備する」など）
	/// </summary>
	/// <returns></returns>
	public string GetVerbUseAction()
	{
		switch ( itemType )
		{
			case ItemType.Food:		return "食べる";
			case ItemType.Weapon:	return "装備する";
			case ItemType.Armor:	return "装備する";
			case ItemType.Facility:	return "設置する";
			case ItemType.Potion:	return "飲む";
			default:
				Debug.LogError($"アイテム使用動詞を登録されていないアイテムです。ID:{id} Name:{name} Type:{itemType}");
				return "エラー";
		}
	}

	public string GetDescription()
	{
		var str = $"{name}\n";
		switch (itemType)
		{
			case ItemType.Weapon:	str += $"攻撃力:+{EffectValue1.ToString("f0")}";	break;
			case ItemType.Armor:	str += $"防御力:+{EffectValue1.ToString("f0")}";	break;
			case ItemType.Facility: str += $"設置できます。";						break;
			case ItemType.Food:		str += GetEfficacysDescription();				break;
			case ItemType.Potion:	str += GetEfficacysDescription();				break;
		}
		return str;
	}

	private string GetEfficacysDescription()
	{
		var str = new StringBuilder();
		for(int i = 0; i < efficacys.Length; ++i)
		{
			var eff = efficacys[i];
			switch (eff.Type)
			{
				case EfficacyType.AddHP:				str.Append($"HPを{eff.Value}回復"); break;
				case EfficacyType.AddMP:				str.Append($"MPを{eff.Value}回復"); break;
				case EfficacyType.AddFood:				str.Append($"満腹度を{eff.Value}回復"); break;
				case EfficacyType.RegeneHP:				str.Append($"HPを{eff.Duration}秒間{eff.Value}ずつ回復"); break;
				case EfficacyType.RegeneMP:				str.Append($"MPを{eff.Duration}秒間{eff.Value}ずつ回復"); break;
				case EfficacyType.AttackCoefficient:	str.Append($"攻撃力を{eff.Duration}秒間{eff.Value}上昇"); break;
				case EfficacyType.DefenceCoefficient:	str.Append($"防御力を{eff.Duration}秒間{eff.Value}上昇"); break;
				case EfficacyType.SpeedCoefficient:		str.Append($"移動速度を{eff.Duration}秒間{eff.Value}上昇"); break;
			}
			if (i < efficacys.Length - 1)
			{
				// 次がある
				str.Append("、");
			}
		}
		if(efficacys.Length > 0)
		{
			str.Append("します。");
		}
		return str.ToString();
	}

	public bool IsEquipment()
	{
		switch (Type)
		{
			case ItemType.Weapon:
			case ItemType.Armor:
				return true;
			default:
				return false;
		}
	}
}

public enum ItemType
{
	Food,
	Weapon,
	NoUse,
	Facility,
	Armor,
	Potion,
	MAX,
}
