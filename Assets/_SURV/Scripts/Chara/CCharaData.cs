using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "SURV/CCharaData", fileName = "CCharaData")]
public class CCharaData : ScriptableObject
{
    public CChara[] m_pcCharas;
}
