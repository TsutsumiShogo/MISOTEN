using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PushAEffect : MonoBehaviour
{
    public GameObject PushA;            // 「PushA」のテキスト
    private float Elapsedtime;          // 経過時間
    private Vector3 DefaultSize;        // 初期サイズ
    private Vector3 ScaleUpSize;        // 拡大後のサイズ
    private bool SizeChangeflg;         // 拡大縮小の切り替えフラグ     拡大:false    縮小:true

    //-----------------
    // 点滅用変数
    private float m_scallmag = 20.0f;   // スケール変化速度倍率
    private int m_flashCnt = 0;         // 点滅回数カウンタ
    private const int FLASH_NUM = 10;   // 点滅回数
    public bool m_flashEnd = false;     // 点滅終了フラグ
    public bool m_buttonFlg = false;    // ボタン押フラグ


    // Use this for initialization
    void Start()
    {
        DefaultSize = this.gameObject.transform.localScale;       // 初期サイズの取得
        ScaleUpSize = Vector3.zero;     // 初期化
        ScaleUpSize.x = 1.2f;           // ｘの初期化
        ScaleUpSize.y = 1.2f;           // yの初期化
        ScaleUpSize.z = 1.2f;           // zの初期化
        SizeChangeflg = false;          // trueで縮小、falseで拡大
        
    }

    // Update is called once per frame
    void Update()
    {
        Elapsedtime += Time.deltaTime;

        if (!m_buttonFlg)
        {
            // 待機状態
            IdolAnim();
        }
        else
        {
            if (m_flashCnt < FLASH_NUM)
            {
                // 点滅演出
                PushAnim();
            }
            else
            {
                m_flashEnd = true;
            }
        }
    
    }


    //========================================================
    // 未公開関数
    //========================================================
    // --------------------------------
    // ボタン押下待機拡縮演出
    void IdolAnim()
    {
        #region 拡大
        if (!SizeChangeflg)
        {
            PushA.transform.localScale = Vector3.Lerp(DefaultSize, ScaleUpSize, Elapsedtime);
            if (PushA.gameObject.transform.localScale.x >= ScaleUpSize.x)
            {
                Elapsedtime = 0.0f;     // 経過時間の初期化
                PushA.transform.localScale = ScaleUpSize;
                SizeChangeflg = true;   // フラグ:縮小
            }
        }
        #endregion
        #region 縮小
        else if (SizeChangeflg)
        {
            PushA.transform.localScale = Vector3.Lerp(ScaleUpSize, DefaultSize, Elapsedtime);
            if (PushA.transform.localScale.x <= DefaultSize.x)
            {
                Elapsedtime = 0.0f;     // 経過時間の初期化
                PushA.transform.localScale = DefaultSize;
                SizeChangeflg = false;  // フラグ:拡大
            }
        }
        #endregion
    }
    //---------------------------------
    // ボタン押下時アニメーション
    void PushAnim()
    {
        #region 拡大
        if (!SizeChangeflg)
        {
            PushA.transform.localScale = Vector3.Lerp(DefaultSize, ScaleUpSize, Elapsedtime * m_scallmag);
            if (PushA.gameObject.transform.localScale.x >= ScaleUpSize.x)
            {
                Elapsedtime = 0.0f;     // 経過時間の初期化
                PushA.transform.localScale = ScaleUpSize;
                SizeChangeflg = true;   // フラグ:縮小
                m_flashCnt++;
            }
        }
        #endregion
        #region 縮小
        else if (SizeChangeflg)
        {
            PushA.transform.localScale = Vector3.Lerp(ScaleUpSize, DefaultSize, Elapsedtime * m_scallmag);
            if (PushA.transform.localScale.x <= DefaultSize.x)
            {
                Elapsedtime = 0.0f;     // 経過時間の初期化
                PushA.transform.localScale = DefaultSize;
                SizeChangeflg = false;  // フラグ:拡大
                m_flashCnt++;
            }
        }
        #endregion
    }
    
}
