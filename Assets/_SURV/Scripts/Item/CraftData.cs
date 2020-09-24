using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "SURV/CraftData", fileName = "CraftData" )]
public class CraftData : ScriptableObject
{
	[UnityEngine.Serialization.FormerlySerializedAs("craftStatusList")]
	public CraftRecipe[] craftRecipeList;
}
