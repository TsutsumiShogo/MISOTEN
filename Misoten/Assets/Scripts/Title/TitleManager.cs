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
       
    }

    //---------------------------------
    // Action タイトルシーン更新
    public T_SceneManager.SceneType Action()
    {    
        if (m_pushAFlg){
            // ボタンを押した後
            if (m_UI.GetComponent<PushAEffect>().m_flashEnd){
                // メニューへ遷移処理
                m_UI.SetActive(false);
                m_Logo.SetActive(false);
                
                return T_SceneManager.SceneType.MENU;
            }
        }else{
            // ボタン押下待機
            InputFunc();
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
                m_UI.GetComponent<PushAEffect>().m_buttonFlg = true;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                ga++;
                if (RankingManager.CheckRankIn(ga)){
                    RankingManager.UpdateRanking(ga, "HAL");
                    SaveContainer.Save();
                    SaveContainer.CheckRanking();
                }
            }
        }
    }



}
