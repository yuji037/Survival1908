using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "SURV/EfficacyData", fileName = "EfficacyData")]
public class EfficacyData : ScriptableObject
{
	[Serializable]
	public class Efficacy
	{
		[SerializeField] private int	id			= default;
		[SerializeField] private Sprite	statusIcon	= default;
		
		public int ID { get { return id; } }
		public Sprite StatusIcon { get { return statusIcon; } }
	}

	[SerializeField] private Efficacy[] efficacys = default;
	public Efficacy[] Efficacys { get { return efficacys; } }


#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(Efficacy))]
	public class EfficacyDrawer : CustomLabelPropertyDrawer
	{
		protected override string GetOverrideLabel(SerializedProperty property)
		{
			var idProp = property.FindPropertyRelative("id");
			if(idProp == null) { return null; }
			var id = idProp.intValue;
			if(false == (Enum.GetValues(typeof(EfficacyType)) as int[]).Contains(id)) { return "未定義ID"; }

			return ((EfficacyType)id).ToString();
		}
	}
#endif
}