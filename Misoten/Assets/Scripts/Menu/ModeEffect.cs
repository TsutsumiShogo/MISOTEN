using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ModeEffect : MonoBehaviour {

    //----------------------------
    // 拡縮演出用　
    public float m_maxScall;                // 最大サイズ
    public float m_scallMag = 1.5f;         // 倍率    
    public float m_defaultScall;            // 標準サイズ
    public float m_time = 1.0f;        // 速度の割合
    public Color m_selectColor;             // 選択時カラー
    public Color m_defaultColor;            // 通常時カラー
    private float m_speed;                  // 拡縮速度
    private bool m_UpDownFlg;               // true で拡大、falseで縮小
    private bool m_scallingFlg = false;     // 拡縮演出フラグ


	
	//===============================================================
    // 未公開関数　

    //---------------------------------
    // Update 更新処理
	
    //===============================================================
    // 公開関数　MenuManagerで呼び出す

    //---------------------------------
    // Init 初期化処理
    public void Init() {
        m_defaultScall = 1;
        m_maxScall = m_defaultScall * m_scallMag;
        //m_speed = (m_maxScall - m_defaultScall) / m_speedPer;
       
    }

    //---------------------------------
    // Action 更新処理
    public void Action()
    {
        if (m_scallingFlg)
        {
            Vector3 _scall;
            //-------------------------
            // 拡縮処理
            if (m_UpDownFlg)
            {
                //-----------------------
                // 拡大
                _scall.x = transform.localScale.x + (m_maxScall - m_defaultScall)*Time.deltaTime/m_time;
                _scall.y = transform.localScale.y + (m_maxScall - m_defaultScall) * Time.deltaTime / m_time;
                _scall.z = 1;
                transform.localScale = _scall;
                if (_scall.x >= m_maxScall)
                {
                    m_UpDownFlg = false;
                    transform.localScale = new Vector3(m_maxScall, m_maxScall, m_maxScall);
                }
            }
            else
            {
                //-----------------------
                // 縮小
                _scall.x = transform.localScale.x - (m_maxScall - m_defaultScall) * Time.deltaTime / m_time;
                _scall.y = transform.localScale.y - (m_maxScall - m_defaultScall) * Time.deltaTime / m_time;
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

    //---------------------------------
    // OnScalling 拡縮開始命令
    public void OnScalling(){
        m_scallingFlg = true;
    }
    //---------------------------------
    // OnScalling 拡縮開始命令
    public void OnScalling( bool _color )
    {
        m_scallingFlg = true;
        if (_color){
            GetComponent<Text>().color = m_selectColor;
            GetComponent<Outline>().effectColor = m_defaultColor;
        }
    }

    //---------------------------------
    // OffScalling 拡縮停止命令
    public void OffScalling(){
        m_scallingFlg = false;
        transform.localScale = new Vector3( m_defaultScall, m_defaultScall, m_defaultScall);
    }
    //---------------------------------
    // OffScalling 拡縮停止命令
    public void OffScalling(bool _color)
    {
        m_scallingFlg = false;
        transform.localScale = new Vector3(m_defaultScall, m_defaultScall, m_defaultScall);
        if (_color)
        {
            GetComponent<Text>().color = m_defaultColor;
            GetComponent<Outline>().effectColor = new Color(1,1,1,1);
        }
    }
}
