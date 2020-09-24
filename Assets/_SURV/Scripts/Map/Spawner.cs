using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapUtility;

interface ISpawnable
{
	void SetSpawner(Spawner spawner);
}

public abstract class Spawner : MonoBehaviour
{
	[SerializeField] private GameObject prefab = default;
	[SerializeField] private float spawnAreaX = 10f;
	[SerializeField] private float spawnAreaY = 10f;
	[SerializeField] private float spawnInterval = 20f;
	[SerializeField] private int aliveObjectMax = 10;
	[SerializeField] private bool respawnInfinite = true;

	private float nextSpawnTime = float.PositiveInfinity;
	private int spawnedCount = 0;
	private List<GameObject> aliveObjectList = new List<GameObject>();

	void Start()
	{
		SetNextSpawnTime();
	}

	void Update()
	{
		if( false == respawnInfinite
		&& spawnedCount >= aliveObjectMax)
		{
			// もう生まれない
			return;
		}
		if (IngameTime.Time >= nextSpawnTime)
		{
			SetNextSpawnTime();

			if (aliveObjectList.Count >= aliveObjectMax)
				return;

			if (!FindSpacePosition(this.transform.position, new Vector2(spawnAreaX, spawnAreaY), out var pos)) { return; }

			var newObj = Spawn(pos);
			OnSpawned(newObj);
		}
	}

	private GameObject Spawn(Vector3 pos)
	{
		var newObj = Instantiate(prefab, pos, Quaternion.identity, transform);
		newObj.GetComponent<ISpawnable>().SetSpawner(this);
		aliveObjectList.Add(newObj);
		return newObj;
	}

	protected virtual void OnSpawned(GameObject newObj) { }

	private void SetNextSpawnTime()
	{
		nextSpawnTime = IngameTime.Time + spawnInterval * Random.Range(0.5f, 1.5f);
	}

	public void UnregisterDeadObject(GameObject obj)
	{
		aliveObjectList.Remove(obj);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		//Gizmos.DrawSphere(transform.position, 0.1f);
		Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaX * 2, spawnAreaY * 2, 0f));
	}
}

