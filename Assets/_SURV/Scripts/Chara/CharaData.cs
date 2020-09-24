using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "SURV/CharaData", fileName = "CharaData")]
public class CharaData : ScriptableObject
{
    public Chara[] m_pcCharas;
}
