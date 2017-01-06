using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM_MiniMapManager : MonoBehaviour {

    //定数定義
    public int[] cameraPoint = new int[3];

    //オブジェクト
    [SerializeField]
    private GameObject MINIMAP_CELL_PREFAB;     //動的生成させたいので
    [SerializeField]
    private GameObject cameraObject;            //ズームアウト処理を掛けたい。
    private GM_MiniMapPlayers minimapPlayer;    //ミニマップのプレイヤー管理部


    //外部公開変数
    public GM_MathManager mathManager;              //マスマネージャ
    public List<GM_MiniMapCell> minimapCellList;    //ミニマップのセルのリスト
    
    //内部変数
    private int nowCameraPointNo;
    private int updateCellNo;       //更新対象セル番号


	// Use this for initialization
	void Awake () {
        mathManager = GameObject.Find("SceneChangeManager").transform.Find("GameMainObjects/Stage/MathManager").GetComponent<GM_MathManager>();
        minimapPlayer = transform.FindChild("Map_Players").GetComponent<GM_MiniMapPlayers>();	
    }
    //マスマネージャーのInitが呼ばれた後に呼んで下さい。
    public void Init()
    {
        //カメラ位置を初期位置へ
        Vector3 camPos = cameraObject.transform.position;
        camPos.y = cameraPoint[0];
        cameraObject.transform.position = camPos;
        nowCameraPointNo = 0;

        Transform mapCellListObj = transform.FindChild("Map_CellList");

        //更新セル番号を初期化
        updateCellNo = 0;

        //ミニマッププレイヤー初期化
        minimapPlayer.Init(GM_StaticParam.g_selectCharacter[0], GM_StaticParam.g_selectCharacter[1], GM_StaticParam.g_selectCharacter[2]);

        GameObject temp;
        for (int i = 0; i < mathManager.cells.Count; ++i)
        {
            //ミニマップ用セル作成
            temp = Instantiate(MINIMAP_CELL_PREFAB);

            //セルの親を決める
            temp.transform.parent = mapCellListObj;

            //生成したミニマップセルの位置を修正
            temp.transform.position = mathManager.cells[i].transform.position + transform.position;

            //ミニマップセルリストに追加
            minimapCellList.Add(temp.GetComponent<GM_MiniMapCell>());

            //ミニマップセルにステージ番号とセルタイプを伝達
            minimapCellList[i].stageCellObj = mathManager.cells[i];

            //セルタイプ別でミニマップマスを作成してもらう
            minimapCellList[i].CreateMath();

        }

        //ミニマップセルリストに初期化を伝達
        for (int i = 0; i < minimapCellList.Count; ++i)
        {
            minimapCellList[i].Init();
        }

        //最初のステージを開始
        StartStage(GM_MathManager.EMathStageNo.STAGE1);

    }
    public void Delete()
    {
        //ミニマップのすべてのセルとマスを削除
        for (int i = 0; i < minimapCellList.Count; ++i)
        {
            Destroy(minimapCellList[i].gameObject);
        }
        minimapCellList.Clear();
    }
	
	// Update is called once per frame
	void Update () {
        //同時セル更新数
        for (int i = 0; i < 3; ++i)
        {
            //更新対象のセルを決める
            updateCellNo++;
            if (updateCellNo >= minimapCellList.Count)
            {
                updateCellNo = 0;
            }

            //セルの更新
            minimapCellList[updateCellNo].UpdateMath();
        }

        //カメラ位置更新
        Vector3 camPos;     //計算用位置情報
        float distance;     //目的地までの距離
        camPos = cameraObject.transform.position;
        distance = cameraPoint[nowCameraPointNo] - camPos.y;
        camPos.y += distance * 0.1f;
        cameraObject.transform.position = camPos;
	}

    //=============================公開関数=====================================
    //指定のステージ番号のセルを有効化
    public void StartStage(GM_MathManager.EMathStageNo stageNo)
    {
        for (int i = 0; i < minimapCellList.Count; ++i)
        {
            if (minimapCellList[i].stageCellObj.stageNo == stageNo)
            {
                minimapCellList[i].CellStart();
            }
        }

        nowCameraPointNo = (int)stageNo;
    }
}
