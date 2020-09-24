using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapEnterPoints : MonoBehaviour
{
	private Transform[] points;
	public IReadOnlyList<Transform> Points { get { return points; } }

	public void Init()
	{
		points = GetComponentsInChildren<Transform>().Where(tr => tr != transform).ToArray();
	}
}
