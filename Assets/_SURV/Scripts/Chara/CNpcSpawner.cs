using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNpcSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject npcPrefab;

	[SerializeField]
	private float spawnAreaX = 10f;
	[SerializeField]
	private float spawnAreaY = 10f;

	[SerializeField]
	private float spawnInterval = 20f;
	[SerializeField]
	private int aliveNpcMax = 10;

	private float elapsedTime = 0f;

	private float nextSpawnTime = 0f;

	private List<CNpc> aliveNpcList = new List<CNpc>();

	// Start is called before the first frame update
	void Start()
    {
		SetNextSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
		if(Time.time >= nextSpawnTime )
		{
			SetNextSpawnTime();

			if ( aliveNpcList.Count >= aliveNpcMax )
				return;

			var pos = this.transform.position + new Vector3(
				Random.Range(-spawnAreaX, spawnAreaX),
				Random.Range(-spawnAreaY, spawnAreaY),
				0f);
			var newNpcObj = Instantiate(npcPrefab, pos, Quaternion.identity, transform);
			var newNpc = newNpcObj.GetComponent<CNpc>();
			newNpc.spawner = this;

			aliveNpcList.Add(newNpc);
		}
    }

	void SetNextSpawnTime()
	{
		nextSpawnTime = Time.time + spawnInterval * Random.Range(0.5f, 1.5f);
	}

	public void UnregisterNpc(CNpc npc)
	{
		aliveNpcList.Remove(npc);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		//Gizmos.DrawSphere(transform.position, 0.1f);
		Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaX * 2, spawnAreaY * 2, 0f));
	}
}
