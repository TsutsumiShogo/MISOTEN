using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RE_LastScore : MonoBehaviour {

    //===============================================================
    // 公開関数

    // Init -初期化
    //---------------------------------
    //
    public void Init(){
        float _score = GameObject.Find("RE_TotalScore").GetComponent<TotalScore>().m_score; // スコア
        // スコアセット
        transform.FindChild("score").GetComponent<Text>().text = string.Format("{0:#,##0}", _score);
        transform.GetComponent<TextInEffect>().Init();
    }

    // Action - 更新処理
    //---------------------------------
    //
    public TotalScore.RE_TOTAL_STEP Action()
    {
        if( !transform.GetComponent<TextInEffect>().m_goalFlg){
            transform.GetComponent<TextInEffect>().Action();
            return TotalScore.RE_TOTAL_STEP.LASTSCORE_IN;
        }
        return TotalScore.RE_TOTAL_STEP.NEXT_BUTTON;
        
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
