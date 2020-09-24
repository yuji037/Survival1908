using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftDataMan : Singleton<CraftDataMan>
{
	private CraftFacility currentFacility = null;
	private List<CraftRecipe> craftRecipeList = new List<CraftRecipe>();

	public IReadOnlyList<CraftRecipe> CraftRecipeList { get { return craftRecipeList; } }
	public CraftFacility CurrentFacility { get { return currentFacility; } }

	public CraftDataMan()
	{
		var _craftStatusList = Resources.Load<CraftData>("CraftData").craftRecipeList;
		foreach ( var status in _craftStatusList )
		{
			craftRecipeList.Add(status);
		}
	}

	public void SetCurrentFacility(CraftFacility facility)
	{
		currentFacility = facility;
	}

	public bool CanCraftCondition(CraftRecipe recipe)
	{
		if(currentFacility == null)
		{
			return recipe.craftConditionType <= CraftConditionType.None;
		}
		if (currentFacility.ConditionType < recipe.craftConditionType)
		{
			return false;
		}
		return true;
	}
}
