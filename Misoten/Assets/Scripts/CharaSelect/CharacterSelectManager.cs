using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour {


    //-----------------------
    // キャラクタ選択オブジェクト
    private CameraMove m_main;
    public GameObject[] m_PlayerSelect;
    private charSelect[] m_charSel = new charSelect[3];
    public GameObject m_panel;
    private Image m_image;
    public bool m_AllSelected = false;     // 全員決定状態
    private float m_color = 0;

	// Use this for initialization
	void Start () {
        m_main = GameObject.Find("Main Camera").GetComponent<CameraMove>();
        for(int i=0;i<3;i++){
            m_charSel[i] = m_PlayerSelect[i].GetComponent<charSelect>();
        }
        m_image = m_panel.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if( !m_AllSelected ){
            // キャラセレ
            if (m_main.CharaSelectFlg)
            {
                for (int i = 0; i < 3; i++)
                {
                    m_charSel[i].Selecting();
                }
            }
            
            if (m_charSel[0].m_decisionFlg && m_charSel[1].m_decisionFlg && m_charSel[2].m_decisionFlg){
                // 全員決定状態か
                m_AllSelected = true;
            }
        }
        else
        {
            WhiteOut();
        }
        
	}

    void WhiteOut()
    {
        if( m_color < 1  ){
            m_color+=0.01f;
            m_image.color = new Color(1,1,1,m_color);
        }
        else
        {
            Application.LoadLevel("GameMain");
        }

    }
}
