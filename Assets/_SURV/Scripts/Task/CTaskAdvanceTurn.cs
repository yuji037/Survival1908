using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTaskAdvanceTurn : CTask
{
	public override void OnStart ()
	{
		base.OnStart();

		CPartyStatus.Instance.OnTurnElapsed();
		CSituationStatus.Instance.OnTurnElapsed();
        CGameCoordinator.Instance.UpdateInputAction();

		IsEnd = true;
	}
}
