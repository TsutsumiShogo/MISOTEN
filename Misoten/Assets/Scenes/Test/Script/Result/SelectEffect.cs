using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SelectEffect : MonoBehaviour {


    //-----------------------
    // 公開パラメータ
    public Color m_defaultColor;        // デフォルトカラー
    public float m_alpha;               // どこまで薄くするか
    public float m_flashTime;           // 点滅速度
    public int m_flashCount;            // 点滅回数(何回で終わるか)

    //-----------------------
    // 未公開パラメータ
    private int m_flashCounter;         // 点滅回数計測
    private bool m_swichFlg;            // 点滅処理用フラグ

    //===============================================================
    // 公開関数
    
    // Init - 初期化処理
    //---------------------------------
    //
    public void Init(){
        m_flashCounter = 0; // 計測を初期化
    }

    // Action - 更新処理
    //---------------------------------
    // bool - 終了フラグ
    //
    public bool Action(){
        
        if(!m_swichFlg){
            GetComponent<Text>().color = new Color( m_defaultColor.r, m_defaultColor.g, m_defaultColor.b, GetComponent<Text>().color.a + (m_alpha - 1.0f) * Time.deltaTime / m_flashTime);
            if(GetComponent<Text>().color.a <= m_alpha){
                GetComponent<Text>().color = new Color(m_defaultColor.r, m_defaultColor.g, m_defaultColor.b, m_alpha);
                m_flashCounter++;
                m_swichFlg = true;
            }
        }
        else
        {
            GetComponent<Text>().color = new Color(m_defaultColor.r, m_defaultColor.g, m_defaultColor.b, GetComponent<Text>().color.a - (m_alpha - 1) * Time.deltaTime / m_flashTime);
            if (GetComponent<Text>().color.a >= 1.0f){
                GetComponent<Text>().color = new Color(m_defaultColor.r, m_defaultColor.g, m_defaultColor.b, 1);
                m_flashCounter++;
                m_swichFlg = false;
            }
        }

        // 回数を超えると終了フラグを返す
        if( m_flashCounter >= m_flashCount){
            return false;
        }

        return true;       // 実行中
    }

    //===============================================================
    // 未公開関数

	
}
