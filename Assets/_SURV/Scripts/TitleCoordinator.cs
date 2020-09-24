using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleCoordinator : MonoBehaviour
{
	[SerializeField] private Button nextButton; 
	[SerializeField] private Button quickBattleButton;

	private void Start()
	{
		GameCoordinator.Instance.GoTitle();
	}

	public void GoIngame(string ingameMapSceneName)
	{
		GameCoordinator.Instance.GoIngame(ingameMapSceneName);
	}

	// デバッグ機能ボタン
	public void DebugSaveReset()
	{
		DebugMenu.SaveReset();
	}
}
