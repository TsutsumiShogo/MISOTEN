using UnityEngine;
using System.Collections;

public class PlayerUnit : MonoBehaviour {

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

	// Use this for initialization
	void Awake () {
        //部品取得
        collisionActor = transform.GetComponent<PlayerCollisionActor>();
        animCon = transform.GetComponent<PlayerAnimCon>();
        status = transform.GetComponent<PlayerStatus>();
        controll = transform.GetComponent<PlayerControll>();

        sprayCon = transform.GetComponentInChildren<PlayerSprayControll>();
        collisionBill = transform.GetComponentInChildren<PlayerCheckCollisionBill>();

        nextState = PlayerStatus.EStateTransition.START;

        motionTimeCount = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
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

    public void StartPlayer()
    {
        nextState = PlayerStatus.EStateTransition.STAND;
        ChangeStateTransitionProcess();
    }
    public void StopPlayer()
    {
        nextState = PlayerStatus.EStateTransition.END;
        ChangeStateTransitionProcess();
    }

    //============================非公開関数=================================
    //状態を切り替える前の共通処理
    private void ChangeStateTransitionProcess()
    {
        //ステータスへ反映
        status.SetStateTransition(nextState);

        //アニメーションを切り替える
        //animCon.ChangeAnimation(status.GetStateTransition().status);

        //モーション切り替えに伴って時間初期化
        motionTimeCount = 0.0f;

        //状態切り替え時に何か特別な処理が必要であれば実行
        switch (nextState)
        {
            //system
            case PlayerStatus.EStateTransition.START:
                break;
            case PlayerStatus.EStateTransition.END:
                break;

            //normal
            case PlayerStatus.EStateTransition.STAND:
                break;
            case PlayerStatus.EStateTransition.WALK:
                break;
            case PlayerStatus.EStateTransition.RUN:
                break;

            //action
            case PlayerStatus.EStateTransition.SOWING_SEEDS:
                break;
            case PlayerStatus.EStateTransition.GROWING:
                break;
            case PlayerStatus.EStateTransition.SPRAY:
                break;

            //billAction
            case PlayerStatus.EStateTransition.GROWING_BILL:
                break;

            //damage
            case PlayerStatus.EStateTransition.KNOCKBACK:
                break;

            default:
                break;
        }
    }

    //状態毎に処理を行う
    private int EveryStatusUpdate(PlayerStatus.EStateTransition state, PlayerStatus.EStateTransitionMode stateMode)
    {
        //移動入力の方向を取得
        Vector2 inputVec = Vector2.zero;
        
        /*
        if(Input.GetKey(KeyCode.RightArrow) == true)
        {
            inputVec.x += 10.0f;
        }
        if(Input.GetKey(KeyCode.LeftArrow) == true)
        {
            inputVec.x -= 10.0f;
        }
        if(Input.GetKey(KeyCode.UpArrow) == true)
        {
            inputVec.y += 10.0f;
        }
        if(Input.GetKey(KeyCode.DownArrow) == true)
        {
            inputVec.y -= 10.0f;
        }
        */

        inputVec.x = XboxController.GetLeftX(PLAYER_NO);
        inputVec.y = XboxController.GetLeftY(PLAYER_NO);

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
                    nextState = PlayerStatus.EStateTransition.WALK;
                
                
                break;
            case PlayerStatus.EStateTransition.WALK:
                
                //移動処理
                controll.SetMoveVec(inputVec);

                //移動入力がなければ立ち状態へ
                if (inputVec.magnitude <= INPUT_MOVE_JUDGE_LENGTH)
                    nextState = PlayerStatus.EStateTransition.STAND;
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
                controll.SetMoveVec(inputVec);
                sprayCon.Spray(PlayerSprayControll.EPlayerSprayMode.SEED, 0.1f);
                break;
            case PlayerStatus.EStateTransition.GROWING:
                //移動処理
                controll.SetMoveVec(inputVec * 0.5f);
                sprayCon.Spray(PlayerSprayControll.EPlayerSprayMode.GRAW, 0.1f);
                break;
            case PlayerStatus.EStateTransition.SPRAY:
                //移動処理
                controll.SetMoveVec(inputVec * 0.75f);
                sprayCon.Spray(PlayerSprayControll.EPlayerSprayMode.COLOR, 0.1f);
                break;

            //billAction
            case PlayerStatus.EStateTransition.GROWING_BILL:
                //ビルを成長させる
                collisionBill.GrowthBill();
                break;

            //damage
            case PlayerStatus.EStateTransition.KNOCKBACK:
                if (motionTimeCount > 0.6f)
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
                //スプレー切り替え
                if (XboxController.GetButtonL(PLAYER_NO) == true)
                {
                    status.ChangeSprayMode(false);
                }
                if (XboxController.GetButtonR(PLAYER_NO) == true)
                {
                    status.ChangeSprayMode(true);
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

                //ビルが目の前に存在しつつＸボタンが押されていたら
                if(collisionBill.CheckCollisionBill() == true &&
                    XboxController.GetButtonHoldX(PLAYER_NO) == true)
                {
                    nextState = PlayerStatus.EStateTransition.GROWING_BILL;
                }

                break;
            case PlayerStatus.EStateTransitionMode.ACTION:
                //スプレーあたり判定のサイズを変化
                Vector3 pos = transform.position;
                Vector3 scale = Vector3.one;
                float sprayScalePercent = 0.0f;

                if (motionTimeCount < status.SPRAY_MAX_SCALE_NEED_TIME)
                {
                    sprayScalePercent = motionTimeCount / status.SPRAY_MAX_SCALE_NEED_TIME;
                }
                else
                {
                    sprayScalePercent = 1.0f;
                }
                scale.x = scale.z = sprayScalePercent;
                pos = transform.position + (transform.forward * sprayScalePercent * 2.25f);
                sprayCon.transform.position = pos;
                sprayCon.transform.localScale = scale;

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
