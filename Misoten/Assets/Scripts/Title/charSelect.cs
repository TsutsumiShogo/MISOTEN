using UnityEngine;
using System.Collections;

public class charSelect : MonoBehaviour {

    public int m_selectNo = 0;
    public int m_defaultNo = 0;
    public int m_playerId;
    public GameObject[] m_char;
    public GameObject[] m_cursor;
    public GameObject m_decision;
    public GameObject[] m_otherChar;
    private bool moveLeft;
    private bool moveRight;
    private float m_nowPos;
    public float m_moveTime = 0.5f; // 移動にかかる時間
    public Vector3[] m_default = new Vector3[3];
    private Vector3 m_bigScall;        // 拡大サイズ
    private Vector3 m_defaultScall;    // 縮小サイズ
    public bool m_decisionFlg;
    

    //===============================================================
    // 公開関数　CharacterSelectManagerで呼び出す

    //---------------------------------
    // Init 初期化処理
	public void Init () {
        
        moveLeft = false;
        moveRight = false;

        // デフォルト座標保持
        //m_default[0] = m_char[2].transform.localPosition;
        //m_default[1] = m_char[0].transform.localPosition;
        //m_default[2] = m_char[1].transform.localPosition;

        m_selectNo = m_defaultNo;

        switch (m_selectNo){
            case 0:
                m_char[0].transform.localPosition = new Vector3( m_default[1].x, m_char[0].transform.localPosition.y, m_char[0].transform.localPosition.z);
                m_char[1].transform.localPosition = new Vector3( m_default[2].x, m_char[1].transform.localPosition.y, m_char[1].transform.localPosition.z);
                m_char[2].transform.localPosition = new Vector3( m_default[0].x, m_char[2].transform.localPosition.y, m_char[2].transform.localPosition.z);
                break;
            case 1:
                m_char[0].transform.localPosition = new Vector3( m_default[0].x, m_char[0].transform.localPosition.y, m_char[0].transform.localPosition.z);
                m_char[1].transform.localPosition = new Vector3( m_default[1].x, m_char[1].transform.localPosition.y, m_char[1].transform.localPosition.z);
                m_char[2].transform.localPosition = new Vector3( m_default[2].x, m_char[2].transform.localPosition.y, m_char[2].transform.localPosition.z);
                break;
            case 2:
                m_char[0].transform.localPosition = new Vector3( m_default[2].x, m_char[0].transform.localPosition.y, m_char[0].transform.localPosition.z);
                m_char[1].transform.localPosition = new Vector3( m_default[0].x, m_char[1].transform.localPosition.y, m_char[1].transform.localPosition.z);
                m_char[2].transform.localPosition = new Vector3( m_default[1].x, m_char[2].transform.localPosition.y, m_char[2].transform.localPosition.z);
                break;
        }   

        m_defaultScall = m_cursor[0].transform.localScale;
        m_bigScall = new Vector3(30,38,1);
        m_decision.SetActive(false);
        m_decisionFlg = false;
	}
	
    //---------------------------------
    // Action 更新処理
    public void Action() {
        Selecting();
        MoveAction();
	}

    //---------------------------------
    // Selecting 選択処理
    private void Selecting()
    {
        //------------------
        // アクション中でなければ移動
        if (!moveLeft && !moveRight && !m_decisionFlg)
        {
            //------------------
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
            //------------------
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
            //------------------
            // 選択
            if (XboxController.GetButtonA(m_playerId)){
                if (m_otherChar[0].GetComponent<charSelect>().m_decisionFlg) {
                    if (m_otherChar[0].GetComponent<charSelect>().m_selectNo == this.m_selectNo)
                        return;
                }
                if (m_otherChar[1].GetComponent<charSelect>().m_decisionFlg){
                    if (m_otherChar[1].GetComponent<charSelect>().m_selectNo == this.m_selectNo)
                        return;
                }
                GM_StaticParam.g_selectCharacter[m_playerId] = m_selectNo;
                m_decisionFlg = true;
                m_decision.SetActive(true);
            }  
        }
        //---------------------
        // B押下で選択取り消し
        if (XboxController.GetButtonB(m_playerId)){
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
                m_char[2].transform.localPosition = new Vector3( m_default[0].x, m_char[2].transform.localPosition.y, m_char[2].transform.localPosition.z);
                m_char[1].transform.localPosition = new Vector3( m_default[2].x, m_char[1].transform.localPosition.y, m_char[1].transform.localPosition.z);
                break;
                // プレイヤー2
            case 1:
                m_char[0].transform.localPosition = new Vector3( m_default[0].x, m_char[0].transform.localPosition.y, m_char[0].transform.localPosition.z);
                m_char[2].transform.localPosition = new Vector3( m_default[2].x, m_char[2].transform.localPosition.y, m_char[2].transform.localPosition.z);
                break;
                // プレイヤー3
            case 2:
                m_char[1].transform.localPosition = new Vector3( m_default[0].x, m_char[1].transform.localPosition.y, m_char[1].transform.localPosition.z);
                m_char[0].transform.localPosition = new Vector3( m_default[2].x, m_char[0].transform.localPosition.y, m_char[0].transform.localPosition.z);
                break;
        }
    }

    //---------------------------------
    // MoveAction　キャラクタ移動＋カーソル演出
    private void MoveAction(){
        
        //
        if (moveLeft){
            for(int i= 0; i<3;i++){
                m_char[i].transform.localPosition = new Vector3(m_char[i].transform.localPosition.x - ((m_default[2].x - m_default[1].x)*Time.deltaTime/m_moveTime), m_char[i].transform.localPosition.y, m_char[i].transform.localPosition.z);
            }
            if (m_char[m_selectNo].transform.localPosition.x <= m_default[1].x)
            {
                m_char[m_selectNo].transform.localPosition = new Vector3(　m_default[1].x , m_char[m_selectNo].transform.localPosition.y, m_char[m_selectNo].transform.localPosition.z);
                m_cursor[0].transform.localScale = m_defaultScall;
                moveLeft = false;
            }
        }

        // 
        if (moveRight){
            for (int i = 0; i < 3; i++){
                m_char[i].transform.localPosition = new Vector3(m_char[i].transform.localPosition.x + ((m_default[2].x - m_default[1].x) * Time.deltaTime / m_moveTime), m_char[i].transform.localPosition.y, m_char[i].transform.localPosition.z);
            }
            if (m_char[m_selectNo].transform.localPosition.x >= m_default[1].x){
                m_char[m_selectNo].transform.localPosition = new Vector3(m_default[1].x, m_char[m_selectNo].transform.localPosition.y, m_char[m_selectNo].transform.localPosition.z);
                m_cursor[1].transform.localScale = m_defaultScall;
                moveRight = false;
            }
        }
    }
}
