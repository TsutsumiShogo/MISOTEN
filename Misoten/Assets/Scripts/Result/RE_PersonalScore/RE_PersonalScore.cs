using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RE_PersonalScore : MonoBehaviour {

    //---------------------------------
    // 
    public enum RE_PERSONAL_STEP
    {
        SCORE_ROLL = 0,
        END_STEP,
        RE_PERSONAL_STEP_MAX,
    }

    public enum COLOR_ID
    {
        RED = 0,
        GREEN,
        BLUE,
    }

    private RE_PERSONAL_STEP m_nowStep;
    private RE_PERSONAL_STEP m_nextStep;
    // キャラクタ―オブジェクト
    [SerializeField]
    private GameObject[] m_characterObj = new GameObject[3];

    public Vector3[] m_characterPos = new Vector3[3];       // ポジション
    public Vector3[] m_characterRot = new Vector3[3];       // 回転角

    [SerializeField]
    private GameObject[] m_playerScore = new GameObject[3]; // スコア

    [SerializeField]
    private GameObject[] m_plants = new GameObject[3];      // プラント


    private Text[] m_scoreText = new Text[3];
    private string m_strUnity;
    private string[] m_strScore = new string[3];
    private float m_timer = 0.0f;
    private float[] m_score = new float[3];
    private int[] m_rank = new int[3];
    private bool m_seFlg = false;
    //===============================================================
    // 公開関数

    // Init - 初期化
    //---------------------------------
    //
    public void Init()
    {
        m_seFlg = false;
        m_nowStep = m_nextStep = RE_PERSONAL_STEP.SCORE_ROLL;
        m_timer = 0.0f;
        m_strUnity = "点";

        // スコアセット
        for( int i =0;i<3;i++){
            m_score[i] = GM_ScoreCtrl.GetPlayerScore(i);
        }

        CheckRank();    // 順位計算

        for (int i = 0; i < 3; i++){
            // player順にキャラオブジェクトを配置
            m_characterObj[GM_StaticParam.g_selectCharacter[i]].transform.localPosition = m_characterPos[i];
            m_characterObj[GM_StaticParam.g_selectCharacter[i]].transform.localRotation = Quaternion.Euler( m_characterRot[i]);
            m_scoreText[i] = m_playerScore[i].transform.FindChild("score").GetComponent<Text>();
            m_playerScore[i].transform.FindChild("rank").GetComponent<RE_PlayerRank>().Init();
            m_scoreText[i].text = "0" + m_strUnity;
            m_plants[i].GetComponent<RE_PlantEffect>().Init(m_rank[i], GM_StaticParam.g_selectCharacter[i]); // 順位セット
        }

        transform.localPosition = new Vector3(800, 0, 0);   // 初期位置
    }

    // Action - 更新処理
    //---------------------------------
    //
    public bool Action()
    {
        switch (m_nowStep) {
            //-----------------------
            // スコアロール
            case RE_PERSONAL_STEP.SCORE_ROLL:
                m_nextStep = ScoreRoll();
                for( int i = 0; i < 3; i++){
                    m_plants[i].GetComponent<RE_PlantEffect>().Action();
                }
                if(m_nextStep != m_nowStep) {
                    return false;
                }
                break;
        }

        return true;
    }

    //===============================================================
    // 未公開関数

    // ScoreRoll - スクロール
    //---------------------------------
    //
    private RE_PERSONAL_STEP ScoreRoll()
    {
        // Debug.Log("スコアロール");
        m_timer += Time.deltaTime;
        if ((m_timer <= 5.4f) && (m_timer > 0.2f) ) {
            if( !m_seFlg)
            {
                SoundManager.PlaySe("roll", 5);
                m_seFlg = true;
            }
            int _score;
            _score = Random.Range(100000, 999999);
            m_strScore[0] = string.Format("{0:#,##0}", _score);
            _score = Random.Range(100000, 999999);
            m_strScore[1] = string.Format("{0:#,##0}", _score);
            _score = Random.Range(100000, 999999);
            m_strScore[2] = string.Format("{0:#,##0}", _score);
            m_scoreText[0].text = m_strScore[0] + m_strUnity;
            m_scoreText[1].text = m_strScore[1] + m_strUnity;
            m_scoreText[2].text = m_strScore[2] + m_strUnity;


        }else if(m_timer > 5.4f)
        {
            for (int i = 0; i < 3; i++)
            {
                m_strScore[i] = string.Format("{0:#,##0}", GM_ScoreCtrl.GetPlayerScore(i));
                m_scoreText[i].text = m_strScore[i] + m_strUnity;
            }
        }
         
        if(m_timer >= 6.0f){
            SoundManager.PlaySe("cheer2", 2);        // 歓声SE
            for (int i = 0; i < 3; i++){    
                m_playerScore[i].transform.FindChild("rank").GetComponent<RE_PlayerRank>().OnScall(m_rank[i]+1);    
            }
            return RE_PERSONAL_STEP.END_STEP;
        }
        return RE_PERSONAL_STEP.SCORE_ROLL;
    
    }

    // CheckRank -　順位計算
    //---------------------------------
    //
    private void CheckRank()
    {
        if((m_score[(int)COLOR_ID.RED] == m_score[(int)COLOR_ID.GREEN]) && (m_score[(int)COLOR_ID.GREEN] == m_score[(int)COLOR_ID.BLUE])){
            // 1位.1位.1位
            m_rank[(int)COLOR_ID.RED] = 0;
            m_rank[(int)COLOR_ID.GREEN] = 0;
            m_rank[(int)COLOR_ID.BLUE] = 0;
            return;
        }

        if (m_score[(int)COLOR_ID.RED] > m_score[(int)COLOR_ID.GREEN])
        {
            if (m_score[(int)COLOR_ID.RED] > m_score[(int)COLOR_ID.BLUE])
            {
                m_rank[(int)COLOR_ID.RED] = 0;
                if (m_score[(int)COLOR_ID.GREEN] > m_score[(int)COLOR_ID.BLUE])
                {
                    // 1位.2位.3位
                    m_rank[(int)COLOR_ID.GREEN] = 1;
                    m_rank[(int)COLOR_ID.BLUE] = 2;
                }
                else if (m_score[(int)COLOR_ID.GREEN] == m_score[(int)COLOR_ID.BLUE])
                {
                    // 1位.2位.2位
                    m_rank[(int)COLOR_ID.GREEN] = 1;
                    m_rank[(int)COLOR_ID.BLUE] = 1;
                }
                else
                {
                    // 1位.3位.2位
                    m_rank[(int)COLOR_ID.GREEN] = 2;
                    m_rank[(int)COLOR_ID.BLUE] = 1;
                }
            }
            else if(m_score[(int)COLOR_ID.RED] == m_score[(int)COLOR_ID.BLUE])
            {
                // 1位.3位.1位
                m_rank[(int)COLOR_ID.RED] = 0;
                m_rank[(int)COLOR_ID.GREEN] = 2;
                m_rank[(int)COLOR_ID.BLUE] = 0;
            }
            else
            {
                // 2位.3位.1位
                m_rank[(int)COLOR_ID.RED] = 1;
                m_rank[(int)COLOR_ID.GREEN] = 2;
                m_rank[(int)COLOR_ID.BLUE] = 0;
            }
        }
        else
        {
            if (m_score[(int)COLOR_ID.GREEN] > m_score[(int)COLOR_ID.BLUE])
            {
                m_rank[(int)COLOR_ID.GREEN] = 0;
                if (m_score[(int)COLOR_ID.RED] > m_score[(int)COLOR_ID.BLUE])
                {
                    // 2位.1位.3位
                    m_rank[(int)COLOR_ID.RED] = 1;
                    m_rank[(int)COLOR_ID.BLUE] = 2;
                }
                else if(m_score[(int)COLOR_ID.RED] == m_score[(int)COLOR_ID.BLUE])
                {
                    // 2位.1位.2位
                    m_rank[(int)COLOR_ID.RED] = 1;
                    m_rank[(int)COLOR_ID.BLUE] = 1;
                }
                else
                {
                    // 3位.1位.2位
                    m_rank[(int)COLOR_ID.RED] = 2;
                    m_rank[(int)COLOR_ID.BLUE] = 1;
                }
            }
            else if(m_score[(int)COLOR_ID.GREEN] == m_score[(int)COLOR_ID.BLUE])
            {
                // 3位.1位.1位
                m_rank[(int)COLOR_ID.RED] = 2;
                m_rank[(int)COLOR_ID.GREEN] = 0;
                m_rank[(int)COLOR_ID.BLUE] = 0;
            }
            else
            {
                // 3位.2位.1位
                m_rank[(int)COLOR_ID.RED] = 2;
                m_rank[(int)COLOR_ID.GREEN] = 1;
                m_rank[(int)COLOR_ID.BLUE] = 0;
            }
        }
    }
}
