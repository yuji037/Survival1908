using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CItemUIElement
{
    public string itemId;
    public GameObject gameObject;
    public Button button;
    public Text text;
}

public class CInventryMan : CSingletonMonoBehaviour<CInventryMan>
{
    [SerializeField]
    private GameObject m_oWindow;

    private Dictionary<string, int> m_dicItemHasCount = new Dictionary<string, int>();

    [SerializeField]
    private GameObject m_oInventryCellParent;
    [SerializeField]
    private GameObject m_oInventryCellElement;

    private List<CItemUIElement> m_lsInventryElements = new List<CItemUIElement>();

    [SerializeField]
    private Image m_imgSelectingInventryUI;

    private string m_sCurrentSelectItemId;

    [SerializeField]
    private Text m_txtSelectItemAction;

    private int m_iSelectInventryIndex;

    public void Init()
    {
        //Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_oInventryCellElement.SetActive(false);
        DispWindow(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetHasItemCount(string sID)
    {
        if (false == m_dicItemHasCount.ContainsKey(sID))
        {
            return 0;
        }
        return m_dicItemHasCount[sID];
    }
    public void GainItemCount(string sID, int iCount)
    {
        if (false == m_dicItemHasCount.ContainsKey(sID))
        {
            m_dicItemHasCount[sID] = 0;
        }
        m_dicItemHasCount[sID] += iCount;
    }

    public void DispWindow(bool bDisp)
    {
        m_oWindow.SetActive(bDisp);
        if (bDisp)
        {
            UpdateInventryUI();
        }
    }

    public void UpdateInventryUI()
    {
        int iElement = 0;
        foreach (var hasCount in m_dicItemHasCount)
        {

            if (hasCount.Value == 0)
                continue;

            CItemUIElement element = null;
            if (iElement >= m_lsInventryElements.Count)
            {
                element = new CItemUIElement();
                var obj = Instantiate(m_oInventryCellElement);
                element.gameObject = obj;
                obj.transform.SetParent(m_oInventryCellParent.transform, false);
                element.text = obj.GetComponentInChildren<Text>();
                element.button = obj.GetComponentInChildren<Button>();
                m_lsInventryElements.Add(element);
                // WARNING:ここ既にアクティブかどうかチェックした方が、動作軽くなるかも
                element.gameObject.SetActive(true);
            }
            else
            {
                element = m_lsInventryElements[iElement];
                // WARNING:ここ既にアクティブかどうかチェックした方が、動作軽くなるかも
                element.gameObject.SetActive(true);
            }

            var itemStatus = CItemDataMan.Instance.GetItemStatusById(hasCount.Key);

            var sLabelTxt = itemStatus.Name;
            if (hasCount.Value >= 2)
                sLabelTxt += " ×" + hasCount.Value;
            element.text.text = sLabelTxt;
            element.itemId = itemStatus.ID;
            element.button.onClick.RemoveAllListeners();
            int index = iElement;
            element.button.onClick.AddListener(() =>
                {
                    Debug.Log("select" + index);
                    SelectInventryItem(index);
                });

            iElement++;
        }
        while (m_iSelectInventryIndex >= iElement)
        {
            // インベントリのアイテム種類数より選択インデックスが大きかった場合
            m_iSelectInventryIndex--;
        }
        // 再度選択
        SelectInventryItem(m_iSelectInventryIndex);

        for (; iElement < m_lsInventryElements.Count; ++iElement)
        {
            // WARNING:ここ既にアクティブかどうかチェックした方が、動作軽くなるかも
            m_lsInventryElements[iElement].gameObject.SetActive(false);
        }
    }

    private void SelectInventryItem(int index)
    {
        if (index < 0)
        {
            m_imgSelectingInventryUI.enabled = false;
            return;
        }
        m_imgSelectingInventryUI.enabled = true;

        m_iSelectInventryIndex = index;
        var element = m_lsInventryElements[index];
        m_sCurrentSelectItemId = element.itemId;

        var itemStatus = CItemDataMan.Instance.GetItemStatusById(m_sCurrentSelectItemId);

        m_imgSelectingInventryUI.transform.parent.GetComponent<RectTransform>()
            .localPosition = element.gameObject.GetComponent<RectTransform>().localPosition;

        switch (itemStatus.ItemType)
        {
            case eItemType.Food:
                m_txtSelectItemAction.transform.parent.gameObject.SetActive(true);
                m_txtSelectItemAction.text = "食べる";
                break;
            case eItemType.Weapon:
                m_txtSelectItemAction.transform.parent.gameObject.SetActive(true);
                m_txtSelectItemAction.text = "装備する";
                break;
            case eItemType.Facility:
                m_txtSelectItemAction.transform.parent.gameObject.SetActive(true);
                m_txtSelectItemAction.text = "設置する";
                break;
            case eItemType.NoUse:
                m_txtSelectItemAction.transform.parent.gameObject.SetActive(false);
                break;
        }
    }

    public void UseSelectedItem()
    {
        var itemStatus = CItemDataMan.Instance.GetItemStatusById(m_sCurrentSelectItemId);
        itemStatus.Use();
        m_dicItemHasCount[m_sCurrentSelectItemId]--;
        UpdateInventryUI();
        CPartyStatus.Instance.UpdatePartyText();
        CSituationStatus.Instance.UpdateSituationText();
    }
}
