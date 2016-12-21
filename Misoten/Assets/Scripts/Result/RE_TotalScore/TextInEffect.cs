using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInEffect : MonoBehaviour {

    public enum EFFECT_TYPE{
        LEFT = 0,
        UP,
        RIGHT,
        DOWN,
    };

    public float m_targetPoint = 0;         // 目標地点    inspecterで設定
    public float m_startPoint = 0;          // 開始地点
    public float m_time = 0;                // 到達までの時間(秒)
    public EFFECT_TYPE m_effectType;        // エフェクトタイプ
    
    public bool m_goalFlg = false;         // 到達フラグ
    //===============================================================
    // 公開関数

    // Init - 初期化処理
    //---------------------------------
    //
    public void Init()
    {
        m_goalFlg = false;      // 到達フラグ初期化
        float _score = GameObject.Find("RE_TotalScore").GetComponent<TotalScore>().m_score; // スコア
        // スコアセット
        transform.FindChild("score").GetComponent<Text>().text = string.Format("{0:#,##0}", _score);

       
        //---------------------
        // 開始位置設定
        if ((m_effectType == EFFECT_TYPE.LEFT) || (m_effectType == EFFECT_TYPE.RIGHT)){
            transform.localPosition = new Vector3(m_startPoint, transform.localPosition.y, transform.localPosition.z);
        }else{
            transform.localPosition = new Vector3(transform.localPosition.x, m_startPoint, transform.localPosition.z);
        }
    }

   
    // Action 
    //---------------------------------
    //
    public TotalScore.RE_TOTAL_STEP Action(){
        Move();
        if( m_goalFlg)
        {
            return TotalScore.RE_TOTAL_STEP.BONUS_IN;
        }
        return TotalScore.RE_TOTAL_STEP.SCORE_IN;
    }

    //===============================================================
    // 未公開関数

    // Move - 移動処理
    //---------------------------------
    // 
    private void Move()
    {
        if (!m_goalFlg)
        {
            switch (m_effectType)
            {
                // 左移動
                case EFFECT_TYPE.LEFT:
                    m_goalFlg = MoveLeft();
                    break;
                // 上移動
                case EFFECT_TYPE.UP:
                    m_goalFlg = MoveUp();
                    break;
                // 右移動
                case EFFECT_TYPE.RIGHT:
                    break;
                // 下移動
                case EFFECT_TYPE.DOWN:
                    break;
            }
        }
    }

    // MoveLeft - 左方向移動
    //---------------------------------
    //
    private bool MoveLeft()
    {
        Vector3 _pos = transform.localPosition;
        _pos.x = transform.localPosition.x - (m_startPoint - m_targetPoint) * Time.deltaTime / m_time;
        if (_pos.x <= m_targetPoint){
            _pos.x = m_targetPoint;
            transform.localPosition = _pos;
            return true;
        }
        transform.localPosition = _pos;
        return false;
    }

    // MoveUp - 上方向移動
    //---------------------------------
    //
    private bool MoveUp()
    {
        Vector3 _pos = transform.localPosition;
        _pos.y = transform.localPosition.y - (m_startPoint - m_targetPoint) * Time.deltaTime / m_time;
        if (_pos.y >= m_targetPoint)
        {
            _pos.y = m_targetPoint;
            transform.localPosition = _pos;
            return true;
        }
        transform.localPosition = _pos;
        return false;
    }


	
}
