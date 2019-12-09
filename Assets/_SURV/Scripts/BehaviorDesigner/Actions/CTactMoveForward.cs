using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.SURV
{
	//[TaskDescription("Compares the property value to the value specified. Returns success if the values are the same.")]
	[TaskCategory("SURV")]
	public class CTactMoveForward : CTactBase
	{
		//[Tooltip("")]
		public float durationTime = 2f;

		protected override TaskStatus TaskUpdate()
		{

			if(elapsedTime < durationTime )
			{
				owner.SetAIMovement(owner.Direction, elapsedTime);
				return TaskStatus.Running;
			}

			return TaskStatus.Success;
		}
	}
}

