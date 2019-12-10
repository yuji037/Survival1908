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

		[Tooltip("-設定値～設定値だけdurationTimeを変化させる")]
		public float durationRandomize = 0f;

		[Tooltip("ターゲットへの誘導角度(/秒) 0で誘導しない")]
		public float homingTargetSpeed = 0f;


		private float _durationTime = 0f;

		public override void OnStart()
		{
			base.OnStart();

			_durationTime = durationTime;
			if ( durationRandomize > 0f )
				_durationTime += Random.Range(-durationRandomize, durationRandomize);
		}

		protected override TaskStatus TaskUpdate()
		{

			if(elapsedTime < durationTime )
			{
				if ( homingTargetSpeed > 0f )
					owner.LookAtTarget(homingTargetSpeed);

				owner.SetAIMovement(owner.Direction, elapsedTime);
				return TaskStatus.Running;
			}

			return TaskStatus.Success;
		}
	}
}

