using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RE_Bonus : MonoBehaviour {

    private bool m_openWindow = false;  // ウィンドウ出現
    [SerializeField]
    private GameObject m_mathManager;                   // マスManager

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
        for (int i = 0; i < m_bar.Length; i++){
            m_bar[i].GetComponent<RE_BonusBar>().Init();
        }
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
                    SoundManager.PlaySe("cursol", 1);
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
        m_clearCnt = 0;
        CheckColorNum();        // カラー数
        if( m_clearCnt == 0){
            m_bar[0].GetComponent<RE_BonusBar>().OnActive(m_barPos[0]);
        }
    }
    
    // CheckColorNUm - 何色花が植えられているか
    //---------------------------------
    //
    private void CheckColorNum(){
        int _colorNum = 0;
        for (int i = 0; i < 7; i++) {
            switch (i) {
                case 0:
                    if( m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.RED) > 0){
                        _colorNum++;
                    }
                    break;
                case 1:
                    if (m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.GREEN) > 0)
                    {
                        _colorNum++;
                    }
                    break;
                case 2:
                    if (m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.BLUE) > 0)
                    {
                        _colorNum++;
                    }
                    break;
                case 3:
                    if (m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.CYAN) > 0)
                    {
                        _colorNum++;
                    }
                    break;
                case 4:
                    if (m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.MAGENTA) > 0)
                    {
                        _colorNum++;
                    }
                    break;
                case 5:
                    if (m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.WHITE) > 0)
                    {
                        _colorNum++;
                    }
                    break;
                case 6:
                    if (m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.YELLOW) > 0)
                    {
                        _colorNum++;
                    }
                    break;
            }
        
        }
        // 7色
        if(  _colorNum >= 7){
            m_bar[1].GetComponent<RE_BonusBar>().OnActive(m_barPos[0]);
            GameObject.Find("RE_TotalScore").GetComponent<TotalScore>().m_afterScore += GameObject.Find("RE_TotalScore").GetComponent<TotalScore>().m_afterScore*0.3f;
            m_clearCnt++;
        }
        else if(_colorNum >= 5){
            m_bar[2].GetComponent<RE_BonusBar>().OnActive(m_barPos[0]);
            GameObject.Find("RE_TotalScore").GetComponent<TotalScore>().m_afterScore += GameObject.Find("RE_TotalScore").GetComponent<TotalScore>().m_afterScore * 0.2f;
            m_clearCnt++;
        }
        else if(_colorNum >= 3){
            m_bar[3].GetComponent<RE_BonusBar>().OnActive(m_barPos[0]);
            GameObject.Find("RE_TotalScore").GetComponent<TotalScore>().m_afterScore += GameObject.Find("RE_TotalScore").GetComponent<TotalScore>().m_afterScore * 0.1f;
            m_clearCnt++;
        }
        Debug.Log("colorCHECK");
    }

}
