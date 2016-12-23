using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RE_PlayerRank : MonoBehaviour {

    private float m_overScall = 0.2f;
    private float m_defaultScall = 1.0f;
    private float m_speed = 1.0f;
    private float m_timer = 0.0f;
    private bool m_upFlg = false;
    private bool m_onFlg = false;
    public Color[] m_color = new Color[3];
    //===============================================================
    // 公開関数

    // Init - 初期化
    //---------------------------------
    //
    public void Init(){
        m_upFlg = false;
        m_onFlg = false;
        transform.localScale = new Vector3(0, 0, 1);
        
    }

    // OnScall - 順位表示開始
    //---------------------------------
    public void OnScall(int _rank){
        GetComponent<Text>().text = _rank.ToString() + "位";
        GetComponent<Text>().color = m_color[_rank - 1];
        m_onFlg = true;
    }

    //===============================================================
    // 未公開関数

	// Update - 毎フレーム更新
    //---------------------------------
	void Update () {
		if( m_onFlg){
            if (!m_upFlg){
                // 拡大出現
                transform.localScale = new Vector3(
                    transform.localScale.x + (m_defaultScall + m_overScall) * Time.deltaTime / m_speed,
                    transform.localScale.y + (m_defaultScall + m_overScall) * Time.deltaTime / m_speed,
                    1);
                if(transform.localScale.x >= (m_defaultScall + m_overScall)){
                    transform.localScale = new Vector3((m_defaultScall + m_overScall), (m_defaultScall + m_overScall), 1);
                    m_upFlg = true;
                }
            }
            else
            {
                // オーバー分縮小
                transform.localScale = new Vector3(
                    transform.localScale.x - m_overScall*Time.deltaTime / 0.2f,
                    transform.localScale.y - m_overScall*Time.deltaTime / 0.2f,
                    1);
                if (transform.localScale.x <= m_defaultScall)
                {
                    transform.localScale = new Vector3(m_defaultScall, m_defaultScall, 1);
                    m_onFlg = false;
                }
            }
        }
	}
}
