using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_LastQ : MonoBehaviour {

    [SerializeField]
    private GameObject[] m_mode = new GameObject[2];    // 項目オブジェクト

    private bool m_openFlg;     // 開く
    private bool m_closeFlg;    // 閉じる
    private float m_time = 0.25f;    // 開くのにかかる時間
    private int m_selectNo;         // 選択項目
    //===============================================================
    // 公開関数
    
    // Init - 初期化
    //---------------------------------
    //
    public void Init(){
        transform.localScale = new Vector3(1, 0, 1);    // 初期見えないように
        m_mode[0].GetComponent<ModeEffect>().Init();
        m_mode[1].GetComponent<ModeEffect>().Init();
        m_selectNo = 0;
        m_closeFlg = false;
        m_openFlg = false;
    }

    // Action - 更新処理
    //---------------------------------
    //
    public void Action(){
        if( !m_openFlg && !m_closeFlg){
            m_mode[m_selectNo].GetComponent<ModeEffect>().Action();
        }
        if( XboxController.GetLeftTriggerDown_All() || Input.GetKeyDown(KeyCode.UpArrow)){
            if (m_selectNo != 0){
                SoundManager.PlaySe("cursol", 1);
                m_selectNo = 0;
                m_mode[0].GetComponent<ModeEffect>().OnScalling(true);
                m_mode[1].GetComponent<ModeEffect>().OffScalling(true);
            }
        }
        if(XboxController.GetLeftTriggerUp_All() || Input.GetKeyDown(KeyCode.DownArrow)){
            if (m_selectNo != 1){
                SoundManager.PlaySe("cursol", 1);
                m_selectNo = 1;
                m_mode[0].GetComponent<ModeEffect>().OffScalling(true);
                m_mode[1].GetComponent<ModeEffect>().OnScalling(true);
            }
        }
        if( XboxController.GetButtonA_All() || Input.GetKeyDown(KeyCode.A)){
            SoundManager.StopBgm();     // bgm停止
            SoundManager.PlaySe("decision_1", 0);
            if(m_selectNo == 0){
                GameObject.Find("SceneChangeManager").GetComponent<SceneChangeManager>().SceneChange(SceneChangeManager.ESceneNo.SCENE_STORY);
            }
            else{
                GameObject.Find("SceneChangeManager").GetComponent<SceneChangeManager>().SceneChange(SceneChangeManager.ESceneNo.SCENE_GAME);
            }
        }
    }

    // Open - ウィンドウ出現
    //---------------------------------
    //
    public void Open(){
        m_openFlg = true;
    }
    // Close - ウィンドウ出現
    //---------------------------------
    //
    public void Close()
    {
        m_closeFlg = true;
    }

    //===============================================================
    // 未公開関数

    // Update - 更新処理 ウィンドウの出現処理のみ
    //---------------------------------
    //
    void Update () {
        //---------------
        // 閉じる処理
		if( m_closeFlg){
            transform.localScale = new Vector3(1, transform.localScale.y - 1.0f * Time.deltaTime / m_time, 1);
            if (transform.localScale.y <= 0.0f){
                transform.localScale = new Vector3(1, 0, 1);
                m_closeFlg = false;
            }
        }
        //--------------
        // 開く処理
        if (m_openFlg)
        {
            transform.localScale = new Vector3(1, transform.localScale.y + 1.0f * Time.deltaTime / m_time, 1);
            if (transform.localScale.y >= 1.0f){
                transform.localScale = new Vector3(1, 1, 1);
                m_mode[0].GetComponent<ModeEffect>().OnScalling(true);
                m_mode[1].GetComponent<ModeEffect>().OffScalling(true);
                m_openFlg = false;
            }
        }
    }
}
