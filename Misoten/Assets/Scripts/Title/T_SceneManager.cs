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

    public GameObject m_mainCamera;             // メインカメラ取得

    // 各シーンマネージャーオブジェクト取得
    [SerializeField]
    private GameObject[] m_sceneManagers = new GameObject[4];
    // 各シーンキャンバス取得
    [SerializeField]
    private GameObject[] m_sceneCanvas = new GameObject[4];

    private Vector3 m_MenuPos;              // メニューポジション
    private Vector3 m_CharaSelePos;         // キャラセレクトポジション
    private Vector3 m_RankingPos;           // ランキングポジション

    private bool m_cameraMoveFlg = false;   // カメラ移動フラグ
    private float m_timer = 0;
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
        m_nextSceneType = SceneType.TITLE;
        m_nowSceneType = SceneType.TITLE;
        m_sceneManagers[(int)SceneType.TITLE].GetComponent<TitleManager>().Init(); // タイトル初期化処理

        Debug.Log("lo");
        m_sceneCanvas[(int)SceneType.TITLE].SetActive(true);
        m_sceneCanvas[(int)SceneType.MENU].SetActive(false);
        m_sceneCanvas[(int)SceneType.CHARCTER_SELECT].SetActive(false);
        m_sceneCanvas[(int)SceneType.RANKING].SetActive(false);
        m_mainCamera.transform.position = new Vector3(3000.0f, 7.65f, 0);
        // メニュー
        m_MenuPos = m_sceneManagers[(int)SceneType.MENU].transform.position;
        m_MenuPos.z = 0;
        // キャラセレ
        m_CharaSelePos = m_sceneManagers[(int)SceneType.CHARCTER_SELECT].transform.position;
        m_CharaSelePos.z = 0;
        // ランキング
        m_RankingPos = m_sceneManagers[(int)SceneType.RANKING].transform.position;
        m_RankingPos.z = 0;
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
                    if(m_nextSceneType == SceneType.CHARCTER_SELECT){
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
                    if (m_nextSceneType == SceneType.MENU){
                        m_cameraMoveType = CameraMoveType.FROM_CHARASELECT_TO_MENU;
                    }
                }
                break;
            //--------------------------
            // ランキング
            case SceneType.RANKING:
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
                m_mainCamera.transform.position = m_MenuPos;
                m_cameraMoveFlg = false;
                m_sceneManagers[(int)m_nowSceneType].GetComponent<MenuManager>().Init();
                m_sceneCanvas[(int)SceneType.TITLE].SetActive(false);
                m_sceneCanvas[(int)m_nowSceneType].SetActive(true);
                break;
                //--------------------
                // メニューからキャラセレへ
            case CameraMoveType.FROM_MENU_TO_CHARACTORSELECE:
                m_mainCamera.transform.position = m_CharaSelePos;
                m_cameraMoveFlg = false;
                m_sceneManagers[(int)m_nowSceneType].GetComponent<CharacterSelectManager>().Init();
                m_sceneCanvas[(int)SceneType.MENU].SetActive(false);
                m_sceneCanvas[(int)m_nowSceneType].SetActive(true);
                break;
                //--------------------
                // メニューからランキングへ
            case CameraMoveType.FROM_MENU_TO_RANKING:
                m_mainCamera.transform.position = m_RankingPos;
                m_cameraMoveFlg = false;
                m_sceneCanvas[(int)SceneType.MENU].SetActive(false);
                m_sceneCanvas[(int)m_nowSceneType].SetActive(true);
                m_sceneManagers[(int)m_nowSceneType].GetComponent<RankingManager>().Init();
                break;
                //--------------------
                // キャラセレからメニューへ
            case CameraMoveType.FROM_CHARASELECT_TO_MENU:
                 m_mainCamera.transform.position = m_MenuPos;
                m_cameraMoveFlg = false;
                m_sceneCanvas[(int)m_nowSceneType].SetActive(true);
                m_sceneCanvas[(int)SceneType.CHARCTER_SELECT].SetActive(false);
                m_sceneManagers[(int)m_nowSceneType].GetComponent<MenuManager>().Init();
                break;
                //--------------------
                // ランキングからメニューへ
            case CameraMoveType.FROM_RANKING_TO_MENU:
                break;
                //--------------------
                // メニューからタイトルへ
            case CameraMoveType.FROM_MENU_TO_TITLE:
                break;
        }

    }

}
