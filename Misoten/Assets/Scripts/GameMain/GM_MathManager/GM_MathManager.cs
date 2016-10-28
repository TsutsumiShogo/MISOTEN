using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM_MathManager : MonoBehaviour {

    //定数定義
    public int[] MATH_EXP_MAX = new int[2];

    //セルオブジェクト達
    public List<GM_MathCell> cells;

    //内部変数
    int UPDATE_PARTITION_NUM = 10;
    int lastUpdatePartitionNo = 0;         //最後に更新したパーティション番号
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
        lastUpdatePartitionNo = 0;
        timeCount = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
	    //ローカル変数定義
        int updatePartitionNo = (int)((float)(UPDATE_PARTITION_NUM) * timeCount);

        //内部時間を経過させる
        timeCount += Time.deltaTime;
        if (timeCount > 1.0f) timeCount -= 1.0f;

        //自然成長を一定時間毎に動作させる
        for (int i = lastUpdatePartitionNo; i < updatePartitionNo; ++i)
        {
            //セルの分割数から更新対象を把握させる
            int cellNum = cells.Count / (UPDATE_PARTITION_NUM);
            int cellStartNo = cellNum * i;
            int cellEndNo = cellStartNo + cellNum;
            if (cellStartNo > cells.Count)
            {
                cellStartNo = cells.Count;
            }
            if (cellEndNo > cells.Count)
            {
                cellEndNo = cells.Count;
            }

            //最後のパーティション番号の時だけセル番号を最後に指定する
            if (i == updatePartitionNo - 1)
            {
                cellEndNo = cells.Count;
            }

            //自然成長伝達
            for (int j = cellStartNo; j < cellEndNo; ++j)
            {
                cells[j].PrantGrowth(1);
            }
        }

	}
}
