using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

    //========================定数定義========================
    public Color PLAYER_COLOR;                      //キャラ毎で違う。初期値はUnity側からセット
    public float DEFAULT_MOVE_SPEED = 5.0f;         //毎秒移動速度
    public float GRAWING_MOVE_PERCENT = 0.6f;       //成長アクション中の移動速度を
    public float SPRAY_MAX_SCALE_NEED_TIME = 0.5f;  //スプレーをどれだけ長押しすれば最大範囲で撒けるようになるか

    //状態遷移
    public enum EStateTransitionMode
    {
        SYSTEM_MODE,
        NORMAL,
        ACTION,
        BILL_ACTION,
        DAMAGE,
        STATE_TRANSITION_MODE_NUM
    };
    public enum EStateTransition
    {
        //system
        START,
        END,

        //normal
        STAND,          //立ち状態
        WALK,           //歩き状態
        RUN,            //移動状態

        //action
        SOWING_SEEDS,   //種まき          //移動速い。長押しで種まき可
        GROWING,        //成長            //移動遅い(目安6割)
        SPRAY,          //スプレー        //立ち止まってスプレー１秒くらい

        //billAction
        GROWING_BILL,   //ビル専用成長    //立ち止まって成長

        //damage
        KNOCKBACK,      //ノックバック

        STATE_TRANSITION_NUM
    };
    

    //========================構造体定義===========================
    public struct SStateTransition
    {
        public EStateTransitionMode stateMode; //おおざっぱなモード
        public EStateTransition status;        //詳細なモード
    }

    //========================変数定義============================
    //プレイヤーステータス
    public PlayerSprayControll.EPlayerSprayMode playerSprayMode;    //スプレーモード
    public SStateTransition sStateTransition;  //状態
    public float moveSpeedParam;               //移動速度
    public EStateTransition testStatus;
    //========================実行関数============================
	// Use this for initialization
	void Awake () {
        Init();
	}
    public void Init()
    {
        //初期の状態設定
        playerSprayMode = PlayerSprayControll.EPlayerSprayMode.SEED;
        sStateTransition.stateMode = EStateTransitionMode.SYSTEM_MODE;
        sStateTransition.status = EStateTransition.START;

        //パラメーター設定
        moveSpeedParam = DEFAULT_MOVE_SPEED;
    }
    
	
	// Update is called once per frame
	void Update () {
        testStatus = sStateTransition.status;
	    //マスのレベルをチェックしてレベル毎に移動速度パラメーターを更新する。

	}

    //スプレーの状態を変える
    public void ChangeSprayMode(bool _rightFlg)
    {
        //LRボタンで切り替える方向を変えるので
        if (_rightFlg == true)
        {
            playerSprayMode++;
            if (playerSprayMode > PlayerSprayControll.EPlayerSprayMode.COLOR)
            {
                playerSprayMode = PlayerSprayControll.EPlayerSprayMode.SEED;
            }
        }
        else
        {
            playerSprayMode--;
            if (playerSprayMode < PlayerSprayControll.EPlayerSprayMode.SEED)
            {
                playerSprayMode = PlayerSprayControll.EPlayerSprayMode.COLOR;
            }
        }
    }

    //プレイヤーの状態をセットする
    public void SetStateTransition(EStateTransition state)
    {
        //おかしいステータスを排除
        if (state < 0 || state >= EStateTransition.STATE_TRANSITION_NUM)
            return;

        sStateTransition.status = state;
        sStateTransition.stateMode = ChackStateTransitionMode(state);
    }
    //おおざっぱなプレイヤーの状態を取得
    public EStateTransitionMode GetStateTransitionMode()
    {
        return sStateTransition.stateMode;
    }
    //プレイヤーの状態を取得
    public EStateTransition GetStateTransition()
    {
        return sStateTransition.status;
    }


    //========================非公開実行関数============================

    //詳細状態からおおざっぱな状態を取得する。
    private EStateTransitionMode ChackStateTransitionMode(EStateTransition state)
    {
        if (state < EStateTransition.STAND)
        {
            return EStateTransitionMode.SYSTEM_MODE;
        }
        if (state < EStateTransition.SOWING_SEEDS)
        {
            return EStateTransitionMode.NORMAL;
        }
        if (state < EStateTransition.GROWING_BILL)
        {
            return EStateTransitionMode.ACTION;
        }
        if (state < EStateTransition.KNOCKBACK)
        {
            return EStateTransitionMode.BILL_ACTION;
        }
        if (state < EStateTransition.STATE_TRANSITION_NUM)
        {
            return EStateTransitionMode.DAMAGE;
        }
        return EStateTransitionMode.STATE_TRANSITION_MODE_NUM;
    }
}
