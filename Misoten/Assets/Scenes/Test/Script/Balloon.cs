using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {

    private float m_maxUp = 0.25f;
    private float m_minDown = -0.25f;
    private Vector3 m_defaultPos;
    private float m_speed = 0.01f;
    private bool m_bUpdownFlg = true;   // trueでアップ、falseでダウン

	// Use this for initialization
	void Start () {
        m_defaultPos = this.transform.position;    
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = this.transform.position;
        if (m_bUpdownFlg)
        {   
            // 上昇処理
            pos.y += m_speed;
            this.transform.position = pos;
            if (pos.y >= (m_defaultPos.y+m_maxUp))
                m_bUpdownFlg = false;
        }else{
            // 下降処理
            pos.y -= m_speed;
            this.transform.position = pos;
            if (pos.y <= (m_defaultPos.y + m_minDown))
                m_bUpdownFlg = true;
        }
	}
}
