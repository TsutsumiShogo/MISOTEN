using UnityEngine;
using System.Collections;

public class charSelect : MonoBehaviour {

    public int m_selectNo = 0;
    public int m_playerId;
    public GameObject[] m_char;
    public GameObject[] m_cursor;
    public GameObject m_decision;
    public GameObject[] m_otherChar;
    private bool moveLeft;
    private bool moveRight;
    private float m_nowPos;
    public float m_speed;
    private Vector3[] m_default = new Vector3[3];
    private Vector3 m_bigScall;        // 拡大サイズ
    private Vector3 m_defaultScall;    // 縮小サイズ
    private bool m_decisionFlg;


	// Use this for initialization
	void Start () {
        moveLeft = false;
        moveRight = false;

        // デフォルト座標保持
        m_default[0] = m_char[2].transform.position;
        m_default[1] = m_char[0].transform.position;
        m_default[2] = m_char[1].transform.position;
        m_defaultScall = m_cursor[0].transform.localScale;
        m_bigScall = new Vector3(30,38,1);
        m_decision.SetActive(false);
        m_decisionFlg = false;
	}
	
	// Update is called once per frame
	void Update () {
        Selecting();
        moveAction();
	}

    //---------------------------------------------------------------
    // 選択処理
    //---------------------------------------------------------------
    void Selecting()
    {
        // アクション中でなければ移動
        if (!moveLeft && !moveRight && !m_decisionFlg)
        {
            // 左押
            if (XboxController.GetLeftTriggerLeft(m_playerId))
            {
                SetPosition();
                m_selectNo++;
                m_selectNo = m_selectNo % 3;
                m_nowPos = m_default[m_selectNo].x;
                m_cursor[0].transform.localScale = m_bigScall; 
                moveLeft = true;
            }
            // 右押
            if (XboxController.GetLeftTriggerRight(m_playerId))
            {
                SetPosition();
                m_selectNo--;
                if (m_selectNo < 0) m_selectNo = 2;
                m_nowPos = m_default[m_selectNo].x;
                m_cursor[1].transform.localScale = m_bigScall; 
                moveRight = true;
            }
            // 選択
            if (XboxController.GetButtonA(m_playerId))
            {
                if (m_otherChar[0].GetComponent<charSelect>().m_decisionFlg) {
                    if (m_otherChar[0].GetComponent<charSelect>().m_selectNo == this.m_selectNo)
                        return;
                }
                if (m_otherChar[1].GetComponent<charSelect>().m_decisionFlg)
                {
                    if (m_otherChar[1].GetComponent<charSelect>().m_selectNo == this.m_selectNo)
                        return;
                }
                m_decisionFlg = true;
                m_decision.SetActive(true);
            }  
        }
        if (XboxController.GetButtonB(m_playerId))
        {
            m_decisionFlg = false;
            m_decision.SetActive(false);
        }

    }

    //---------------------------------------------------------------
    // 座標設定
    //---------------------------------------------------------------
    void SetPosition(){
        
        // 
        switch (m_selectNo)
        {
                // プレイヤー1
            case 0:
                m_char[2].transform.position = m_default[0];
                m_char[1].transform.position = m_default[2];
                break;
                // プレイヤー2
            case 1:
                m_char[0].transform.position = m_default[0];
                m_char[2].transform.position = m_default[2];
                break;
                // プレイヤー3
            case 2:
                m_char[1].transform.position = m_default[0];
                m_char[0].transform.position = m_default[2];
                break;
        }
    }
    //---------------------------------------------------------------
    // 動く
    //---------------------------------------------------------------
    void moveAction(){
        
        //   
        if (moveLeft){
            for(int i= 0; i<3;i++){
                m_char[i].transform.position = new Vector3(m_char[i].transform.position.x - m_speed, m_char[i].transform.position.y, m_char[i].transform.position.z);
            }
            if ( m_char[m_selectNo].transform.position.x <= m_default[1].x ){
                m_char[m_selectNo].transform.position = m_default[1];
                m_cursor[0].transform.localScale = m_defaultScall;
                moveLeft = false;
            }
        }

        // 
        if (moveRight){
            for (int i = 0; i < 3; i++){
                m_char[i].transform.position = new Vector3(m_char[i].transform.position.x + m_speed, m_char[i].transform.position.y, m_char[i].transform.position.z);
            }
            if (m_char[m_selectNo].transform.position.x >= m_default[1].x){
                m_char[m_selectNo].transform.position = m_default[1];
                m_cursor[1].transform.localScale = m_defaultScall;
                moveRight = false;
            }
        }
    }
}
