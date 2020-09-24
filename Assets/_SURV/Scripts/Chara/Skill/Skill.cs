using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

// 

[Serializable]
public class Skill
{
	public int					id								= default;
	public string				dispName						= default;
	public Sprite				icon							= default;
	public int					costMP							= default;
	public int					effectValue1					= default;
	public float				castTime						= default;
	public GameObject			projectile						= default;
	public Vector2[]			projectileAppearOffsetPosList	= default;
	public float				posRandomizeRadius				= default;
	public float[]				projectileAppearOffsetRotList	= default;
	public int					shootCountMax					= default;
	public float				shootInterval					= default;
	public SkillStartActionType startActionType					= default;
	public string				burstSound						= default;
	public float				knockbackPower					= default;
}

public enum SkillStartActionType
{
	StepForward,
	MagicCircle01,
}

public class SkillLoader
{
	private SkillLoader() { }

	private static SkillLoader instance = null;

	public static SkillLoader Instance
	{
		get
		{
			if(instance == null )
			{
				instance = new SkillLoader();
			}
			return instance;
		}
	}

	private Dictionary<int, Skill> skillDict;

	public Skill Load(int id)
	{
		if(skillDict == null )
		{
			var skillData = Resources.Load<SkillData>("SkillData");
			skillDict = new Dictionary<int, Skill>();
			foreach(var skill in skillData.skills )
			{
				skillDict[skill.id] = skill;
			}
		}

		return skillDict[id];
	}
}
