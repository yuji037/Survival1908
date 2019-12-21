using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CChara
{
    public string name;
	public string id;
	public float maxHp;
    public float hp;
	public float atkNaked;
	public float defNaked;
    public int expHaving;

    public CChara(string _Name)
    {
        name = _Name;
    }

    public virtual float GetAtk(){
        return atkNaked;
    }
    public virtual float GetDef(){
        return defNaked;
    }

    public virtual void HealHP(float fValue)
    {
        hp += fValue;
        if (hp > maxHp) hp = maxHp;
    }

	public virtual void OnDead(){
        //CPartyStatus.Instance.GetPartyChara(0).GainExp(expHaving);
	}
}