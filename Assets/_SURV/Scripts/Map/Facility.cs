using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacilityType
{
    None,
    Shelter,
    Bonfire,
    MAX,
}

[System.Serializable]
public class Facility
{
    public FacilityType type;
	public float effectValue;
}
