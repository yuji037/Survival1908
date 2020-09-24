using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class NpcStatusModule : StatusModule
{
	private float gainExp;
	public override float MP { get { return 0; } }
	public override float MaxMP { get { return 0; } }
	public float GainExp { get { return gainExp; } }

	public override void InitStatus(CustomStatusRate customStatusRate)
	{
		SetMaxHP(statusMasterData.HP * statusTable.NpcHPRate * customStatusRate.maxHpRate);
		AddChangeHP(MaxHP - HP);
		atk *= customStatusRate.atkRate;
		def *= customStatusRate.defRate;
		gainExp = statusMasterData.OneNpcExp;
	}

	public override void InitContinueStatus(float hp, float mp, float food) { }
}
