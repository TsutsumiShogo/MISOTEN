using UnityEngine;
using System.Collections;

public class PlayerAnimCon : MonoBehaviour {

    //private PlayerUnit playerUnit;      //プレイヤーUnit
    private PlayerStatus playerStatus;  //プレイヤーステータス
    private PlayerCheckCollisionBill checkCollisionBill;    //ビル専用スプレー対象ビル
    private Animator animator;      //プレイヤーのAnimator本体

    public Camera playerCamera;     //追随カメラオブジェクト

    //パラメータ
    private float cameraDistancePercent;

	// Use this for initialization
	void Awake () {
        //playerUnit = transform.GetComponent<PlayerUnit>();
        playerStatus = transform.GetComponent<PlayerStatus>();
        checkCollisionBill = transform.GetComponentInChildren<PlayerCheckCollisionBill>();
        animator = transform.Find("Model").GetComponent<Animator>();

        Init();
	}
    public void Init()
    {
        cameraDistancePercent = 1.0f;
    }
	
	// Update is called once per frame
	void Update () {
        //カメラ制御
        if (playerCamera)
        {
            Vector3 camPosVec;
            camPosVec = Vector3.back * 4.0f;
            camPosVec += Vector3.up * 5.2f;

            //カメラの距離を計算
            if (playerStatus.GetStateTransition() == PlayerStatus.EStateTransition.GROWING_BILL)
            {
                //大ビルのカメラ距離
                if (checkCollisionBill.billParam.flowerType == GM_MathFlowerParam.EFlowerType.BigBill)
                {
                    cameraDistancePercent = 3.0f * 0.05f + cameraDistancePercent * 0.95f;
                }
                else//通常ビルのカメラ距離
                {
                    cameraDistancePercent = 2.0f * 0.05f + cameraDistancePercent * 0.95f;
                }
            }
            else//ビル成長じゃない通常のカメラ距離
            {
                cameraDistancePercent = 1.0f * 0.05f + cameraDistancePercent * 0.95f;
            }
            camPosVec = camPosVec * cameraDistancePercent;

            playerCamera.transform.position = transform.position + camPosVec;
        }
	}

    //Animatorにアニメーション切り替えを伝える。
    public void ChangeAnimation(PlayerStatus.EStateTransition state, bool _moveFlg)
    {
        if (state != PlayerStatus.EStateTransition.END)
        {
            animator.SetBool("MoveFlg", _moveFlg);
            animator.SetInteger("AnimNo", (int)state);
            animator.SetTrigger("AnimTrig");
        }
        else
        {
            animator.SetBool("MoveFlg", _moveFlg);
            animator.SetInteger("AnimNo", (int)PlayerStatus.EStateTransition.STAND);
            animator.SetTrigger("AnimTrig");
        }
    }

    //アニメーションの速さを変更する。
    public void ChangeAnimationSpeed(float speedPercent)
    {
        animator.speed = speedPercent;
    }
}
