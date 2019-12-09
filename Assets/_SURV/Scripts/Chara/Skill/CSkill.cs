using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

// [FormerlySerializedAs("effect")]

[Serializable]
public class CSkill
{
	public int					id;
	public string				dispName;
	public int					attackPower;
	public float				castTime;
	public Vector2[]			projectileAppearOffsets;
	public float				randomizeRadius;
	public GameObject			projectile;
	public int					shootCountMax;
	public float				shootInterval;
	public SkillStartActionType startActionType;
	public float				knockbackPower;
}

public enum SkillStartActionType
{
	StepForward,
	MagicCircle01,
}

public enum SkillType
{
	Melee,
	Range,
}

public class CSkillLoader
{
	private CSkillLoader() { }

	private static CSkillLoader instance = null;

	public static CSkillLoader Instance
	{
		get
		{
			if(instance == null )
			{
				instance = new CSkillLoader();
			}
			return instance;
		}
	}

	private Dictionary<int, CSkill> skillDict;

	public CSkill Load(int id)
	{
		if(skillDict == null )
		{
			var skillData = Resources.Load<CSkillData>("CSkillData");
			skillDict = new Dictionary<int, CSkill>();
			foreach(var skill in skillData.skills )
			{
				skillDict[skill.id] = skill;
			}
		}

		return skillDict[id];
	}
}
