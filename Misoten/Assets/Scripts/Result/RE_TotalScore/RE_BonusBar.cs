using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RE_BonusBar : MonoBehaviour {

    [SerializeField]
    private GameObject[] m_text = new GameObject[2];    // テキストオブジェクト
    private float m_targetPos;      // 移動目標座標
    private bool m_activeFlg;       // 使用フラグ
    private float m_time;           // 移動にかかる時間
    private float m_alpha;          // α値
    //===============================================================
    // 公開関数
   
    // Init - 初期化処理
    //---------------------------------
    public void Init(){
        transform.localPosition = new Vector3(0.0f, -50.0f, 0.0f);      // 初期座標
        m_text[0].GetComponent<Text>().color = new Color(0, 0, 0, 0);   // 黒文字透明     
        m_text[1].GetComponent<Text>().color = new Color(0, 0, 0, 0);   // 黒文字透明     
        m_activeFlg = false;                                            // 使用フラグ初期化
        m_targetPos = 0.0f;
        m_alpha = 0.0f;
        m_time = 0.75f;
    }
    
    // Action - 更新処理
    //---------------------------------
    //
    public bool Action(){
        if( m_activeFlg){
            // 使用フラグを立てないと処理されない
            m_alpha += 1.0f * Time.deltaTime / m_time;  // α値
            transform.localPosition = new Vector3(0,transform.localPosition.y + (m_targetPos+50.0f)*Time.deltaTime/m_time,0);
            m_text[0].GetComponent<Text>().color = new Color(0, 0, 0, m_alpha);
            m_text[1].GetComponent<Text>().color = new Color(0, 0, 0, m_alpha);
            if ( transform.localPosition.y >= m_targetPos){
                transform.localPosition = new Vector3(0, m_targetPos, 0);
                m_text[0].GetComponent<Text>().color = new Color(0, 0, 0, 1);
                m_text[1].GetComponent<Text>().color = new Color(0, 0, 0, 1);
                m_activeFlg = false;
                return true;
            }
        }
        return false;
    }

    // OnActive - 使用フラグを立てる
    //---------------------------------
    // float    - 目標座標を設定
    //---------------------------------
    //
    public void OnActive( float _target ){
        m_activeFlg = true;
        m_targetPos = _target;
    }

    public bool GetActive()
    {
        return m_activeFlg;
    }
    //===============================================================
    // 未公開関数

   
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
