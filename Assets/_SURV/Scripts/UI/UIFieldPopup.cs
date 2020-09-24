using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFieldPopup : MonoBehaviour
{
	[SerializeField] private GameObject popupObj = default;
	[SerializeField] private Text text = default;
	[SerializeField] private Slider slider = default;

	private static GameObject fieldPopupCanvasPrefab = null;

	private Transform followTrans;

	public static void Initialize()
	{
		if (fieldPopupCanvasPrefab == null)
		{
			fieldPopupCanvasPrefab = Resources.Load<GameObject>("Prefabs/FieldPopupCanvas");
		}
	}

	public static UIFieldPopup SecureUI(Transform parent)
	{
		if (fieldPopupCanvasPrefab == null)
		{
			Initialize();
		}
		var obj = Instantiate(fieldPopupCanvasPrefab, parent);
		return obj.GetComponentInChildren<UIFieldPopup>();
	}

	// Start is called before the first frame update
	void Start()
    {
		popupObj.SetActive(false);
		DispSlider(false);
	}

	void LateUpdate()
    {
		if (!popupObj.activeSelf) { return; }

		var pos = IngameCoordinator.Instance.Camera.WorldToScreenPoint(followTrans.position);
		transform.position = pos;
	}

	public void Popup(Transform followTrans)
	{
		this.followTrans = followTrans;
		popupObj.SetActive(true);
	}

	public void SetText(string message)
	{
		text.text = message;
	}

	public void SetSliderRate(float rate)
	{
		DispSlider(true);
		slider.value = rate;
	}

	public void DispSlider(bool isOn)
	{
		if (slider.gameObject.activeSelf != isOn)
			slider.gameObject.SetActive(isOn);
	}

	public void Deactive()
	{
		popupObj.SetActive(false);
	}
}
