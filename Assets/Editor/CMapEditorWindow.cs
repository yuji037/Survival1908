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
    }

    private CMapData            m_cMapData;
    private CLocationStatus[]   m_pcLocationStatus;
	private float 				m_fCellSize 			= 25f;
    private Vector2             m_vLocationTypeDispPos  = new Vector2(0f,60f);
    private Vector2             m_vMapDispPos           = new Vector2(90f,60f);
    private Vector2             m_vAlreadyEditPos       = new Vector2(-1,-1);
	private int 				m_iSelectLocationType 	= 0;


    private void OnEnable(){
        m_pcLocationStatus = Resources.Load<CLocationData>("CLocationData").m_pcLocationStatus;
		m_cMapData = CMapMan.LoadMapData();
    }

    private void OnGUI(){
        wantsMouseMove = true;

		if (GUILayout.Button("Init", GUILayout.MaxWidth(100f)) &&
			EditorUtility.DisplayDialog("警告", "新規作成してもよろしいですか？", "OK", "Cancel")) {

			m_cMapData = new CMapData();
			m_cMapData.map = new CMapCellArray[10];
			for (int i = 0; i < m_cMapData.map.Length; ++i) {
				m_cMapData.map[i] = new CMapCellArray();
				m_cMapData.map[i].data = new CMapCell[10];
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

		// 選択用パレット
		for (int i = 0; i < m_pcLocationStatus.Length; ++i) {
			EditorGUI.DrawRect(
				new Rect(
					m_vLocationTypeDispPos + new Vector2(0f, i * m_fCellSize),
					new Vector2(m_fCellSize, m_fCellSize)),
				m_pcLocationStatus[i].DebugMapColor);
		}

		// 選択した色表示
		EditorGUI.DrawRect(
			new Rect(
				new Vector2(45f, 60f),
				new Vector2(m_fCellSize, m_fCellSize)),
			m_pcLocationStatus[m_iSelectLocationType].DebugMapColor);

		// マップ表示
		for (int h = 0; h < m_cMapData.map.Length; ++h) {
			for (int w = 0; w < m_cMapData.map[h].Leength; ++w) {
                EditorGUI.DrawRect(
                    new Rect(
                        m_vMapDispPos + new Vector2(w * m_fCellSize, (m_cMapData.map.Length - 1 - h) * m_fCellSize),
                        new Vector2(m_fCellSize, m_fCellSize)),
                    m_pcLocationStatus[m_cMapData.map[h][w].iLocationType].DebugMapColor +
                    Color.red * Mathf.PingPong((float)EditorApplication.timeSinceStartup, 1f));
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
				SelectLocationType(Event.current.mousePosition);
				EditLocation(Event.current.mousePosition);
			}
			else if (e.type == EventType.MouseDrag) {
				EditLocation(Event.current.mousePosition);
			}
            else if (e.type == EventType.MouseUp) {
                m_vAlreadyEditPos = new Vector2(-1, -1);
            }
        }

    }

    private void Update(){
        Repaint();
    }

    private void SelectLocationType(Vector2 vPos){
		vPos -= m_vLocationTypeDispPos;
		vPos /= m_fCellSize;

		int w = Mathf.FloorToInt(vPos.x);
		int h = Mathf.FloorToInt(vPos.y);

		if (h < 0 || h >= m_pcLocationStatus.Length ||
			w < 0 || w >= 1)
			return;

		m_iSelectLocationType = h;
    }

    private void EditLocation(Vector2 vPos){
		vPos -= m_vMapDispPos;
        vPos /= m_fCellSize;

        int w = Mathf.FloorToInt(vPos.x);
        int h = m_cMapData.map.Length - 1 - Mathf.FloorToInt(vPos.y);

		if (h < 0 || h >= m_cMapData.map.Length ||
			w < 0 || w >= m_cMapData.map[0].Leength)
            return;

        var vEditPos = new Vector2(w, h);
        if (m_vAlreadyEditPos == vEditPos)
            return;
        m_vAlreadyEditPos = vEditPos;

		m_cMapData.map[h][w].iLocationType = m_iSelectLocationType;
    }

}
