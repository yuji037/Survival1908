using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CSituationStatus : CSingletonMonoBehaviour<CSituationStatus>
{
    public 	    List<CChara> 	m_lsEnemy{ get; private set; }

    public 		float 	m_fDispTextInterval 	= 0.05f;
    private 	float 	m_fDispTextTimer 		= 0f;
    private 	string 	m_sDispText 			= "";
    private 	int 	m_iDispTextLength 		= 0;
    private 	float 	m_fNextUpdateTime 		= 0f;
    private     bool    m_IsUpdating            = false;

    [SerializeField]
    private     Text    m_textMessage;
      
	public  CLocationStatus[]    m_pcLocationStatus;
    public  string              m_sLocationName = "空き地";
	public  int                 m_iLocationType = 0;

    // Use this for initialization
    public void Init()
    {
        m_pcLocationStatus = Resources.Load<CLocationData>("CLocationData").m_pcLocationStatus;
        m_lsEnemy = new List<CChara>();

        CMapMan.Instance.Init();

        UpdateSituationText();
    }

    // Update is called once per frame
    void Update()
    {
        if (false == m_IsUpdating) {
            return;
        }

        m_fDispTextTimer += Time.deltaTime;

        if (m_fDispTextTimer > m_fNextUpdateTime)
        {
            UpdateMessage();
        }
    }

    void UpdateMessage()
    {
        var sLength = m_fDispTextTimer / m_fDispTextInterval;
        if (sLength >= m_iDispTextLength)
        {
            sLength = m_iDispTextLength;
        }
        m_textMessage.text = m_sDispText.Substring(0, Mathf.FloorToInt(sLength));
        m_fNextUpdateTime += m_fDispTextInterval;

    }

    public void DispMessage(string sText)
    {
        m_sDispText = sText;
        m_fDispTextTimer = 0f;
        m_iDispTextLength = sText.Length;
        m_fNextUpdateTime = 0f;
        m_IsUpdating = true;
    }

	public void UpdateSituationText()
	{
		var sDispText = "";

		foreach (var enemy in m_lsEnemy) {

			sDispText +=
				enemy.Name + "\nHP : " +
				enemy.Hp.ToString("f0") + "\n";
		}

        sDispText += GetCurrentLocationName();

		DispMessage(sDispText);
	}

	public void RegisterChara(CChara chara){
		m_lsEnemy.Add(chara);
	}
	public void UnregisterChara(CChara chara){
		m_lsEnemy.Remove(chara);
	}

	public CChara GetChara(int index){
		if (index >= m_lsEnemy.Count) {
			return null;
		}
		return m_lsEnemy [index];
	}

    public int GetLocationStatusCount()
    {
        return m_pcLocationStatus.Length;
    }

    public string GetCurrentLocationName()
    {
        return m_pcLocationStatus[m_iLocationType].Name;
    }

    public string GetLocationName(int iLocationType)
    {
        return m_pcLocationStatus[iLocationType].Name;
    }

    public CFacility GetCurrentFacility(){
        var ivPos = CPartyStatus.Instance.GetPartyPos();
        return CMapMan.Instance.GetMapFacility(ivPos.x, ivPos.y);
    }

    public void UpdateLocation(){
        var vPos = CPartyStatus.Instance.m_vPartyPos;
        m_iLocationType = CMapMan.Instance.GetMapType(
            Mathf.RoundToInt(vPos.x),
            Mathf.RoundToInt(vPos.y));
    }

	public List<string> SearchItem(){
        return m_pcLocationStatus[m_iLocationType].SearchItem();
	}

	public void OnTurnElapsed(){

		m_lsEnemy = m_lsEnemy.Where(c => c.Hp > 0).ToList();

		UpdateSituationText();
	}
}
