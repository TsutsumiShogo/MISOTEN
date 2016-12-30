using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RankList : MonoBehaviour {

    // ランキングオブジェクト格納用
    public GameObject[] m_rankObj = new GameObject[(int)SaveContainer.SAVE_KEY.SAVE_KEY_MAX];

    private float m_cameraTimer;    // タイマー
    private float m_cTime = 5.0f;   // 移動完了
    private bool m_flg = false;
    private float m_topPos = -1350.0f;
    private float m_pos;
    private bool m_underFlg;
    //===============================================================
    // 公開関数 - RankingManagerで呼び出す

    // Init - 初期化処理
    //---------------------------------
    //
    public void Init(){
        m_cameraTimer = 0.0f;
        SetObject();            // オブジェクト取得
        SetData();              // データセット
        m_flg = false;
        m_underFlg = false;
    }

    // ResultInit - リザルトランキング用処理
    //---------------------------------
    //
    public void ResultInit()
    {
        m_cameraTimer = 0.0f;
        m_flg = false;
        SetObject();            // オブジェクト取得
        SetData();              // データセット
        transform.localPosition = new Vector3(0, 0, 0);
        for( int i=0;i<10;i++)
        {
            m_rankObj[i].transform.FindChild("rank").GetComponent<Text>().color = new Color(1, 1, 1);
            m_rankObj[i].transform.FindChild("name").GetComponent<Text>().color = new Color(1, 1, 1);
            m_rankObj[i].transform.FindChild("Scores/score").GetComponent<Text>().color = new Color(1, 1, 1);
            m_rankObj[i].transform.FindChild("Scores/unit").GetComponent<Text>().color = new Color(1, 1, 1);
        }
    } 

    public void SetRank(){
        m_cameraTimer = 0.0f;
        SetObject();            // オブジェクト取得
        SetData();              // データセット
        
        // ランクインがあれば
        if(RankingManager.g_rankInFlg){
            m_rankObj[RankingManager.g_changeRank].transform.FindChild("rank").GetComponent<Text>().color = new Color(1, 1, 0);
            m_rankObj[RankingManager.g_changeRank].transform.FindChild("name").GetComponent<Text>().color = new Color(1, 1, 0);
            m_rankObj[RankingManager.g_changeRank].transform.FindChild("Scores/score").GetComponent<Text>().color = new Color(1, 1, 0);
            m_rankObj[RankingManager.g_changeRank].transform.FindChild("Scores/unit").GetComponent<Text>().color = new Color(1, 1, 0);
        }
    }

    // Action - 更新処理
    //---------------------------------
    //
    public bool Action(){
        m_cameraTimer += Time.deltaTime;
        Vector3 _pos = GameObject.Find("BG_MoveObj").transform.localPosition;
        if (m_cameraTimer > 2.0f){
            if (!m_flg){
                transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    transform.localPosition.y + m_topPos * Time.deltaTime / m_cTime,
                    transform.localPosition.z);
                _pos.y -= 1350.0f * Time.deltaTime / m_cTime;
                GameObject.Find("BG_MoveObj").transform.localPosition = _pos;
                if ( m_topPos >= transform.localPosition.y){
                    transform.localPosition = new Vector3(transform.localPosition.x, m_topPos,transform.localPosition.z);
                    m_pos = transform.localPosition.y;
                    GameObject.Find("BG_MoveObj").transform.localPosition = new Vector3(_pos.x, 0.0f, _pos.z);
                    m_flg = true;
                }
            }else{
                if (!m_underFlg)
                {
                    m_pos -= XboxController.GetLeftY(0) * 3.0f;
                    if (m_pos > 0)
                    {
                        m_pos = 0;
                    }
                    if (m_pos < -1350.0f)
                    {
                        m_pos = -1350.0f;
                    }
                    transform.localPosition = new Vector3(0, m_pos, 1);
                    _pos = GameObject.Find("BG_MoveObj").transform.localPosition;
                    _pos.y = 1350.0f + m_pos;
                    GameObject.Find("BG_MoveObj").transform.localPosition = _pos;
                    if (XboxController.GetButtonBack_All() || Input.GetKeyDown(KeyCode.Backspace)){
                        m_underFlg = true;
                    }
                    if (m_pos == 0)
                    {
                        return false;
                    }
                }
                else
                {
                    m_pos += 1350.0f*Time.deltaTime/0.2f;
                    transform.localPosition = new Vector3(0, m_pos, 1);
                    _pos = GameObject.Find("BG_MoveObj").transform.localPosition;
                    _pos.y = 1350.0f + m_pos;
                    GameObject.Find("BG_MoveObj").transform.localPosition = _pos;
                    if (transform.localPosition.y >= 0.0f){
                        transform.localPosition = new Vector3(transform.localPosition.x, 0.0f, transform.localPosition.z);
                        m_pos = 0;
                        _pos.y = 1350.0f + m_pos;
                        GameObject.Find("BG_MoveObj").transform.localPosition = _pos;
                        m_underFlg = false;
                    }
                }
            }
        }
        return true;
    }

    // ResultAction - リザルトランキング用更新処理
    //---------------------------------
    //
    public bool ResultAction()
    {
        m_cameraTimer += Time.deltaTime;
        if (m_cameraTimer > 2.0f)
        {
            if (!m_flg)
            {
                transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    transform.localPosition.y + m_topPos * Time.deltaTime / m_cTime,
                    transform.localPosition.z);
                if (m_topPos >= transform.localPosition.y)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, m_topPos, transform.localPosition.z);
                    m_pos = transform.localPosition.y;
                    m_flg = true;
                    return false;
                }
            }
        }
        return true;
    }

    //===============================================================
    // 未公開関数

    void Start()
    {
        Debug.Log("start");
        transform.localPosition = new Vector3(0, 0, 0);
    }


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
