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
    public GameObject hexagonPrefab_Flower;
    public GameObject hexagonPrefab_House;
    public GameObject hexagonPrefab_Bill;
    public GameObject hexagonPrefab_BigBill;
    public Vector3[] mathPos = new Vector3[9];

    public int[] MATH_EXP_MAX_FLOWER = new int[2];  //花の最大経験値量
    public int[] MATH_EXP_MAX_HOUSE = new int[2];   //家の最大経験値量
    public int[] MATH_EXP_MAX_BILL = new int[2];    //中ビルの最大経験値量
    public int[] MATH_EXP_MAX_BIGBILL = new int[2]; //大ビルの最大経験値量

    public int ADD_GROUWTH_POINT = 2;

    //レベル系
    public int MAX_TOTAL_FLOWER_LEVEL;
    public int totalFlowerLevel;

    //スコア系
    public int MAX_TOTAL_FLOWER_SCORE;
    public int totalFlowerScore;

    //セルオブジェクト達
    public List<GM_MathCell> cells;

    //ステージオブジェクト
    [SerializeField]
    private GameObject[] stageObj = new GameObject[3];

    //内部変数
    private GM_MathBillList billList;
    int lastUpdateCellNo = 0;
    float timeCount = 0.0f;
    float levelGetTimeCount = 0.0f;

    private int updateColorFlowerCount;
    public int[] colorFlowerNum = new int[(int)GM_MathFlowerParam.EFlowerColor.MAX_NUM];

    //debug用
    [SerializeField]
    private int debug_StageTypeNo;
    public int _randamStageTypeNo;

    void Awake()
    {
        billList = transform.GetComponent<GM_MathBillList>();
    }
    //初期化
    public void Init()
    {
        //生成するステージパターン番号を決定
        _randamStageTypeNo = Random.Range(0, 3);    //min <= range < max

        debug_StageTypeNo = _randamStageTypeNo;
        
        //生成するステージパターンに合わせてステージオブジェクトをアクティブに
        for (int i = 0; i < 3; ++i)
        {
            if (stageObj[i] != null)
            {
                if (i == _randamStageTypeNo)
                {
                    stageObj[i].SetActive(true);
                }
                else
                {
                    stageObj[i].SetActive(false);
                }
            }
        }

        //各セルを初期化
        for (int i = 0; i < cells.Count; ++i)
        {
            cells[i].Init(_randamStageTypeNo);
        }

        //内部変数初期化
        lastUpdateCellNo = 0;
        timeCount = 0.0f;
        levelGetTimeCount = 0.0f;
        totalFlowerLevel = 0;
        totalFlowerScore = 0;
        updateColorFlowerCount = 0;
        
        //ビルリストを作成
        billList.MakeBillList();

        //植物の最大レベルとスコアを計算
        MAX_TOTAL_FLOWER_LEVEL = 0;
        MAX_TOTAL_FLOWER_SCORE = 0;
        for (int i = 0; i < cells.Count; ++i)
        {
            MAX_TOTAL_FLOWER_LEVEL += cells[i].GetMaxTotalFlowerLevel();
            MAX_TOTAL_FLOWER_LEVEL += cells[i].GetMaxTotalFlowerScore();
        }

        //STAGE1のセルを利用開始状態へ
        StartStage(EMathStageNo.STAGE1);
        for (int i = 0; i < (int)GM_MathFlowerParam.EFlowerColor.MAX_NUM; ++i)
        {
            colorFlowerNum[i] = CalcFlowerColorNum((GM_MathFlowerParam.EFlowerColor)i);
        }
    }
	
	// Update is called once per frame
	void Update () {
	    //ローカル変数定義
        bool timeCountResetFlg = false;

        //レベルとスコアを取得する時間を経過させる
        levelGetTimeCount += Time.deltaTime;
        if (levelGetTimeCount > 0.5f)
        {
            levelGetTimeCount -= 0.5f;

            //レベルとスコアの総数を取得する
            totalFlowerLevel = 0;
            totalFlowerScore = 0;
            for (int i = 0; i < cells.Count; ++i)
            {
                totalFlowerLevel += cells[i].GetTotalFlowerLevel();
                totalFlowerScore += cells[i].GetTotalFlowerScore();
            }
        }

        //内部時間を経過させる
        timeCount += Time.deltaTime;
        if (timeCount > 1.0f)
        {
            timeCount -= 1.0f;
            timeCountResetFlg = true;
        }

        //植物の自然成長処理
        PrantGrowthProcess(timeCountResetFlg);

        //マスの末端オブジェクトの更新
        for (int i = 0; i < cells.Count; ++i)
        {
            cells[i].UpdateFlower();
        }

        //色花のカウントをチェック(8フレームに1回)
        colorFlowerNum[updateColorFlowerCount] = CalcFlowerColorNum((GM_MathFlowerParam.EFlowerColor)updateColorFlowerCount);
        updateColorFlowerCount++;
        if (updateColorFlowerCount >= (int)GM_MathFlowerParam.EFlowerColor.MAX_NUM)
        {
            updateColorFlowerCount = 0;
        }
	}

    //=============================公開関数=====================================
    //指定のステージ番号のセルを有効化
    public void StartStage(EMathStageNo stageNo)
    {
        for (int i = 0; i < cells.Count; ++i)
        {
            if (cells[i].stageNo == stageNo)
            {
                cells[i].CellStart();
            }
        }
    }

    //ミニマップ上で白以上になっている割合
    public float GetGreenPercent()
    {
        int _greenMaxNum = 0;
        int _greenNum = 0;
        float _greenPercent = 0.0f;

        for (int i = 0; i < cells.Count; ++i)
        {
            _greenMaxNum += cells[i].GetFlowerNum();
        }
        for (int i = 0; i < cells.Count; ++i)
        {
            _greenNum += cells[i].GetGreenFlowerNum();
        }
        _greenPercent = (float)_greenNum / (float)_greenMaxNum;

        return _greenPercent;
    }

    //指定の色の花の数を調べる
    public int GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor _color)
    {
        int _num = 0;

        _num = colorFlowerNum[(int)_color];

        return _num;
    }
    private int CalcFlowerColorNum(GM_MathFlowerParam.EFlowerColor _color)
    {
        int _num = 0;

        for (int i = 0; i < cells.Count; ++i)
        {
            _num += cells[i].GetFlowerColorNum(_color);
        }

        return _num;
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
