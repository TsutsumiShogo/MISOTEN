using UnityEngine;
using System.Collections;



public class MenuManager : MonoBehaviour {

    

    // モードオブジェクト
    [SerializeField]
    private GameObject[] m_modObj = new GameObject[2];

    private int m_selectId = 0;     // モードID
    private bool m_pushFlg = false;  // 決定ボタンを押したか

    // モード定数
    private const int CHARACTOR_SELECT = 0;     // キャラクタセレクト
    private const int RANKING = 1;              // ランキング

	//===============================================================
    // 公開関数　T_SceneManagerで呼び出す
    
    //---------------------------------
    // Init 初期化処理
    public void Init()
    {
        m_modObj[CHARACTOR_SELECT].GetComponent<ModeEffect>().Init();
        m_modObj[RANKING].GetComponent<ModeEffect>().Init();
        m_modObj[m_selectId].GetComponent<ModeEffect>().OnScalling();
    }
	
    //---------------------------------
    // Action メニュー更新処理
    public T_SceneManager.SceneType Action(){

        if (!m_pushFlg){
            // モード選択処理
            if (m_selectId == CHARACTOR_SELECT){
                //-------------------------
                // 右入力処理
                if (XboxController.GetLeftTriggerRight(0) || Input.GetKeyDown(KeyCode.RightArrow)){
                    m_selectId = RANKING;
                    m_modObj[CHARACTOR_SELECT].GetComponent<ModeEffect>().OffScalling();
                    m_modObj[RANKING].GetComponent<ModeEffect>().OnScalling();
                }
            }else if( m_selectId == RANKING ){
                //-------------------------
                // 左入力処理
                if (XboxController.GetLeftTriggerLeft(0) || Input.GetKeyDown(KeyCode.LeftArrow)){
                    m_selectId = CHARACTOR_SELECT;
                    m_modObj[CHARACTOR_SELECT].GetComponent<ModeEffect>().OnScalling();
                    m_modObj[RANKING].GetComponent<ModeEffect>().OffScalling();
                }
            }
            // 決定ボタン処理
            if (XboxController.GetButtonA_All() || Input.GetKeyDown(KeyCode.A)){
                if (m_selectId == CHARACTOR_SELECT){
                    return T_SceneManager.SceneType.CHARCTER_SELECT;
                }else{
                    return T_SceneManager.SceneType.RANKING;
                }
            }
        }


        return T_SceneManager.SceneType.MENU;   // 繊維せず
    }

}
