using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RE_PlantEffect : MonoBehaviour {

    private int m_id;
    private int m_rank;    // plantId
    private int m_flowerNo;   // マテリアル
    private int m_actionStpe = 0;   // アクションステップ

    private bool m_stepFlg_1;
    public float m_animeTime = 0.01f;   // アニメーション間隔
    private int m_frameNum;             // Frame番号

    public Sprite[] m_sprites;          // スプライト
    public Sprite[] m_flowerRed;
    public Sprite[] m_flowerGreen;
    public Sprite[] m_flowerBlue;
    private float m_dTimer = 0;         // アニメーション用タイマー
    private float m_fTimer = 0;         // フラワータイマー
    private float m_firstWait = 0;

    private float m_defaultScall = 1.0f;
    private float[] m_scall = new float[2];
    private float m_overScall = 0.2f;
    private bool m_onFlg = false;
    private bool m_upFlg = false;
    //===============================================================
    // 公開関数
    
    // Init - 初期化
    //---------------------------------
    //
    public void Init(int _rank,int _id)
    {
        m_defaultScall = 1.0f;
        transform.localScale = new Vector3(m_defaultScall, m_defaultScall, m_defaultScall);
        m_scall[0] = 1.5f;
        m_scall[1] = 2.0f;
        m_firstWait = 0.0f;
        m_id = _id;
        m_fTimer = 0.0f;
        m_flowerNo = 0;
        m_rank = 2-_rank;
        m_stepFlg_1 = false;
        m_frameNum = 0;
        m_actionStpe = 0;
        m_dTimer = 0;
        m_onFlg = false;
        m_upFlg = false;
        GetComponent<Image>().sprite = m_sprites[0];
    }
    
    // Action - 更新処理
    //---------------------------------
    //
    public void Action()
    {
        m_firstWait += Time.deltaTime;
        if (m_firstWait >= 1.0f)
        {
            Scall();
            switch (m_actionStpe)
            {
                //--------------------
                // 芽が生えるよ
                case 0:
                    if (!m_stepFlg_1)
                    {
                        m_stepFlg_1 = Animation();
                        if (m_stepFlg_1)
                        {
                            m_actionStpe++;
                        }
                    }
                    break;
                //--------------------
                // つぼみになるよ
                case 1:
                    GreadUp();
                    break;
                //--------------------
                // 花になるよ
                case 2:
                    GreadUp();
                    break;
            }
        }
    }
   
    //===============================================================
    // 非公開関数
    
    // Animation - アニメーション
    //---------------------------------
	// bool - 完了フラグ
    //
    private bool Animation()
    {
        m_dTimer += Time.deltaTime;
        if ( m_animeTime < m_dTimer){
            //m_dTimer = 0.0f;
            m_frameNum++;
            if( m_frameNum >= m_sprites.Length){
                return true;
            }
            GetComponent<Image>().sprite = m_sprites[m_frameNum];
        }
        return false;
    }

    
    // GreadUp
    //---------------------------------
    //
    private void GreadUp()
    {
        m_fTimer += Time.deltaTime;
        if (m_fTimer > 2.0f)
        {
            m_fTimer = 0;
            if (m_rank > 0)
            {
                switch (m_id) {
                    case 0:
                        GetComponent<Image>().sprite = m_flowerRed[m_flowerNo];
                        break;
                    case 1:
                        GetComponent<Image>().sprite = m_flowerGreen[m_flowerNo];
                        break;
                    case 2:
                        GetComponent<Image>().sprite = m_flowerBlue[m_flowerNo];
                        break;
                }
                m_flowerNo++;
                m_frameNum++;
                m_actionStpe++;
                m_rank--;
                m_onFlg = true;
            }
        }
    }

    // Scall
    //---------------------------------
    //
    private void Scall()
    {
        if (m_onFlg)
        {
            if (!m_upFlg)
            {
                // 拡大出現
                transform.localScale = new Vector3(
                    transform.localScale.x + (m_scall[m_actionStpe - 2] + m_overScall) * Time.deltaTime / 0.5f,
                    transform.localScale.y + (m_scall[m_actionStpe - 2] + m_overScall) * Time.deltaTime / 0.5f,
                    1);
                if (transform.localScale.x >= (m_scall[m_actionStpe - 2] + m_overScall))
                {
                    transform.localScale = new Vector3((m_scall[m_actionStpe - 2] + m_overScall), (m_scall[m_actionStpe - 2] + m_overScall), 1);
                    m_upFlg = true;
                }
            }
            else
            {
                // オーバー分縮小
                transform.localScale = new Vector3(
                    transform.localScale.x - m_overScall * Time.deltaTime / 0.2f,
                    transform.localScale.y - m_overScall * Time.deltaTime / 0.2f,
                    1);
                if (transform.localScale.x <= m_scall[m_actionStpe - 2])
                {
                    transform.localScale = new Vector3(m_scall[m_actionStpe - 2], m_scall[m_actionStpe - 2], 1);
                    m_onFlg = false;
                    m_upFlg = false;
                }
            }
        }
    }
}
