using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eFacilityType
{
    None,
    Shelter,
    Bonfire,
    MAX,
}

[System.Serializable]
public class CFacility
{
    public eFacilityType type;
	public float effectValue;
}
