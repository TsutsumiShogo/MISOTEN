using UnityEngine;
using System.Collections;

public class flower : MonoBehaviour {

    private bool m_scallFlg = false;
    private bool m_growFlg = false;
    private float m_maxUp = 1.5f;
    private float m_maxScall =  0.2f;
    private float m_defaultScall = 0.1f;
    private float m_speed = 0.01f;
    private float m_speedUp = 0.05f;
    private bool m_upDown = false;  // ture で大きく、false で小さく
    private GM_MathFlowerParam m_param;
    public GameObject m_particle;
    public AudioClip m_audio;
    private AudioSource m_audioSource;

    public void Init(){
        m_audioSource = GameObject.Find("ObjectManager").GetComponent<AudioSource>();
        m_audioSource.clip = m_audio;
    }

    // ----------------------------------------------------
    //  更新処理
    // レベルアップ時にスケール演出
    // ----------------------------------------------------
    void Update(){
        if (m_scallFlg){    
            //--------------------------------------
            // スケール拡縮処理
            //--------------------------------------
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

        if (m_param != null)
        {
            //----------------------------------------
            // 成長中のみ、パーティクルエフェクト生成
            if (m_param.GetGrowthNowPlayerNum() > 0)
            {
                m_particle.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                m_particle.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    // -------------------------------
    //  スケール拡縮開始命令
    // -------------------------------
    public void scallOn(){
        m_scallFlg = true;
        m_upDown = true;
        m_audioSource.Play();
    }

    //-------------------------------
    // パラメータオブジェクト取得
    //-------------------------------
    public void SetParam(GM_MathFlowerParam _param){
        m_param = _param;
    }

    
}
