using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RE_LastScore : MonoBehaviour {

    private float m_score;
    private float m_afterScore;
    private float m_rollTimer;
    private bool m_rollSeFlg;
    //===============================================================
    // 公開関数

    // Init -初期化
    //---------------------------------
    //
    public void Init(){
        m_rollSeFlg = false;
        m_rollTimer = 0.0f;
        m_score = GameObject.Find("RE_TotalScore").GetComponent<TotalScore>().m_score; // スコア
        m_afterScore = GameObject.Find("RE_TotalScore").GetComponent<TotalScore>().m_afterScore; // スコア

        // ランキング更新処理
        Debug.Log("Rank");
        if (RankingManager.CheckRankIn((int)m_afterScore)){
            RankingManager.UpdateRanking((int)m_afterScore, "No." + SaveContainer.g_playCount.ToString().PadLeft(4,'0'));
            SaveContainer.g_playCount++;
            SaveContainer.Save();
        }
        // スコアセット
        //transform.FindChild("score").GetComponent<Text>().text = string.Format("{0:#,##0}", m_score);
        transform.FindChild("score").GetComponent<Text>().text = "";
        transform.GetComponent<TextInEffect>().Init();
    }

    // Action - 更新処理
    //---------------------------------
    //
    public TotalScore.RE_TOTAL_STEP Action()
    {
        if (!transform.GetComponent<TextInEffect>().m_goalFlg) {
            transform.GetComponent<TextInEffect>().Action();
            transform.FindChild("score").GetComponent<Text>().text = "";
        } else if (m_rollTimer < 3.0f) {
            if(!m_rollSeFlg){
                m_rollSeFlg = true;
                SoundManager.PlaySe("lastscore_roll",1);
            }
            m_rollTimer += Time.deltaTime;
            int _score;
            _score = Random.Range(100000, 999999);
            transform.FindChild("score").GetComponent<Text>().text = string.Format("{0:#,##0}", _score);
        } else{
            SoundManager.StopSe(1);
            SoundManager.PlaySe("lastscore_stop", 1);
            transform.FindChild("score").GetComponent<Text>().text = string.Format("{0:#,##0}", m_afterScore);

            return TotalScore.RE_TOTAL_STEP.NEXT_BUTTON;
            
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
