using UnityEngine;
using System.Collections;

public class PlayerControll : MonoBehaviour {

    private PlayerStatus playerStatus;
    private Rigidbody rigid;
    private Vector3 moveVec;    //移動入力
    private float notMoveTime;  //移動入力を受け付けない時間

	// Use this for initialization
	void Awake () {
        playerStatus = transform.GetComponent<PlayerStatus>();
        rigid = transform.GetComponent<Rigidbody>();
        notMoveTime = 0;
	}
    public void Init()
    {
        notMoveTime = 0;
    }
	// Update is called once per frame
	void Update () {
        if (notMoveTime > 0.0f)
        {
            notMoveTime -= Time.deltaTime;
        }
	}

    //移動処理
    public void Move()
    {
        //ベクトルの一時入れ物
        Vector3 yVector;
        Vector3 xzVector;

        //今までかかっていた重力を保存
        yVector = xzVector = rigid.velocity;
        yVector.z = yVector.x = 0.0f;
        xzVector.y = 0.0f;

        //移動処理
        rigid.velocity = yVector + xzVector * 0.85f + (moveVec * playerStatus.DEFAULT_MOVE_SPEED) * 0.15f;
        //rigid.MovePosition( transform.position + (xzVector * 0.85f + (moveVec * playerStatus.DEFAULT_MOVE_SPEED * Time.deltaTime) * 0.15f));
        moveVec = moveVec * 0.95f;
    }

    //移動情報をセットする
    public bool SetMoveVec(Vector2 stick)
    {
        //移動入力を許可しない時間がセットされていた
        if (notMoveTime > 0.0f)
        {
            return false;
        }

        //移動ベクトルを保存
        moveVec.x = stick.x;
        moveVec.z = stick.y;
        moveVec.y = 0.0f;

        if (moveVec.magnitude > 1.0f)
        {
            moveVec = moveVec.normalized;
        }

        //方向を変更
        Quaternion rot = transform.rotation;

        //0ベクトル回避
        if (stick.x != 0 || stick.y != 0)
        {
            //回転を計算
            float angle;
            angle = Mathf.Atan2(stick.x, stick.y);
            rot = Quaternion.AngleAxis(angle/3.1415f*180.0f, Vector3.up);
            
            //回転情報を代入
            transform.rotation = rot;
        }

        return true;
    }

    public void SetVector(Vector3 _vec)
    {
        rigid.velocity = _vec;
    }
    public void SetNotMoveTime(float _time)
    {
        notMoveTime = _time;
        moveVec = Vector3.zero;
    }
}
