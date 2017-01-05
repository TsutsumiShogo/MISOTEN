using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Bonus : MonoBehaviour {

    private bool m_openWindow = false;  // ウィンドウ出現

    [SerializeField]
    private GameObject[] m_bar = new GameObject[1];     // ボーナスバー
    private int m_clearCnt;                             // ボーナス数
    private float[] m_barPos = new float[3];            // バーのポジション
    private int m_actionNo;
    private float m_timer;
    //===============================================================
    // 公開関数

    // Init - 初期化処理
    //---------------------------------
    //
    public void Init(){
        m_openWindow = false;
        transform.localScale = new Vector3(0,1,1);
        m_clearCnt = 0;
        m_bar[0].GetComponent<RE_BonusBar>().Init();
        GameObject.Find("RE_TotalScore").GetComponent<TotalScore>().m_afterScore = GameObject.Find("RE_TotalScore").GetComponent<TotalScore>().m_score;
        m_actionNo = 0;
        m_timer = 0.0f;
        // 座標設定
        m_barPos[0] = 75.0f;

        CheckBonus();

    }

    // Action - 更新処理
    //---------------------------------
    //
    public TotalScore.RE_TOTAL_STEP Action(){
        if (!m_openWindow)
        {
            OpenWindow();
        }else{
            if( m_actionNo <= m_clearCnt){
                if( m_bar[m_actionNo].GetComponent<RE_BonusBar>().Action()){
                    m_actionNo++;
                }
            }else{
                m_timer += Time.deltaTime;
                if (m_timer > 1.0f)
                {
                    return TotalScore.RE_TOTAL_STEP.LASTSCORE_IN;
                }
            }
            
        }

        return TotalScore.RE_TOTAL_STEP.BONUS_IN;
    }
    //===============================================================
    // 未公開関数

    // OpenWindow - ウィンドウ出現処理
    //--------------------------------
    //
    private void OpenWindow(){
        transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime, 1, 1);
        if (transform.localScale.x > 1.0f)
        {
            m_openWindow = true;
        }
    }

    // CheckBonus - ミッションボーナスのクリアチェック
    //---------------------------------
    //
    private void CheckBonus(){
        if( m_clearCnt == 0){
            m_bar[0].GetComponent<RE_BonusBar>().OnActive(m_barPos[0]);
        }
    }
    
}
