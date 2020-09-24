using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectMan : SingletonMonoBehaviour<EffectMan>
{
	private Dictionary<string, COneEffectSet> effectDict = new Dictionary<string, COneEffectSet>();

	private void Start()
	{
		var effectSettings = Resources.Load<EffectSettings>("EffectSettings");

		foreach(var oneEffSet in effectSettings.effects )
		{
			if ( string.IsNullOrWhiteSpace(oneEffSet.id) )
				continue;

			effectDict[oneEffSet.id] = oneEffSet;
		}
	}

	public GameObject PlayOneEffect(string id, Vector3 wldPosition, Quaternion wldRotation, Transform parent = null, float lifeTime = 3f)
	{
		if(false == effectDict.ContainsKey(id) )
		{
			Debug.LogError("指定されたエフェクトはありません。 ID:" + id);
		}

		var oneEffSet = effectDict[id];
		if ( parent == null )
			parent = this.transform;

		var eff = Instantiate(oneEffSet.effect, wldPosition, wldRotation, parent);

		if ( lifeTime > 0f )
			Destroy(eff, lifeTime);

		return eff;
	}
}

[Serializable]
public class COneEffectSet
{
	public string		id;
	public GameObject	effect;
}
