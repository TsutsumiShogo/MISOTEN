using UnityEngine;
using System.Collections;

public class SelectMode : MonoBehaviour {

    public GameObject[] m_signBoards;   // 看板オブジェクト
    public GameObject m_mainCamera;     // メインカメラ  

    private bool m_scallUpDownFlg;      // true なら拡大、false なら縮小
    private bool m_decideFlg;           // 決定フラグ   

    private float m_maxScall;           // 最大サイズ
    private float m_defaultScall;       // 初期サイズ
    private float m_speed;              // 拡縮速度
    private int m_no;

    private int     m_cnt;
    private bool    m_scallFlg;

	// Use this for initialization
	void Start () {
        m_defaultScall = m_signBoards[0].transform.localScale.x;
        m_maxScall = 1.1f;
        m_speed = 0.005f;
        m_cnt = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (m_mainCamera.GetComponent<CameraMove>().CharaSelectFlg)
            m_no = 0;   // プレイ選択状態なら
        else
            m_no = 1;   // ランキング選択状態なら

        ScallAction();  // 拡縮演出

	}

    //-----------------------------------------------
    // 選択状態　拡縮演出
    //-----------------------------------------------
    void ScallAction()
    {
        // 
        if (m_scallUpDownFlg){
            m_signBoards[m_no].transform.localScale = 
                new Vector3( m_signBoards[m_no].transform.localScale.x + m_speed,
                            m_signBoards[m_no].transform.localScale.y + m_speed, 
                            m_signBoards[m_no].transform.localScale.z + m_speed);
            if (m_signBoards[m_no].transform.localScale.x >= m_maxScall){
                m_scallUpDownFlg = false;
                m_signBoards[m_no].transform.localScale = new Vector3(m_maxScall, m_maxScall, m_maxScall);
            }
        }
        else
        {
            m_signBoards[m_no].transform.localScale =
             new Vector3(m_signBoards[m_no].transform.localScale.x - m_speed,
                         m_signBoards[m_no].transform.localScale.y - m_speed,
                         m_signBoards[m_no].transform.localScale.z - m_speed);
            if (m_signBoards[m_no].transform.localScale.x <= m_defaultScall)
            {
                m_scallUpDownFlg = true;
                m_signBoards[m_no].transform.localScale = new Vector3(m_defaultScall, m_defaultScall, m_defaultScall);
            }
        }
    }

    //-----------------------------------------------
    // 決定時　アクション
    //-----------------------------------------------
    public void DesideAction(){
        if (m_cnt < 10)
        {
            if (m_scallFlg)
            {
                m_signBoards[m_no].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                m_scallFlg = false;
                m_cnt++;
            }
            else
            {
                m_signBoards[m_no].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                m_scallFlg = true;
                m_cnt++;
            }
        }
    }
}
