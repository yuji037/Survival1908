using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CMapEditorWindow : EditorWindow
{
    private static CMapEditorWindow window;

    [MenuItem("SURV/マップエディタ")]
    private static void Open()
    {
        // 生成
        window = GetWindow<CMapEditorWindow>("マップエディタ");
        window.minSize = new Vector2(800f, 600f);
    }

    private CMapData            m_cMapData;
    private CLocationStatus[]   m_pcLocationStatus;
    private CNpcEncountStatus[] m_pcNpcEncountStatus;
    private float 				m_fCellSize 			= 25f;
    private Vector2             m_vCellTypeDispPos  = new Vector2(0f,90f);
    private Vector2             m_vMapDispPos           = new Vector2(90f,90f);
    private Vector2             m_vAlreadyEditPos       = new Vector2(-1,-1);
	private int 				m_iSelectLocationType 	= 0;
	private int 				m_iSelectEncountType 	= 0;

    private int                 m_iSelectMapEditMode = 0;
    private enum eMapEditMode
    {
        Location,
        Encount,
    }

    private void OnEnable(){
        m_pcLocationStatus      = Resources.Load<CLocationData>("CLocationData").locationStatusList;
        m_pcNpcEncountStatus    = Resources.Load<CNpcEncountData>("CNpcEncountData").npcEncountStatusList;
        m_cMapData = CMapMan.LoadMapData();
    }

    private void OnGUI(){
        wantsMouseMove = true;

		if (GUILayout.Button("Init", GUILayout.MaxWidth(100f)) &&
			EditorUtility.DisplayDialog("警告", "新規作成してもよろしいですか？", "OK", "Cancel")) {

			m_cMapData = new CMapData();
			m_cMapData.map = new CMapCellArray[20];
			for (int i = 0; i < m_cMapData.map.Length; ++i) {
				m_cMapData.map[i] = new CMapCellArray();
				m_cMapData.map[i].data = new CMapCell[20];
			}
			CMapMan.SaveMapData(m_cMapData);
		}

        if (m_cMapData == null)
            return;

		if (GUILayout.Button("Save")){
			CMapMan.SaveMapData(m_cMapData);
		}

		if (m_cMapData.map == null)
			return;

        m_iSelectMapEditMode =
            GUILayout.Toolbar(m_iSelectMapEditMode, System.Enum.GetNames(typeof(eMapEditMode)));

        // 選択用パレット
        switch ((eMapEditMode)m_iSelectMapEditMode)
        {
            case eMapEditMode.Location:
                for (int i = 0; i < m_pcLocationStatus.Length; ++i)
                {
                    Color color = m_pcLocationStatus[i].debugMapColor;

                    EditorGUI.DrawRect(
                        new Rect(
                            m_vCellTypeDispPos + new Vector2(0f, i * m_fCellSize),
                            new Vector2(m_fCellSize, m_fCellSize)),
                        color);
                }
                break;
            case eMapEditMode.Encount:
                for (int i = 0; i < m_pcNpcEncountStatus.Length; ++i)
                {
                    Color color = m_pcNpcEncountStatus[i].debugMapColor;

                    EditorGUI.DrawRect(
                        new Rect(
                            m_vCellTypeDispPos + new Vector2(0f, i * m_fCellSize),
                            new Vector2(m_fCellSize, m_fCellSize)),
                        color);
                }
                break;
        }

        // 選択した色表示
        {

            Color color = Color.white;
            string label = "";

            switch ((eMapEditMode)m_iSelectMapEditMode)
            {
                case eMapEditMode.Location:
                    color = m_pcLocationStatus[m_iSelectLocationType].debugMapColor;
                    break;
                case eMapEditMode.Encount:
                    color = m_pcNpcEncountStatus[m_iSelectEncountType].debugMapColor;
                    break;
            }
            EditorGUI.DrawRect(
                new Rect(
                    new Vector2(45f, 90f),
                    new Vector2(m_fCellSize, m_fCellSize)), color);
        }

		// マップ表示
		for (int h = 0; h < m_cMapData.map.Length; ++h) {
			for (int w = 0; w < m_cMapData.map[h].Leength; ++w) {
                var color = m_pcLocationStatus[m_cMapData.map[h][w].iLocationType].debugMapColor;
                switch ((eMapEditMode)m_iSelectMapEditMode)
                {
                    case eMapEditMode.Encount:
                        var fRate = Mathf.PingPong((float)EditorApplication.timeSinceStartup, 1.5f) * 1.5f;
                        fRate = Mathf.Clamp(fRate, 0f, 0.9f);
                        color =
                            color * (1f - fRate) +
                            m_pcNpcEncountStatus[m_cMapData.map[h][w].iEncountType].debugMapColor * fRate;
                        break;
                }
                EditorGUI.DrawRect(
                    new Rect(
                        m_vMapDispPos + new Vector2(w * m_fCellSize, (m_cMapData.map.Length - 1 - h) * m_fCellSize),
                        new Vector2(m_fCellSize, m_fCellSize)), color);
            }
        }

        using (new GUILayout.HorizontalScope()) {
//            if (GUILayout.Button("test")) {
//
//            }
        }
        var e = Event.current;
        if (e.isMouse ){
			if (e.type == EventType.MouseDown) {
				SelectCellType(Event.current.mousePosition);
				EditMapPosition(Event.current.mousePosition);
			}
			else if (e.type == EventType.MouseDrag) {
				EditMapPosition(Event.current.mousePosition);
			}
            else if (e.type == EventType.MouseUp) {
                m_vAlreadyEditPos = new Vector2(-1, -1);
            }
        }

    }

    private void Update(){
        Repaint();
    }

    private void SelectCellType(Vector2 vPos){
		vPos -= m_vCellTypeDispPos;
		vPos /= m_fCellSize;

		int w = Mathf.FloorToInt(vPos.x);
		int h = Mathf.FloorToInt(vPos.y);

        switch ((eMapEditMode)m_iSelectMapEditMode)
        {
            case eMapEditMode.Location:
                if (h < 0 || h >= m_pcLocationStatus.Length ||
                    w < 0 || w >= 1)
                    return;
                m_iSelectLocationType = h;
                break;
            case eMapEditMode.Encount:
                if (h < 0 || h >= m_pcNpcEncountStatus.Length ||
                    w < 0 || w >= 1)
                    return;
                m_iSelectEncountType = h;
                break;
        }
    }

    private void EditMapPosition(Vector2 vPos){
		vPos -= m_vMapDispPos;
        vPos /= m_fCellSize;

        int w = Mathf.FloorToInt(vPos.x);
        int h = m_cMapData.map.Length - 1 - Mathf.FloorToInt(vPos.y);

		if (h < 0 || h >= m_cMapData.map.Length ||
			w < 0 || w >= m_cMapData.map[0].Leength)
            return;

        var vEditPos = new Vector2(w, h);
        //if (m_vAlreadyEditPos == vEditPos)
        //    return;
        m_vAlreadyEditPos = vEditPos;

        switch ((eMapEditMode)m_iSelectMapEditMode)
        {
            case eMapEditMode.Location:
                m_cMapData.map[h][w].iLocationType = m_iSelectLocationType;
                break;
            case eMapEditMode.Encount:
                m_cMapData.map[h][w].iEncountType = m_iSelectEncountType;
                break;
        }
    }
}
