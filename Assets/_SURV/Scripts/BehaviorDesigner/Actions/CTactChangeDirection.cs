using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.SURV
{
	//[TaskDescription("Compares the property value to the value specified. Returns success if the values are the same.")]
	[TaskCategory("SURV")]
	public class CTactChangeDirection : CTactBase
	{
		[Tooltip("falseの場合ターゲットに向く")]
		public bool isRandom = true;

		protected override TaskStatus TaskUpdate()
		{
			var direction = Vector2.down;

			if ( isRandom )
			{
				direction = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)) * direction;
				owner.SetDirection(direction);
			}
			else
			{
				owner.LookAtTarget();
			}


			return TaskStatus.Success;
		}
	}
}

