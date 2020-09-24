using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RPGのパーティメンバーになり得るもの。一応他クライアントのプレイヤーを想定。
/// </summary>
[RequireComponent(typeof(PartyCharaStatusModule))]
public class PartyChara : Chara
{
	private EquipmentModule equipmentModule;
	public EquipmentModule EquipmentModule { get => equipmentModule; }

	public override void Awake()
	{
		base.Awake();
	}

	public override void Begin()
	{
		base.Begin();
		equipmentModule = new EquipmentModule();
		(statusModule as PartyCharaStatusModule).SetEquipmentModule(equipmentModule);
	}

	public override void Tick()
	{
		base.Tick();
	}

	public override void ReceiveDamage(AttackInfo attackInfo)
	{
		base.ReceiveDamage(attackInfo);
	}

	public string GetEquippingItemName(EquipmentPart part)
	{
		return equipmentModule.GetEquippingItemName(part);
	}

	public void ChangeMass(float diff)
	{
		rigidbdy2D.mass += diff;
	}
}

public enum EquipmentPart
{
	Weapon,
	Head,
	Body,
	Accessory1,
	Accessory2,
	MAX,
}
