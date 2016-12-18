using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RankList : MonoBehaviour {

    // ランキングオブジェクト格納用
    public GameObject[] m_rankObj = new GameObject[(int)SaveContainer.SAVE_KEY.SAVE_KEY_MAX];
 
    //===============================================================
    // 公開関数 - RankingManagerで呼び出す

    // Init - 初期化処理
    //---------------------------------
    //
    public void Init(){
        SetObject();    // オブジェクト取得
        SetData();      // データセット
    }

    // Action - 更新処理
    //---------------------------------

    //===============================================================
    // 未公開関数

    // SetObject - 各スコアオブジェクトを取得する
    //---------------------------------
    //
    private void SetObject(){
        for (int i = 0; i < (int)SaveContainer.SAVE_KEY.SAVE_KEY_MAX; i++){
            int j = i+1;
            string str = "Rank_" + j.ToString();
            m_rankObj[i] = GameObject.Find(str);
        }

    }

    // SetData - データを設定
    //---------------------------------
    // 
    private void SetData(){
        for( int i=0;i<(int)SaveContainer.SAVE_KEY.SAVE_KEY_MAX;i++){
            m_rankObj[i].transform.FindChild("Scores/score").GetComponent<Text>().text = SaveContainer.g_rankingScore[i].ToString();
            m_rankObj[i].transform.FindChild("name").GetComponent<Text>().text = SaveContainer.g_rankingName[i];
        }
    }

	
}
