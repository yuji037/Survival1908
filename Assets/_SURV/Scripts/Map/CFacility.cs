using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eFacilityType
{
    Shelter,
    MAX,
}

[System.Serializable]
public class CFacility
{
    public eFacilityType eType;
    public float fEffectValue;
}
