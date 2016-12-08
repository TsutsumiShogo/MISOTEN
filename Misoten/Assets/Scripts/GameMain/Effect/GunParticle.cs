using UnityEngine;
using System.Collections;

public class GunParticle : MonoBehaviour {

    public GameObject m_RightGun;           // 右スプレーガン
    public GameObject m_LeftGun;            // 左スプレーガン
    public GameObject m_RightParticleObj;       // 右パーティクルオブジェクト
    public GameObject m_LeftParticleObj;        // 左パーティクルオブジェクト
    private ParticleSystem m_RightParticle;     // 右パーティクル
    private ParticleSystem m_LeftParticle;      // 左パーティクル

    public bool m_SprayFlg = false;

	// Use this for initialization
	void Start () {
        // パーティクルオブジェクトから取得
        m_RightParticle = m_RightParticleObj.GetComponent<ParticleSystem>();
        m_LeftParticle = m_LeftParticleObj.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {

        if (m_SprayFlg){
            m_RightParticleObj.transform.position = m_RightGun.transform.localPosition;
            m_LeftParticleObj.transform.position = m_LeftGun.transform.position;
        }
	    
	}

    // これを呼び出しでパーティクル発動
    public void OnSpray(){
        if (!m_SprayFlg) m_SprayFlg = true;
        m_RightParticle.Play();
        m_LeftParticle.Play();
    }
}
