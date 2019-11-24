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

	// TODO: データ部分と表示部分を分けるべき
    [SerializeField]
    private     Text        m_textMessage;
    [SerializeField]
    private     Slider      m_sliderHP;
    [SerializeField]
    private     Slider      m_sliderFood;
    [SerializeField]
    private     Text        m_textDetail;

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
			// インゲーム画面のパーティステータス
            DispMessage(
                pc.name + 
				"\nLevel : " + pc.Level + 
				//"\nEXP : " + pc.Exp + 
				"\nHP : " + pc.hp.ToString("f0") + " / " + pc.maxHp.ToString("f0") + 
				"\nFood : " + pc.Food.ToString("f0")/* + */
				//"\nATK : " + pc.GetAtk() + 
				//"\nDEF : " + pc.GetDef()
				);

			// 持ち物画面のパーティステータス
			m_textDetail.text = pc.name +
				"\nLevel : " + pc.Level +
				"\nEXP : " + pc.Exp +
				"\nHP : " + pc.hp.ToString("f0") + " / " + pc.maxHp.ToString("f0") +
				"\nFood : " + pc.Food.ToString("f0") +
				"\n攻 : " + pc.GetAtk() +
				"\n守 : " + pc.GetDef() +
				"\n武器 : " + pc.GetEquipmentItemName(EquipmentPart.Weapon) +
				"\n頭 : " + pc.GetEquipmentItemName(EquipmentPart.Head) +
				"\n胴 : " + pc.GetEquipmentItemName(EquipmentPart.Body) +
				"\nアクセ1 : " + pc.GetEquipmentItemName(EquipmentPart.Accessory1) +
				"\nアクセ2 : " + pc.GetEquipmentItemName(EquipmentPart.Accessory2)
				;

			m_sliderHP.value = pc.hp / pc.maxHp;
			
			//// MaxHPによってSliderUIの長さを変える処理
            //var rt = m_sliderHP.GetComponent<RectTransform>();
            //var sizeDelta = rt.sizeDelta;
            //// MaxHp:50 で 100f
            //sizeDelta.x = pc.MaxHp * 2f;
            //rt.sizeDelta = sizeDelta;
            m_sliderFood.value = pc.Food / 100f;
		}
	}

	public void OnTurnElapsed(){
		foreach (var pc in m_lsPartyChara) {
            if (pc.Food > 30) {
                pc.hp += 2;
                if (pc.hp > pc.maxHp)
                    pc.hp = pc.maxHp;
            }
            else if (pc.Food <= 0) {
                pc.hp -= 2;

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