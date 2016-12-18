using UnityEngine;
using System.Collections;

public class RankingManager : MonoBehaviour {

   

    // CheckRankIn - ランクインをチェック
    //---------------------------------
    // int _score - スコア
    //---------------------------------
    //
    public static bool CheckRankIn( int _score)
    {
        for (int _ranking = 0; _ranking < (int)SaveContainer.SAVE_KEY.SAVE_KEY_MAX; _ranking++)
        {
            // ランキング更新をチェック   
            if (_score >= SaveContainer.g_rankingScore[_ranking]){
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
