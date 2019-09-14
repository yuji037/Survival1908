using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTaskMoveArea : CTask
{
	private Vector2 m_vMoveDiff;

	public CTaskMoveArea(Vector2 diff){
		m_vMoveDiff = diff;
	}

	public override void OnStart()
	{
		StartCorutine(TaskCoroutine());
	}

	IEnumerator TaskCoroutine(){

        // 移動可能かチェック
        var vPrePos = CPartyStatus.Instance.m_vPartyPos;
        var vNextPos = vPrePos + m_vMoveDiff;

        var canMove = CMapMan.Instance.CanMoveToPosition(vNextPos);
        if(false == canMove)
        {
            IsEnd = true;
            yield break;
        }

		CMessageWindowMan.Instance.ClearText();
		CMessageWindowMan.Instance.AddText("移動した...");
        CSoundMan.Instance.Play("SE_FootSand00");

        // 位置移動処理
        for (float t = 0; t < 1f; t += Time.deltaTime) {
            var fRate = t;
            CMapMan.Instance.SetDispPartyPos(
                vPrePos  * (1f - fRate) +
                vNextPos * fRate);
            yield return null;
        }
        CMapMan.Instance.SetDispPartyPos(vNextPos);
        CPartyStatus.Instance.SetPartyPos(vNextPos);

        // 敵からの逃避判定
        yield return StartCorutine(CheckEscapeFromEnemy());

        // 敵とのエンカウント判定
        var ivPos = vNextPos.GetIntVector2();
        var npc = CNpcEncountDataMan.Instance.GetEncountNpc(
            CMapMan.Instance.GetEncountType(ivPos.x, ivPos.y));

        if(npc != null)
        {
            CSituationStatus.Instance.RegisterChara(npc);
            CSoundMan.Instance.PlayBGM("BGM_NormalBattle00");
        }

		IsEnd = true;
	}

    IEnumerator CheckEscapeFromEnemy(){
        
        // 敵に遭遇していたら振り切れるか判定
        var lsEnemyDisappear = new List<CChara>();
        foreach (var enemy in CSituationStatus.Instance.m_lsEnemy) {

            var random = Random.Range(0, 100);
            if (random < 30) {
                lsEnemyDisappear.Add(enemy);
            }
        }
        if (lsEnemyDisappear.Count > 0) {

            var sDispText = "";
            if(lsEnemyDisappear.Count < CSituationStatus.Instance.m_lsEnemy.Count){
                // 全員は振り切れなかった
                sDispText += "いくらか";
            }
            sDispText += "敵を振り切った！";
            CMessageWindowMan.Instance.AddText(sDispText);

            foreach(var enemy in lsEnemyDisappear){
                CSituationStatus.Instance.UnregisterChara(enemy);
            }
            CSituationStatus.Instance.UpdateSituationText();

            yield return new WaitForSeconds(1f);

            if(CSituationStatus.Instance.m_lsEnemy.Count == 0){
                // 敵がいなくなった
                CSoundMan.Instance.PlayBGM("BGM_Field00");
            }
        }
    }
}
