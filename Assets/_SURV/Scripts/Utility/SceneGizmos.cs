using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGizmos : MonoBehaviour
{
#if UNITY_EDITOR
	[SerializeField] private Color gizmoColor = Color.yellow;
	[SerializeField] private float radius = 0.5f;

	private void OnDrawGizmosSelected()
	{
		var oldColor = Gizmos.color;
		Gizmos.color = gizmoColor;
		Gizmos.DrawWireSphere(transform.position, radius);
		Gizmos.color = oldColor;
	}

#endif
}
