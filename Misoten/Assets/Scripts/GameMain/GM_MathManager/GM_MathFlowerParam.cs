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
        WHITE,
    };

    //セル
    private GM_MathCell parentCell;

    //自身のコリジョン
    private SphereCollider thisCollider;

    //パラメータ類
    public EFlowerType flowerType = EFlowerType.Flower1;
    public EFlowerLevel flowerLevel = EFlowerLevel.Level0;  //フラワーレベル
    public EFlowerColor flowerColor = EFlowerColor.NONE;    //スプレーされた色
    public float nowEXP = 0;                                //現在の経験値
    public int[] MAX_EXP = new int[2];                      //最大経験値量(外部からセットされたらそれを優先する)
    int objId;
    
    private float colorMixableTimeCount;   //色を混ぜることが出来る残り時間
    
    
    //初回のみ
    void Start()
    {
        //自身のコライダーを取得
        thisCollider = transform.GetComponent<SphereCollider>();

        //親の親オブジェクトにCellスクリプトがあるので保存
        parentCell = transform.parent.parent.GetComponent<GM_MathCell>();

        //親のセルに自らを保存させる。
        parentCell.flowerParams.Add(this);

        //パラメーター初期化
        Init();

        // ゲーム開始時にオブジェクト生成
        if (flowerType == EFlowerType.Bill){
            //レベルアップ
            flowerLevel = EFlowerLevel.Level1;
            //オブジェクト生成
            objId = ObjectManager.CreateObj(transform.position, flowerType, flowerColor);
        }
    }

    //======================公開関数=========================

	// 初期化関数
	public void Init () {
        //あたり判定オフ
        thisCollider.enabled = false;

        //パラメーター初期化
        if (flowerType == EFlowerType.Bill || flowerType == EFlowerType.BigBill)
        {
            flowerLevel = EFlowerLevel.Level1;
        }
        else
        {
            flowerLevel = EFlowerLevel.Level0;
        }
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

        colorMixableTimeCount = 0.0f;
	}
    //これでオンにしないとあたり判定無効
    public void ColliderSwitch(bool _flg)
    {
        thisCollider.enabled = _flg;
    }

    void Update()
    {
        if (colorMixableTimeCount > 0.0f)
        {
            colorMixableTimeCount -= Time.deltaTime;
        }
    }

    //種まき完了後これを実行してほしい。自然成長などが解放される
    public void PrantStart(int playerNo, EFlowerColor _playerColor)
    {
        //このセルの行動が許可されていなければ何もしない
        if(parentCell.startFlg == false){
            return;
        }

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
        //このセルの行動が許可されていなければ何もしない
        if (parentCell.startFlg == false)
        {
            return;
        }

        //自然成長の対象でないレベルなら除外
        if (flowerLevel == EFlowerLevel.Level0 || flowerLevel == EFlowerLevel.Level3)
        {
            return;
        }

        //ビル以外なら自然成長を動作させる
        if (flowerType <= EFlowerType.House)
        {
            //経験値加算
            nowEXP += _addExp;

            //スコア加算(システム側スコア)
            GM_ScoreCtrl.AddPlayerScore(_addExp, 3);

            //レベル計算
            CalcLevel(3);
        }

    }

    //経験値加算汎用処理
    public void AddExp(int playerNo, float _addExp)
    {
        //このセルの行動が許可されていなければ何もしない
        if (parentCell.startFlg == false)
        {
            return;
        }

        //成長の対象でないレベルなら除外
        if (flowerLevel == EFlowerLevel.Level0 || flowerLevel == EFlowerLevel.Level3)
        {
            return;
        }

        //経験値加算
        nowEXP += _addExp;

        //スコア加算
        GM_ScoreCtrl.AddPlayerScore(_addExp, playerNo);

        //レベル計算
        CalcLevel(playerNo);
    }

    //レベル3の時のみ実行可能
    public void AddColor(int playerNo, EFlowerColor _setFlowerColor)
    {
        //このセルの行動が許可されていなければ何もしない
        if (parentCell.startFlg == false)
        {
            return;
        }

        //ビルは色スプレーできない
        if (flowerType == EFlowerType.Bill || flowerType == EFlowerType.BigBill)
        {
            return;
        }

        //成長終了済みでないと通さない
        if (flowerLevel == EFlowerLevel.Level3)
        {
            //色情報セット
            MixColor(_setFlowerColor);

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
        }
    }

    //色混ぜる処理
    private void MixColor(EFlowerColor _addColor)
    {
        //色を混ぜることができる時間では無かったら上書き
        if (colorMixableTimeCount <= 0.0f)
        {
            flowerColor = _addColor;
        }
        //混ぜることが出来る時間なら色計算
        else
        {
            bool _redFlg = false;
            bool _greenFlg = false;
            bool _blueFlg = false;

            //今の色を取得
            switch (flowerColor)
            {
                case EFlowerColor.NONE:
                    break;
                case EFlowerColor.RED:
                    _redFlg = true;
                    break;
                case EFlowerColor.GREEN:
                    _greenFlg = true;
                    break;
                case EFlowerColor.BLUE:
                    _blueFlg = true;
                    break;
                case EFlowerColor.CYAN:
                    _greenFlg = true;
                    _blueFlg = true;
                    break;
                case EFlowerColor.MAGENTA:
                    _redFlg = true;
                    _blueFlg = true;
                    break;
                case EFlowerColor.YELLOW:
                    _redFlg = true;
                    _greenFlg = true;
                    break;
                case EFlowerColor.WHITE:
                    _redFlg = _greenFlg = _blueFlg = true;
                    break;
            }

            //加算する色を計算
            switch (_addColor)
            {
                case EFlowerColor.RED:
                    _redFlg = true;
                    break;
                case EFlowerColor.GREEN:
                    _greenFlg = true;
                    break;
                case EFlowerColor.BLUE:
                    _blueFlg = true;
                    break;
                default:
                    break;
            }

            //混ざった色を計算
            EFlowerColor _flowerColor = EFlowerColor.NONE;
            if (_redFlg == true)
            {
                _flowerColor = EFlowerColor.RED;
                if (_greenFlg == true)
                {
                    _flowerColor = EFlowerColor.YELLOW;
                    if (_blueFlg == true)
                    {
                        _flowerColor = EFlowerColor.WHITE;
                    }

                }
                else
                {
                    if (_blueFlg == true)
                    {
                        _flowerColor = EFlowerColor.MAGENTA;
                    }
                }
            }
            else
            {
                if (_greenFlg == true)
                {
                    _flowerColor = EFlowerColor.GREEN;
                    if (_blueFlg == true)
                    {
                        _flowerColor = EFlowerColor.CYAN;
                    }
                }
                else 
                {
                    if (_blueFlg == true)
                    {
                        _flowerColor = EFlowerColor.BLUE;
                    }
                }
            }
            //色を保存
            flowerColor = _flowerColor;
        }//endif 色混ぜ計算終了

        colorMixableTimeCount = 1.0f;

        //マテリアル変更処理など
        ObjectManager.ChengeColor(objId, flowerColor);      // 色変更処理
    }
}
