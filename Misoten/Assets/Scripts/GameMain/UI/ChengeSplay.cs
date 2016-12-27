using UnityEngine;
using System.Collections;

public class ChengeSplay : MonoBehaviour {

    public GameObject[] m_Icon;           // 種まき
    public int m_frame;
    
    private Vector3 m_centerPos;        // 選択位置
    private Vector3 m_rightPos;         // 右位置
    private Vector3 m_leftPos;          // 左位置

    private bool m_moveFlg;             // 移動フラグ
    private bool m_moveDir;             // 移動向き true 右　false 左

    private float m_moveSpeed;          // 移動速度  

    private float m_selectY;
    private float m_selectX;
    private float m_scallUp;

    private float m_selectScall;
    private float m_defaultScall;

    private int m_frameCnt;

    private int m_selectNo;
    private int m_selectOld;
    private int m_selectOther;


    //===============================================================
    // 公開関数

    // Init - 初期化処理
    //---------------------------------
    //
    public void Init()
    {
        m_moveFlg = false;
        m_selectY = 5.0f / m_frame;
        m_selectX = 30.0f / m_frame;
        m_scallUp = 0.3f / m_frame;
        m_leftPos = new Vector3(-30.0f, -10.0f, 0);
        m_centerPos = new Vector3(0.0f, -5.0f, 0);
        m_rightPos = new Vector3(30.0f, -10.0f, 0);
        m_frameCnt = 0;
        m_selectNo = 1;


        m_Icon[0].transform.localPosition = m_leftPos;
        m_Icon[1].transform.localPosition = m_centerPos;
        m_Icon[2].transform.localPosition = m_rightPos;
        m_Icon[0].transform.localScale = new Vector3(0.4f, 0.4f, 1.0f);
        m_Icon[1].transform.localScale = new Vector3(0.7f, 0.7f, 1.0f);
        m_Icon[2].transform.localScale = new Vector3(0.4f, 0.4f, 1.0f);
    }

    // SplayChenge - スプレー切り替え演出 プレイヤー操作から呼び出される
    //---------------------------------
    //
    public void SplayChenge(bool _dir)
    {
        m_moveFlg = true;
        m_moveDir = _dir;
        m_selectOld = m_selectNo;
        if (_dir)
        {
            m_selectNo++;
            m_selectNo = m_selectNo % 3;
            if (m_selectNo == 2)
                m_selectOther = 0;
            else
                m_selectOther = m_selectNo + 1;
        }
        else
        {
            m_selectNo--;
            if (m_selectNo < 0)
            {
                m_selectNo = 2;
                m_selectOther = 1;
            }
            else if (m_selectNo == 0)
                m_selectOther = 2;
            else
                m_selectOther = 0;
        }
    }

    
    //===============================================================
    // 未公開関数

    // Use this for initialization
    void Start () {
        


    }
	
	// Update is called once per frame
	void Update () {
        if (m_moveFlg){
            move();
        }
	}


    

    // スプレー移動処理
    void move(){
        
        // 右と切り替え
        if (m_moveDir){
            // 右のが真ん中へ
            m_Icon[m_selectNo].transform.localPosition = new Vector3(m_Icon[m_selectNo].transform.localPosition.x - m_selectX, m_Icon[m_selectNo].transform.localPosition.y + m_selectY, m_Icon[m_selectNo].transform.localPosition.z);
            m_Icon[m_selectNo].transform.localScale = new Vector3(m_Icon[m_selectNo].transform.localScale.x + m_scallUp, m_Icon[m_selectNo].transform.localScale.y + m_scallUp, 1.0f);
            
            // 真ん中のが左へ
            m_Icon[m_selectOld].transform.localPosition = new Vector3(m_Icon[m_selectOld].transform.localPosition.x - m_selectX, m_Icon[m_selectOld].transform.localPosition.y - m_selectY, m_Icon[m_selectOld].transform.localPosition.z);
            m_Icon[m_selectOld].transform.localScale = new Vector3(m_Icon[m_selectOld].transform.localScale.x - m_scallUp, m_Icon[m_selectOld].transform.localScale.y - m_scallUp, 1.0f);
            m_frameCnt++;

            if(m_frameCnt > m_frame){
                m_frameCnt = 0;
                m_moveFlg = false;
                m_Icon[m_selectNo].transform.localPosition = m_centerPos;
                m_Icon[m_selectNo].transform.localScale = new Vector3(0.7f, 0.7f, 1.0f);
                m_Icon[m_selectOld].transform.localPosition = m_leftPos;
                m_Icon[m_selectOld].transform.localScale = new Vector3(0.4f, 0.4f, 1.0f);
                m_Icon[m_selectOther].transform.localPosition = m_rightPos;   
            }
        }
        else
        {
            // 左と切り替え
            // 左のが真ん中へ
            m_Icon[m_selectNo].transform.localPosition = new Vector3(m_Icon[m_selectNo].transform.localPosition.x + m_selectX, m_Icon[m_selectNo].transform.localPosition.y + m_selectY, m_Icon[m_selectNo].transform.localPosition.z);
            m_Icon[m_selectNo].transform.localScale = new Vector3(m_Icon[m_selectNo].transform.localScale.x + m_scallUp, m_Icon[m_selectNo].transform.localScale.y + m_scallUp, 1.0f);
            
            // 真ん中のが右へ
            m_Icon[m_selectOld].transform.localPosition = new Vector3(m_Icon[m_selectOld].transform.localPosition.x + m_selectX, m_Icon[m_selectOld].transform.localPosition.y - m_selectY, m_Icon[m_selectOld].transform.localPosition.z);
            m_Icon[m_selectOld].transform.localScale = new Vector3(m_Icon[m_selectOld].transform.localScale.x - m_scallUp, m_Icon[m_selectOld].transform.localScale.y - m_scallUp, 1.0f);
            m_frameCnt++;

            if (m_frameCnt > m_frame){
                m_frameCnt = 0;
                m_moveFlg = false;
                m_Icon[m_selectNo].transform.localPosition = m_centerPos;
                m_Icon[m_selectNo].transform.localScale = new Vector3(0.7f, 0.7f, 1.0f);
                m_Icon[m_selectOld].transform.localPosition = m_rightPos;
                m_Icon[m_selectOld].transform.localScale = new Vector3(0.4f, 0.4f, 1.0f);
                m_Icon[m_selectOther].transform.localPosition = m_leftPos;
            }
        }
    }
}
