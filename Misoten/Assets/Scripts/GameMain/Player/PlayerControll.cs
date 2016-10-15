﻿using UnityEngine;
using System.Collections;

public class PlayerControll : MonoBehaviour {

    private PlayerStatus playerStatus;
    private Rigidbody rigid;
    private Vector3 moveVec;    //移動入力

	// Use this for initialization
	void Awake () {
        playerStatus = transform.GetComponent<PlayerStatus>();
        rigid = transform.GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //移動処理
    public void Move()
    {
        //高さ方向のベクトルの一時入れ物
        Vector3 yVector;

        //今までかかっていた重力を保存
        yVector = rigid.velocity;
        yVector.z = yVector.x = 0.0f;

        //移動処理
        rigid.velocity = yVector + moveVec;
        moveVec = moveVec * 0.8f;
    }

    //移動情報をセットする
    public void SetMoveVec(Vector2 stick)
    {
        //移動ベクトルを保存
        moveVec.x = stick.x;
        moveVec.z = stick.y;
        moveVec.y = 0.0f;

        //方向を変更
        Quaternion rot = transform.rotation;

        //0ベクトル回避
        if (stick.x != 0 && stick.y != 0)
        {
            //回転を計算
            float angle;
            angle = Mathf.Atan2(stick.y, stick.x);
            rot = Quaternion.AngleAxis(angle, Vector3.up);
            
            //回転情報を代入
            transform.rotation = rot;
        }
    }
}