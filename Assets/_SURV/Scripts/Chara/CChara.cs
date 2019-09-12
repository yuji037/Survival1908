using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CChara
{
    public string Name;
    public float MaxHp;
    public float Hp;
    public float AtkNaked;
    public float DefNaked;
    public int ExpHaving;

    public CChara(string _Name)
    {
        Name = _Name;
    }

    public virtual float GetAtk(){
        return AtkNaked;
    }
    public virtual float GetDef(){
        return DefNaked;
    }

	public virtual void OnDead(){
        var fFood = 0f;
        switch (Name) {
            case "唸る野犬":
                fFood = 10f;
                break;
            case "狼":
                fFood = 30f;
                break;
            case "ゴリラ":
                fFood = 50f;
                break;
        }
        CPartyStatus.Instance.GetPartyChara(0).GainFood(fFood);
        CPartyStatus.Instance.GetPartyChara(0).GainExp(ExpHaving);
	}
}
