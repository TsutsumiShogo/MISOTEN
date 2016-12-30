using UnityEngine;
using System.Collections;

public class T_SceneManager : MonoBehaviour {

    // ==========================================
    //  列挙型定義
    // シーンタイプ
    public enum SceneType
    {
        TITLE = 0,              // タイトル
        MENU,               // メニュー
        CHARCTER_SELECT,    // キャラセレクト
        RANKING,            // ランキング
    };

    // カメラ移動タイプ
    private enum CameraMoveType
    {
        FROM_TITLE_TO_MENU = 0,             // タイトルからメニューへ
        FROM_MENU_TO_CHARACTORSELECE,       // メニューからキャラセレクトへ
        FROM_MENU_TO_RANKING,               // メニューからランキングへ
        FROM_CHARASELECT_TO_MENU,           // キャラクタセレクトからメニューへ
        FROM_RANKING_TO_MENU,               // ランキングからメニューへ
        FROM_MENU_TO_TITLE,                 // タイトルからメニューへ
    }

    private SceneType m_nowSceneType;       // 現在のシーンを保持
    private SceneType m_nextSceneType;      // シーン遷移先を設定

    private CameraMoveType m_cameraMoveType;    // カメラ移動処理を選択

    public GameObject m_moveBackObj;             // カメラ演出用動く背景

    // 各シーンマネージャーオブジェクト取得
    [SerializeField]
    private GameObject[] m_sceneManagers = new GameObject[4];
    // 各シーンキャンバス取得
    [SerializeField]
    private GameObject[] m_sceneCanvas = new GameObject[4];

    private Vector3 m_titlePos;             // タイトルポジション
    private Vector3 m_menuPos;              // メニューポジション
    private Vector3 m_charaSelePos;         // キャラセレクトポジション
    private Vector3 m_rankingPos;           // ランキングポジション

    private bool m_cameraMoveFlg = false;   // カメラ移動フラグ
    private float m_timer = 0;
    public float m_cTimer = 0;
    private const float TIME = 0.0f;
    //===============================================================
    // 未公開関数

    void start(){
        Init();
    }
   
