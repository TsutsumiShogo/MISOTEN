using UnityEngine;
using System.Collections;

public class GM_MathFlowerParam : MonoBehaviour {

    //定数定義
    public enum EFlowerType
    {
        Flower1,    //花１
        Flower2,    //花２
        Flower3,    //花３
        House,      //家         //これから下は特殊経験値が必要
        Bill,       //中ビル
        BigBill,    //大ビル
    };

    public enum EFlowerLevel
    {
        Level0 = 0,     //種まき前
        Level1,         //植物成長中
        Level2,         //成長完了
        Level3,         //着色済み
    };
    public enum EFlowerColor
    {
        NONE,
        RED,
        GREEN,
        BLUE,
        CYAN,
        MAGENTA,
        YELLOW,
    };

    //セル
    private GM_MathCell parentCell;

    //パラメータ類
    public EFlowerType flowerType = EFlowerType.Flower1;
    public EFlowerLevel flowerLevel = EFlowerLevel.Level0;  //フラワーレベル
    public EFlowerColor flowerColor = EFlowerColor.NONE;    //スプレーされた色
    public float nowEXP = 0;                                //現在の経験値
    public int[] MAX_EXP = new int[2];                      //最大経験値量(外部からセットされたらそれを優先する)
    int objId;
   
   
 
    //初回のみ
    void Start()
    {
        //親の親オブジェクトにCellスクリプトがあるので保存
        parentCell = transform.parent.parent.GetComponent<GM_MathCell>();

        //親のセルに自らを保存させる。
        parentCell.flowerParams.Add(this);

        //パラメーター初期化
        Init();
    }

    //======================公開関数=========================

	// 初期化関数
	public void Init () {
        //パラメーター初期化
        flowerLevel = EFlowerLevel.Level0;
        flowerColor = EFlowerColor.NONE;
        nowEXP = 0.0f;

        //必要経験値量情報をマネージャーから取得する
        MAX_EXP[0] = parentCell.manager.MATH_EXP_MAX_FLOWER[0];    //level1→2に必要な経験値
        MAX_EXP[1] = parentCell.manager.MATH_EXP_MAX_FLOWER[1];    //level2→3に必要な経験値
        if (flowerType == EFlowerType.House)
        {
            MAX_EXP[0] = parentCell.manager.MATH_EXP_MAX_HOUSE[0];    //level1→2に必要な経験値
            MAX_EXP[1] = parentCell.manager.MATH_EXP_MAX_HOUSE[1];    //level2→3に必要な経験値
        }
        if (flowerType == EFlowerType.Bill)
        {
            MAX_EXP[0] = parentCell.manager.MATH_EXP_MAX_BILL[0];    //level1→2に必要な経験値
            MAX_EXP[1] = parentCell.manager.MATH_EXP_MAX_BILL[1];    //level2→3に必要な経験値
        }
        if (flowerType == EFlowerType.BigBill)
        {
            MAX_EXP[0] = parentCell.manager.MATH_EXP_MAX_BIGBILL[0];    //level1→2に必要な経験値
            MAX_EXP[1] = parentCell.manager.MATH_EXP_MAX_BIGBILL[1];    //level2→3に必要な経験値
        }
	}

    //種まき完了後これを実行してほしい。自然成長などが解放される
    public void PrantStart(int playerNo, EFlowerColor _playerColor)
    {
        if (flowerLevel == EFlowerLevel.Level0)
        {
            //レベルアップ
            flowerLevel = EFlowerLevel.Level1;

            //色設定
            flowerColor = _playerColor;

            //オブジェクト生成
            objId = ObjectManager.CreateObj(transform.position, flowerType, flowerColor);

            //スコア加算
            GM_ScoreCtrl.AddPlayerScore(100, playerNo);
        }
    }

    //自然成長用の処理
    public void PrantGrowth(int _addExp)
    {
        //自然成長の対象でないレベルなら除外
        if (flowerLevel == EFlowerLevel.Level0 || flowerLevel == EFlowerLevel.Level3)
        {
            return;
        }

        //一番小さい種類の花なら自然成長を動作させる
        if (flowerType < EFlowerType.House)
        {
            //経験値加算
            nowEXP += _addExp;

            //スコア加算(システム側スコア)
            GM_ScoreCtrl.AddPlayerScore(1, 3);

            //レベル計算
            CalcLevel(3);
        }

    }

    //経験値加算汎用処理
    public void AddExp(int playerNo, int _addExp)
    {
        //成長の対象でないレベルなら除外
        if (flowerLevel == EFlowerLevel.Level0 || flowerLevel == EFlowerLevel.Level3)
        {
            return;
        }

        //経験値加算
        nowEXP += _addExp;

        //スコア加算
        GM_ScoreCtrl.AddPlayerScore(1, playerNo);

        //レベル計算
        CalcLevel(playerNo);
    }

    //レベル3の時のみ実行可能
    public void AddColor(int playerNo, EFlowerColor _setFlowerColor)
    {
        //成長終了済みでないと通さない
        if (flowerLevel == EFlowerLevel.Level3)
        {
            //色情報セット
            flowerColor = _setFlowerColor;

            //マテリアル変更処理
            

            return;
        }
    }

    //======================非公開関数=========================

    //現在経験値を元にレベル計算を行う
    private void CalcLevel(int playerNo)
    {
        //レベルアップ対象でないレベルなら除外
        if (flowerLevel == EFlowerLevel.Level3)
        {
            return;
        }

        //計算用にレベルをint型へ変換
        int i_flowerLevel = (int)flowerLevel - 1;

        //レベルアップに必要な経験値が貯まったらレベルUP
        if (nowEXP > MAX_EXP[i_flowerLevel])
        {
            //レベルアップ処理
            nowEXP -= MAX_EXP[i_flowerLevel];
            flowerLevel++;

            //スコア加算
            GM_ScoreCtrl.AddPlayerScore(200 * (int)flowerLevel, playerNo);

            //レベル3へ上がったら経験値を0へ戻す
            if (flowerLevel == EFlowerLevel.Level3)
            {
                nowEXP = 0;
            }

            //レベルアップ毎に必要な処理があればここに記述
            ObjectManager.LevelUp(objId, (int)flowerLevel, flowerType);
            Debug.Log("levelup");
        }
    }
}
