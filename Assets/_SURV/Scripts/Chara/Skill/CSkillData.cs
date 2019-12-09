using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SURV/CSkillData", fileName = "CSkillData")]
public class CSkillData : ScriptableObject
{
	public CSkill[] skills;
}