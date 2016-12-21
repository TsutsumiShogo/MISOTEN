using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Header : RE_BaseAction {

    private float m_defaultScall = 1.0f;
    private float m_maxScall = 3.0f;
    private float m_time = 1.0f;
    private bool m_scallFlg = false;
    //===============================================================
    // 公開関数

    // Init - 初期化
    //---------------------------------
    //
    public void Init(){
        m_scallFlg = true;
        transform.localScale = new Vector3(0, 1, 1);
    }

    // Action - 更新処理
    //---------------------------------
    // 
    public TotalScore.RE_TOTAL_STEP Action()
    {
        if (m_scallFlg){
            transform.localScale = new Vector3(transform.localScale.x + (3.0f * Time.deltaTime / m_time), 1, 1);
            if(transform.localScale.x >= m_maxScall){
                transform.localScale = new Vector3(3, 1, 1);
                m_scallFlg = false;
            }
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x - (2.0f * Time.deltaTime / m_time), 1, 1);
            if (transform.localScale.x <= m_defaultScall)
            {
                transform.localScale = new Vector3(1, 1, 1);
                m_scallFlg = false;
                return TotalScore.RE_TOTAL_STEP.SCORE_IN;
            }
        }
        return TotalScore.RE_TOTAL_STEP.HEADER_IN;
    }

    //===============================================================
    // 未公開関数
  
	
}
