using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

    private int ga = 120;

    public GameObject m_Logo;       // タイトルロゴ
    public GameObject m_UI;         // ボタンUI
 
    private bool m_pushAFlg = false;        // AButton押下フラグ
    private bool m_flashUIFlg = false;      // UI点滅フラグ
 
    //===============================================================
    // 公開関数　T_SceneManagerで呼び出す

    //---------------------------------
    // Init 初期化処理
    public void Init()
    {
        m_pushAFlg = false;
        m_flashUIFlg = false;
        m_UI.GetComponent<SelectEffect>().Init();

    }

    //---------------------------------
    // Action タイトルシーン更新
    public T_SceneManager.SceneType Action()
    {    
        if (m_pushAFlg){
            // ボタンを押した後
            if (!m_UI.GetComponent<SelectEffect>().Action()){
                return T_SceneManager.SceneType.MENU;
            }
        }else{
            // ボタン押下待機
            InputFunc();
            m_UI.GetComponent<SelectEffect>().WaitAction();
        }
        return T_SceneManager.SceneType.TITLE;
    }

    //===============================================================
    // 未公開関数
    //---------------------------------
    // Input 入力処理
    private void InputFunc() {
        if (!m_pushAFlg){
            // Aボタンでメニューへ
            if (Input.GetKeyDown(KeyCode.A) || XboxController.GetButtonA_All()){
                m_pushAFlg = true;
                //SoundManager.PlaySe("call_1", 4);
                SoundManager.PlaySe("decision_1",4);   
            }       
        }
    }
}
