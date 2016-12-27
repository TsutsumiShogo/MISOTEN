using UnityEngine;
using System.Collections;

public class RankingManager : MonoBehaviour {


    [SerializeField]    // Unity上でセット
    private GameObject m_rankingList;       // ランキングリスト


    //------------------
    // static 変数
    public static bool g_rankInFlg;        // ランクインフラグ
    public static int g_changeRank;        // 挿入されるランキングを保持

    //===============================================================
    // 公開関数 - T_SceneManegerで呼び出す

    // Init - 初期化処理
    //---------------------------------
    //
    public void Init(){
        m_rankingList.GetComponent<RankList>().Init();
    }

    // Action - 更新処理
    //---------------------------------
    //
    public T_SceneManager.SceneType Action()
    {
        m_rankingList.GetComponent<RankList>().Action();
        if ( XboxController.GetButtonBack_All()){
            return T_SceneManager.SceneType.MENU;
        }
        return T_SceneManager.SceneType.RANKING;
    }

    //===============================================================
    // 未公開関数

  


    //===============================================================
    // Static公開関数　

    // CheckRankIn - ランクインをチェック
    //---------------------------------
    // int _score - スコア
    //---------------------------------
    //
    public static bool CheckRankIn( int _score)
    {
        g_rankInFlg = false;
        for (int _ranking = 0; _ranking < (int)SaveContainer.SAVE_KEY.SAVE_KEY_MAX; _ranking++)
        {
            // ランキング更新をチェック   
            if (_score >= SaveContainer.g_rankingScore[_ranking]){
                g_changeRank = _ranking;
                g_rankInFlg = true;
                return true;
            }
        }
        return false;
    }

    // UpdateRanking - ランキング更新
    //---------------------------------
    // int      _score  - スコア
    // string   _name   - 名前
    //---------------------------------
    //
    public static void UpdateRanking(int _score, string _name)
    {
        int[] _subScores = new int[(int)SaveContainer.SAVE_KEY.SAVE_KEY_MAX];
        string[] _subName = new string[(int)SaveContainer.SAVE_KEY.SAVE_KEY_MAX];

        for (int _ranking = 0; _ranking < (int)SaveContainer.SAVE_KEY.SAVE_KEY_MAX; _ranking++)
        {
            // ランキング更新をチェック   
            if (_score >= SaveContainer.g_rankingScore[_ranking])
            {
               
                for (int i = _ranking + 1, j = _ranking; i < (int)SaveContainer.SAVE_KEY.SAVE_KEY_MAX; i++, j++)
                {
                    _subScores[i] = SaveContainer.g_rankingScore[j];
                    _subName[i] = SaveContainer.g_rankingName[j];
                }
                SaveContainer.g_rankingScore[_ranking] = _score;      
                SaveContainer.g_rankingName[_ranking] = _name;
                for (int i = _ranking + 1; i < (int)SaveContainer.SAVE_KEY.SAVE_KEY_MAX; i++)
                {
                    SaveContainer.g_rankingScore[i] = _subScores[i];
                    SaveContainer.g_rankingName[i] = _subName[i];
                }
                return;
            }
        }



    }
  
    
}
