using UnityEngine;
using System.Collections;

public class ResultManager : MonoBehaviour {

    [SerializeField]
    private GameObject m_resultMenu;        // リザルトメニュー
    [SerializeField]
    private GameObject m_totalScore;        // トータルスコア
    private bool m_menuFlg = false;

    private bool m_totalFlg = false;


    //===============================================================
    // 公開関数
    public void Init(){
        m_totalFlg = false;
        m_menuFlg = false;
        m_resultMenu.GetComponent<RE_Menu>().Init();
        m_totalScore.GetComponent<TotalScore>().Init();
    }

    //===============================================================
    // 未公開関数
    // Use this for initialization
    void Start()
    {
        
    }

    void Update () {
        if (!m_totalFlg){
            m_totalFlg = m_totalScore.GetComponent<TotalScore>().Action();
        }else { 
            m_menuFlg = true;
        }

        if (m_menuFlg)
        {
            // メニュー
            m_resultMenu.GetComponent<RE_Menu>().Action();      
        }
	}
}
