using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChengeSplay : MonoBehaviour {

    [SerializeField]
    private GameObject[] m_Icon = new GameObject[6];            // 種まき

    private int[] m_IconId = new int[6];

    private float m_time = 0.3f;                                // 0.5秒で処理速度

    public Vector3[] m_iconPos = new Vector3[6];                      // 座標

    private bool m_moveFlg;             // 移動フラグ
    private bool m_moveDir;             // 移動向き true 右　false 左

    private float m_selectY;
    private float m_selectX;
    private float m_scallUp;

    private float m_selectScall;
    private float m_defaultScall;

    private int m_frameCnt;

    private int m_selectNo;
    private int m_selectOld;
    private int m_selectOther;
    

    private Color[] m_color = new Color[3];  // LRボタンカラー
    private Color[] m_colorLR = new Color[2];

    [SerializeField]
    private GameObject[] m_button = new GameObject[2];  // LRButton
    [SerializeField]
    private GameObject m_splayName;                     // スプレーの名前
    private string[] m_splayStr = new string[3];
    
    private bool[] m_pushFlg = new bool[2];
    private float[] m_pushTimer = new float[2];
    private int m_select = 0;
    
    //===============================================================
    // 公開関数

    // Init - 初期化処理
    //---------------------------------
    //
    public void Init()
    {
        m_moveFlg = false;
      
        // 座標初期化
        for( int i=0;i<6;i++){
            m_Icon[i].transform.localPosition = m_iconPos[i];
            m_IconId[i] = i;
        }

        // スケール初期化
        m_Icon[0].transform.localScale = new Vector3(0.4f, 0.4f, 1.0f);
        m_Icon[1].transform.localScale = new Vector3(0.7f, 0.7f, 1.0f);
        m_Icon[2].transform.localScale = new Vector3(0.4f, 0.4f, 1.0f);
        m_Icon[3].transform.localScale = new Vector3(0.4f, 0.4f, 1.0f);
        m_Icon[4].transform.localScale = new Vector3(0.4f, 0.4f, 1.0f);
        m_Icon[5].transform.localScale = new Vector3(0.4f, 0.4f, 1.0f);


        // スプレー以外のUI
        m_select = 1;
        m_splayStr[0] = "カラーモード";
        m_splayStr[1] = "種まきモード";
        m_splayStr[2] = "成長モード";
        m_color[0] = new Color(255.0f / 255.0f, 143.0f / 255.0f, 239.0f / 255.0f);
        m_color[1] = new Color(115.0f / 255.0f, 255.0f / 255.0f, 0);
        m_color[2] = new Color(255.0f / 255.0f, 142.0f / 255.0f, 0);
        m_colorLR[0] = new Color(176.0f / 255.0f, 176.0f / 255.0f, 176.0f / 255.0f);
        m_colorLR[1] = new Color(129.0f / 255.0f, 129.0f / 255.0f, 129.0f / 255.0f);
        m_button[0].GetComponent<Image>().color = m_colorLR[0];
        m_button[1].GetComponent<Image>().color = m_colorLR[0];
        m_pushFlg[0] = false;
        m_pushFlg[1] = false;
        m_splayName.GetComponent<Text>().text = m_splayStr[1];
        m_splayName.GetComponent<Text>().color = m_color[1];
    }

    // SplayChenge - スプレー切り替え演出 プレイヤー操作から呼び出される
    //---------------------------------
    //
    public void SplayChenge(bool _dir)
    {
        m_moveFlg = true;
        m_moveDir = _dir;
        if (_dir) {
            m_select--;
            if (m_select < 0) m_select = 2;
        }else{ 
            m_select = (m_select + 1) % 3;

        }
        
        m_splayName.GetComponent<Text>().text = m_splayStr[m_select];
        m_splayName.GetComponent<Text>().color = m_color[m_select];
        SoundManager.PlaySe("chengesplay", 6);
    }

    
    //===============================================================
    // 未公開関数

    // Use this for initialization
    void Start () {
        


    }
	
	// Update is called once per frame
	void Update () {
        if (m_moveFlg){
            move();
        }
        for(int i = 0; i < 2; i++){
            if( m_pushFlg[i]){
                m_pushTimer[i] -= Time.deltaTime;
                if(m_pushTimer[i]<= 0){
                    m_pushFlg[i] = false;
                    m_button[i].GetComponent<Image>().color = m_colorLR[0];
                }
            }
        }
	}




    // move - スプレー移動処理
    //---------------------------------
    //
    void move(){

        if (m_moveDir)
        {

            //-------------------------
            // Rボタン入力処理
            m_pushFlg[0] = true;
            m_pushTimer[0] = 0.3f;
            m_button[0].GetComponent<Image>().color = m_colorLR[1];
            RollRight();


        }
        else
        {
            //-------------------------
            // Lボタン入力処理
            m_pushFlg[1] = true;
            m_pushTimer[1] = 0.3f;
            m_button[1].GetComponent<Image>().color = m_colorLR[1];
            RollLeft();
        }
    }

    // RollRight - 右回転
    //---------------------------------
    //
    private void RollRight(){
       
        for( int i = 0; i<6;i++){
            int _next = (m_IconId[i] + 1)%6;
            switch (m_IconId[i]){
                case 0:
                    m_Icon[i].transform.localPosition = new Vector3(
                        m_Icon[i].transform.localPosition.x + (m_iconPos[_next].x - m_iconPos[m_IconId[i]].x) * Time.deltaTime / m_time,
                        m_Icon[i].transform.localPosition.y + (m_iconPos[_next].y - m_iconPos[m_IconId[i]].y) * Time.deltaTime / m_time,
                        1.0f);
                    m_Icon[i].transform.localScale = new Vector3(
                        m_Icon[i].transform.localScale.x +  0.3f * Time.deltaTime / m_time,
                        m_Icon[i].transform.localScale.y +  0.3f * Time.deltaTime / m_time,
                        1.0f);
                    break;
                case 1:
                    m_Icon[i].transform.localPosition = new Vector3(
                         m_Icon[i].transform.localPosition.x + (m_iconPos[_next].x - m_iconPos[m_IconId[i]].x) * Time.deltaTime / m_time,
                         m_Icon[i].transform.localPosition.y + (m_iconPos[_next].y - m_iconPos[m_IconId[i]].y) * Time.deltaTime / m_time,
                         1.0f);
                    m_Icon[i].transform.localScale = new Vector3(
                        m_Icon[i].transform.localScale.x - 0.3f * Time.deltaTime / m_time,
                        m_Icon[i].transform.localScale.y - 0.3f * Time.deltaTime / m_time,
                        1.0f);
                    break;
                case 2:
                    m_Icon[i].transform.localPosition = new Vector3(
                        m_Icon[i].transform.localPosition.x + (m_iconPos[_next].x - m_iconPos[m_IconId[i]].x) * Time.deltaTime / m_time,
                        m_Icon[i].transform.localPosition.y + (m_iconPos[_next].y - m_iconPos[m_IconId[i]].y) * Time.deltaTime / m_time,
                        1.0f);
                    break;
                case 3:
                    m_Icon[i].transform.localPosition = new Vector3(
                          m_Icon[i].transform.localPosition.x + (m_iconPos[_next].x - m_iconPos[m_IconId[i]].x) * Time.deltaTime / m_time,
                          m_Icon[i].transform.localPosition.y + (m_iconPos[_next].y - m_iconPos[m_IconId[i]].y) * Time.deltaTime / m_time,
                          1.0f);
                    break;
                case 4:
                    m_Icon[i].transform.localPosition = new Vector3(
                         m_Icon[i].transform.localPosition.x + (m_iconPos[_next].x - m_iconPos[m_IconId[i]].x) * Time.deltaTime / m_time,
                         m_Icon[i].transform.localPosition.y + (m_iconPos[_next].y - m_iconPos[m_IconId[i]].y) * Time.deltaTime / m_time,
                         1.0f);
                    break;
                case 5:
                    m_Icon[i].transform.localPosition = new Vector3(
                         m_Icon[i].transform.localPosition.x + (m_iconPos[_next].x - m_iconPos[m_IconId[i]].x) * Time.deltaTime / m_time,
                         m_Icon[i].transform.localPosition.y + (m_iconPos[_next].y - m_iconPos[m_IconId[i]].y) * Time.deltaTime / m_time,
                         1.0f);
                    break;
                
            }
        }
       
        for( int i = 0;i<6;i++){
            if (m_IconId[i] == 0){
                if(m_Icon[i].transform.localPosition.x >= m_iconPos[1].x){
                    for( int j = 0;j<6;j++){
                        int __next = (m_IconId[j] + 1) % 6;
                        m_Icon[j].transform.localPosition = m_iconPos[__next];
                        m_IconId[j] = __next;
                    }
                    m_moveFlg = false;
                    break;
                }
            }
        } 
    }


    // RollLeft - 左回転
    //---------------------------------
    //
    private void RollLeft()
    {

        for (int i = 0; i < 6; i++)
        {
            int _next = (m_IconId[i] - 1);
            if (_next < 0) _next = 5;
            switch (m_IconId[i])
            {
                case 0:
                    m_Icon[i].transform.localPosition = new Vector3(
                        m_Icon[i].transform.localPosition.x + (m_iconPos[_next].x - m_iconPos[m_IconId[i]].x) * Time.deltaTime / m_time,
                        m_Icon[i].transform.localPosition.y + (m_iconPos[_next].y - m_iconPos[m_IconId[i]].y) * Time.deltaTime / m_time,
                        1.0f);
                    break;
                case 1:
                    m_Icon[i].transform.localPosition = new Vector3(
                         m_Icon[i].transform.localPosition.x + (m_iconPos[_next].x - m_iconPos[m_IconId[i]].x) * Time.deltaTime / m_time,
                         m_Icon[i].transform.localPosition.y + (m_iconPos[_next].y - m_iconPos[m_IconId[i]].y) * Time.deltaTime / m_time,
                         1.0f);
                    m_Icon[i].transform.localScale = new Vector3(
                        m_Icon[i].transform.localScale.x - 0.3f * Time.deltaTime / m_time,
                        m_Icon[i].transform.localScale.y - 0.3f * Time.deltaTime / m_time,
                        1.0f);
                    break;
                case 2:
                    m_Icon[i].transform.localPosition = new Vector3(
                        m_Icon[i].transform.localPosition.x + (m_iconPos[_next].x - m_iconPos[m_IconId[i]].x) * Time.deltaTime / m_time,
                        m_Icon[i].transform.localPosition.y + (m_iconPos[_next].y - m_iconPos[m_IconId[i]].y) * Time.deltaTime / m_time,
                        1.0f);
                    m_Icon[i].transform.localScale = new Vector3(
                        m_Icon[i].transform.localScale.x + 0.3f * Time.deltaTime / m_time,
                        m_Icon[i].transform.localScale.y + 0.3f * Time.deltaTime / m_time,
                        1.0f);
                    break;
                case 3:
                    m_Icon[i].transform.localPosition = new Vector3(
                          m_Icon[i].transform.localPosition.x + (m_iconPos[_next].x - m_iconPos[m_IconId[i]].x) * Time.deltaTime / m_time,
                          m_Icon[i].transform.localPosition.y + (m_iconPos[_next].y - m_iconPos[m_IconId[i]].y) * Time.deltaTime / m_time,
                          1.0f);
                    break;
                case 4:
                    m_Icon[i].transform.localPosition = new Vector3(
                         m_Icon[i].transform.localPosition.x + (m_iconPos[_next].x - m_iconPos[m_IconId[i]].x) * Time.deltaTime / m_time,
                         m_Icon[i].transform.localPosition.y + (m_iconPos[_next].y - m_iconPos[m_IconId[i]].y) * Time.deltaTime / m_time,
                         1.0f);
                    break;
                case 5:
                    m_Icon[i].transform.localPosition = new Vector3(
                         m_Icon[i].transform.localPosition.x + (m_iconPos[_next].x - m_iconPos[m_IconId[i]].x) * Time.deltaTime / m_time,
                         m_Icon[i].transform.localPosition.y + (m_iconPos[_next].y - m_iconPos[m_IconId[i]].y) * Time.deltaTime / m_time,
                         1.0f);
                    break;

            }
        }

        for (int i = 0; i < 6; i++)
        {
            if (m_IconId[i] == 2)
            {
                if (m_Icon[i].transform.localPosition.x <= m_iconPos[1].x)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        int __next = (m_IconId[j] - 1);
                        if (__next < 0) __next = 5;
                        m_Icon[j].transform.localPosition = m_iconPos[__next];
                        m_IconId[j] = __next;
                    }
                    m_moveFlg = false;
                    break;
                }
            }
        }
    }



}
