using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class CMapEditorWindow : EditorWindow
{
    private static CMapEditorWindow window;

    [MenuItem("SURV/マップエディタ")]
    private static void Open()
    {
        // 生成
        window = GetWindow<CMapEditorWindow>("マップエディタ");
        window.minSize = new Vector2(1400f, 600f);
    }

    private CMapData            mapData;
	private Sprite[]			mapChipTextures;
	private int					mapChipXLength		= 30;
	private int					mapChipYLength		= 16;
	private CLocationStatus[]   locationStatusList;
    private CNpcEncountStatus[] npcEncountStatusList;
    private float 				cellSize 			= 25f;
    private Vector2             cellTypeDispPos		= new Vector2(0f,90f);
    private Vector2             selectChipDispPos	= new Vector2(800f,90f);
	private Vector2             mapDispPos          = new Vector2(830f,90f);
    private Vector2             alreadyEditPos      = new Vector2(-1,-1);
	private int 				selectLocationType 	= 0;
	private int 				selectEncountType 	= 0;

    private int                 selectMapEditMode = 0;
    private enum eMapEditMode
    {
        Location,
        Encount,
    }

    private void OnEnable(){
        locationStatusList      = Resources.Load<CLocationData>("CLocationData").locationStatusList;
        npcEncountStatusList    = Resources.Load<CNpcEncountData>("CNpcEncountData").npcEncountStatusList;
        mapData = CMapMan.LoadMapData();
		var texObjs = AssetDatabase.LoadAllAssetsAtPath("Assets/_SURV/Images/20120219_4859043.png");
		mapChipTextures = new Sprite[texObjs.Length - 1];

		// 0番目は分割前画像なので除く
		for ( int i = 1; i < texObjs.Length; ++i )
		{
			mapChipTextures[i - 1] = texObjs[i] as Sprite;
		}

		//if ( mapChipTextures.Length == 0 )
		//	Debug.Log("texなし");
		//foreach(var tex in mapChipTextures )
		//{
		//	Debug.Log(tex.name);
		//}
    }

    private void OnGUI(){
        wantsMouseMove = true;

		if (GUILayout.Button("Init", GUILayout.MaxWidth(100f)) &&
			EditorUtility.DisplayDialog("警告", "新規作成してもよろしいですか？", "OK", "Cancel")) {

			mapData = new CMapData();
			mapData.map = new CMapCellArray[20];
			for (int i = 0; i < mapData.map.Length; ++i) {
				mapData.map[i] = new CMapCellArray();
				mapData.map[i].data = new CMapCell[20];
			}
			CMapMan.SaveMapData(mapData);
		}

        if (mapData == null)
            return;

		if (GUILayout.Button("Save")){
			CMapMan.SaveMapData(mapData);
		}

		if (mapData.map == null)
			return;

        selectMapEditMode =
            GUILayout.Toolbar(selectMapEditMode, System.Enum.GetNames(typeof(eMapEditMode)));

        // 選択用パレット
        switch ((eMapEditMode)selectMapEditMode)
        {
            case eMapEditMode.Location:
     //           for (int i = 0; i < locationStatusList.Length; ++i)
     //           {
     //               Color color = locationStatusList[i].debugMapColor;

					//EditorGUI.DrawRect(
     //                   new Rect(
     //                       cellTypeDispPos + new Vector2(0f, i * cellSize),
     //                       new Vector2(cellSize, cellSize)),
     //                   color);
     //           }
				for(int y = 0; y < mapChipYLength; ++y )
				{
					for(int x = 0; x < mapChipXLength; ++x )
					{
						DispMapChipTexture(
							x + y * mapChipXLength,
							cellTypeDispPos + new Vector2(x, y) * ( cellSize + 1 ), 
							cellSize);
					}
				}
                break;
            case eMapEditMode.Encount:
                for (int i = 0; i < npcEncountStatusList.Length; ++i)
                {
                    Color color = npcEncountStatusList[i].debugMapColor;

                    EditorGUI.DrawRect(
                        new Rect(
                            cellTypeDispPos + new Vector2(0f, i * cellSize),
                            new Vector2(cellSize, cellSize)),
                        color);
                }
                break;
        }

        // 選択した色表示
        {

            Color color = Color.white;
            string label = "";

            switch ((eMapEditMode)selectMapEditMode)
            {
                case eMapEditMode.Location:
                    //color = locationStatusList[selectLocationType].debugMapColor;
					DispMapChipTexture(selectLocationType, selectChipDispPos, cellSize);
                    break;
                case eMapEditMode.Encount:
                    color = npcEncountStatusList[selectEncountType].debugMapColor;
					EditorGUI.DrawRect(
						new Rect(
							selectChipDispPos,
							new Vector2(cellSize, cellSize)), color);
					break;
            }
        }

		// マップ表示
		for (int h = 0; h < mapData.map.Length; ++h) {
			for (int w = 0; w < mapData.map[h].Leength; ++w) {
				var color = Color.clear;
				//var color = locationStatusList[mapData.map[h][w].iLocationType].debugMapColor;
				var pos = mapDispPos + new Vector2(w * cellSize, ( mapData.map.Length - 1 - h ) * cellSize);
				DispMapChipTexture(mapData.map[h][w].iLocationType, pos, cellSize);
                switch ((eMapEditMode)selectMapEditMode)
                {
                    case eMapEditMode.Encount:
                        var fRate = Mathf.PingPong((float)EditorApplication.timeSinceStartup, 1.5f) * 1.5f;
                        fRate = Mathf.Clamp(fRate, 0f, 0.9f);
                        color =
                            color * (1f - fRate) +
                            npcEncountStatusList[mapData.map[h][w].iEncountType].debugMapColor * fRate;
                        break;
                }
                EditorGUI.DrawRect(
                    new Rect(
                        pos,
                        new Vector2(cellSize, cellSize)), color);
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
                alreadyEditPos = new Vector2(-1, -1);
            }
        }

    }

    private void Update(){
        Repaint();
    }

	private void DispMapChipTexture(int index, Vector2 position, float size)
	{
		// 描画領域.
		Rect boxRect = new Rect(position, new Vector2(size, size));

		// 対象のSprite.
		Sprite sprite = mapChipTextures[index];

		// spriteの親テクスチャー上のRect座標を取得.
		Rect rectPosition = sprite.textureRect;

		// 親テクスチャーの大きさを取得.
		float parentWith = sprite.texture.width;
		float parentHeight = sprite.texture.height;

		// spriteの座標を親テクスチャーに合わせて正規化.
		Rect normalRect = new Rect(
			rectPosition.x / parentWith,
			rectPosition.y / parentHeight,
			rectPosition.width / parentWith,
			rectPosition.height / parentHeight
			);

		// 描画.
		GUI.DrawTextureWithTexCoords(
			boxRect,
			sprite.texture,
			normalRect
			);
	}

    private void SelectCellType(Vector2 vPos){
		vPos -= cellTypeDispPos;
		vPos /= (cellSize + 1);

		int w = Mathf.FloorToInt(vPos.x);
		int h = Mathf.FloorToInt(vPos.y);

        switch ((eMapEditMode)selectMapEditMode)
        {
            case eMapEditMode.Location:
                if (h < 0 || h >= mapChipYLength ||
                    w < 0 || w >= mapChipXLength)
                    return;
                selectLocationType = w + h * mapChipXLength;
                break;
            case eMapEditMode.Encount:
                if (h < 0 || h >= npcEncountStatusList.Length ||
                    w < 0 || w >= 1)
                    return;
                selectEncountType = h;
                break;
        }
    }

    private void EditMapPosition(Vector2 vPos){
		vPos -= mapDispPos;
        vPos /= cellSize;

        int w = Mathf.FloorToInt(vPos.x);
        int h = mapData.map.Length - 1 - Mathf.FloorToInt(vPos.y);

		if (h < 0 || h >= mapData.map.Length ||
			w < 0 || w >= mapData.map[0].Leength)
            return;

        var vEditPos = new Vector2(w, h);
        //if (m_vAlreadyEditPos == vEditPos)
        //    return;
        alreadyEditPos = vEditPos;

        switch ((eMapEditMode)selectMapEditMode)
        {
            case eMapEditMode.Location:
                mapData.map[h][w].iLocationType = selectLocationType;
                break;
            case eMapEditMode.Encount:
                mapData.map[h][w].iEncountType = selectEncountType;
                break;
        }
    }
}
