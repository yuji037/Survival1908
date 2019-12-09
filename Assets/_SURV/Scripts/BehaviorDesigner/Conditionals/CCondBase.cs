using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.SURV
{
	//[TaskDescription("Compares the property value to the value specified. Returns success if the values are the same.")]
	[TaskCategory("SURV")]
	[TaskIcon("{SkinColor}ReflectionIcon.png")]
	public class CCondBase : Conditional
	{
		protected CNpc owner;

		public override void OnAwake()
		{
			owner = gameObject.GetComponent<CNpc>();
		}

		protected virtual TaskStatus TaskUpdate()
		{
			return TaskStatus.Success;
		}

#region ActionCallback
		sealed public override TaskStatus OnUpdate()
		{
			if ( owner.isWincing )
				return TaskStatus.Failure;

			return TaskUpdate();
		}

#endregion

	}
}
