using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SURV/SkillData", fileName = "SkillData")]
public class SkillData : ScriptableObject
{
	public Skill[] skills;
}