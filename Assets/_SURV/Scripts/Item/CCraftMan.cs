using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCraftMan : CSingletonMonoBehaviour<CCraftMan>
{
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
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateCraftUI()
    {
        if(m_pcCraftStatus == null)
        {
            m_pcCraftStatus = Resources.Load<CCraftData>("CCraftData").m_pcCraftStatus;
            foreach(var status in m_pcCraftStatus)
            {
                m_dicCraftStatus[status.DstItemUnit.ItemID] = status;
            }
        }

        int iElement = 0;
        foreach (var cs in m_pcCraftStatus)
        {
            // 表示しない条件
            //if (craftStatus.Value == 0)
            //    continue;

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

            var sLabelTxt = cs.DstItemUnit.ItemName;
            if (cs.DstItemUnit.Count >= 2)
                sLabelTxt += " ×" + cs.DstItemUnit.Count;
            element.text.text = sLabelTxt;
            element.itemId = cs.DstItemUnit.ItemID;
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

    private void OnEnable()
    {
        Awake();
        UpdateCraftUI();
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

        m_imgSelectingCraftUI.transform.parent.GetComponent<RectTransform>()
            .localPosition = element.gameObject.GetComponent<RectTransform>().localPosition;

        bool canCreate = true;
        m_txtCraftDesctiption.text = "必要アイテム\n";
        foreach(var srcUnit in craftStatus.SrcItemUnitList)
        {
            m_txtCraftDesctiption.text += srcUnit.ItemName + " x" + srcUnit.Count + "\n";
            // 必要アイテム数チェック
            if (CInventryMan.Instance.GetHasItemCount(srcUnit.ItemID) < srcUnit.Count)
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
        foreach (var srcUnit in m_dicCraftStatus[m_sCurrentSelectItemId].SrcItemUnitList)
        {
            CInventryMan.Instance.GainItemCount(srcUnit.ItemID, -1 * srcUnit.Count);
        }

        // 製作アイテム増加
        CInventryMan.Instance.GainItemCount(
            m_dicCraftStatus[m_sCurrentSelectItemId].DstItemUnit.ItemID,
            m_dicCraftStatus[m_sCurrentSelectItemId].DstItemUnit.Count);

        UpdateCraftUI();
    }
}
