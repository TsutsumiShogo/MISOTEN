using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

    //========================定数定義========================
    public Color PLAYER_COLOR;              //キャラ毎で違う。初期値はUnity側からセット
    public float DEFAULT_MOVE_SPEED = 2.0f; //毎秒移動速度
    public float GRAWING_MOVE_PERCENT = 0.6f;//成長アクション中の移動速度を


    //状態遷移
    public enum EStateTransitionMode
    {
        SYSTEM_MODE,
        NORMAL,
        ACTION,
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
        WEAPON_CHANGE,  //武器切り替え    //一応一瞬だけど用意
        SOWING_SEEDS,   //種まき          //移動速い。連打で種まき可
        GROWING,        //成長            //移動遅い(目安6割)
        SPRAY,          //スプレー        //立ち止まってスプレー１秒くらい

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
    public SStateTransition sStateTransition;  //状態
    public float moveSpeedParam;               //移動速度

    //========================実行関数============================
	// Use this for initialization
	void Awake () {
        //初期の状態設定
        sStateTransition.stateMode = EStateTransitionMode.SYSTEM_MODE;
        sStateTransition.status = EStateTransition.START;

        //パラメーター設定
        moveSpeedParam = DEFAULT_MOVE_SPEED;
        
	}
	
	// Update is called once per frame
	void Update () {
	    //マスのレベルをチェックしてレベル毎に移動速度パラメーターを更新する。

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
        if (state < EStateTransition.WEAPON_CHANGE)
        {
            return EStateTransitionMode.NORMAL;
        }
        if (state < EStateTransition.KNOCKBACK)
        {
            return EStateTransitionMode.ACTION;
        }
        if (state < EStateTransition.STATE_TRANSITION_NUM)
        {
            return EStateTransitionMode.DAMAGE;
        }
        return EStateTransitionMode.STATE_TRANSITION_MODE_NUM;
    }
}
