using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Bonus : MonoBehaviour {

    private bool m_openWindow = false;  // ウィンドウ出現


    //===============================================================
    // 公開関数

    // Init - 初期化処理
    //---------------------------------
    //
    public void Init(){
        m_openWindow = false;
        transform.localScale = new Vector3(0,1,1);
    }

    // Action - 更新処理
    //---------------------------------
    //
    public TotalScore.RE_TOTAL_STEP Action(){
        if (!m_openWindow)
        {
            OpenWindow();
        }else{
            return TotalScore.RE_TOTAL_STEP.LASTSCORE_IN;
        }

        return TotalScore.RE_TOTAL_STEP.BONUS_IN;
    }
    //===============================================================
    // 未公開関数
    private void OpenWindow(){
        transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime, 1, 1);
        if (transform.localScale.x > 1.0f)
        {
            m_openWindow = true;
        }
    }
    
}
