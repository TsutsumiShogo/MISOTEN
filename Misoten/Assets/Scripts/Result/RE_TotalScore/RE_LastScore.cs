using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RE_LastScore : MonoBehaviour {

    private float m_score;
    private float m_afterScore;
    //===============================================================
    // 公開関数

    // Init -初期化
    //---------------------------------
    //
    public void Init(){
        m_score = GameObject.Find("RE_TotalScore").GetComponent<TotalScore>().m_score; // スコア
        m_afterScore = GameObject.Find("RE_TotalScore").GetComponent<TotalScore>().m_afterScore; // スコア

        // ランキング更新処理
        Debug.Log("Rank");
        if (RankingManager.CheckRankIn((int)m_afterScore)){
            RankingManager.UpdateRanking((int)m_afterScore, "NEW");
            SaveContainer.Save();
        }
        // スコアセット
        transform.FindChild("score").GetComponent<Text>().text = string.Format("{0:#,##0}", m_score);
        transform.GetComponent<TextInEffect>().Init();
    }

    // Action - 更新処理
    //---------------------------------
    //
    public TotalScore.RE_TOTAL_STEP Action()
    {
        if( !transform.GetComponent<TextInEffect>().m_goalFlg){
            transform.GetComponent<TextInEffect>().Action();
        }
        else
        {
            if( m_score < m_afterScore){
                m_score++;
            }
            else
            {
                return TotalScore.RE_TOTAL_STEP.NEXT_BUTTON;
            }
        }

        return TotalScore.RE_TOTAL_STEP.LASTSCORE_IN;
        
    }
     


    //===============================================================
    // 未公開関数

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
