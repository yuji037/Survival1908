using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUtility
{
	private const int MapLayerMask = 1 << (int)ObjectLayer.Default;
	public static bool FindSpacePosition(Vector2 origin, Vector2 areaHalfSize, out Vector2 pos)
	{
		int attempt = 0;
		while (attempt < 10)
		{
			pos = origin + new Vector2(
				Random.Range(-areaHalfSize.x, areaHalfSize.x),
				Random.Range(-areaHalfSize.y, areaHalfSize.y));

			var hit = Physics2D.CircleCast(pos, 0.5f, Vector2.up, MapLayerMask);

			if (hit.collider == null)
			{
				// マップコライダーに当たっていないのでOK
				return true;
			}
			attempt++;
		}

		//Debug.Log($"ランダム場所取得に失敗しました");
		pos = Vector3.zero;
		return false;
	}
}
