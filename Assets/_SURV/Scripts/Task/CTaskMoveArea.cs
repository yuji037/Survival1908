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
		CMessageWindowMan.Instance.ClearText();

		CMessageWindowMan.Instance.AddText("移動した...");
        CSoundMan.Instance.Play("SE_FootSand00");

        // 位置移動処理
        var vPrePos = CPartyStatus.Instance.m_vPartyPos;
        var vNextPos = vPrePos + m_vMoveDiff;
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

        if (vNextPos.x >= 7) {
            var random = Random.Range(0, 100);
            if (random < 30) {
                // 敵にエンカウント
                var enemy = new CChara("唸る野犬");
                enemy.Hp = 15;
                enemy.AtkNaked = 4;
                enemy.ExpHaving = 4;
                CSituationStatus.Instance.RegisterChara(enemy);
                CSoundMan.Instance.PlayBGM("BGM_NormalBattle00");
            }
            else if (random < 40) {
                var enemy = new CChara("狼");
                enemy.Hp = 30;
                enemy.AtkNaked = 10;
                enemy.ExpHaving = 10;
                CSituationStatus.Instance.RegisterChara(enemy);
                CSoundMan.Instance.PlayBGM("BGM_NormalBattle00");
            }
        }
        else if (vNextPos.y >= 7) {
            var random = Random.Range(0, 100);
            if (random < 30) {
                // 敵にエンカウント
                var enemy = new CChara("ゴリラ");
                enemy.Hp = 100;
                enemy.AtkNaked = 25;
                enemy.ExpHaving = 50;
                CSituationStatus.Instance.RegisterChara(enemy);
                CSoundMan.Instance.PlayBGM("BGM_NormalBattle00");
            }
            else if (random < 60) {
                var enemy = new CChara("狼");
                enemy.Hp = 30;
                enemy.AtkNaked = 10;
                enemy.ExpHaving = 10;
                CSituationStatus.Instance.RegisterChara(enemy);
                CSoundMan.Instance.PlayBGM("BGM_NormalBattle00");
            }
        }
        else if (vNextPos.y <= 4) {
            var random = Random.Range(0, 100);
            if (random < 5) {
                // 敵にエンカウント
                var enemy = new CChara("ゴリラ");
                enemy.Hp = 100;
                enemy.AtkNaked = 25;
                enemy.ExpHaving = 50;
                CSituationStatus.Instance.RegisterChara(enemy);
                CSoundMan.Instance.PlayBGM("BGM_NormalBattle00");
            }
            else if (random < 35) {
                var enemy = new CChara("狼");
                enemy.Hp = 30;
                enemy.AtkNaked = 10;
                enemy.ExpHaving = 10;
                CSituationStatus.Instance.RegisterChara(enemy);
                CSoundMan.Instance.PlayBGM("BGM_NormalBattle00");
            }
        }
        else {
            var random = Random.Range(0, 100);
            if (random < 10) {
                // 敵にエンカウント
                var enemy = new CChara("唸る野犬");
                enemy.Hp = 15;
                enemy.AtkNaked = 4;
                enemy.ExpHaving = 4;
                CSituationStatus.Instance.RegisterChara(enemy);
                CSoundMan.Instance.PlayBGM("BGM_NormalBattle00");
            }
            else if (random < 20) {
                var enemy = new CChara("狼");
                enemy.Hp = 30;
                enemy.AtkNaked = 10;
                enemy.ExpHaving = 10;
                CSituationStatus.Instance.RegisterChara(enemy);
                CSoundMan.Instance.PlayBGM("BGM_NormalBattle00");
            }
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
