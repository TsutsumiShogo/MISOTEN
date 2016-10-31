using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM_MathManager : MonoBehaviour {

    //定数定義
    public enum EMathStageNo
    {
        STAGE1,
        STAGE2,
        STAGE3,
    };

    //公開変数
    public int[] MATH_EXP_MAX = new int[2];
    public int ADD_GROUWTH_POINT = 2;

    //セルオブジェクト達
    public List<GM_MathCell> cells;

    //内部変数
    int lastUpdateCellNo = 0;
    float timeCount = 0.0f;


    //初期化
    public void Init()
    {
        //各セルを初期化
        for (int i = 0; i < cells.Count; ++i)
        {
            cells[i].Init();
        }

        //内部変数初期化
        lastUpdateCellNo = 0;
        timeCount = 0.0f;

        //STAGE2,3のセルを無効化し、STAGE1のセルを利用開始状態へ
        for (int i = 0; i < cells.Count; ++i)
        {
            if (cells[i].stageNo == EMathStageNo.STAGE2 || cells[i].stageNo == EMathStageNo.STAGE3)
            {
                cells[i].gameObject.SetActive(false);
            }
            else
            {
                //利用開始
                cells[i].CellStart();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	    //ローカル変数定義
        bool timeCountResetFlg = false;

        //内部時間を経過させる
        timeCount += Time.deltaTime;
        if (timeCount > 1.0f)
        {
            timeCount -= 1.0f;
            timeCountResetFlg = true;
        }

        //植物の自然成長処理
        PrantGrowthProcess(timeCountResetFlg);

	}

    //=============================公開関数=====================================
    public void StartStage(EMathStageNo stageNo)
    {
        //STAGE2または3のセルを有効化する
        for (int i = 0; i < cells.Count; ++i)
        {
            if (cells[i].stageNo == stageNo)
            {
                cells[i].gameObject.SetActive(true);
                cells[i].CellStart();
            }
        }
    }


    //===========================非公開関数=====================================
    //植物の自然成長処理
    private void PrantGrowthProcess(bool timeCountResetFlg)
    {
        int endCellNo = lastUpdateCellNo + 2;

        //最後の番地を超過しないように
        if (endCellNo > cells.Count)
        {
            endCellNo = cells.Count;
        }

        //タイムカウントがリセットされていたら最後の番地まで強制指定
        if (timeCountResetFlg == true)
        {
            endCellNo = cells.Count;
        }

        //自然成長を一定時間毎に動作させる
        for (; lastUpdateCellNo < endCellNo; ++lastUpdateCellNo)
        {
            //自然成長伝達
            cells[lastUpdateCellNo].PrantGrowth(ADD_GROUWTH_POINT);
        }

        //タイムカウントがリセットされていたら更新番号を０へ戻す
        if (timeCountResetFlg == true)
        {
            lastUpdateCellNo = 0;
        }
    }
}
