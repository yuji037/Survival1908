using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMarkerUI : SingletonMonoBehaviour<RootMarkerUI>
{
	[SerializeField]
	private RectTransform rectTransform = null;
	public RectTransform RectTransform { get => rectTransform; }
}
