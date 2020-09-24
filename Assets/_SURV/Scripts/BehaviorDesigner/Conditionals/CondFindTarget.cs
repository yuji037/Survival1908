using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.SURV
{
	//[TaskDescription("Compares the property value to the value specified. Returns success if the values are the same.")]
	[TaskCategory("SURV")]
	[TaskIcon("{SkinColor}ReflectionIcon.png")]
	public class CondFindTarget : CondBase
	{
		[Tooltip("視野角")]
		public float sightAngle = 45f;
		[Tooltip("視野距離")]
		public float sightRange = 5f;

		protected override TaskStatus TaskUpdate()
		{
			if ( owner.Target == null )
			{
				// 異常検出のため　本来はFailure
				return TaskStatus.Running;
			}

			var dirOwner = owner.DirectionRightAngle;
			var dirToTarget = owner.Target.transform.position - owner.transform.position;

			if ( Vector2.Angle(dirOwner, dirToTarget) <= sightAngle &&
					Vector2.Distance(owner.Target.transform.position, owner.transform.position) <= sightRange )
			{
				return TaskStatus.Success;
			}

			return TaskStatus.Failure;
		}

		//public override void OnReset()
		//{
		//}
	}
}
