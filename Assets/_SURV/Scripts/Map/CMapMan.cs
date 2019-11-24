using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CMapMan : CSingletonMonoBehaviour<CMapMan>
{
    [SerializeField]
    private GameObject m_oMapCellPrefab;

    [SerializeField]
    private GameObject m_oMapCellParent;

    private GameObject[][] m_poMapElement2D;

    private CMapData m_cMapData;

    public int WIDTH{ get; private set; }
    public int HEIGHT{ get; private set; }

    private GameObject m_oPartyCell;
    private Image m_imgPartyCell;

    private float m_fCellWidth;
    private float m_fCellHeight;

    public void Init(){

//        m_cMapData = Resources.Load<CMapData>("CMapData");
		m_cMapData = LoadMapData();

        // デバッグ的にランダム作成
        WIDTH = m_cMapData.map[0].Leength;
        HEIGHT = m_cMapData.map.Length;
//        m_cMapData.map = new int[HEIGHT,WIDTH];
//        int iStatusCount = CSituationStatus.Instance.GetLocationStatusCount();
//        for (int h = 0; h < HEIGHT; ++h) {
//            for (int w = 0; w < WIDTH; ++w) {
//                m_cMapData.map[h, w] = Random.Range(0, iStatusCount);
//            }
//        }

        var cellRT = m_oMapCellPrefab.GetComponent<RectTransform>();
        m_fCellWidth    = cellRT.rect.width;
        m_fCellHeight   = cellRT.rect.height;

        m_poMapElement2D = new GameObject[HEIGHT][];

        // UI生成
        for (int h = 0; h < m_cMapData.map.Length; ++h) {
            m_poMapElement2D[h] = new GameObject[WIDTH];
            for (int w = 0; w < m_cMapData.map[h].Leength; ++w) {
                var cMapCell = m_cMapData.map[h][w];
                int iLocationType = cMapCell.iLocationType;
                var oCell = Instantiate(m_oMapCellPrefab);
                var rt = oCell.GetComponent<RectTransform>();
                oCell.transform.SetParent(m_oMapCellParent.transform, false);
                rt.localPosition = new Vector3(m_fCellWidth * w, m_fCellHeight * h);

                // デバッグ的に色を変える
				var c = CSituationStatus.Instance.m_pcLocationStatus[iLocationType].debugMapColor;
				c.a *= 0.2f;
                oCell.GetComponentInChildren<Image>().color = c;
                oCell.GetComponentInChildren<Text>().text = "";
                m_poMapElement2D[h][w] = oCell;

                if(cMapCell.cFacility.type == eFacilityType.None)
                {
                    cMapCell.cFacility = null;
                }
            }
        }

        m_oPartyCell = Instantiate(m_oMapCellPrefab);
        m_oPartyCell.transform.SetParent(this.transform);
        m_oPartyCell.GetComponent<RectTransform>().localPosition = Vector3.zero;
        m_oPartyCell.GetComponentInChildren<Text>().text = "";
        m_imgPartyCell = m_oPartyCell.GetComponentInChildren<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var color = m_imgPartyCell.color;
        color.a = Mathf.PingPong(Time.time, 1f) * 0.5f;
        m_imgPartyCell.color = color;
    }

    public int GetMapType(int x, int y)
    {
		return m_cMapData.map[y][x].iLocationType;
    }

    public int GetEncountType(int x, int y)
    {
        return m_cMapData.map[y][x].iEncountType;
    }

    public CFacility GetMapFacility(int x, int y)
    {
        return m_cMapData.map[y][x].cFacility;
    }

    public void SetMapFacility(int x, int y, CFacility cFacility)
    {
        m_cMapData.map[y][x].cFacility = cFacility;

        Text cText = m_poMapElement2D[y][x].GetComponentInChildren<Text>();

        if(cFacility == null)
        {
            cText.text = "";
            return;
        }
        switch (cFacility.type)
        {
            case eFacilityType.Shelter:
                cText.text = "家";
                break;
            case eFacilityType.Bonfire:
                cText.text = "火";
                break;
        }
    }

    public void SetDispPartyPos(Vector2 partyPos){
        var rtParent = m_oMapCellParent.GetComponent<RectTransform>();
        rtParent.localPosition = partyPos * new Vector3(m_fCellWidth * -1, m_fCellHeight * -1, 0);
    }

    public bool CanMoveToPosition(Vector2 vPos)
    {
        var ivPos = new IntVector2(
            Mathf.RoundToInt(vPos.x),
            Mathf.RoundToInt(vPos.y));

        if( ivPos.y < 0                                 ||
            ivPos.y >= m_cMapData.map.Length            ||
            ivPos.x < 0                                 ||
            ivPos.x >= m_cMapData.map[ivPos.y].Leength)
        {
            return false;
        }

        var sLocationName = CSituationStatus.Instance.
            GetLocationName(m_cMapData.map[ivPos.y][ivPos.x].iLocationType);

        switch (sLocationName)
        {
            case "海":
                return false;
        }

        return true;
    }

	public static void SaveMapData(CMapData cMapData)
	{
		StreamWriter writer;

		string jsonstr = JsonUtility.ToJson (cMapData);

		writer = new StreamWriter("Assets/Resources/CMapData.json", false);
		writer.Write (jsonstr);
		writer.Flush ();
		writer.Close ();
	}

	public static CMapData LoadMapData()
	{
		string datastr = "";
		StreamReader reader;
		reader = new StreamReader ("Assets/Resources/CMapData.json");
		datastr = reader.ReadToEnd ();
		reader.Close ();

		return JsonUtility.FromJson<CMapData> (datastr);
	}
}
