using UnityEngine;
using System.Collections;

public class ResultManager : MonoBehaviour {

    [SerializeField]
    private GameObject m_resultMenu;        // リザルトメニュー
    [SerializeField]
    private GameObject m_totalScore;        // トータルスコア
    [SerializeField]
    private GameObject m_personalScore;     // 個人成績
    
    private bool m_menuFlg = false;

    private bool m_totalFlg = true;
    private bool m_moveFlg = false;
    private bool m_personalFlg = false;

    private float m_timer;                  // タイマー

    //===============================================================
    // 公開関数
    
    // Init - 初期化
    //---------------------------------
    //
    public void Init(){
        m_timer = 0.0f;
        m_totalFlg = true;
        m_menuFlg = false;
        m_resultMenu.GetComponent<RE_Menu>().Init();
        m_totalScore.GetComponent<TotalScore>().Init();
        m_personalScore.GetComponent<RE_PersonalScore>().Init();
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
        if (!m_moveFlg) {
            if (m_totalFlg){
                //---------------------
                // トータルスコアパート
                m_totalFlg = m_totalScore.GetComponent<TotalScore>().Action();
                if (!m_totalFlg){
                    m_moveFlg = true;
                    m_personalFlg = true;
                }
            }
            if (m_personalFlg){
                //---------------------
                // 個人成績パート
                m_personalFlg = m_personalScore.GetComponent<RE_PersonalScore>().Action();
                if (!m_personalFlg){
                    m_menuFlg = true;
                }
            }
        }
        else
        {
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
                m_moveFlg = false;
            }
        }

        if (m_menuFlg){
            Debug.Log("メニュー開くよ");
            m_timer += Time.deltaTime;
            if (m_timer >= 3.0f)
            {
                // メニュー
                m_resultMenu.GetComponent<RE_Menu>().Action();
            }
        }
	}
}
