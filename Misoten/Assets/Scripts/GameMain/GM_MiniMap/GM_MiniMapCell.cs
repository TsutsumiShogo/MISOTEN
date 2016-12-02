using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM_MiniMapCell : MonoBehaviour {


    //オブジェクト
    [SerializeField]
    private GameObject MINIMAP_MATH_FLOWER_PREFAB;      //動的生成させたいので
    [SerializeField]
    private GameObject MINIMAP_MAHH_BILL_PREFAB;        //動的生成させたいので

    //ミニマップマネージャ
    private GM_MiniMapManager minimapManager;

    //このセルの下層のマスリスト
    public List<GM_MiniMapMath> minimapMathList;
    //マス目描画スクリプト
    public GM_MathCellColorControll mathCellColorCon;

    //同期対象のセル
    public GM_MathCell stageCellObj;

	// Use this for initialization
    void Awake()
    {
        //ミニマップマネージャを保存
        minimapManager = GameObject.Find("MiniMapManager").GetComponent<GM_MiniMapManager>();
        //セル枠描画スクリプトを保存
        mathCellColorCon = gameObject.GetComponentInChildren<GM_MathCellColorControll>();
    }
    //MiniMapManager.Init()から呼ばれる
	public void CreateMath () {
        GameObject temp;
	    //マスを作成する
        if (stageCellObj.cellType == GM_MathCell.ECellType.CELL_FLOWER || stageCellObj.cellType == GM_MathCell.ECellType.CELL_HOUSE)
        {
            for (int i = 0; i < 9; ++i)
            {
                temp = Instantiate(MINIMAP_MATH_FLOWER_PREFAB);
                temp.transform.parent = transform;
                temp.transform.position = transform.position + minimapManager.mathManager.mathPos[i] * minimapManager.mathManager.transform.localScale.x;
                temp.transform.localScale = temp.transform.localScale * minimapManager.mathManager.transform.localScale.x;
                //生成したマスをリストへ追加
                minimapMathList.Add(temp.GetComponent<GM_MiniMapMath>());

                //同期対象のマスを伝達
                minimapMathList[i].SetMathInfo(stageCellObj.math[i]);
            }
        }
        else//ビルマスを作成する
        {
            temp = Instantiate(MINIMAP_MAHH_BILL_PREFAB);
            temp.transform.parent = transform;
            temp.transform.position = transform.position;
            temp.transform.localScale = temp.transform.localScale * minimapManager.mathManager.transform.localScale.x;

            //生成したマスをリストへ追加
            minimapMathList.Add(temp.GetComponent<GM_MiniMapMath>());

            //同期対象のマスを伝達
            minimapMathList[0].SetMathInfo(stageCellObj.math[0]);
        }

        

	}
    public void Init()
    {
        //セル枠表示の初期化
        mathCellColorCon.Init();
        //マスの初期化
        for (int i = 0; i < minimapMathList.Count; ++i)
        {
            minimapMathList[i].Init(this);
        }
    }
    public void CellStart()
    {
        //セル枠を表示
        mathCellColorCon.ChangeMathColor(Color.white);
        //マスを表示
        for (int i = 0; i < minimapMathList.Count; ++i)
        {
            minimapMathList[i].MathStart();
        }
    }
	
    //このセルと下層のマスの更新
	public void UpdateMath () {
        for (int i = 0; i < minimapMathList.Count; ++i)
        {
            minimapMathList[i].UpdateMath();
        }
	}
}
