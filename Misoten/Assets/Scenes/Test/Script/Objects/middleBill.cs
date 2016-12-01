using UnityEngine;
using System.Collections;

public class middleBill : MonoBehaviour {

    private GM_MathFlowerParam m_param;             // パラメータ
    public GameObject m_levelPartcleObj;            // レベルアップ時エフェクト
    public GameObject m_particleObj;                // パーティクルオブジェクト
    private ParticleSystem m_particle;              // パーティクルシステム
    private ParticleSystem m_levelParticle;       // 特別アクションパーティクル
    private bool m_special;                         // specialアクションFlag
    private bool m_particleOn = false;
    private int m_menberNum = 0;


    // ----------------------------------------------------
	//  Use this for initialization
    // ----------------------------------------------------
	void Start () {
        
	}

    // ----------------------------------------------------
    //  初期化処理
    // ----------------------------------------------------
    public void Init(){
        // パーティクルコンポ―ネント取得
        m_particle = m_particleObj.GetComponent<ParticleSystem>();
        m_levelParticle = m_levelPartcleObj.GetComponent<ParticleSystem>();
    }
	    
	// Update is called once per frame
	void Update () {
        if (m_param != null){
            m_menberNum = m_param.GetGrowthNowPlayerNum();
        } 
        Growing();
	}

    // ----------------------------------------------------
    //  成長中に処理する
    // ----------------------------------------------------
    void Growing(){
        if(m_menberNum>0){
            // -------------------------------------
            // 成長中であればパーティクルを飛ばす
            if (m_menberNum >= 2){
                // 特別アクション
                m_special = true;
            }else{
                // 特別アクション
                m_special = false;
            }
            Debug.Log("kaoaa");
            if (!m_particleOn){
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

    //-------------------------------
    // パラメータオブジェクト取得
    //-------------------------------
    public void SetParam(GM_MathFlowerParam _param){
        m_param = _param;
    }

    //-------------------------------
    //  レベルアップ時エフェクト
    //-------------------------------
    public void LevelUpEff(){
        m_levelParticle.Play();
    }
}
