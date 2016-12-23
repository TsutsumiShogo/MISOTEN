using UnityEngine;
using System.Collections;

public class RE_Menu : MonoBehaviour {

    //-----------------------
    // 遷移先
    enum TO_MODE
    {
        REPLAY = 0,
        TO_CHARACTER_SELECT,
        TO_MENU,
        TO_MODE_MAX,
    }

    private GameObject[] m_mode = new GameObject[(int)TO_MODE.TO_MODE_MAX];

    private bool m_startFlg = false;    // メニューウィンドウ出現フラグ
   
    private int m_selectNo = 0;         // 選択

    private bool m_desided = false;     // 遷移先決定フラグ

    //-----------------------
    // ウィンドウ演出に使用
    private float m_openTime = 0.25f;    // ウィンドウが開くのにかかる速度
    
    //===============================================================
    // 公開関数 - RE_Managerで呼び出す

	// Init - 初期化処理 
    //---------------------------------
	//
    public void Init(){
        // オブジェクト取得
        m_mode[(int)TO_MODE.REPLAY] = transform.FindChild("Replay").gameObject;
        m_mode[(int)TO_MODE.TO_CHARACTER_SELECT] = transform.FindChild("toCharSelect").gameObject;
        m_mode[(int)TO_MODE.TO_MENU] = transform.FindChild("toMenu").gameObject;
        
        // スケール初期化
        transform.localScale = new Vector3(1, 0, 1);
        for (int i = 0; i < (int)TO_MODE.TO_MODE_MAX; i++)
        {
            m_mode[i].GetComponent<ModeEffect>().Init();
        }
        m_selectNo = 0;         // 選択初期化
        m_startFlg = false;     // 開始フラグ初期化
        m_desided = false;      // 決定フラグ初期化
        m_mode[m_selectNo].GetComponent<ModeEffect>().OnScalling(true);
        m_mode[m_selectNo+1].GetComponent<ModeEffect>().OffScalling(true);
        m_mode[m_selectNo + 2].GetComponent<ModeEffect>().OffScalling(true);
    }

    // Action - 更新処理
    //---------------------------------
    //
    public void Action(){
        if (!m_startFlg){
            // ウィンドウ出現演出
            m_startFlg = OpenWindow();
        }
        else
        {
            // 遷移先選択
            for (int i = 0; i < (int)TO_MODE.TO_MODE_MAX; i++){
                m_mode[i].GetComponent<ModeEffect>().Action();
            }
            if (!m_desided){
                SelectToMode();
            }
        }

    }

    //===============================================================
    // 未公開関数

    // OpenWindow - ウィンドウ出現演出
    //---------------------------------
    //
    private bool OpenWindow(){

        transform.localScale = new Vector3(1, transform.localScale.y + (1.0f*Time.deltaTime/m_openTime), 1);
        if (transform.localScale.y >= 1.0f){
            transform.localScale = new Vector3(1, 1, 1);
            return true;
        }
        return false;
    }

    // SelectToMode - 遷移先選択
    //---------------------------------
    //
    private void SelectToMode(){

        if (XboxController.GetLeftTriggerDown_All())
        {
            m_selectNo--;
            if (m_selectNo < 0){
                m_selectNo = 0;
            }
            else
            {
                m_mode[m_selectNo + 1].GetComponent<ModeEffect>().OffScalling(true);
                m_mode[m_selectNo].GetComponent<ModeEffect>().OnScalling(true);
            }
        }
        if (XboxController.GetLeftTriggerUp_All())
        {
            m_selectNo++;
            if (m_selectNo > 2){
                m_selectNo = 2;
            }
            else
            {
                m_mode[m_selectNo - 1].GetComponent<ModeEffect>().OffScalling(true);
                m_mode[m_selectNo].GetComponent<ModeEffect>().OnScalling(true);
            }
        }

        //-------------------
        // 遷移先決定
        if (XboxController.GetButtonA_All())
        {
            m_desided = true;
            GameObject.Find("MobsManager").GetComponent<MobsManager>().Clean();
            
            switch( m_selectNo)
            {
                case 0:
                    GameObject.Find("SceneChangeManager").GetComponent<SceneChangeManager>().SceneChange(SceneChangeManager.ESceneNo.SCENE_GAME);
                    break;
                case 1:
                    GM_StaticParam.g_titleStartStep = 2;
                    GameObject.Find("SceneChangeManager").GetComponent<SceneChangeManager>().SceneChange(SceneChangeManager.ESceneNo.SCENE_TITLE);
                    break;
                case 2:
                    GM_StaticParam.g_titleStartStep = 1;
                    GameObject.Find("SceneChangeManager").GetComponent<SceneChangeManager>().SceneChange(SceneChangeManager.ESceneNo.SCENE_TITLE);
                    break;
                   
            }
            
        }

    }
}
