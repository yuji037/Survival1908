using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx.Async;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameCoordinator : SingletonMonoBehaviour<GameCoordinator>
{
	//=============================================================
	// 定数
	//=============================================================
	private const string ROOT_SCENE_NAME = "Root_Common";

	//=============================================================
	// フィールド
	//=============================================================
	[SerializeField] private GameObject		titlePrefab			= default;
	[SerializeField] private GameObject		ingamePrefab		= default;
	[SerializeField] private GameObject		effectManPrefab		= default;
	[SerializeField] private GameObject		soundManPrefab		= default;
	[SerializeField] private EfficacyData	efficacyData		= default;
	[SerializeField] private Transform		objectRoot			= default;			// シーンをまたいでも消したくないオブジェクトの親

	private static bool			isQuickIngame = false;
	private GameObject			titleInstance;
	private GameObject			uiRootPrefab;
	private IngameCoordinator	ingameCoordinator;

	//=============================================================
	// プロパティ
	//=============================================================
	public EfficacyData EfficacyData { get { return efficacyData; } }

	//=============================================================
	// メソッド
	//=============================================================
	protected override void Awake()
    {
		base.Awake();

		CreateCommonObjects();
		if ( isQuickIngame )
		{
			return;
		}
		if (titleInstance == null )
		{
			titleInstance = Instantiate(titlePrefab);
		}
    }

	private void Update()
	{
		DebugMenu.InputDebug();
	}

	private void CreateCommonObjects()
	{
		Instantiate(effectManPrefab);
		Instantiate(soundManPrefab);
	}

	public void GoTitle()
	{
		ActivateUI(false);
	}

	public void GoIngame(string mapSceneName)
	{
		if ( titleInstance != null)
		{
			Destroy(titleInstance);
		}
		if(ingameCoordinator == null)
		{
			var obj = Instantiate(ingamePrefab, objectRoot);
			ingameCoordinator = obj.GetComponent<IngameCoordinator>();
		}
		ingameCoordinator.SwitchIngameScene(mapSceneName, 0);
	}

	public void ActivateUI(bool isIngame)
	{
		if(RootMarkerUI.Instance == null)
		{
			uiRootPrefab = Resources.Load<GameObject>("Prefabs/UI_Root");
			Instantiate(uiRootPrefab);
		}
		RootMarkerUI.Instance.GetComponentInChildren<UIIngame>(true).gameObject.SetActive(isIngame);
	}

#if UNITY_EDITOR
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static async void LoadFirstScene()
	{
		Debug.Log("LoadFirstScene");
		var activeSceneName = SceneManager.GetActiveScene().name;
		// "Root_Common"でないならクイックインゲーム
		if (activeSceneName.StartsWith("Map_"))
		{
			isQuickIngame = true;
			var quickIngameSceneName = SceneManager.GetActiveScene().name;
			SceneManager.LoadScene(ROOT_SCENE_NAME, LoadSceneMode.Single);

			await UniTask.WaitUntil(() => Instance != null);
			Instance.GoIngame(quickIngameSceneName);
		}
	}
#endif
}