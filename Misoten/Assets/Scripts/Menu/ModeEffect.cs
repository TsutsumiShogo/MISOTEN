using UnityEngine;
using System.Collections;

public class ModeEffect : MonoBehaviour {

    //----------------------------
    // 拡縮演出用　
    public float m_maxScall;                // 最大サイズ
    public float m_scallMag = 1.5f;         // 倍率    
    public float m_defaultScall;            // 標準サイズ
    public float m_speedPer = 20.0f;                // 速度の割合
    private float m_speed;                   // 拡縮速度
    private bool m_UpDownFlg;               // true で拡大、falseで縮小
    private bool m_scallingFlg = false;     // 拡縮演出フラグ


	
	//===============================================================
    // 未公開関数　

    //---------------------------------
    // Update 更新処理
	void Update () {
        if (m_scallingFlg){
            Vector3 _scall;
            //-------------------------
            // 拡縮処理
            if (m_UpDownFlg)
            {   
                //-----------------------
                // 拡大
                _scall.x = transform.localScale.x + m_speed;
                _scall.y = transform.localScale.y + m_speed;
                _scall.z = 1;
                transform.localScale = _scall;
                if (_scall.x >= m_maxScall) {
                    m_UpDownFlg = false;
                    transform.localScale = new Vector3(m_maxScall, m_maxScall, m_maxScall);
                }
            }
            else
            {
                //-----------------------
                // 縮小
                _scall.x = transform.localScale.x - m_speed;
                _scall.y = transform.localScale.y - m_speed;
                _scall.z = 1;
                transform.localScale = _scall;
                if (_scall.x <= m_defaultScall)
                {
                    m_UpDownFlg = true;
                    transform.localScale = new Vector3(m_defaultScall, m_defaultScall, m_defaultScall);
                }
            }
        }
	}

    //===============================================================
    // 公開関数　MenuManagerで呼び出す

    //---------------------------------
    // Init 初期化処理
    public void Init() {
        m_defaultScall = transform.localScale.x;
        m_maxScall = m_defaultScall * m_scallMag;
        m_speed = (m_maxScall - m_defaultScall) / m_speedPer;
       
    }

    //---------------------------------
    // OnScalling 拡縮開始命令
    public void OnScalling(){
        m_scallingFlg = true;
    }

    //---------------------------------
    // OffScalling 拡縮停止命令
    public void OffScalling(){
        m_scallingFlg = false;
        transform.localScale = new Vector3( m_defaultScall, m_defaultScall, m_defaultScall);
    }
}