    //-----------------------------------------------------
    // Init ここで一括して初期化を行う
    public void Init()
    {
        m_titlePos = new Vector3(0, 0, 0);                      // タイトル座標
        m_menuPos = new Vector3(0, 1350.0f, 0);                 // メニュー座標
        m_charaSelePos = new Vector3(1600.0f, 1350.0f, 0);      // キャラセレ座標

        // タイトルシーン開始位置を決定 2周目にここで
        switch (GM_StaticParam.g_titleStartStep){
            case 0:
                m_moveBackObj.transform.localPosition = m_titlePos;     // 座標初期化
                m_nextSceneType = SceneType.TITLE;
                m_nowSceneType = SceneType.TITLE;
                m_sceneManagers[(int)SceneType.TITLE].GetComponent<TitleManager>().Init(); // タイトル初期化処理
                m_sceneCanvas[(int)SceneType.TITLE].SetActive(true);
                m_sceneCanvas[(int)SceneType.MENU].SetActive(false);
                m_sceneCanvas[(int)SceneType.CHARCTER_SELECT].SetActive(false);
                m_sceneCanvas[(int)SceneType.RANKING].SetActive(false);
                //m_mainCamera.transform.position = new Vector3(3000.0f, 7.65f, 0);
                break;
            case 1:
                m_moveBackObj.transform.localPosition = m_menuPos;     // 座標初期化
                m_nextSceneType = SceneType.MENU;
                m_nowSceneType = SceneType.MENU;
                m_sceneManagers[(int)SceneType.MENU].GetComponent<MenuManager>().Init(); // タイトル初期化処理
                m_sceneCanvas[(int)SceneType.TITLE].SetActive(false);
                m_sceneCanvas[(int)SceneType.MENU].SetActive(true);
                m_sceneCanvas[(int)SceneType.CHARCTER_SELECT].SetActive(false);
                m_sceneCanvas[(int)SceneType.RANKING].SetActive(false);
                //m_mainCamera.transform.position = m_MenuPos;
                break;
            case 2:
                m_nextSceneType = SceneType.CHARCTER_SELECT;
                m_nowSceneType = SceneType.CHARCTER_SELECT;
                m_sceneManagers[(int)SceneType.CHARCTER_SELECT].GetComponent<CharacterSelectManager>().Init(); // タイトル初期化処理
                m_sceneCanvas[(int)SceneType.TITLE].SetActive(false);
                m_sceneCanvas[(int)SceneType.MENU].SetActive(false);
                m_sceneCanvas[(int)SceneType.CHARCTER_SELECT].SetActive(true);
                m_sceneCanvas[(int)SceneType.RANKING].SetActive(false);
                //m_mainCamera.transform.position = m_CharaSelePos;
                break;
        }
       

      
       
        
       
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= TIME)
        {
            
            m_timer = 0;
            if (!m_cameraMoveFlg)
            {
                //-------------------------
                // 各シーン更新処理を行う

                Action();
            }
            else
            {
                //-------------------------
                // カメラ移動処理を行う
                MoveCamera();
            }
        }
    }

    //-----------------------------------------------------
    // Action  各シーン更新処理
    private void Action()
    {
        // 現在のシーンによって処理を変更
        switch (m_nowSceneType)
        {
            //--------------------------
            // タイトル
            case SceneType.TITLE:
                m_nextSceneType = m_sceneManagers[(int)SceneType.TITLE].GetComponent<TitleManager>().Action();
                if (m_nextSceneType != m_nowSceneType){
                    m_nowSceneType = m_nextSceneType;
                    m_cameraMoveFlg = true;
                    m_sceneCanvas[(int)SceneType.TITLE].SetActive(false);
                    m_cameraMoveType = CameraMoveType.FROM_TITLE_TO_MENU;
                }
                break;
            //--------------------------
            // メニュー
            case SceneType.MENU:
                m_nextSceneType = m_sceneManagers[(int)SceneType.MENU].GetComponent<MenuManager>().Action();
                if (m_nextSceneType != m_nowSceneType){
                    m_nowSceneType = m_nextSceneType;
                    m_cameraMoveFlg = true;
                    m_sceneCanvas[(int)SceneType.MENU].SetActive(false);
                    if (m_nextSceneType == SceneType.CHARCTER_SELECT){
                        m_cameraMoveType = CameraMoveType.FROM_MENU_TO_CHARACTORSELECE;
                    }else{
                        m_cameraMoveType = CameraMoveType.FROM_MENU_TO_RANKING;
                    }
                }
                break;
            //--------------------------
            // キャラクターセレクト
            case SceneType.CHARCTER_SELECT:
                m_nextSceneType = m_sceneManagers[(int)SceneType.CHARCTER_SELECT].GetComponent<CharacterSelectManager>().Action();
                if (m_nextSceneType != m_nowSceneType)
                {
                    m_nowSceneType = m_nextSceneType;
                    m_cameraMoveFlg = true;
                    m_sceneCanvas[(int)SceneType.CHARCTER_SELECT].SetActive(false);
                    if (m_nextSceneType == SceneType.MENU){
                        m_cameraMoveType = CameraMoveType.FROM_CHARASELECT_TO_MENU;
                    }
                }
                break;
            //--------------------------
            // ランキング
            case SceneType.RANKING:
                m_nextSceneType = m_sceneManagers[(int)SceneType.RANKING].GetComponent<RankingManager>().Action();
                if (m_nextSceneType != m_nowSceneType)
                {
                    m_nowSceneType = m_nextSceneType;
                    m_cameraMoveFlg = true;
                    if (m_nextSceneType == SceneType.MENU)
                    {
                        m_cameraMoveType = CameraMoveType.FROM_RANKING_TO_MENU;
                    }
                }
                break;
        }
    }

    //-----------------------------------------------------
    // MoveCamera カメラ移動処理 現在のSceneと次のSceneから移動処理を決める。
    private void MoveCamera(){
        // 移動タイプによって処理
        switch( m_cameraMoveType){
                //--------------------
                // タイトルからメニューへ
            case CameraMoveType.FROM_TITLE_TO_MENU:
                m_cTimer += Time.deltaTime;
                if (m_cTimer > 0.45f)
                {
                    m_moveBackObj.transform.localPosition = new Vector3(0, m_moveBackObj.transform.localPosition.y + m_menuPos.y * Time.deltaTime / 1.0f, 0);
                }else {
                    m_moveBackObj.transform.localPosition = new Vector3(0, m_moveBackObj.transform.localPosition.y + m_menuPos.y * Time.deltaTime / 0.5f, 0);
                }
                if( m_moveBackObj.transform.localPosition.y >= m_menuPos.y){
                    m_moveBackObj.transform.localPosition = m_menuPos;
                    m_cameraMoveFlg = false;
                    m_sceneManagers[(int)m_nowSceneType].GetComponent<MenuManager>().Init();
                    m_sceneCanvas[(int)m_nowSceneType].SetActive(true);
                }
                break;
                //--------------------
                // メニューからキャラセレへ
            case CameraMoveType.FROM_MENU_TO_CHARACTORSELECE:
                //m_mainCamera.transform.position = m_CharaSelePos;
                m_moveBackObj.transform.localPosition = new Vector3(m_moveBackObj.transform.localPosition.x + m_charaSelePos.x * Time.deltaTime / 1.0f, 1350.0f, 0);
                if (m_moveBackObj.transform.localPosition.x >= m_charaSelePos.x)
                {
                    m_moveBackObj.transform.localPosition = m_charaSelePos;
                    m_cameraMoveFlg = false;
                    m_sceneManagers[(int)m_nowSceneType].GetComponent<CharacterSelectManager>().Init();
                    m_sceneCanvas[(int)m_nowSceneType].SetActive(true);
                }
              
                break;
                //--------------------
                // メニューからランキングへ
            case CameraMoveType.FROM_MENU_TO_RANKING:
                //m_mainCamera.transform.position = m_RankingPos;
                
                m_cameraMoveFlg = false;

                m_sceneCanvas[(int)m_nowSceneType].SetActive(true);
                m_sceneManagers[(int)m_nowSceneType].GetComponent<RankingManager>().Init();
                break;
                //--------------------
                // キャラセレからメニューへ
            case CameraMoveType.FROM_CHARASELECT_TO_MENU:
                //m_mainCamera.transform.position = m_MenuPos;
                m_moveBackObj.transform.localPosition = new Vector3(m_moveBackObj.transform.localPosition.x - m_charaSelePos.x * Time.deltaTime / 1.0f, 1350.0f, 0);
                if (m_moveBackObj.transform.localPosition.x <= m_menuPos.x)
                {
                    m_moveBackObj.transform.localPosition = m_menuPos;
                    m_cameraMoveFlg = false;
                    m_sceneManagers[(int)m_nowSceneType].GetComponent<MenuManager>().Init();
                    m_sceneCanvas[(int)m_nowSceneType].SetActive(true);
                }
                break;
                //--------------------
                // ランキングからメニューへ
            case CameraMoveType.FROM_RANKING_TO_MENU:
                //m_mainCamera.transform.position = m_MenuPos;
                m_cameraMoveFlg = false;
                m_sceneCanvas[(int)m_nowSceneType].SetActive(true);
                m_sceneCanvas[(int)SceneType.RANKING].SetActive(false);
                m_sceneManagers[(int)m_nowSceneType].GetComponent<MenuManager>().Init();
                break;
                //--------------------
                // メニューからタイトルへ
            case CameraMoveType.FROM_MENU_TO_TITLE:
                break;
        }

    }

}
