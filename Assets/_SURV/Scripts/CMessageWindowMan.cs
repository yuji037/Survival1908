using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMessageWindowMan : CSingletonMonoBehaviour<CMessageWindowMan>
{

    public      float           m_fDispTextInterval     = 0.05f;
    private     float           m_fDispTextTimer        = 0f;
    private     List<string>    m_lsDispText             = new List<string>();
    private     int             m_iDispTextLength       = 0;
    private     float           m_fNextUpdateTime       = 0f;
    private     bool            m_IsUpdating            =false;

    [SerializeField]
    private     Text    m_textMessage;

    // Use this for initialization
    void Start()
    {
        ClearText();
    }

    // Update is called once per frame
    void Update()
    {
        if (false == m_IsUpdating) {
            return;
        }

        m_fDispTextTimer += Time.deltaTime;

        if(m_fDispTextTimer > m_fNextUpdateTime)
        {
            UpdateMessage();
        }
    }

    void UpdateMessage()
    {
        if (m_lsDispText.Count == 0)
            return;

        var sDispText = "";
        int i;
        for(i = 0; i < m_lsDispText.Count - 1; ++i)
        {
            sDispText += m_lsDispText[i];
            sDispText += "\n";
        }

        // 最後に指示された文字列を
        // 時間をかけて表示
        var sLength = m_fDispTextTimer / m_fDispTextInterval;
        if (sLength >= m_iDispTextLength)
        {
            m_IsUpdating = false;
            sLength = m_iDispTextLength;
        }
		sDispText += m_lsDispText[i].Substring(0, Mathf.FloorToInt(sLength));
		m_textMessage.text = sDispText;
        m_fNextUpdateTime += m_fDispTextInterval;

    }

    public void AddText(string sText)
    {
        m_lsDispText.Add(sText);
        m_fDispTextTimer = 0f;
        m_iDispTextLength = sText.Length;
        m_fNextUpdateTime = 0f;
		if (m_lsDispText.Count > 5) {
			m_lsDispText.RemoveAt (0);
		}
        m_IsUpdating = true;
    }

    public void ClearText()
    {
        m_lsDispText.Clear();
        m_textMessage.text = "";
    }
}
