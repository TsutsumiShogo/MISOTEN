using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM_MathManager : MonoBehaviour {

    //定数定義
    public int[] MATH_EXP_MAX = new int[2];
    public int ADD_GROUWTH_POINT = 2;

    //セルオブジェクト達
    public List<GM_MathCell> cells;

    //内部変数
    int lastUpdateCellNo = 0;
    float timeCount = 0.0f;

	// Use this for initialization
	void Start () {
        //初期化関数
        Init();
	}

    //初期化
    void Init()
    {
        //各セルを通して初期化を伝達
        for (int i = 0; i < cells.Count; ++i)
        {
            cells[i].Init();
        }

        //内部変数初期化
        lastUpdateCellNo = 0;
        timeCount = 0.0f;
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
