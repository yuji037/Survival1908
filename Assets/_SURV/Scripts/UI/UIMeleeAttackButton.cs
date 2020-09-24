using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Async;

public class UIMeleeAttackButton : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
		await UniTask.WaitUntil(() => LocalPlayer.Instance != null);
		GetComponent<Button>().onClick.AddListener(LocalPlayer.Instance.OnPressAction1Button);
    }
}
