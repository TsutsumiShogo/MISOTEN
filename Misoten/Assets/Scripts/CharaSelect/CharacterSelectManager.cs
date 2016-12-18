using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour {


    //-----------------------
    // キャラクタ選択オブジェクト
    private bool m_allSelectedFlg = false;      // 全員選択完了
    public GameObject[] m_PlayerSelect;
    private charSelect[] m_charSelect = new charSelect[3];
    
    private Image m_image;
    public bool m_AllSelected = false;     // 全員決定状態
    private float m_color = 0;

    //===============================================================
    // 公開関数　T_SceneManagerで呼び出す
    
    //---------------------------------
    // Init 初期化処理
    public void Init(){
        for (int i = 0; i < 3; i++)
        {
            m_charSelect[i] = m_PlayerSelect[i].GetComponent<charSelect>();
            m_charSelect[i].Init();
        }
       
    }
	
	
    //---------------------------------
    // Action 更新処理
	public T_SceneManager.SceneType Action() {
        if (!m_allSelectedFlg){
            // キャラセレクト
            for (int i = 0; i < 3; i++){
                m_charSelect[i].Action();
            }            
            if (m_charSelect[0].m_decisionFlg && m_charSelect[1].m_decisionFlg && m_charSelect[2].m_decisionFlg){
                // 全員決定状態か
                m_allSelectedFlg = true;
                
            }
        }else{
            // 各プレイヤー選択キャラクターをセット
            for( int i=0;i<3;i++ ){
                GM_StaticParam.g_selectCharacter[i] = m_charSelect[i].m_selectNo;
            }
            // ゲームメインに遷移
            GameObject.Find("SceneChangeManager").GetComponent<SceneChangeManager>().SceneChange(SceneChangeManager.ESceneNo.SCENE_GAME);
        }

        if (Input.GetKeyDown(KeyCode.Backspace)){
            return T_SceneManager.SceneType.MENU;
        }
        

        return T_SceneManager.SceneType.CHARCTER_SELECT;
	}
    // ホワイトアウト演出
    void WhiteOut()
    {
        if( m_color < 1  ){
            m_color+=0.01f;
            m_image.color = new Color(1,1,1,m_color);
        }
        else
        {
            // ゲームメインに遷移
            GameObject.Find("SceneChangeManager").GetComponent<SceneChangeManager>().SceneChange(SceneChangeManager.ESceneNo.SCENE_GAME);
        }

    }
}
