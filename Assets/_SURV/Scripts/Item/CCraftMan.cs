using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCraftMan : CSingletonMonoBehaviour<CCraftMan>
{
    [SerializeField]
    private GameObject m_oWindow;

    private CCraftStatus[]                      m_pcCraftStatus;
    private Dictionary<string, CCraftStatus>    m_dicCraftStatus = new Dictionary<string, CCraftStatus>();

    [SerializeField]
    private GameObject      m_oCraftCellParent;
    [SerializeField]
    private GameObject      m_oCraftCellElement;

    private List<CItemUIElement> m_lsCraftElements = new List<CItemUIElement>();

    [SerializeField]
    private Image           m_imgSelectingCraftUI;

    private string          m_sCurrentSelectItemId;

    [SerializeField]
    private Button          m_buttonCraftAction;
    [SerializeField]
    private Text            m_txtSelectCraftAction;

    private int             m_iSelectCraftIndex;

    [SerializeField]
    private Text            m_txtCraftDesctiption;

    // Start is called before the first frame update
    void Start()
    {
        m_oCraftCellElement.SetActive(false);
        DispWindow(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DispWindow(bool bDisp)
    {
        m_oWindow.SetActive(bDisp);
        if (bDisp)
        {
            UpdateCraftUI();
        }
    }

    public void UpdateCraftUI()
    {
        if(m_pcCraftStatus == null)
        {
            m_pcCraftStatus = Resources.Load<CCraftData>("CCraftData").craftStatusList;
            foreach(var status in m_pcCraftStatus)
            {
                m_dicCraftStatus[status.dstItemUnit.itemID] = status;
            }
        }

        int iElement = 0;
        foreach (var cs in m_pcCraftStatus)
        {
            // 表示しない条件
            var isDisp = false;
            //var cFacility = CSituationStatus.Instance.GetCurrentFacility();
            //switch (cs.craftConditionType)
            //{
            //    case eCraftConditionType.None:
            //        isDisp = true;
            //        break;
            //    case eCraftConditionType.Bonfire:
            //        if( cFacility != null &&
            //            cFacility.type == eFacilityType.Bonfire)
            //        {
            //            isDisp = true;
            //        }
            //        break;

            //}
            if (false == isDisp)
            {
                continue;
            }

            CItemUIElement element = null;
            if (iElement >= m_lsCraftElements.Count)
            {
                element = new CItemUIElement();
                var obj = Instantiate(m_oCraftCellElement);
                element.gameObject = obj;
                obj.transform.SetParent(m_oCraftCellParent.transform, false);
                element.text    = obj.GetComponentInChildren<Text>();
                element.button  = obj.GetComponentInChildren<Button>();
                m_lsCraftElements.Add(element);
                // WARNING:ここ既にアクティブかどうかチェックした方が、動作軽くなるかも
                element.gameObject.SetActive(true);
            }
            else
            {
                element = m_lsCraftElements[iElement];
                // WARNING:ここ既にアクティブかどうかチェックした方が、動作軽くなるかも
                element.gameObject.SetActive(true);
            }

            var sLabelTxt = cs.dstItemUnit.itemName;
            if (cs.dstItemUnit.count >= 2)
                sLabelTxt += " ×" + cs.dstItemUnit.count;
            element.text.text = sLabelTxt;
            element.itemId = cs.dstItemUnit.itemID;
            element.button.onClick.RemoveAllListeners();
            int index = iElement;
            element.button.onClick.AddListener(() =>
            {
                SelectCraftItem(index);
            });

            iElement++;
        }
        while (m_iSelectCraftIndex >= iElement)
        {
            // インベントリのアイテム種類数より選択インデックスが大きかった場合
            m_iSelectCraftIndex--;
        }
        // 再度選択
        SelectCraftItem(m_iSelectCraftIndex);

        for (; iElement < m_lsCraftElements.Count; ++iElement)
        {
            // WARNING:ここ既にアクティブかどうかチェックした方が、動作軽くなるかも
            m_lsCraftElements[iElement].gameObject.SetActive(false);
        }
    }

    private void SelectCraftItem(int index)
    {
        if (index < 0)
        {
            m_imgSelectingCraftUI.enabled = false;
            return;
        }
        m_imgSelectingCraftUI.enabled = true;

        m_iSelectCraftIndex = index;
        var element = m_lsCraftElements[index];

        // 何を選択したか記憶
        m_sCurrentSelectItemId = element.itemId;

        var craftStatus = m_dicCraftStatus[m_sCurrentSelectItemId];

		//m_imgSelectingCraftUI.transform.parent.GetComponent<RectTransform>()
		//    .localPosition = element.gameObject.GetComponent<RectTransform>().localPosition;
		var selectingUiTransform = m_imgSelectingCraftUI.transform.parent.GetComponent<RectTransform>();
		selectingUiTransform.SetParent(element.gameObject.transform);
		selectingUiTransform.position = element.gameObject.transform.position;

		bool canCreate = true;
        m_txtCraftDesctiption.text = "必要アイテム\n";
        foreach(var srcUnit in craftStatus.srcItemUnitList)
        {
            m_txtCraftDesctiption.text += srcUnit.itemName + " x" + srcUnit.count + "\n";
            // 必要アイテム数チェック
            if (CInventryMan.Instance.GetHasItemCount(srcUnit.itemID) < srcUnit.count)
            {
                canCreate = false;
            }
        }

        m_buttonCraftAction.interactable = canCreate;

        //m_txtSelectCraftAction.text = "食べる";
    }

    public void CraftSelectedItem()
    {
        // 必要アイテム消費
        foreach (var srcUnit in m_dicCraftStatus[m_sCurrentSelectItemId].srcItemUnitList)
        {
            CInventryMan.Instance.ManipulateItemCount(srcUnit.itemID, -1 * srcUnit.count);
        }

        // 製作アイテム増加
        CInventryMan.Instance.ManipulateItemCount(
            m_dicCraftStatus[m_sCurrentSelectItemId].dstItemUnit.itemID,
            m_dicCraftStatus[m_sCurrentSelectItemId].dstItemUnit.count);

        UpdateCraftUI();
    }
}
