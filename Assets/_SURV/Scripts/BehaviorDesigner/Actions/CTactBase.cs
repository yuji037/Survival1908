using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using BehaviorDesigner.Runtime;

namespace BehaviorDesigner.Runtime.Tasks.SURV
{

	//[TaskDescription("Returns a TaskStatus of running. Will only stop when interrupted or a conditional abort is triggered.")]
	[TaskCategory("SURV")]
	public class CTactBase : Action
	{

		//[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		//public SharedGameObject targetGameObject;

		protected CNpc owner;

		protected float elapsedTime = 0f;

		public override void OnAwake()
		{
			owner = gameObject.GetComponent<CNpc>();
		}

		protected virtual TaskStatus TaskUpdate()
		{
			return TaskStatus.Success;
		}

		public override void OnEnd()
		{
			elapsedTime = 0f;
		}

		#region ActionCallback
		sealed public override TaskStatus OnUpdate()
		{
			if ( owner.isWincing )
				return TaskStatus.Failure;

			elapsedTime += Time.deltaTime;
			return TaskUpdate();
		}
		#endregion

	}
}

