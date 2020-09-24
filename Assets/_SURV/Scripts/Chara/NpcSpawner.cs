using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapUtility;

public class NpcSpawner : Spawner
{
	[SerializeField] private int npcLevel = 1;

	protected override void OnSpawned(GameObject newObj)
	{
		var newNpc = newObj.GetComponent<Npc>();
		newNpc.SetLevel(npcLevel);
		// いったんEnemyだけ受け付け
		CharasController.Instance.RegisterChara(newNpc, CharaBorder.Enemy);
		newNpc.SetTarget();
	}
}
