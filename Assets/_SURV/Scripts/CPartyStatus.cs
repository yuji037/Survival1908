using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPartyStatus : CSingletonMonoBehaviour<CPartyStatus>
{

	private     List<CPartyChara> m_lsPartyChara = new List<CPartyChara>();
               
    public      float       m_fDispTextInterval     = 0.05f;
    private     float       m_fDispTextTimer        = 0f;
    private     string      m_sDispText             = "";
    private     int         m_iDispTextLength       = 0;
    private     float       m_fNextUpdateTime       = 0f;
    private     bool        m_IsUpdating            = false;

    public      Vector2     m_vPartyPos { get; private set; }
    private     IntVector2  m_ivPartyPos;

    [SerializeField]
    private     Text        m_textMessage;

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

    private void UpdateMessage()
    {
        var sLength = m_fDispTextTimer / m_fDispTextInterval;
        if (sLength >= m_iDispTextLength)
        {
            m_IsUpdating = false;
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

	public void AppendPartyChara(CPartyChara partyChara){
		m_lsPartyChara.Add(partyChara);
	}

	public CPartyChara GetPartyChara(int index){
		if (index >= m_lsPartyChara.Count) {
			return null;
		}
		return m_lsPartyChara [index];
	}

    public IntVector2 GetPartyPos()
    {
        return m_ivPartyPos;
    }

    public void SetPartyPos(Vector2 vPos){
        m_vPartyPos = vPos;
        m_ivPartyPos = new IntVector2(
            Mathf.RoundToInt(vPos.x),
            Mathf.RoundToInt(vPos.y));
        CSituationStatus.Instance.UpdateLocation();
    }

	public void UpdatePartyText()
	{
		foreach (var pc in m_lsPartyChara) {
            DispMessage(
                pc.Name + "\nLevel : " +
                pc.Level + "\nEXP : " +
                pc.Exp + "\nHP : " +
                pc.Hp.ToString("f0") + " / " + pc.MaxHp.ToString("f0") + "\nATK : " +
                pc.GetAtk() + "\nDEF : " +
                pc.GetDef() + "\nFood : " +
                pc.Food.ToString("f0") + "\n脱出の秘宝 : " + 
                CInventryMan.Instance.GetHasItemCount("NoUse00") + " / 4");
		}
	}

	public void OnTurnElapsed(){
		foreach (var pc in m_lsPartyChara) {
            if (pc.Food > 30) {
                pc.Hp += 2;
                if (pc.Hp > pc.MaxHp)
                    pc.Hp = pc.MaxHp;
            }
            else if (pc.Food <= 0) {
                pc.Hp -= 2;

            }
			pc.Food -= 2;
            if (pc.Food < 0)
                pc.Food = 0;
		}

		// ゲームオーバー、キャラ死亡時の処理も入れる

		UpdatePartyText();
	}
}

public struct IntVector2
{
    public int x;
    public int y;

    public IntVector2(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}

public static class Vector2Extention
{
    public static IntVector2 GetIntVector2(this Vector2 vec2)
    {
        return new IntVector2(
            Mathf.RoundToInt(vec2.x),
            Mathf.RoundToInt(vec2.y));
    }
}