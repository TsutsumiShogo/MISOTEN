using UnityEngine;
using System.Collections;

public class ResultManager : MonoBehaviour {

    [SerializeField]
    private GameObject m_resultMenu;        // リザルトメニュー
    private bool m_menuFlg = false;

	// Use this for initialization
	void Start () {
        m_resultMenu.GetComponent<RE_Menu>().Init();
	}
	
	// Update is called once per frame
	void Update () {
        if (XboxController.GetButtonA_All()){
            m_menuFlg = true;
         
        }

        if (m_menuFlg)
        {
            m_resultMenu.GetComponent<RE_Menu>().Action();
        }
	}
}
