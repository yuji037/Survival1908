using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTaskAttack : CTask
{

    CChara m_cAttacker;
    CChara m_cTarget;
    string m_sAttackMessage;

	string m_sAttackSeId;


	public CTaskAttack(CChara cAttacker, CChara cTarget, string sAttackMessage, string sAttackSeId)
    {
        m_cAttacker = cAttacker;
        m_cTarget = cTarget;
        m_sAttackMessage = sAttackMessage;
		m_sAttackSeId = sAttackSeId;
    }

    public override void OnStart()
    {
		base.OnStart();

		if (m_cAttacker.hp <= 0) {
			// 既に死亡
			IsEnd = true;
			return;
		}
		StartCorutine(TaskCoroutine());
    }

	IEnumerator TaskCoroutine(){

		CMessageWindowMan.Instance.ClearText();

        // 攻撃手段メッセージ
		CMessageWindowMan.Instance.AddText(m_cAttacker.name + "は" + m_cTarget.name + m_sAttackMessage + "！");
		yield return new WaitForSeconds(1f);

        // ダメージ計算
        var fRanDamageRate = Random.Range(0f, 1f);
        var fDamage = m_cAttacker.GetAtk() * fRanDamageRate * 3.0f;
        fDamage -= m_cTarget.GetDef();
        if (fDamage < 1f)
            fDamage = 1f;
		fDamage = Mathf.Ceil(fDamage);
		var fRanHit = Random.Range(0f, 1f);
        if (fRanHit < 0.9f)
		{
            Debug.Log(
                "粗ダメ : " + (m_cAttacker.GetAtk() * fRanDamageRate * 3.0f).ToString("f0") +
                "\n防御軽減 : " + (m_cTarget.GetDef() * -1).ToString("f0") +
                "\nダメージ : " + fDamage.ToString("f0"));
			CMessageWindowMan.Instance.AddText(m_cTarget.name + "に" + Mathf.CeilToInt(fDamage) + "のダメージ！");
			m_cTarget.hp -= fDamage;
			if (m_cTarget.hp < 0)
				m_cTarget.hp = 0;
			CPartyStatus.Instance.UpdatePartyText();
			CSituationStatus.Instance.UpdateSituationText();
			CSoundMan.Instance.Play(m_sAttackSeId);

			yield return new WaitForSeconds(1f);

			// 死亡チェック
			yield return StartCorutine(CheckDeathCoroutine());

		}
		else
		{
			CMessageWindowMan.Instance.AddText("攻撃は外れてしまった！");
			CSoundMan.Instance.Play("SE_Miss00");
			yield return new WaitForSeconds(1f);
		}
		IsEnd = true;
	}

	IEnumerator CheckDeathCoroutine(){
		if (m_cTarget.hp <= 0)
		{
			CMessageWindowMan.Instance.AddText(m_cTarget.name + "を倒した！");
			yield return new WaitForSeconds(1f);
            var player = CPartyStatus.Instance.GetPartyChara(0);
            var preLevel = player.Level;
			m_cTarget.OnDead();

            // ドロップアイテム
            var dropItemList = CCharaItemDropDataMan.Instance.GetDropItemsByCharaName(m_cTarget.name);
            if (dropItemList.Count != 0) {

                var dropItemText = "";
                for(int i = 0; i < dropItemList.Count; ++i)
                {
                    var itemUnit = dropItemList[i];
                    dropItemText += itemUnit.itemName + " " + itemUnit.count + "個";
                    if (i < dropItemList.Count - 1)
                        dropItemText += "、";

                    CInventryMan.Instance.ManipulateItemCount(itemUnit.itemID, itemUnit.count);
                }
                CMessageWindowMan.Instance.AddText(dropItemText + "を手に入れた！");
                yield return new WaitForSeconds(1f);
            }
            if (preLevel != player.Level) {
                CMessageWindowMan.Instance.AddText(player.name + "は成長を感じた！");
                yield return new WaitForSeconds(1f);
//                MessageWindowMan.Instance.AddText(player.Name + "はレベル"+player.Level + "になった！");
            }
			//m_cAttacker.Food += 30f;
			CSoundMan.Instance.PlayBGM("BGM_Field00");
		}
	}

    //public override void OnEnd()
    //{
    //    base.OnEnd();
    //    CMessageWindowMan.Instance.AddText("終了");
    //}

}
