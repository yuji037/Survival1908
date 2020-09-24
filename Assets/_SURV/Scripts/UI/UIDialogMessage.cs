using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogMessage : SingletonMonoBehaviour<UIDialogMessage>
{
	private const float DISP_WINDOW_TIME = 3f;

	[SerializeField] public     float	dispTextInterval    = 0.05f;
	[SerializeField] private	Text	messageText			= default;
	[SerializeField] private	Button	proceedEventButton	= default;

	private float			dispTextTimer	= 0f;
    private List<string>	dispTexts		= new List<string>();
    private int				dispTextLength	= 0;
    private float			nextUpdateTime	= 0f;
    private bool			isUpdatingText	= false;
	
	private float			dispWindowRemainTime = 0f;

	protected override void Awake()
	{
		GetComponent<Canvas>().enabled = true;
		base.Awake();
	}

	// Use this for initialization
	private void Start()
    {
        ClearText();
		DispWindow(false);
		proceedEventButton.onClick.AddListener(ProceedEvent);
	}

	// Update is called once per frame
	private void Update()
    {
		CheckUpdateText();
		CheckDispWindow();
	}

	private void CheckUpdateText()
    {
		if (false == isUpdatingText) { return; }

		dispTextTimer += Time.unscaledDeltaTime;

		if (dispTextTimer <= nextUpdateTime) { return; }

		if (dispTexts.Count == 0)
            return;

        var dispText = "";
        int i;
        for(i = 0; i < dispTexts.Count - 1; ++i)
        {
            dispText += dispTexts[i];
            dispText += "\n";
        }

        // 最後に指示された文字列を
        // 時間をかけて表示
        var length = dispTextTimer / dispTextInterval;
        if (length >= dispTextLength)
        {
            isUpdatingText = false;
            length = dispTextLength;
        }
		dispText += dispTexts[i].Substring(0, Mathf.FloorToInt(length));
		messageText.text = dispText;
        nextUpdateTime += dispTextInterval;
    }

	private void CheckDispWindow()
	{
		if(dispWindowRemainTime > 0f)
		{
			dispWindowRemainTime -= Time.unscaledDeltaTime;
			if(dispWindowRemainTime <= 0f)
			{
				DispWindow(false);
			}
		}
	}

    public void AddText(string sText)
    {
        dispTexts.Add(sText);
        dispTextTimer = 0f;
        dispTextLength = sText.Length;
        nextUpdateTime = 0f;
		if (dispTexts.Count > 5) {
			dispTexts.RemoveAt (0);
		}
        isUpdatingText = true;
    }

	public void ShowOnWindowContinuous()
	{
		DispWindow(true);
		dispWindowRemainTime = -1f;
	}

	public void ShowOffWindow()
	{
		DispWindow(false);
	}

	public void ShowOnWindowForSeconds(float second)
	{
		dispWindowRemainTime = second;
		DispWindow(true);
	}

    public void ClearText()
    {
        dispTexts.Clear();
        messageText.text = "";
    }

	private void DispWindow(bool isDisp)
	{
		if(gameObject.activeSelf != isDisp)
		{
			gameObject.SetActive(isDisp);
		}
	}

	private void ProceedEvent()
	{
		LocalPlayer.Instance?.TryProceedFieldEvent();
	}
}
