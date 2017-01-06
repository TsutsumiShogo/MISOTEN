using UnityEngine;
using System.Collections;

public class ResultManager : MonoBehaviour {

    [SerializeField]
    private GameObject m_resultMenu;        // リザルトメニュー
    [SerializeField]
    private GameObject m_totalScore;        // トータルスコア
    [SerializeField]
    private GameObject m_personalScore;     // 個人成績
    [SerializeField]
    private GameObject m_rankingList;       // ランキング
    [SerializeField]
    private GameObject m_backObj;           // 背景オブジェクト
    [SerializeField]
    private GameObject m_next;              // 次へ
    private bool m_menuFlg = false;

    private bool m_totalFlg = true;
    private bool m_toPersonalFlg = false;   // トータルから個人成績へ
    private bool m_toRankingFlg = false;    // 個人成績からランキングへ
    private bool m_personalFlg = false;
    private bool m_rankingFlg = false;
    private bool m_pushAFlg = false;
    private float m_timer;                  // タイマー

    private bool m_bgmFlg = false;          // bgm再生フラグ

    //===============================================================
    // 公開関数
    
    // Init - 初期化
    //---------------------------------
    //
    public void Init(){
        Debug.Log("リザルト初期化");
        m_timer = 0.0f;
        m_totalFlg = true;
        m_personalFlg = false;
        m_rankingFlg = false;
        m_menuFlg = false;
        m_toPersonalFlg = false;
        m_toRankingFlg = false;

        m_resultMenu.GetComponent<RE_Menu>().Init();
        m_totalScore.GetComponent<TotalScore>().Init();
        m_personalScore.GetComponent<RE_PersonalScore>().Init();
        m_next.GetComponent<RE_NextButton>().Init();
        m_rankingList.GetComponent<RE_Ranking>().Init();

        m_backObj.transform.localPosition = new Vector3(0.0f, 0, 0);
        m_bgmFlg = false;

    }

    //===============================================================
    // 未公開関数
    // Use this for initialization
    void Start()
    {
        
    }
    // Update - 毎フレーム処理
    //---------------------------------
    //
    void Update () {
        if( !m_bgmFlg){
            m_bgmFlg = true;
            SoundManager.PlayBgm("result_bgm");
        }
        if (!m_toPersonalFlg && !m_toRankingFlg && !m_pushAFlg) {
            if (m_totalFlg){
                //---------------------
                // トータルスコアパート
                m_totalFlg = m_totalScore.GetComponent<TotalScore>().Action();
                if (!m_totalFlg){
                    m_toPersonalFlg = true;
                    m_personalFlg = true;
                }
            }
            if (m_personalFlg){
                //---------------------
                // 個人成績パート
                m_personalFlg = m_personalScore.GetComponent<RE_PersonalScore>().Action();
                if (!m_personalFlg){
                    m_rankingFlg = true;
                    m_pushAFlg = true;
                    m_rankingList.GetComponent<RE_Ranking>().Set();
                    //m_toRankingFlg = true;
                }
            }
            if(m_rankingFlg)
            {
                //----------------------
                // ランキング表示パート
                m_rankingFlg = m_rankingList.GetComponent<RE_Ranking>().Action();
                if (!m_rankingFlg)
                {
                    m_menuFlg = true;
                }
            
            }
        }
        else if (m_pushAFlg)
        {
            m_next.GetComponent<RE_NextButton>().Action();
            if (m_rankingFlg){
                if (XboxController.GetButtonA_All()){
                    SoundManager.PlaySe("decision_1", 2);
                    m_next.GetComponent<RE_NextButton>().Init();
                    m_toRankingFlg = true;
                    m_pushAFlg = false;
                }
            }
        }
        

        if(m_toPersonalFlg){
            //----------------------
            // トータルから個人成績へ
            m_totalScore.transform.localPosition = new Vector3(
                m_totalScore.transform.localPosition.x - 800.0f * Time.deltaTime,
                m_totalScore.transform.localPosition.y,
                m_totalScore.transform.localPosition.z);
            m_personalScore.transform.localPosition = new Vector3(
                m_personalScore.transform.localPosition.x - 800.0f * Time.deltaTime,
                m_personalScore.transform.localPosition.y,
                m_personalScore.transform.localPosition.z);
            if(m_personalScore.transform.localPosition.x <= 0){
                m_totalScore.transform.localPosition = new Vector3(-800.0f, 0, 0);
                m_personalScore.transform.localPosition = new Vector3(0,0,0);
                m_toPersonalFlg = false;
            }
        }

        if (m_toRankingFlg){
            //-------------------------
            // 個人成績からランキングへ
            m_backObj.transform.localPosition = new Vector3(
                m_backObj.transform.localPosition.x - 10.0f * Time.deltaTime / 3.0f,
                m_backObj.transform.localPosition.y,
                m_backObj.transform.localPosition.z);
            m_rankingList.transform.localPosition = new Vector3(
                m_rankingList.transform.localPosition.x - 700.0f * Time.deltaTime / 2.0f,
                m_rankingList.transform.localPosition.y,
                m_rankingList.transform.localPosition.z);
            m_personalScore.transform.localPosition = new Vector3(
                m_personalScore.transform.localPosition.x - 1000.0f * Time.deltaTime / 2.0f,
                m_personalScore.transform.localPosition.y,
                m_personalScore.transform.localPosition.z);
            if ( m_backObj.transform.localPosition.x <= -10.0f){
                m_backObj.transform.localPosition = new Vector3(-10.0f, 0, 0);
                m_rankingList.transform.localPosition = new Vector3(-50, 0, 0);
                m_personalScore.transform.localPosition = new Vector3(-1000.0f, 0, 0);
                m_toRankingFlg = false;
            }
        }

        if (m_menuFlg){
            // Debug.Log("メニュー開くよ");
            m_timer += Time.deltaTime;
            if (m_timer >= 3.0f)
            {
                // メニュー
                m_resultMenu.GetComponent<RE_Menu>().Action();
            }
        }
	}
}
