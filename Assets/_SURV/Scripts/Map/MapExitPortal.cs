using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapExitPortal : MonoBehaviour
{
	[SerializeField] private string enterMapScene;
	[SerializeField] private int spawnPointIndex;

	[SerializeField] private AreaTrigger popupAreaTrigger;
	[SerializeField] private AreaTrigger switchSceneAreaTrigger;

	private UIFieldPopup uiFieldPopup;

	// Start is called before the first frame update
	void Start()
    {
		popupAreaTrigger.onTriggerEnter2D = Popup;
		popupAreaTrigger.onTriggerExit2D = DeactivePopup;
		switchSceneAreaTrigger.onTriggerEnter2D = SwitchScene;
		uiFieldPopup = UIFieldPopup.SecureUI(transform);
		uiFieldPopup.SetText(enterMapScene);
		uiFieldPopup.DispSlider(false);
	}

	private void Popup(Collider2D collision)
	{
		if(collision.gameObject != LocalPlayer.Instance.gameObject) { return; }
		uiFieldPopup.Popup(transform);
	}

	private void DeactivePopup(Collider2D collision)
	{
		if(collision.gameObject != LocalPlayer.Instance.gameObject) { return; }
		uiFieldPopup.Deactive();
	}

	private void SwitchScene(Collider2D collision)
	{
		if(collision.gameObject != LocalPlayer.Instance.gameObject) { return; }
		IngameCoordinator.Instance.SwitchIngameScene(enterMapScene, spawnPointIndex);
	}
}
