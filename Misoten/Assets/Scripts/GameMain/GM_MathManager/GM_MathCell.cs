using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM_MathCell : MonoBehaviour {

    //定数定義
    public enum ECellType
    {
        CELL_FLOWER,
        CELL_HOUSE,
        CELL_BILL,
        CELL_BIGBILL,
    };
    public bool[] houseSettiFlg = new bool[9];
    public bool[] houseSettiFlg2 = new bool[9];
    public bool[] houseSettiFlg3 = new bool[9];
    //変数定義
    //Init()でセット
    public List<GM_MathMath> math;
    //Init()でセット
    public List<GM_MathFlowerParam> flowerParams;
    //マネージャオブジェクト
    public GM_MathManager manager;
    //マス目描画スクリプト
    public GM_MathCellColorControll mathCellColorCon;


    //このセルのステージ番号
    public GM_MathManager.EMathStageNo stageNo;     //Unity上でセット
    //生成マスのパターン
    [SerializeField]
    private ECellType[] createCellType = new ECellType[3];   //Unity上でセット
    //このセルのタイプ
    public ECellType cellType;                      //Init()関数で決定

    public bool startFlg;   //このセルが行動を開始しているかどうか

	//初回のみ
	void Awake () {
        //親の親オブジェクトにMathManagerがあるので保存
        manager = transform.parent.parent.GetComponent<GM_MathManager>();
	    //親の親オブジェクトにMathManagerがあるのでそこに自らをセット
        manager.cells.Add(this);
        //マス目描画スクリプトを保存
        mathCellColorCon = GetComponentInChildren<GM_MathCellColorControll>();

	}
	
    //ゲーム開始時に初期化
    public void Init(int stagePatternNo)
    {
        bool _houseFlg = false;

        //もしマスが既にあれば削除
        if (math.Count > 0)
        {
            for (int i = 0; i < math.Count; ++i)
            {
                Destroy(math[i].gameObject);
            }
            math.Clear();
            flowerParams.Clear();
        }

        //ステージパターン番号でよろしくない値が来たら修正する
        if (stagePatternNo < 0 || stagePatternNo > 2)
        {
            stagePatternNo = 0;
        }
        cellType = createCellType[stagePatternNo];

        //セルのタイプで生成するマスを変える
        switch (cellType)
        {
            case ECellType.CELL_FLOWER:
                //花マスの生成
                for (int i = 0; i < 9; ++i)
                {
                    CreateMath(manager.hexagonPrefab_Flower, manager.mathPos[i]);
                }
                break;
            case ECellType.CELL_HOUSE:
                //家か花の生成
                for (int i = 0; i < 9; ++i)
                {
                    switch(stagePatternNo)
                    {
                        case 0:
                            _houseFlg = houseSettiFlg[i];
                            break;
                        case 1:
                            _houseFlg = houseSettiFlg2[i];
                            break;
                        case 2:
                            _houseFlg = houseSettiFlg3[i];
                            break;
                    }

                    if (_houseFlg == true)
                    {
                        CreateMath(manager.hexagonPrefab_House, manager.mathPos[i]);
                    }
                    else
                    {
                        CreateMath(manager.hexagonPrefab_Flower, manager.mathPos[i]);
                    }
                }
                break;
            case ECellType.CELL_BILL:
                //ビルの生成
                CreateMath(manager.hexagonPrefab_Bill, Vector3.zero);
                break;
            case ECellType.CELL_BIGBILL:
                //大ビルの生成
                CreateMath(manager.hexagonPrefab_BigBill, Vector3.zero);
                break;
        }

        //各パラメーターを初期化
        for (int i = 0; i < flowerParams.Count; ++i)
        {
            flowerParams[i].Init();
        }

        //マス色管理システムを初期化
        mathCellColorCon.Init();

        startFlg = false;
    }
    //セル利用開始時に呼ばれる
    public void CellStart()
    {
        mathCellColorCon.ChangeMathColor(Color.white);
        //各あたり判定を有効にする
        for (int i = 0; i < flowerParams.Count; ++i)
        {
            flowerParams[i].ColliderSwitch(true);
        }
        startFlg = true;
    }
    //末端スクリプトの更新
    public void UpdateFlower()
    {
        for (int i = 0; i < flowerParams.Count; ++i)
        {
            flowerParams[i].UpdateFlower();
        }
    }

    //自然成長
    public void PrantGrowth(int addExp)
    {
        //自然成長を伝達
        for (int i = 0; i < flowerParams.Count; ++i)
        {
            flowerParams[i].PrantGrowth(addExp);
        }
    }

    //このセルの中の花のレベルの総数を計算
    public int GetTotalFlowerLevel()
    {
        int levelCount = 0;

        for (int i = 0; i < flowerParams.Count; ++i)
        {
            levelCount += (int)flowerParams[i].flowerLevel;
        }

        return levelCount;
    }
    //このセルの中の花のレベルの最大総数を計算
    public int GetMaxTotalFlowerLevel()
    {
        int levelCount = 0;

        for (int i = 0; i < flowerParams.Count; ++i)
        {
            levelCount += (int)GM_MathFlowerParam.EFlowerLevel.Level3;
        }

        return levelCount;
    }

    public int GetFlowerNum()
    {
        int _flowerNum = flowerParams.Count;

        return _flowerNum;
    }
    public int GetGreenFlowerNum()
    {
        int _greenFlowerNum = 0;

        for (int i = 0; i < flowerParams.Count; ++i)
        {
            switch (flowerParams[i].flowerType)
            {
                case GM_MathFlowerParam.EFlowerType.Flower1:
                case GM_MathFlowerParam.EFlowerType.Flower2:
                case GM_MathFlowerParam.EFlowerType.Flower3:
                case GM_MathFlowerParam.EFlowerType.House:
                    if (flowerParams[i].flowerLevel == GM_MathFlowerParam.EFlowerLevel.Level1)
                    {
                        _greenFlowerNum++;
                    }
                    break;
                case GM_MathFlowerParam.EFlowerType.Bill:
                case GM_MathFlowerParam.EFlowerType.BigBill:
                    if (flowerParams[i].flowerLevel == GM_MathFlowerParam.EFlowerLevel.Level2)
                    {
                        _greenFlowerNum++;
                    }
                    break;
            }
        }

        return _greenFlowerNum;
    }

    //このセルの中の花のスコアの総数を計算
    public int GetTotalFlowerScore()
    {
        int _scoreCount = 0;

        for (int i = 0; i < flowerParams.Count; ++i)
        {
            _scoreCount += GetFlowerScore(flowerParams[i].flowerType, flowerParams[i].flowerLevel);
        }

        return _scoreCount;
    }
    //このセルの中の花のスコアの最大スコアを計算
    public int GetMaxTotalFlowerScore()
    {
        int _scoreCount = 0;

        for (int i = 0; i < flowerParams.Count; ++i)
        {
            _scoreCount += GetFlowerScore(flowerParams[i].flowerType, GM_MathFlowerParam.EFlowerLevel.Level3);
        }

        return _scoreCount;
    }

    //タイプとレベル別でスコアを定義
    public int GetFlowerScore(GM_MathFlowerParam.EFlowerType _type, GM_MathFlowerParam.EFlowerLevel _level)
    {
        int _scorePoint = 0;

        //レベル０なら関係なくスコア無し
        if (_level == GM_MathFlowerParam.EFlowerLevel.Level0)
        {
            return 0;
        }

        switch (_type)
        {
            case GM_MathFlowerParam.EFlowerType.Flower1:
            case GM_MathFlowerParam.EFlowerType.Flower2:
            case GM_MathFlowerParam.EFlowerType.Flower3:
                switch (_level)
                {
                    case GM_MathFlowerParam.EFlowerLevel.Level1:
                        _scorePoint = 1;
                        break;
                    case GM_MathFlowerParam.EFlowerLevel.Level2:
                        _scorePoint = 2;
                        break;
                    case GM_MathFlowerParam.EFlowerLevel.Level3:
                        _scorePoint = 7;
                        break;
                }
                break;
            case GM_MathFlowerParam.EFlowerType.House:
                switch (_level)
                {
                    case GM_MathFlowerParam.EFlowerLevel.Level1:
                        _scorePoint = 5;
                        break;
                    case GM_MathFlowerParam.EFlowerLevel.Level2:
                        _scorePoint = 10;
                        break;
                    case GM_MathFlowerParam.EFlowerLevel.Level3:
                        _scorePoint = 20;
                        break;
                }
                break;
            case GM_MathFlowerParam.EFlowerType.Bill:
                switch (_level)
                {
                    case GM_MathFlowerParam.EFlowerLevel.Level1:
                        _scorePoint = 0;
                        break;
                    case GM_MathFlowerParam.EFlowerLevel.Level2:
                        _scorePoint = 100;
                        break;
                    case GM_MathFlowerParam.EFlowerLevel.Level3:
                        _scorePoint = 500;
                        break;
                }
                break;
            case GM_MathFlowerParam.EFlowerType.BigBill:
                switch (_level)
                {
                    case GM_MathFlowerParam.EFlowerLevel.Level1:
                        _scorePoint = 0;
                        break;
                    case GM_MathFlowerParam.EFlowerLevel.Level2:
                        _scorePoint = 300;
                        break;
                    case GM_MathFlowerParam.EFlowerLevel.Level3:
                        _scorePoint = 1500;
                        break;
                }
                break;
        }

        //スコアを返す
        return _scorePoint;
    }
    //マスの生成関数
    private void CreateMath(GameObject _instantiateObject, Vector3 _posCorrection)
    {
        GameObject temp;
        GM_MathMath tempMath;

        //マスの生成
        temp = Instantiate(_instantiateObject);
        temp.transform.parent = transform;
        temp.transform.position = transform.position + _posCorrection * manager.transform.localScale.x;
        //マスのスクリプトを取得
        tempMath = temp.GetComponent<GM_MathMath>();
        math.Add(tempMath);

        //マスに自らをセット
        tempMath.parentCell = this;

        //マスのスクリプトを通じて孫の花パラメータ取得
        for (int i = 0; i < tempMath.flowerParams.Count; ++i)
        {
            flowerParams.Add(tempMath.flowerParams[i]);
        }
    }

    //レベル最大かつ指定の色の花の数を返す(noneですべての花の個数)
    public int GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor _color)
    {
        int _num = 0;

        //このセルの全オブジェクト検索
        for (int i = 0; i < flowerParams.Count; ++i)
        {
            //花じゃないなら何もしない
            if(flowerParams[i].flowerType == GM_MathFlowerParam.EFlowerType.BigBill || 
                flowerParams[i].flowerType == GM_MathFlowerParam.EFlowerType.Bill ||
                flowerParams[i].flowerType == GM_MathFlowerParam.EFlowerType.House)
            {
                continue;
            }

            //全ての花の個数を調べる指示だった
            if (_color == GM_MathFlowerParam.EFlowerColor.NONE)
            {
                _num++;
                continue;
            }

            //レベル最大じゃないなら何もしない
            if (flowerParams[i].flowerLevel != GM_MathFlowerParam.EFlowerLevel.Level3)
            {
                continue;
            }

            //指定の色の花なら数に入れる
            if (flowerParams[i].flowerColor == _color)
            {
                _num++;
            }
        }

        return _num;
    }
}
