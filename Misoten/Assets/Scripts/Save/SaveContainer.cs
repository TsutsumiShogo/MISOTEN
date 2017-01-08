using UnityEngine;
using System.Collections;
using System;

public class SaveContainer : MonoBehaviour {

    //-----------------------
    // セーブキー
    public enum SAVE_KEY
    {
        RANKING_1 = 0,
        RANKING_2,
        RANKING_3,
        RANKING_4,
        RANKING_5,
        RANKING_6,
        RANKING_7,
        RANKING_8,
        RANKING_9,
        RANKING_10,
        SAVE_KEY_MAX,
    }

    // ランキングスコアデータ
    public static int[] g_rankingScore = new int[(int)SAVE_KEY.SAVE_KEY_MAX];

    // ランキング名前データ
    public static string[] g_rankingName = new string[(int)SAVE_KEY.SAVE_KEY_MAX];

    // プレイ回数
    public static int g_playCount;

    private bool m_clearDataFlg = true;        // 起動時、これがtrueでデータ消去　エディタ起動用

    //===============================================================
    // 未公開関数
 
    // Awake
    //--------------------------------------
    //
    void Awake() {
        if (m_clearDataFlg){
            ClearData();
        }
        // 初期化＆ロード
        Initalize();
        Load();
    }

    // Initalize - 初期化
    //--------------------------------------
    //
    public static void Initalize()
    {
        for (int i = 0; i < (int)SAVE_KEY.SAVE_KEY_MAX; i++)
        {
            g_rankingScore[i] = 0;
        }
    }

    // Load - 読み込み
    //--------------------------------------
    //
    public static void Load()
    {
        // 全データ読み込み
        // データがない場合　0を格納
        for (int i = 0; i < (int)SAVE_KEY.SAVE_KEY_MAX; i++)
        {
            g_rankingScore[i] = PlayerPrefs.GetInt("Score_" + i.ToString(),0);
            g_rankingName[i] = PlayerPrefs.GetString("Name_" + i.ToString(), "No."+"TEST");
        }
        g_playCount = PlayerPrefs.GetInt("PlayCount", 0);
        Debug.Log("読み込み完了");
    }

    // Save ランキングデータを更新
    //--------------------------------------
    //
    public static void Save()
    {
        for (int i = 0; i < (int)SAVE_KEY.SAVE_KEY_MAX; i++)
        {
            PlayerPrefs.SetInt("Score_" + i.ToString(), g_rankingScore[i]);
            PlayerPrefs.SetString("Name_" + i.ToString(), g_rankingName[i]);
        }
        PlayerPrefs.SetInt("PlayCount", g_playCount);
        PlayerPrefs.Save();     // セーブデータ更新
    }

    // CheckRanking - ランキングスコア表示
    //--------------------------------------
    //
    public static void CheckRanking()
    {
        for (int i = 0; i < (int)SaveContainer.SAVE_KEY.SAVE_KEY_MAX; i++)
        {
            Debug.Log("Rankig " + i.ToString() + " -> " + g_rankingScore[i].ToString() + " " + g_rankingName[i]);
        }
    }
   

    // ClearData - データ初期化関数
    //--------------------------------------
    //
    private static void ClearData(){
        Debug.Log("セーブデータ初期化");
        for (int i = 0; i < (int)SAVE_KEY.SAVE_KEY_MAX; i++)
        {
            PlayerPrefs.SetInt("Score_" + i.ToString(), 0);
            PlayerPrefs.SetString("Name_" + i.ToString(), "No." + "TEST");
        }
        PlayerPrefs.SetInt("PlayCount", 0);
        PlayerPrefs.Save();     // セーブデータ更新
    }
}
