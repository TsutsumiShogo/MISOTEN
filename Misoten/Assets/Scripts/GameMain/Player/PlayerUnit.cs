﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUnit : MonoBehaviour {

    public PlayerManager playerManager;             //Initでセットされる
    //各種プレイヤーモジュール部品
    private PlayerCollisionActor collisionActor;    //衝突時の処理に対応
    private PlayerAnimCon animCon;                  //アニメーション管理
    private PlayerStatus status;                    //状態の管理
    private PlayerControll controll;                //プレイヤーの行動を担当

    private PlayerSprayControll sprayCon;           //スプレーオブジェクト
    private PlayerCheckCollisionBill collisionBill; //ビルとのあたり判定チェックやビルへの操作を行う

    private PlayerStatus.EStateTransition nextState;    //次回の状態遷移先

    //定数定義
    private const float INPUT_MOVE_JUDGE_LENGTH = 0.1f; //移動入力と認める傾き
    private const float INPUT_DASH_JUDGE_LENGTH = 0.6f; //ダッシュ入力と認める傾き

    //内部変数
    private float motionTimeCount;                  //モーションの経過時間(MAX 600sc)

    //公開変数
    public int PLAYER_NO;
    public GM_MathFlowerParam.EFlowerColor PLAYER_COLOR;
    public bool moveFlg;

    private float m_soundTimer = 0.0f;

    // 色パーティクル@栗栖
    [SerializeField]
    private GameObject m_particleColor;

    private bool m_canvasInit;      // キャンバス初期設定用
    private string[] m_canvasPass = new string[3];
    private bool m_splayChangeFlg;
    private float m_splayTimer;
    private bool m_growFlg;
    private float m_growTimer;
	// Use this for initialization
	void Awake () {
        m_canvasPass[0] = "1PCanvas/";
        m_canvasPass[1] = "2PCanvas/";
        m_canvasPass[2] = "3PCanvas/";
        //部品取得
        collisionActor = transform.GetComponent<PlayerCollisionActor>();
        animCon = transform.GetComponent<PlayerAnimCon>();
        status = transform.GetComponent<PlayerStatus>();
        controll = transform.GetComponent<PlayerControll>();

        sprayCon = transform.GetComponentInChildren<PlayerSprayControll>();
        collisionBill = transform.GetComponentInChildren<PlayerCheckCollisionBill>();

	}

    public void Init(PlayerManager _manager) {
        m_splayTimer = 0.0f;
        m_splayChangeFlg = false;
        m_soundTimer = 0.0f;
        nextState = PlayerStatus.EStateTransition.START;

        motionTimeCount = 0.0f;
        moveFlg = false;
        m_canvasInit = false;
        animCon.Init();
        status.Init();
        controll.Init();
        collisionBill.Init();

        switch (PLAYER_NO)
        {
            // SprayUI初期化
            case 0:
                GameObject.Find("1PCanvas/SplayMode").GetComponent<ChengeSplay>().Init();
                break;
            case 1:
                GameObject.Find("2PCanvas/SplayMode").GetComponent<ChengeSplay>().Init();
                break;
            case 2:
                GameObject.Find("3PCanvas/SplayMode").GetComponent<ChengeSplay>().Init();
                break;
         
    }
        
    
    
        playerManager = _manager;
    }
	
	// Update is called once per frame
	void Update (){
        if( !m_canvasInit){
            m_canvasInit = true;
            switch( PLAYER_COLOR) {
                case GM_MathFlowerParam.EFlowerColor.RED:
                    GameObject.Find(m_canvasPass[PLAYER_NO]+"Image").GetComponent<Image>().color = new Color(255.0f/255.0f,0.0f,125.0f / 255.0f);
                    GameObject.Find(m_canvasPass[PLAYER_NO] + "PlayerIdFrame").GetComponent<Image>().color = new Color(255.0f / 255.0f, 0.0f, 125.0f / 255.0f);
                    GameObject.Find(m_canvasPass[PLAYER_NO] + "PlayerId").GetComponent<Text>().color = new Color(255.0f / 255.0f, 0.0f, 125.0f / 255.0f);
                    GameObject.Find(m_canvasPass[PLAYER_NO] + "PlayerId").GetComponent<Text>().text = (PLAYER_NO + 1).ToString() + "P";
                    break;
                case GM_MathFlowerParam.EFlowerColor.GREEN:
                    GameObject.Find(m_canvasPass[PLAYER_NO] + "Image").GetComponent<Image>().color = new Color(0.0f, 255.0f / 255.0f, 0.0f);
                    GameObject.Find(m_canvasPass[PLAYER_NO] + "PlayerIdFrame").GetComponent<Image>().color = new Color(0.0f, 255.0f / 255.0f, 0.0f);
                    GameObject.Find(m_canvasPass[PLAYER_NO] + "PlayerId").GetComponent<Text>().color = new Color(0.0f, 255.0f / 255.0f, 0.0f);
                    GameObject.Find(m_canvasPass[PLAYER_NO] + "PlayerId").GetComponent<Text>().text = (PLAYER_NO + 1).ToString() + "P";
                    break;
                case GM_MathFlowerParam.EFlowerColor.BLUE:
                    GameObject.Find(m_canvasPass[PLAYER_NO] + "Image").GetComponent<Image>().color = new Color(40.0f / 255.0f, 184.0f / 255.0f, 255.0f / 255.0f);
                    GameObject.Find(m_canvasPass[PLAYER_NO] + "PlayerIdFrame").GetComponent<Image>().color = new Color(40.0f / 255.0f, 184.0f / 255.0f, 255.0f / 255.0f);
                    GameObject.Find(m_canvasPass[PLAYER_NO] + "PlayerId").GetComponent<Text>().color = new Color(40.0f / 255.0f, 184.0f / 255.0f, 255.0f / 255.0f);
                    GameObject.Find(m_canvasPass[PLAYER_NO] + "PlayerId").GetComponent<Text>().text = (PLAYER_NO + 1).ToString() + "P";
                    break;
            }
        }
        int checkNo;

        //モーション経過時間を進める。
        motionTimeCount += Time.deltaTime;

        //状態が切り替わるならば
        if (status.GetStateTransition() != nextState)
        {
            //切り替わったときの共通処理を通す。
            ChangeStateTransitionProcess();
        }

        //状態毎の更新を行う。
        checkNo = EveryStatusUpdate(status.GetStateTransition(), status.GetStateTransitionMode());

        //状態に狂いがあったら立ちへ戻す。
        if (checkNo < 0) status.SetStateTransition(PlayerStatus.EStateTransition.STAND);

        //移動
        controll.Move();
	}

    public void StartPlayer(){
        nextState = PlayerStatus.EStateTransition.STAND;
        ChangeStateTransitionProcess();
    }

    public void StopPlayer(){
        if (status.GetStateTransition() != PlayerStatus.EStateTransition.END)
        {
            nextState = PlayerStatus.EStateTransition.END;
            ChangeStateTransitionProcess();
        }
    }

    public void KnockBack(Vector3 _knockBackVec){
        //吹き飛びベクトルセット
        controll.SetVector(_knockBackVec);

        //状態を変更
        nextState = PlayerStatus.EStateTransition.KNOCKBACK;
        ChangeStateTransitionProcess();
        GameObject.Find("Bee").GetComponent<Bee>().HitBee();    // 蜂に接触を伝える

        // 悲鳴再生
        switch (PLAYER_NO) {         
            case 0:
                SoundManager.PlaySe("damege_1", 5);
                break;
            case 1:
                SoundManager.PlaySe("damege_2", 6);
                break;
            case 2:
                SoundManager.PlaySe("damege_3", 7);
                break;
        }

    }

    //============================非公開関数=================================
    //状態を切り替える前の共通処理
    private void ChangeStateTransitionProcess()
    {
        //ステータスへ反映
        status.SetStateTransition(nextState);

        //状態切り替え時に何か特別な処理が必要であれば実行
        switch (nextState)
        {
            // system
            case PlayerStatus.EStateTransition.START:
                break;
            case PlayerStatus.EStateTransition.END:
                break;

            // normal
            case PlayerStatus.EStateTransition.STAND:
                moveFlg = false;
                break;
            case PlayerStatus.EStateTransition.WALK:
                moveFlg = true;
                break;
            case PlayerStatus.EStateTransition.RUN:
                moveFlg = true;
                break;

            // action
            case PlayerStatus.EStateTransition.SOWING_SEEDS:
                break;
            case PlayerStatus.EStateTransition.GROWING:
                break;
            case PlayerStatus.EStateTransition.SPRAY:
                break;

            // billAction
            case PlayerStatus.EStateTransition.GROWING_BILL:
                moveFlg = false;
                break;

            // damage
            case PlayerStatus.EStateTransition.KNOCKBACK:
                moveFlg = false;
                break;

            default:
                break;
        }

        //アニメーションを切り替える
        animCon.ChangeAnimation(status.GetStateTransition(), moveFlg);

        //モーション切り替えに伴って時間初期化
        motionTimeCount = 0.0f;
    }

    //状態毎に処理を行う
    private int EveryStatusUpdate(PlayerStatus.EStateTransition state, PlayerStatus.EStateTransitionMode stateMode)
    {
        //移動入力の方向を取得
        Vector2 inputVec = Vector2.zero;

        inputVec.x = XboxController.GetLeftX(PLAYER_NO);
        inputVec.y = XboxController.GetLeftY(PLAYER_NO);
        
        // 3P操作
        if( PLAYER_NO == 2){
            if (Input.GetKey(KeyCode.N)){
                inputVec.x = -1.0f;
            }
            if (Input.GetKey(KeyCode.M))
            {
                inputVec.x = 1.0f;
            }
        
        }

        //詳細な状態毎の処理
        switch (state)
        {
            //system
            case PlayerStatus.EStateTransition.START:
                inputVec = Vector2.zero;
                controll.SetMoveVec(inputVec);
                break;
            case PlayerStatus.EStateTransition.END:
                inputVec = Vector2.zero;
                controll.SetMoveVec(inputVec);
                break;

            //normal
            case PlayerStatus.EStateTransition.STAND:
                //移動入力があれば歩き状態へ切り替え
                if (inputVec.magnitude > INPUT_MOVE_JUDGE_LENGTH)
                {
                    nextState = PlayerStatus.EStateTransition.WALK;
                }
                
                
                break;
            case PlayerStatus.EStateTransition.WALK:
                m_soundTimer = 0.0f;
                //移動処理
                controll.SetMoveVec(inputVec);

                //移動入力がなければ立ち状態へ
                if (inputVec.magnitude <= INPUT_MOVE_JUDGE_LENGTH)
                {
                    nextState = PlayerStatus.EStateTransition.STAND;
                }
                else
                {
                    //移動入力があり、一定時間が立てばダッシュ状態へ
                    if (inputVec.y >= INPUT_DASH_JUDGE_LENGTH && motionTimeCount > 1.0f)
                    {
                        nextState = PlayerStatus.EStateTransition.RUN;
                    }
                }
                
                break;
            case PlayerStatus.EStateTransition.RUN:
                m_soundTimer = 0.0f;
                //移動処理
                controll.SetMoveVec(inputVec);

                //ダッシュ程の傾きが無ければ歩き状態へ
                if (inputVec.magnitude <= INPUT_DASH_JUDGE_LENGTH)
                    nextState = PlayerStatus.EStateTransition.WALK;
                //移動入力がなければ立ち状態へ
                if (inputVec.magnitude <= INPUT_MOVE_JUDGE_LENGTH)
                    nextState = PlayerStatus.EStateTransition.STAND;

                break;

            //action
            case PlayerStatus.EStateTransition.SOWING_SEEDS:
                //移動処理
                m_soundTimer -= Time.deltaTime;
                if (m_soundTimer <= 0.0f){
                    m_soundTimer = 2.0f;
                    SoundManager.PlaySe("seed", 3);
                }
                controll.SetMoveVec(inputVec);
                
                sprayCon.Spray(PlayerSprayControll.EPlayerSprayMode.SEED, 0.1f);
                break;
            case PlayerStatus.EStateTransition.GROWING:
                //移動処理
                controll.SetMoveVec(inputVec * 0.5f);
                if (!m_growFlg)
                {
                    SoundManager.PlaySe("grow_se", 6);
                    m_growFlg = true;
                }
                else {
                    Debug.Log("成長SE");
                    m_growTimer += Time.deltaTime;
                    if (m_growTimer >= 1.0f)
                    {
                        m_growFlg = false;
                        m_growTimer = 0.0f;
                    }
                }
                sprayCon.Spray(PlayerSprayControll.EPlayerSprayMode.GRAW, 0.1f);
                break;
            case PlayerStatus.EStateTransition.SPRAY:
                //移動処理
                controll.SetMoveVec(inputVec * 0.0f);
                sprayCon.Spray(PlayerSprayControll.EPlayerSprayMode.COLOR, 0.1f);
                if (motionTimeCount > 1.0f)
                {
                    nextState = PlayerStatus.EStateTransition.STAND;
                }
                break;

            //billAction
            case PlayerStatus.EStateTransition.GROWING_BILL:
                //ビルが存在すれば動作
                if (collisionBill.CheckCollisionBill() == true)
                {
                    //ビルを成長させる
                    collisionBill.GrowthBill();
                    //向きを固定する
                    transform.rotation = collisionBill.GetBillRot();

                    //最大まで成長していたらビル成長を止める
                    if(collisionBill.billParam.flowerLevel == GM_MathFlowerParam.EFlowerLevel.Level3){
                        nextState = PlayerStatus.EStateTransition.STAND;
                    }
                }
                break;

            //damage
            case PlayerStatus.EStateTransition.KNOCKBACK:
                if (motionTimeCount > 2.0f)
                {
                    nextState = PlayerStatus.EStateTransition.STAND;
                }
                break;
            default:
                //異常検知
                return -1;
        }

        //おおまかな状態での共通処理があれば(こちらが優先される)
        switch (stateMode)
        {
            case PlayerStatus.EStateTransitionMode.SYSTEM_MODE:
                break;
            case PlayerStatus.EStateTransitionMode.NORMAL:
                if (!m_splayChangeFlg)
                {
                    //スプレー切り替え
                    if (XboxController.GetButtonL(PLAYER_NO) == true)
                    {
                        SoundManager.PlaySe("cursol",8);
                        m_splayChangeFlg = true;
                        status.ChangeSprayMode(true);
                        switch (PLAYER_NO)
                        {
                           
                            case 0:
                                GameObject.Find("1PCanvas/SplayMode").GetComponent<ChengeSplay>().SplayChenge(false);
                                break;
                            case 1:
                                GameObject.Find("2PCanvas/SplayMode").GetComponent<ChengeSplay>().SplayChenge(false);
                                break;
                            case 2:
                                GameObject.Find("3PCanvas/SplayMode").GetComponent<ChengeSplay>().SplayChenge(false);
                                break;
                        }
                    }
                    if (XboxController.GetButtonR(PLAYER_NO) == true)
                    {
                        SoundManager.PlaySe("cursol", 8);
                        m_splayChangeFlg = true;
                        status.ChangeSprayMode(false);
                        switch (PLAYER_NO)
                        {
                            case 0:
                                GameObject.Find("1PCanvas/SplayMode").GetComponent<ChengeSplay>().SplayChenge(true);
                                break;
                            case 1:
                                GameObject.Find("2PCanvas/SplayMode").GetComponent<ChengeSplay>().SplayChenge(true);
                                break;
                            case 2:
                                GameObject.Find("3PCanvas/SplayMode").GetComponent<ChengeSplay>().SplayChenge(true);
                                break;
                        }
                    }
                }
                else
                {
                    m_splayTimer += Time.deltaTime;
                    if (m_splayTimer >= 0.3f){
                        m_splayTimer = 0.0f;
                        m_splayChangeFlg = false;
                    }
                }
                //スプレー入力
                if (XboxController.GetButtonHoldA(PLAYER_NO) == true)
                {
                    //スプレーモードで遷移先切り替え
                    switch (status.playerSprayMode)
                    {
                        case PlayerSprayControll.EPlayerSprayMode.SEED:
                            nextState = PlayerStatus.EStateTransition.SOWING_SEEDS;
                            break;
                        case PlayerSprayControll.EPlayerSprayMode.GRAW:
                            nextState = PlayerStatus.EStateTransition.GROWING;
                            break;
                        case PlayerSprayControll.EPlayerSprayMode.COLOR:
                            Debug.Log("色変え");
                            // 悲鳴再生
                            switch (PLAYER_NO)
                            {
                                case 0:
                                    SoundManager.PlaySe("color_1", 5);
                                    break;
                                case 1:
                                    SoundManager.PlaySe("color_2", 6);
                                    break;
                                case 2:
                                    SoundManager.PlaySe("color_3", 7);
                                    break;
                            }
                            m_particleColor.GetComponent<ParticleSystem>().Play();
                            nextState = PlayerStatus.EStateTransition.SPRAY;
                            break;
                    }
                }

                //ビルが目の前に存在しつつＸボタンが押されていたら
                if(collisionBill.CheckCollisionBill() == true &&
                    XboxController.GetButtonHoldX(PLAYER_NO) == true)
                {
                    //成長しきってなければ成長モードに入るのを許可
                    if (collisionBill.billParam.flowerLevel != GM_MathFlowerParam.EFlowerLevel.Level3)
                    {
                        nextState = PlayerStatus.EStateTransition.GROWING_BILL;
                    }
                }
                break;

            case PlayerStatus.EStateTransitionMode.ACTION:
                //色スプレーだけ例外
                if (status.GetStateTransition() != PlayerStatus.EStateTransition.SPRAY)
                {
                    if (!m_splayChangeFlg)
                    {
                        //スプレー切り替え
                        if (XboxController.GetButtonL(PLAYER_NO) == true)
                        {
                            m_splayChangeFlg = true;
                            status.ChangeSprayMode(true);

                            switch (PLAYER_NO)
                            {
                                case 0:
                                    GameObject.Find("1PCanvas/SplayMode").GetComponent<ChengeSplay>().SplayChenge(false);
                                    break;
                                case 1:
                                    GameObject.Find("2PCanvas/SplayMode").GetComponent<ChengeSplay>().SplayChenge(false);
                                    break;
                                case 2:
                                    GameObject.Find("3PCanvas/SplayMode").GetComponent<ChengeSplay>().SplayChenge(false);
                                    break;
                            }
                        }
                        if (XboxController.GetButtonR(PLAYER_NO) == true)
                        {
                            m_splayChangeFlg = true;
                            status.ChangeSprayMode(false);
                            switch (PLAYER_NO)
                            {
                                case 0:
                                    GameObject.Find("1PCanvas/SplayMode").GetComponent<ChengeSplay>().SplayChenge(true);
                                    break;
                                case 1:
                                    GameObject.Find("2PCanvas/SplayMode").GetComponent<ChengeSplay>().SplayChenge(true);
                                    break;
                                case 2:
                                    GameObject.Find("3PCanvas/SplayMode").GetComponent<ChengeSplay>().SplayChenge(true);
                                    break;
                            }
                        }
                    }
                    //スプレー入力
                    if (XboxController.GetButtonHoldA(PLAYER_NO) == true)
                    {
                        //スプレーモードで遷移先切り替え
                        switch (status.playerSprayMode)
                        {
                            case PlayerSprayControll.EPlayerSprayMode.SEED:
                                nextState = PlayerStatus.EStateTransition.SOWING_SEEDS;
                                break;
                            case PlayerSprayControll.EPlayerSprayMode.GRAW:
                                nextState = PlayerStatus.EStateTransition.GROWING;
                                break;
                            case PlayerSprayControll.EPlayerSprayMode.COLOR:
                                nextState = PlayerStatus.EStateTransition.SPRAY;
                                break;
                        }
                    }
                }//endスプレー切り替え@ActionMode

                Vector3 pos = transform.position;
                float sprayScalePercent = 0.0f;

                //色スプレーだけ特別処理で抜ける
                if (state == PlayerStatus.EStateTransition.SPRAY)
                {
                    //スプレーのあたり判定サイズは最初から最大
                    pos = transform.position + (transform.forward * 2.25f);
                    sprayCon.transform.position = pos;
                    sprayCon.ChangeScale(2.25f);
                    break;
                }

                //スプレーあたり判定のサイズを変化
                if (motionTimeCount < status.SPRAY_MAX_SCALE_NEED_TIME){
                    sprayScalePercent = motionTimeCount / status.SPRAY_MAX_SCALE_NEED_TIME;
                }
                else
                {
                    sprayScalePercent = 1.0f;
                }

                float _scall = 2.25f + (0.01f*playerManager.mathManager.GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.GREEN));  // 緑分
                _scall += 0.01f * playerManager.mathManager.GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.CYAN);                // シアン分
                _scall += 0.01f * playerManager.mathManager.GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.YELLOW);              // イエロー分
                _scall += 0.01f * playerManager.mathManager.GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.WHITE);               // ホワイト分
                pos = transform.position + (transform.forward * sprayScalePercent * _scall);
                sprayCon.transform.position = pos;
                sprayCon.ChangeScale(sprayScalePercent * _scall);

                //スプレー入力が無くなった
                if (XboxController.GetButtonHoldA(PLAYER_NO) == false)
                {
                    //移動入力があれば歩き状態へ切り替え
                    if (inputVec.magnitude > INPUT_MOVE_JUDGE_LENGTH)
                        nextState = PlayerStatus.EStateTransition.WALK;
                    else
                        nextState = PlayerStatus.EStateTransition.STAND;
                }

                //ビルが目の前に存在しつつＸボタンが押されていたら
                if (collisionBill.CheckCollisionBill() == true &&
                    XboxController.GetButtonHoldX(PLAYER_NO) == true)
                {
                    nextState = PlayerStatus.EStateTransition.GROWING_BILL;
                }

                //スプレーモード以外で移動開始や移動停止したらモーション切り替え
                if (state != PlayerStatus.EStateTransition.SPRAY)
                {
                    //移動停止したかチェック
                    if (moveFlg == true)
                    {
                        if (inputVec.magnitude < 0.1f)
                        {
                            moveFlg = false;
                            animCon.ChangeAnimation(state, moveFlg);
                        }
                    }
                    //移動開始したかチェック
                    else
                    {
                        if (inputVec.magnitude >= 0.1f)
                        {
                            moveFlg = true;
                            animCon.ChangeAnimation(state, moveFlg);
                        }
                    }
                }

                break;

            case PlayerStatus.EStateTransitionMode.BILL_ACTION:
                //ビルスプレー入力が無くなった
                if (XboxController.GetButtonHoldX(PLAYER_NO) == false)
                {
                    //移動入力があれば歩き状態へ切り替え
                    if (inputVec.magnitude > INPUT_MOVE_JUDGE_LENGTH)
                        nextState = PlayerStatus.EStateTransition.WALK;
                    else
                        nextState = PlayerStatus.EStateTransition.STAND;
                }
                break;

            case PlayerStatus.EStateTransitionMode.DAMAGE:
                break;

            default:
                //異常検知
                return -1;
        }

        //正常終了
        return 0;
    }
}
