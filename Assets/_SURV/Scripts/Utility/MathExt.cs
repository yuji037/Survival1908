using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathExt
{
	public static Vector2 GetRandomPointInCircle(float radius)
	{
		float x, y;

		while ( true )
		{
			x = Random.Range(-1f, 1f);
			y = Random.Range(-1f, 1f);

			var sqR = x * x + y * y;
			if ( sqR < 1f )
			{
				break;
			}
		}
		var ret = new Vector2(x, y) * radius;

		return ret;
	}

}
