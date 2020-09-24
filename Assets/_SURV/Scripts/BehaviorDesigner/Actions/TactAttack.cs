using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.SURV
{
	//[TaskDescription("Compares the property value to the value specified. Returns success if the values are the same.")]
	[TaskCategory("SURV")]
	public class TactAttack : TactBase
	{
		//[Tooltip("")]
		public int attackModuleIndex = default;

		public override void OnStart()
		{
			owner.Attack(attackModuleIndex);
		}
	}
}

