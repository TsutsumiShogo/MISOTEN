using UnityEngine;
using System.Collections;

public class ObjectParam : MonoBehaviour {

    public GM_MathFlowerParam.EFlowerType m_type;
    private bool m_scallFlg = false;
    private bool m_growFlg = false;
    private float m_maxUp = 0.7f;
    private float m_maxScall = 1.5f;
    private float m_defaultScall = 1.0f;
    private float m_speed = 0.1f;
    private float m_speedUp = 0.02f;
    private bool m_upDown = false;  // ture で大きく、false で小さく
    private bool m_paricleFlg = false;
    private GM_MathFlowerParam m_param;
    public AudioClip m_audio;
    private AudioSource m_audioSource;

    public GameObject m_levelPartcleObj;            // レベルアップ時エフェクト
    public GameObject m_particleObj;                // パーティクルオブジェクト
    private float m_seTime = 0.5f;
    private float m_seTimer = 0.0f;

    //-----------------------------------------------------
    // 花用辺巣
    public GameObject     m_compParticleObj;        // 花パーティクルオブジェ
    private ParticleSystem m_compParticle;           // 花パーティクル
    public GameObject m_lastParticleObj;        // 花パーティクルオブジェ
    private ParticleSystem m_lastParticle;           // 花パーティクル
    // ----------------------------------------------------
    // 中ビル用変数
    private ParticleSystem m_particle;              // パーティクルシステム
    private ParticleSystem m_levelParticle;       // 特別アクションパーティクル
    private bool m_special;                         // specialアクションFlag
    private bool m_particleOn = false;
    private int m_menberNum = 0;
    public int m_playerId;
	
    //-------------------------------
    // パラメータオブジェクト取得
    //-------------------------------
    public void SetParam(GM_MathFlowerParam _param)
    {
        m_param = _param;
        m_playerId = m_param.m_plantPlayerId;
    }
    // ----------------------------------------------------
    //  花初期化処理
    // ----------------------------------------------------
    public void FlowerInit()
    {
        m_type = GM_MathFlowerParam.EFlowerType.Flower1;
        m_particle = m_particleObj.GetComponent<ParticleSystem>();
        m_compParticle = m_compParticleObj.GetComponent<ParticleSystem>();
        m_lastParticle = m_lastParticleObj.GetComponent<ParticleSystem>();
    }
    // ----------------------------------------------------
    //  家初期化処理
    // ----------------------------------------------------
    public void HouseInit()
    {
        m_type = GM_MathFlowerParam.EFlowerType.House;
        m_particle = m_particleObj.GetComponent<ParticleSystem>();
        m_levelParticle = m_levelPartcleObj.GetComponent<ParticleSystem>();
    }
    // ----------------------------------------------------
    //  中ビル初期化処理
    // ----------------------------------------------------
    public void MiddleBillInit()
    {
        // パーティクルコンポ―ネント取得
        m_type = GM_MathFlowerParam.EFlowerType.Bill;
        m_particle = m_particleObj.GetComponent<ParticleSystem>();
        m_levelParticle = m_levelPartcleObj.GetComponent<ParticleSystem>();
    }
    


    //-------------------------------
    // 花更新処理
    //-------------------------------
    public void flowerUpdate()
    {
        if (m_scallFlg)
        {
            //--------------------------------------
            // スケール拡縮処理
            //--------------------------------------
            Vector3 scall = transform.localScale;
            Vector3 pos = transform.position;
            if (m_upDown)
            {
                scall.x += m_speed;
                scall.z += m_speed;
                scall.y += m_speed;
                
                pos.y += m_speedUp;
                if (scall.x >= m_maxScall)
                    m_upDown = false;
            }
            else
            {
                scall.x -= m_speed;
                scall.y -= m_speed;
                scall.z -= m_speed;
                pos.y -= m_speedUp;
                if (scall.x <= m_defaultScall)
                {
                    m_scallFlg = false;
                    scall.x = m_defaultScall;
                    scall.y = m_defaultScall;
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
                if (!m_paricleFlg)
                {
                    m_paricleFlg = true;
                    m_particle.Play();
                }
            }
            else
            {
                if (m_paricleFlg)
                {
                    m_paricleFlg = false;
                    m_particle.Stop();
                }
            }
        }
    }
    //-------------------------------
    // 家更新処理
    //-------------------------------
    public void houseUpdate()
    {
        if (m_param != null)
        {
            m_menberNum = m_param.GetGrowthNowPlayerNum();
        }
        Growing();
    }
    //-------------------------------
    // 中ビル更新処理
    //-------------------------------
    public void mibbleBillUpdate(){
    if (m_param != null){
            m_menberNum = m_param.GetGrowthNowPlayerNum();
        } 
        Growing();
    }

    //-------------------------------
    //  レベルアップ時エフェクト
    //-------------------------------
    public void LevelUpEff()
    {
        m_levelParticle.Play();
    }
    // -------------------------------
    //  スケール拡縮開始命令
    // -------------------------------
    public void scallOn()
    {
        m_scallFlg = true;
        m_upDown = true;
        //m_audioSource.Play();
    }
    // -------------------------------
    // 花パーティクル開始命令
    // -------------------------------
    public void startFlowerParticle()
    {
        m_compParticleObj.SetActive(true);
        m_lastParticleObj.SetActive(true);
        m_compParticle.Play();
        m_lastParticle.Play();
    }

    // ----------------------------------------------------
    //  成長中に処理する
    // ----------------------------------------------------
    void Growing()
    {
        if (m_menberNum > 0)
        {
            m_seTimer += Time.deltaTime;
            if (m_seTimer >= m_seTime){
                m_seTimer = 0;
                SoundManager.PlaySe("grow_se", 2);
            }
            // -------------------------------------
            // 成長中であればパーティクルを飛ばす
            if (m_menberNum >= 2)
            {
                // 特別アクション
                m_special = true;
            }
            else
            {
                // 特別アクション
                m_special = false;
            }
            
            if (!m_particleOn)
            {
                m_particleOn = true;
                m_particle.Play();
            }
        }
        else
        {
            // -------------------------------------
            // パーティクルストップ
            if (m_particleOn)
            {
                m_particleOn = false;
                m_particle.Stop();
            }

        }
    }
}
