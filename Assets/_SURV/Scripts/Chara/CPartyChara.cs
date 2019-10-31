using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPartyChara : CChara
{
    public      string      CurrentWeaponId;
    public      float       AtkWeapon;
    public      float       DefArmor;

    public CPartyChara(string _Name) : base(_Name)
    {

    }

    public float Food;

    public int Exp;
    public int Level;

    public override float GetAtk()
    {
        return AtkNaked + AtkWeapon;
    }
    public override float GetDef()
    {
        return DefNaked + DefArmor;
    }

    public void GainExp(int iExp){
        Exp += iExp;
        var iPreLevel = Level;
        int iLeftExp;
        CEXPTable.Instance.GetLevel(Exp, out iLeftExp, out Level);
        if (iPreLevel != Level) {
            // レベルアップ
            MaxHp = 50 + (Level - 1) * 2;
            AtkNaked = 10 + (Level - 1) * 1;
            DefNaked = 0 + (Level - 1) * 1;
        }
    }

    public void GainFood(float fFood){
        Food += fFood;
        if (Food > 100)
            Food = 100;
    }

    public void EquipWeapon(string sWeaponId){
        CurrentWeaponId = sWeaponId;
        var itemStatus = CItemDataMan.Instance.GetItemStatusById(sWeaponId);
        AtkWeapon = itemStatus.EffectValue1;
    }
}
