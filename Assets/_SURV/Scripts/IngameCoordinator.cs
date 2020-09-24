using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx.Async;
using UnityEngine.SceneManagement;
using BehaviorDesigner.Runtime;

public class IngameCoordinator : SingletonMonoBehaviour<IngameCoordinator> 
{
	[SerializeField] private Camera mainCamera = default;
	[SerializeField] private BehaviorManager behaviorManager = default;
	
	private GameObject localPlayerPrefab;
	private GameObject damageTextManPrefab;

	private GameObject localPlayerInstance;

	private Scene currentMapScene;

	private FollowCamera followCamera;

	private MapEnterPoints charaSpawnPoint;

	private bool initialized = false;
	private bool isTimeActive = false;
	private bool isBattleActive = false;

	public Camera Camera { get { return mainCamera; } }
	public BehaviorManager BehaviorManager { get => behaviorManager; }
	public bool IsBattleActive { get => isBattleActive; }

	public void Initialize()
	{
		LoadPrefabs();
		CharasController.Instance.Init();
		CreateCommonObjects();
		ItemInventry.Instance.Load();
		EquipmentInventry.Instance.Load();
		UIIngame.Instance.Initialize();
		initialized = true;
	}

	public async void SwitchIngameScene(string mapSceneName, int spawnPointIndex)
	{
		SetTimeActive(false);

		await UIFadeScreen.Instance.Fade(UIFadeScreen.Screen.Overlay, Color.black, 0.5f);

		GameCoordinator.Instance.ActivateUI(true);

		if (!initialized)
		{
			Initialize();
		}
		if (currentMapScene.IsValid())
		{
			await SceneManager.UnloadSceneAsync(currentMapScene);
		}
		var ao = SceneManager.LoadSceneAsync(mapSceneName, LoadSceneMode.Additive);
		await UniTask.WaitUntil(() => ao.isDone);
		currentMapScene = SceneManager.GetSceneByName(mapSceneName);
		SceneManager.SetActiveScene(currentMapScene);

		Application.targetFrameRate = 60;

		FindMapObjects();
		SetPlayerPosition(spawnPointIndex);

		SoundManager.Instance.PlayBGM("BGM_Field00");
		UIIngame.Instance.SwitchUI(0);
		SetTimeActive(true);
		SetBattleActive(true);
		Physics2D.autoSimulation = false;

		await UIFadeScreen.Instance.Fade(UIFadeScreen.Screen.Overlay, Color.clear, 0.5f);
	}

	// Update is called once per frame
	void Update () {

		if (isTimeActive)
		{
			CharasController.Instance.Tick();
		}

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.K))
		{
			FieldEventManager.Instance.EventCanProceed?.Skip();
		}
#endif
	}

	void FixedUpdate()
	{
		if (isTimeActive)
		{
			CharasController.Instance.FixedTick();
			if (isBattleActive)
			{
				Physics2D.Simulate(Time.fixedDeltaTime);
			}
		}
	}

	public void SetTimeActive(bool value)
	{
		isTimeActive = value;
		Time.timeScale = value ? 1.0f : 0.0f;
	}

	public void SetBattleActive(bool value)
	{
		isBattleActive = value;
	}

	private void LoadPrefabs()
	{
		localPlayerPrefab	= Resources.Load<GameObject>("Prefabs/Chara_00");
		damageTextManPrefab = Resources.Load<GameObject>("Prefabs/DamageTextManager");
	}

	private void CreateCommonObjects()
	{
		if (localPlayerInstance != null)
		{
			Debug.LogError($"既にLocalPlayerがスポーンしています");
		}
		localPlayerInstance = Instantiate(localPlayerPrefab, transform);
		CharasController.Instance.RegisterChara(localPlayerInstance.GetComponent<LocalPlayer>(), CharaBorder.Player);
		followCamera = mainCamera.GetComponent<FollowCamera>();
		followCamera.SetFollowTarget(localPlayerInstance.transform);
		Instantiate(damageTextManPrefab);
	}

	private void FindMapObjects()
	{
		var rootObjects = currentMapScene.GetRootGameObjects();
		foreach(var rootObj in rootObjects )
		{
			if (rootObj.name == "dummyCamera")
			{
				// 消す
				Destroy(rootObj);
			}
			else if (rootObj.name == "MapEnterPoints")
			{
				charaSpawnPoint = rootObj.GetComponent<MapEnterPoints>();
				charaSpawnPoint.Init();
			}
		}
	}

	private void SetPlayerPosition(int spawnPointIndex)
	{
		localPlayerInstance.transform.position = charaSpawnPoint.Points[spawnPointIndex].position;
		localPlayerInstance.transform.rotation = Quaternion.identity;
	}

	private void OnDestroy()
	{
		CharasController.Instance.Clear();
	}
}

