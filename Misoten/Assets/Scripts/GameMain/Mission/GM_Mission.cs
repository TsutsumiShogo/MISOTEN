using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM_Mission : MonoBehaviour {

    //定数定義
    public enum EMissionType
    {
        FLOWER_GROWTH_MISSION,  //花成長ミッション
        FLOWER_COLOR_MISSTION,  //花色変更ミッション
        BILL_GROWTH_MISSION,    //ビル成長ミッション
        BIGBILL_GROWTH_MISSION, //大ビル成長ミッション
    }

    //オブジェクト
    [SerializeField]
    private GameObject MISSION_EFFECT_PREFAB;   //Unity上でセット

    //変数宣言
    private GM_MissionManager manager;
    private List<GM_MathCell> cellColList = new List<GM_MathCell>();                //ミッションエリアにいる全てのセル
    private List<GM_MathFlowerParam> mathColList = new List<GM_MathFlowerParam>();  //ミッションエリアにいる全てのマス

    //公開変数
    public EMissionType missionType;    //ミッションの種類
    public float startTime;             //ミッション開始までの時間
    public float timeCountDown;         //ミッションの残り時間
    public GM_MathFlowerParam.EFlowerColor clearColor;  //色ミッションのみ有効：クリアの色

	//ミッションの初期化
    public void Init(float _timeLimit, EMissionType _type)
    {
        //親のマネージャー取得
        manager = transform.parent.GetComponent<GM_MissionManager>();

        //パラメータ初期化
        missionType = _type;
        startTime = 1.0f;
        timeCountDown = _timeLimit;
        clearColor = GM_MathFlowerParam.EFlowerColor.WHITE;

    }
	public void Init (float _timeLimit, EMissionType _type, GM_MathFlowerParam.EFlowerColor _clearColor) {
        //通常の初期化
        Init(_timeLimit, _type);
        //色を指定
        clearColor = _clearColor;
	}
	
	// Update is called once per frame
	void Update () {
        //ミッション開始前なら何もしない
        if (startTime > 0.0f)
        {
            startTime -= Time.deltaTime;
            return;
        }

        //ミッションの残り時間を減少させる
        timeCountDown -= Time.deltaTime;
        //時間切れなら失敗信号を送る
        if (timeCountDown < 0.0f)
        {
            manager.FailedSignal();
            return;
        }

        //ミッションのクリア状況をチェック
        if (CheckClearMission(missionType) == true)
        {
            //クリアなら成功シグナルを送る
            manager.SuccessSignal();
        }
	}

    private bool CheckClearMission(EMissionType _type)
    {
        //各ミッションの達成状況をチェック
        switch (_type)
        {
            case EMissionType.FLOWER_GROWTH_MISSION:
                //全検索
                for (int i = 0; i < mathColList.Count; ++i)
                {
                    //ビルを除く
                    if (mathColList[i].flowerType == GM_MathFlowerParam.EFlowerType.Flower1 ||
                        mathColList[i].flowerType == GM_MathFlowerParam.EFlowerType.Flower2 ||
                        mathColList[i].flowerType == GM_MathFlowerParam.EFlowerType.Flower3)
                    {
                        //レベル最大じゃなければ失敗
                        if (mathColList[i].flowerLevel != GM_MathFlowerParam.EFlowerLevel.Level3)
                        {
                            return false;
                        }
                    }
                }
                //ここまで来たらミッションクリア
                return true;
            case EMissionType.FLOWER_COLOR_MISSTION:
                //まず全ての花が最大レベルかチェック
                if (CheckClearMission(EMissionType.FLOWER_GROWTH_MISSION) == false)
                {
                    //全ての花が最大レベルで無かったら失敗確定
                    return false;
                }

                //全検索
                for (int i = 0; i < mathColList.Count; ++i)
                {
                    //ビルを除く
                    if (mathColList[i].flowerType == GM_MathFlowerParam.EFlowerType.Flower1 ||
                        mathColList[i].flowerType == GM_MathFlowerParam.EFlowerType.Flower2 ||
                        mathColList[i].flowerType == GM_MathFlowerParam.EFlowerType.Flower3)
                    {
                        //色が指定の色じゃなければ失敗
                        if (mathColList[i].flowerColor != clearColor)
                        {
                            return false;
                        }
                    }
                }
                //ここまで来たらミッションクリア
                return true;
            case EMissionType.BILL_GROWTH_MISSION:
                //全検索
                for (int i = 0; i < mathColList.Count; ++i)
                {
                    //中ビルのみをチェック
                    if (mathColList[i].flowerType == GM_MathFlowerParam.EFlowerType.Bill)
                    {
                        //レベル最大じゃなければ失敗
                        if (mathColList[i].flowerLevel != GM_MathFlowerParam.EFlowerLevel.Level3)
                        {
                            return false;
                        }
                    }
                }
                //ここまで来たらミッションクリア
                return true;
            case EMissionType.BIGBILL_GROWTH_MISSION:
                //全検索
                for (int i = 0; i < mathColList.Count; ++i)
                {
                    //大ビルのみをチェック
                    if (mathColList[i].flowerType == GM_MathFlowerParam.EFlowerType.BigBill)
                    {
                        //レベル最大じゃなければ失敗
                        if (mathColList[i].flowerLevel != GM_MathFlowerParam.EFlowerLevel.Level3)
                        {
                            return false;
                        }
                    }
                }
                //ここまで来たらミッションクリア
                return true;
        }

        //指定外のミッション番号が来たので強制的に失敗判定
        return false;
    }


    void OnTriggerEnter(Collider col)
    {
        GM_MathCell tempCell;
        GameObject tempObj;
        //セルオブジェクトだったらリストに追加
        tempCell = col.gameObject.GetComponent<GM_MathCell>();
        if (tempCell)
        {
            cellColList.Add(tempCell);
            
            //セルオブジェクト内にあるマスリストをコピー
            for (int i = 0; i < tempCell.flowerParams.Count; ++i)
            {
                mathColList.Add(tempCell.flowerParams[i]);
            }

            //エフェクトオブジェクト追加
            switch (missionType) 
            {
                case EMissionType.FLOWER_GROWTH_MISSION:
                    if (tempCell.cellType != GM_MathCell.ECellType.CELL_FLOWER && tempCell.cellType != GM_MathCell.ECellType.CELL_HOUSE)
                    {
                        return;
                    }
                    break;
                case EMissionType.FLOWER_COLOR_MISSTION:
                    if (tempCell.cellType != GM_MathCell.ECellType.CELL_FLOWER && tempCell.cellType != GM_MathCell.ECellType.CELL_HOUSE)
                    {
                        return;
                    }
                    break;
                case EMissionType.BILL_GROWTH_MISSION:
                    if (tempCell.cellType != GM_MathCell.ECellType.CELL_BILL)
                    {
                        return;
                    }
                    break;
                case EMissionType.BIGBILL_GROWTH_MISSION:
                    if (tempCell.cellType != GM_MathCell.ECellType.CELL_BIGBILL)
                    {
                        return;
                    }
                    break;
            }
            tempObj = Instantiate(MISSION_EFFECT_PREFAB);
            tempObj.transform.position = tempCell.transform.position;
            tempObj.transform.parent = transform;
        }
    }

}
