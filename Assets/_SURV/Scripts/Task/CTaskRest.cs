using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTaskRest : CTask
{
    public override void OnStart()
    {
        StartCorutine(TaskCoroutine());
    }

    IEnumerator TaskCoroutine()
    {
        CMessageWindowMan.Instance.ClearText();

        CMessageWindowMan.Instance.AddText("シェルターの中で休んだ…");
        yield return new WaitForSeconds(1f);

        //var sDispText = "";

        // 今は一人だけ
        var chara = CPartyStatus.Instance.GetPartyChara(0);
        var fHeal = chara.maxHp * 0.6f;
        //chara.HealHP(fHeal);

        //CMessageWindowMan.Instance.AddText(sDispText);

        IsEnd = true;
    }
}
