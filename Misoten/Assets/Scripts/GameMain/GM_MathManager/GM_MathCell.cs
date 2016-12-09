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
                //家の生成
                CreateMath(manager.hexagonPrefab_House, Vector3.zero);
                //花の生成
                for (int i = 1; i < 9; ++i)
                {
                    CreateMath(manager.hexagonPrefab_Flower, manager.mathPos[i]);
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
}
