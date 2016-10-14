using UnityEngine;
using System.Collections;

public class PlayerUnit : MonoBehaviour {

    //各種プレイヤーモジュール部品
    private PlayerCollisionActor collisionActor;    //衝突時の処理に対応
    private PlayerAnimCon animCon;                  //アニメーション管理
    private PlayerStatus status;                    //状態の管理
    private PlayerControll controll;                //プレイヤーの行動を担当

    private PlayerStatus.EStateTransition nextState;    //次回の状態遷移先

    //内部変数
    private float motionTimeCount;                  //モーションの経過時間(MAX 600sc)

    //公開変数
    public int PLAYER_NO;

	// Use this for initialization
	void Awake () {
        //部品取得
        collisionActor = transform.GetComponent<PlayerCollisionActor>();
        animCon = transform.GetComponent<PlayerAnimCon>();
        status = transform.GetComponent<PlayerStatus>();
        controll = transform.GetComponent<PlayerControll>();

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
	}

    public void StartPlayer(int _dummy)
    {
        nextState = PlayerStatus.EStateTransition.STAND;
        ChangeStateTransitionProcess();
    }
    public void StopPlayer(int _dummy)
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
            case PlayerStatus.EStateTransition.WEAPON_CHANGE:
                break;
            case PlayerStatus.EStateTransition.SOWING_SEEDS:
                break;
            case PlayerStatus.EStateTransition.GROWING:
                break;
            case PlayerStatus.EStateTransition.SPRAY:
                break;

            default:
                break;
        }
    }

    //状態毎に処理を行う
    private int EveryStatusUpdate(PlayerStatus.EStateTransition state, PlayerStatus.EStateTransitionMode stateMode)
    {
        //Vector2 inputVec = inputRun.GetMove();

        //詳細な状態毎の処理
        switch (state)
        {
            //system
            case PlayerStatus.EStateTransition.START:
                break;
            case PlayerStatus.EStateTransition.END:
                Vector2 vec = Vector2.zero;
                controll.SetMoveVec(vec);

                break;

            //normal
            case PlayerStatus.EStateTransition.STAND:
                /*
                //移動入力があれば歩き状態へ切り替え
                if (inputVec.magnitude > INPUT_MOVE_JUDGE_LENGTH)
                    nextState = PlayerStatus.EStateTransition.WALK;
                 */
                break;
            case PlayerStatus.EStateTransition.WALK:
                /*
                //移動処理
                controll.SetMoveVec(inputRun.GetMove());

                //移動入力がなければ立ち状態へ
                if (inputVec.magnitude <= INPUT_MOVE_JUDGE_LENGTH)
                    nextState = PlayerStatus.EStateTransition.STAND;
                else
                {
                    //移動入力があり、押し込みが深ければダッシュ状態へ
                    if (inputVec.y >= INPUT_MOVE_JUDGE_LENGTH && inputRun.IsDash() == true)
                    {
                        nextState = PlayerStatus.EStateTransition.DASH;
                    }
                }
                */
                break;
            case PlayerStatus.EStateTransition.RUN:
                /*
                //移動処理
                controll.SetMoveVec(inputRun.GetMove(), true);

                //移動入力がなければ立ち状態へ
                if (inputVec.magnitude <= INPUT_MOVE_JUDGE_LENGTH)
                    nextState = PlayerStatus.EStateTransition.STAND;
                else
                {
                    //移動入力があり、ダッシュボタンが押されていなければ歩き状態へ
                    if (inputVec.y < INPUT_MOVE_JUDGE_LENGTH || inputRun.IsDash() == false)
                    {
                        nextState = PlayerStatus.EStateTransition.WALK;
                    }
                }
                */
                break;

            //action
            case PlayerStatus.EStateTransition.WEAPON_CHANGE:
                //モーション終了
                if (motionTimeCount > 0.0f)
                    nextState = PlayerStatus.EStateTransition.STAND;

                break;
            case PlayerStatus.EStateTransition.SOWING_SEEDS:
                break;
            case PlayerStatus.EStateTransition.GROWING:
                break;
            case PlayerStatus.EStateTransition.SPRAY:
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
                break;
            case PlayerStatus.EStateTransitionMode.ACTION:
                break;

            default:
                //異常検知
                return -1;
        }

        //正常終了
        return 0;
    }
}
