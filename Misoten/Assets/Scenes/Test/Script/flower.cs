using UnityEngine;
using System.Collections;

public class flower : MonoBehaviour {

    private bool m_scallFlg = false;
    private float m_maxUp = 1.5f;
    private float m_maxScall =  0.3f;
    private float m_defaultScall = 0.1f;
    private float m_speed = 0.01f;
    private float m_speedUp = 0.05f;
    private bool m_upDown = false;  // ture で大きく、false で小さく

    void Update(){
        if (m_scallFlg){
            Vector3 scall = transform.localScale;
            Vector3 pos = transform.position;
            if (m_upDown){
                scall.x += m_speed;
                scall.z += m_speed;
                pos.y += m_speedUp;
                if (scall.x >= m_maxScall)
                    m_upDown = false;
            }
            else
            {
                scall.x -= m_speed;
                scall.z -= m_speed;
                pos.y -= m_speedUp;
                if (scall.x <= m_defaultScall){
                    m_scallFlg = false;
                    scall.x = m_defaultScall;
                    scall.z = m_defaultScall;
                    pos.y = 0.5f;
                }
            }
            transform.localScale = scall;
            transform.position = pos;
        }
    }

    public void scallOn(){
        m_scallFlg = true;
        m_upDown = true;
    }
   
}
