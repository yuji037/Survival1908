using UnityEngine;

/// <summary>
/// 凸多角形の当たり判定が取れるようにする機能
/// </summary>
public class CustomizeButtonCollider : MonoBehaviour, ICanvasRaycastFilter
{
	[Header("Buttonと同じGameObjectに置くこと。")]

	[SerializeField]
	Vector2[] points = null;

	private const float _unit = 1f / 360f;

	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		// 当たり判定の上書きなし
		if (points == null)
		{
			return true;
		}
		//Vector3 target = eventCamera.ScreenToWorldPoint(sp);
		Vector3 target;
		RectTransformUtility.ScreenPointToWorldPointInRectangle(RootMarkerUI.Instance.transform.GetComponent<RectTransform>(), sp, eventCamera, out target);
		target.x -= transform.position.x;
		target.y -= transform.position.y;

		return IsValid(target);
	}

	private bool IsValid(Vector2 target)
	{
		int wn = 0;
		for (int i = 0; i < points.Length; i++)
		{
			int current = i;
			int next = (i + 1) % points.Length;

			float vt = 0.0f;
			// 上向きの辺。点PがY軸方向について、始点と終点の間にある。（ただし、終点は含まない）
			if ((points[current].y <= target.y) && (points[next].y > target.y))
			{
				// 辺は点Pよりも右側にある。ただし重ならない
				// 辺が点Pと同じ高さになる位置を特定し、その時のXの値と点PのXの値を比較する
				vt = (target.y - points[current].y) / (points[next].y - points[current].y);

				if (target.x < (points[current].x + (vt * (points[next].x - points[current].x))))
				{
					// 上向きの辺と交差した場合は+1
					wn++;
				}
			}
			else if ((points[current].y > target.y) && (points[next].y <= target.y))
			{
				// 辺は点Pよりも右側にある。ただし重ならない
				// 辺が点Pと同じ高さになる位置を特定し、その時のXの値と点PのXの値を比較する
				vt = (target.y - points[current].y) / (points[next].y - points[current].y);

				if (target.x < (points[current].x + (vt * (points[next].x - points[current].x))))
				{
					// 下向きの辺と交差した場合は-1
					wn--;
				}
			}
		}
		return wn != 0;
	}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (points == null)
			{
				return;
			}

			// Gizmosでコライダー部分を表示するかどうか
			bool isDrawGizmos = false;
			foreach (GameObject go in UnityEditor.Selection.gameObjects)
			{
				if (go == this.gameObject)
				{
					isDrawGizmos = true;
					break;
				}
			}
			if (isDrawGizmos == false)
			{
				return;
			}

			Gizmos.color = Color.green;
			Vector3 basePosotion = transform.position;
			Vector3 from;
			Vector3 to;
			for (int i = 0; i < points.Length; i++)
			{
				from = basePosotion;
				from.x += points[i].x;
				from.y += points[i].y;
				int nextIndex = i + 1;
				nextIndex %= points.Length;
				to = basePosotion;
				to.x += points[nextIndex].x;
				to.y += points[nextIndex].y;
				Gizmos.DrawLine(from, to);
			}
		}
#endif
}
