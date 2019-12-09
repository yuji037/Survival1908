using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CGameCoordinator : CSingletonMonoBehaviour<CGameCoordinator> {

    CPartyChara shigeru;

    private List<CTask> m_lsTasks = new List<CTask>();

    [SerializeField]
    private GameObject[]    m_poActionButton;
    private Action[]        m_pActionDelegate = new Action[2];


    // Use this for initialization
    void Start () {
		//m_cInventryMan.Init();

        CSituationStatus.Instance.Init();

        shigeru = new CPartyChara("Shigeru");
		CPartyStatus.Instance.AppendPartyChara(shigeru);
		shigeru.hp = 50;
        shigeru.atkNaked = 10;
		shigeru.Food = 50;
        shigeru.GainExp(0);
        int mapW = CMapMan.Instance.WIDTH;
        int mapH = CMapMan.Instance.HEIGHT;
        var vPartyPos = new Vector2(Mathf.Round(mapW * 0.5f), Mathf.Round(mapH * 0.5f));
        //CPartyStatus.Instance.SetPartyPos(vPartyPos);
        CMapMan.Instance.SetDispPartyPos(vPartyPos);

        CSoundMan.Instance.PlayBGM("BGM_Field00");
        CPartyStatus.Instance.UpdatePartyText();
        CSituationStatus.Instance.UpdateSituationText();

    }
	
	// Update is called once per frame
	void Update () {

        DebugInputKeyboard();


        if (m_lsTasks.Count != 0)
        {
            var cTask = m_lsTasks[0];
            if (cTask.CalledCount == 0)
            {
				cTask.OnInitialize();
                cTask.OnStart();
            }
            else if (false == cTask.IsEnd)
            {
                cTask.OnUpdate();
            }
            else
            {
                cTask.OnEnd();
                m_lsTasks.RemoveAt(0);
            }
			cTask.CalledCount++;
            return;
        }


    }

    private void DebugInputKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            // デバッグ用
            shigeru.hp = 5000;
            shigeru.maxHp = 5000;
            shigeru.atkNaked = 1000;
            CPartyStatus.Instance.UpdatePartyText();
        }

		if ( Input.GetKeyDown(KeyCode.L) )
		{
			CInventryMan.Instance.Load();
			CMessageWindowMan.Instance.AddText("デバッグ: ロード");
		}


		if (Input.GetKeyDown(KeyCode.Z))
        {
            InputButtonAction(0);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            InputButtonAction(1);
        }

        if (m_lsTasks.Count != 0)
            return;

        if (Input.GetKeyDown(KeyCode.W))
        {
			//CMapMan.Instance.SetDispPartyPos()
            //m_lsTasks.Add(new CTaskMoveArea(new Vector2(0, 1)));
            //m_lsTasks.Add(new CTaskAdvanceTurn());
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            //m_lsTasks.Add(new CTaskMoveArea(new Vector2(0, -1)));
            //m_lsTasks.Add(new CTaskAdvanceTurn());
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            //m_lsTasks.Add(new CTaskMoveArea(new Vector2(-1, 0)));
            //m_lsTasks.Add(new CTaskAdvanceTurn());
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            //m_lsTasks.Add(new CTaskMoveArea(new Vector2(1, 0)));
            //m_lsTasks.Add(new CTaskAdvanceTurn());
        }
    }

    public void InputButtonAction(int index)
    {
        if (m_pActionDelegate[index] == null)
            return;

        m_pActionDelegate[index]();
    }

    public void SetInputAction(int index, string btnText, Action action)
    {
        m_poActionButton[index].GetComponentInChildren<Text>().text = btnText;
        m_pActionDelegate[index] = action;
    }

}

