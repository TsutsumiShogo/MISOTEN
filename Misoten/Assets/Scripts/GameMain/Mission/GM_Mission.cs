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

    //変数宣言
    private GM_MissionManager manager;
    private List<GM_MathFlowerParam> mathColList;   //ミッションエリアにいる全てのマス

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
        //ミッションの達成状況をチェック
        int mathCount = 0;
        int clearCount = 0;
        switch (missionType)
        {
            case EMissionType.FLOWER_GROWTH_MISSION:
                //レベル最大の花をカウント
                for (int i = 0; i < mathColList.Count; ++i)
                {
                    if (mathColList[i].flowerType == GM_MathFlowerParam.EFlowerType.Flower1 ||
                        mathColList[i].flowerType == GM_MathFlowerParam.EFlowerType.Flower2 ||
                        mathColList[i].flowerType == GM_MathFlowerParam.EFlowerType.Flower3)
                    {
                        mathCount++;
                        if (mathColList[i].flowerLevel != GM_MathFlowerParam.EFlowerLevel.Level3)
                        {
                            break;
                        }
                        clearCount++;
                    }
                }
                //クリア条件の確認
                if (clearCount >= mathCount)
                {
                    return true;
                }

                break;
            case EMissionType.FLOWER_COLOR_MISSTION:
                break;
            case EMissionType.BILL_GROWTH_MISSION:

                break;
            case EMissionType.BIGBILL_GROWTH_MISSION:
                break;
        }

        return false;
    }


    void OnTriggerEnter(Collider col)
    {
        GM_MathFlowerParam tempParam;

        //花パラメータオブジェクトだったらリストに追加
        tempParam = col.gameObject.GetComponent<GM_MathFlowerParam>();
        if (tempParam)
        {
            mathColList.Add(tempParam);
        }
    }

}
