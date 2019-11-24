using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTaskSearchAround : CTask
{
	public override void OnStart()
	{
		StartCorutine(TaskCoroutine());
	}

	IEnumerator TaskCoroutine(){
		CMessageWindowMan.Instance.ClearText();

		CMessageWindowMan.Instance.AddText("あたりを見回した！");
		yield return new WaitForSeconds(1f);

		var sDispText = "";

		var lsFindItem = CSituationStatus.Instance.SearchItem();
		if (lsFindItem.Count == 0) {
			sDispText = "周囲には何も見つからなかった。";
		} else {
            var player = CPartyStatus.Instance.GetPartyChara(0);

            for(int i = 0; i < lsFindItem.Count; ++i) {
                var itemStatus = CItemDataMan.Instance.GetItemStatusByName(lsFindItem[i]);

                // アイテム獲得
                CInventryMan.Instance.ManipulateItemCount(itemStatus.id, 1);

                sDispText += lsFindItem[i];
                if (i != lsFindItem.Count - 1)
                    sDispText += "、";
			}
            sDispText += "を見つけた！";
		}

		CMessageWindowMan.Instance.AddText(sDispText);

		IsEnd = true;
	}
}
