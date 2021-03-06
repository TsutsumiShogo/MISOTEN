﻿using UnityEngine;
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
        NONE = 0,
        RED,
        GREEN,
        BLUE,
        CYAN,
        MAGENTA,
        YELLOW,
        WHITE,
        MAX_NUM,
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

    private float[] addExpByPlayersTime = new float[3];     //誰から経験値をもらったか一定時間情報を保持する(正の値で有効)
    private float colorMixableTimeCount;   //色を混ぜることが出来る残り時間

    // 誰に植えられたかを保持
    public int m_plantPlayerId;

    //初回のみ
    void Awake()
    {
        //自身のコライダーを取得
        thisCollider = transform.GetComponent<SphereCollider>();

        //親のマスに自らを保存させる
        transform.parent.GetComponent<GM_MathMath>().AddFlowerParam(this);
    }
    void Start()
    {
        //-----------------------------------------
        // 建物オブジェクトはゲーム開始時にオブジェクト生成
        if ((flowerType == EFlowerType.Bill) || (flowerType == EFlowerType.BigBill))
        {
            //レベルアップ
            flowerLevel = EFlowerLevel.Level1;
            //オブジェクト生成
            objId = ObjectManager.CreateObj(transform.position, flowerType, flowerColor, this);
        }
        if ( (flowerType == EFlowerType.House))
        {
            //オブジェクト生成
            objId = ObjectManager.CreateObj(transform.position, flowerType, flowerColor, this);
        }


    }

    //======================公開関数=========================

	// 初期化関数
	public void Init () {
        //親オブジェクトにCellスクリプトがあるので保存
        parentCell = transform.parent.GetComponent<GM_MathMath>().parentCell;

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
        for (int i = 0; i < 3; i++)
        {
            addExpByPlayersTime[i] = 0.0f;
        }
        colorMixableTimeCount = 0.0f;

	}
    //これでオンにしないとあたり判定無効
    public void ColliderSwitch(bool _flg)
    {
        thisCollider.enabled = _flg;
    }

    public void UpdateFlower()
    {
        //色混ぜ可能時間減少
        if (colorMixableTimeCount > 0.0f)
        {
            colorMixableTimeCount -= Time.deltaTime;
        }

        //誰から成長受けたか情報保持時間減少
        for (int i = 0; i < 3; ++i)
        {
            if (addExpByPlayersTime[i] > 0.0f)
            {
                addExpByPlayersTime[i] -= Time.deltaTime;
            }
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

            //スコア加算
            GM_ScoreCtrl.AddPlayerScore((float)parentCell.GetFlowerScore(flowerType, flowerLevel), playerNo);

            //オブジェクト生成 花のみ
            if (flowerType == EFlowerType.Flower1){
                objId = ObjectManager.CreateObj(transform.position, flowerType, flowerColor, this);
            }
            else if(flowerType == EFlowerType.House) {
                //レベルアップ毎に必要な処理があればここに記述
                ObjectManager.LevelUp(objId, (int)flowerLevel, flowerType);
            }
            // 誰に植えられたか保持
            m_plantPlayerId = playerNo;
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

        //誰から成長を受けたか情報を保存
        if (playerNo >= 0 && playerNo <= 2)
        {
            addExpByPlayersTime[playerNo] = 1.0f;   //値の秒数保持する。
        }

        //経験値加算
        nowEXP += _addExp;

        //スコア加算
        GM_ScoreCtrl.AddPlayerScore(Time.deltaTime * 0.5f, playerNo);

        //ビル専用で成長させてる人数でボーナス
        if (flowerType >= EFlowerType.Bill) {
            int _growthNum = GetGrowthNowPlayerNum();
            
            //人数が２人なら
            if (_growthNum == 2)
            {
                //経験値加算
                nowEXP += _addExp * 0.2f;
            }

            //人数が３人なら
            if (_growthNum == 3)
            {
                //経験値加算
                nowEXP += _addExp * 0.5f;
            }
        }

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

    //何人から成長を受けている最中か取得
    public int GetGrowthNowPlayerNum()
    {
        int _grouthNum = 0;

        for (int i = 0; i < 3; ++i)
        {
            if (addExpByPlayersTime[i] > 0.0f)
            {
                _grouthNum++;
            }
        }
        return _grouthNum;
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
            GM_ScoreCtrl.AddPlayerScore((float)parentCell.GetFlowerScore(flowerType, flowerLevel), playerNo);

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
