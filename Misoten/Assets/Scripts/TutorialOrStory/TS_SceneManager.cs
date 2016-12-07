﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TS_SceneManager : MonoBehaviour {

    //定数定義
    public enum E_TSPhaseNo
    {
        TS_Entry,       //１ループで次に進む。
        TS_House,       //雑談
        TS_Rule,        //説明
        TS_Rule2,       //説明
        TS_Play,        //１回目のアクション時間用
        TS_RuleBill,    //説明
        TS_PlayBill,    //２回目のアクション時間用
        TS_End,         //終わりのセリフ
        TS_Exit,        //ゲームシーンへ遷移
        TS_MaxPhaseNum  //フェーズの個数
    };
    public List<float> TEXT_START_WAIT_TIME_LIST;   //入力受付開始までの待機時間。リストの数がテキストの数
    public int[] PHACE_NEXT_TEXT_NO = new int[(int)E_TSPhaseNo.TS_MaxPhaseNum];    //ここで指定のテキスト番号が開始すれば次のフェーズへ移行する
    

    //オブジェクト
    //マス管理マネージャー
    private GM_MathManager mathManager;
    //プレイヤーマネージャー
    private PlayerManager playerManager;
    //UIマネージャー
    private TS_UIManager tutorialUiManager;

    //公開変数
    public E_TSPhaseNo nowPhaseNo;  //今のフェーズ番号
    public int nowTextNo;           //今表示中のセリフ番号 0~MAX_TEXT_NUM-1;
    public bool playerActionFlg;    //プレイヤーのアクションが許可されているフラグ

    //内部変数
    private float nowActionTime;    //行動した時間


	// Use this for initialization
	void Awake () {
        //マスマネージャー保存
        mathManager = gameObject.GetComponentInChildren<GM_MathManager>();
        //プレイヤーマネージャー保存
        playerManager = gameObject.GetComponentInChildren<PlayerManager>();
        //チュートリアルUIマネージャ保存
        tutorialUiManager = GameObject.Find("TutorialUI").GetComponent<TS_UIManager>();
	}
    public void Init()
    {
        //マスマネージャ初期化
        mathManager.Init();

        //プレイヤーマネージャー初期化
        playerManager.Init();

        //チュートリアルUIマネージャー初期化
        tutorialUiManager.Init();

        //パラメータ初期化
        nowPhaseNo = E_TSPhaseNo.TS_Entry;
        nowTextNo = 0;
        nowActionTime = 0;
        NextPhaseProcess();
    }

	// Update is called once per frame
	void Update () {

        //アクション中なら何もしない
        if (nowActionTime > 0.0f)
        {
            nowActionTime -= Time.deltaTime;
            return;
        }

        //ゲームプレイフェーズが終了したならフェーズ番号を進める
        if (nowPhaseNo == E_TSPhaseNo.TS_Play || nowPhaseNo == E_TSPhaseNo.TS_PlayBill)
        {
            NextPhaseProcess();
        }


        if (XboxController.GetButtonA(0))
        {
            //テキスト番号を進める
            NextText();
        }
	}

    //========================テキスト番号の管理===========================
    //テキスト番号を進める
    private void NextText()
    {
        if (nowTextNo >= TEXT_START_WAIT_TIME_LIST.Count-1)
        {
            return;
        }
        nowTextNo++;

        //待機時間設定
        nowActionTime = TEXT_START_WAIT_TIME_LIST[nowTextNo];
        

        //フェーズ変更するべき値かチェック
        if (CheckNextPhaseNo(nowTextNo) == true)
        {
            //フェーズを進めるべき値だったので次のフェーズへ
            NextPhaseProcess();
        }
    }


    //========================フェーズの管理===========================
    //次のフェーズに進めるべき値かチェック
    private bool CheckNextPhaseNo(int _no)
    {
        //フェーズ開始番号と値が一緒かチェック
        for (int i = 0; i < (int)E_TSPhaseNo.TS_MaxPhaseNum; ++i)
        {
            //次のフェーズへの開始番号と今のテキスト番号が同じならtrueを返却
            if (_no == PHACE_NEXT_TEXT_NO[i])
            {
                return true;
            }
        }

        //ここまできたら進めるべきでなかった
        return false;
    }
    //次のフェーズに進める処理
    private void NextPhaseProcess()
    {
        nowPhaseNo++;

        //特定のフェーズ開始時何かしたいことあればここに
        switch(nowPhaseNo){
            case E_TSPhaseNo.TS_Rule:   //家背景を消してステージへ移動
                break;
            case E_TSPhaseNo.TS_Play:   //プレイヤー行動開始
                playerManager.StartPlayers();
                tutorialUiManager.ActiveSwtich(false);
                break;
            case E_TSPhaseNo.TS_RuleBill:   //プレイヤー行動終了
                playerManager.StopPlayers();
                tutorialUiManager.ActiveSwtich(true);
                break;
            case E_TSPhaseNo.TS_PlayBill:   //プレイヤー行動開始
                mathManager.StartStage(GM_MathManager.EMathStageNo.STAGE2);
                playerManager.StartPlayers();
                tutorialUiManager.ActiveSwtich(false);
                break;
            case E_TSPhaseNo.TS_End:    //プレイヤー行動終了
                playerManager.StopPlayers();
                tutorialUiManager.ActiveSwtich(true);
                break;
            case E_TSPhaseNo.TS_Exit:   //次のシーンへ移動

                break;
        }
    }
}
