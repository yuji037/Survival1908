using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CMapMan : CSingletonMonoBehaviour<CMapMan>
{
    [SerializeField]
    private GameObject m_oMapCellElement;

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
