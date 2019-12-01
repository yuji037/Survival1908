using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
	Alpha,
	Bravo,
}

public class CCaster : CActor
{
	[SerializeField]
	private Team teamType;
	public Team TeamType { get { return teamType; } }

	public static bool IsOppositeTeam(CCaster caster1, CCaster caster2)
	{
		return caster1.teamType != caster2.teamType;
	}
}