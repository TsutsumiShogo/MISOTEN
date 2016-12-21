//-----------------------------------------------------------------------------
// FILE_NAME : TotalScore.cs
// SCENE　   : GameScene
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TotalScore : MonoBehaviour {

    //-----------------------
    // 処理ステップ
    public enum RE_TOTAL_STEP
    {
        HEADER_IN = 0,
        SCORE_IN,
        BONUS_IN,
        LASTSCORE_IN,
        NEXT_BUTTON,
        RE_TOTAL_STEP_MAX,
    }

    [SerializeField]
    public GameObject[] m_stepObj;     // ヘッダー

    private Text m_thisText;
    public Text m_rankText;

    private RE_TOTAL_STEP m_nowStep;        // 現在処理ステップ
    private RE_TOTAL_STEP m_nextStep;       // 次回処理ステップ

    // 各ステップ間、待ち時間
    [SerializeField]
    private float[] m_waitTime = new float[(int)RE_TOTAL_STEP.RE_TOTAL_STEP_MAX];
    private float m_waitTimer = 0.0f;       // 時間計測用
    private bool m_waitFlg = false;         // 待ちフラグ

    public float m_score = 0.0f;            // スコア

    // MathManager
    [SerializeField]
    private GM_MathManager m_mathManager;

    //===============================================================
    // 公開関数 - ResultManagerで呼び出す
    
    // Init - 初期化処理1
    //---------------------------------
    //
    public void Init()
    {
        m_score = GM_ScoreCtrl.GetPlayersScore();       // スコア取得
        m_waitFlg = false;
        m_waitTimer = 0.0f;
        m_nowStep = m_nextStep = RE_TOTAL_STEP.HEADER_IN;
        m_stepObj[(int)RE_TOTAL_STEP.HEADER_IN].GetComponent<RE_Header>().Init();
        m_stepObj[(int)RE_TOTAL_STEP.SCORE_IN].GetComponent<TextInEffect>().Init();
        m_stepObj[(int)RE_TOTAL_STEP.BONUS_IN].GetComponent<RE_Bonus>().Init();
        m_stepObj[(int)RE_TOTAL_STEP.LASTSCORE_IN].GetComponent<RE_LastScore>().Init();
        for ( int i =0;i<(int)RE_TOTAL_STEP.RE_TOTAL_STEP_MAX;i++){

        }
    }

    // Action - 更新処理
    //---------------------------------
    //
    public bool Action()
    {
        if (!m_waitFlg){
            switch (m_nowStep)
            {
                //-----------------
                // ヘッダー
                case RE_TOTAL_STEP.HEADER_IN:
                    m_nextStep = m_stepObj[(int)m_nowStep].GetComponent<RE_Header>().Action();
                    if (m_nowStep != m_nextStep)
                    {
                        m_waitFlg = true;
                        m_nowStep = m_nextStep;
                    }
                    break;
                //-----------------
                // スコア
                case RE_TOTAL_STEP.SCORE_IN:
                    m_nextStep = m_stepObj[(int)m_nowStep].GetComponent<TextInEffect>().Action();
                    if (m_nowStep != m_nextStep)
                    {
                        m_waitFlg = true;
                        m_nowStep = m_nextStep;
                    }
                    break;
                //-----------------
                // ボーナス
                case RE_TOTAL_STEP.BONUS_IN:
                    m_nextStep = m_stepObj[(int)m_nowStep].GetComponent<RE_Bonus>().Action();
                    if (m_nowStep != m_nextStep)
                    {
                        m_waitFlg = true;
                        m_nowStep = m_nextStep;
                    }
                    break;
                //-----------------
                // 最終スコア
                case RE_TOTAL_STEP.LASTSCORE_IN:
                    m_nextStep = m_stepObj[(int)m_nowStep].GetComponent<RE_LastScore>().Action();
                    if (m_nowStep != m_nextStep)
                    {
                        m_waitFlg = true;
                        m_nowStep = m_nextStep;
                    }
                    break;
                //-----------------
                // 最終スコア
                case RE_TOTAL_STEP.NEXT_BUTTON:
                    if (XboxController.GetButtonA_All() || Input.GetKeyDown(KeyCode.A))
                    {
                        return true;
                    }
                    break;
            }
        }
        else
        {
            //---------------
            // 待ち時間経過処理
            m_waitTimer += Time.deltaTime;
            if(m_waitTime[(int)m_nowStep] <= m_waitTimer){
                m_waitFlg = false;
                m_waitTimer = 0.0f;
            }
        }
        return false;
    }

    //===============================================================
    // 未公開関数
    
	// Use this for initialization
	void Start () {
        
        /*m_thisText = transform.FindChild("score").GetComponent<Text>();
        float _percent = m_mathManager.totalFlowerLevel / (float)m_mathManager.MAX_TOTAL_FLOWER_LEVEL;
        int score = (int)(_percent * 100.0f);

        if (score >= 100)
            m_rankText.text = "SSS";
        else if (score >= 90)
            m_rankText.text = "SS";
        else if (score >= 80)
            m_rankText.text = "S";
        else if (score >= 70)
            m_rankText.text = "A";
        else if (score >= 50)
            m_rankText.text = "B";
        else if (score >= 30)
            m_rankText.text = "C";
        else if (score >= 20)
            m_rankText.text = "D";
        else
            m_rankText.text = "E";

        m_thisText.text = score.ToString();
        */
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
