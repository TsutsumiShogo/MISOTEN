using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GM_FlowerGageManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] m_gameObj = new GameObject[3];     // Gageオブジェクト
    [SerializeField]
    private GameObject m_mathManager;                       // マスマネージャー

    private int[] m_flowerNum = new int[3];
    //===============================================================
    // 公開関数

    // Init - 初期化
    //---------------------------------
    //
    public void Init(){
        for (int i = 0; i < 3; i++)
        {
            m_gameObj[i].transform.FindChild("Meter").transform.localScale = new Vector3(0, 1, 1);
        }
    }

    // Action - 更新処理 
    //---------------------------------
    //
    public void Action(){
        m_flowerNum[0] = m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.RED);
        m_flowerNum[0] += m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.MAGENTA);
        m_flowerNum[0] += m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.YELLOW);
        m_flowerNum[0] += m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.WHITE);
        m_gameObj[0].transform.FindChild("FlowerCnt").GetComponent<Text>().text = m_flowerNum[0].ToString() + "本";
        m_flowerNum[1] = m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.GREEN);
        m_flowerNum[1] += m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.CYAN);
        m_flowerNum[1] += m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.YELLOW);
        m_flowerNum[1] += m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.WHITE);
        m_gameObj[1].transform.FindChild("FlowerCnt").GetComponent<Text>().text = m_flowerNum[1].ToString() + "本";
        m_flowerNum[2] = m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.BLUE);
        m_flowerNum[2] += m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.MAGENTA);
        m_flowerNum[2] += m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.CYAN);
        m_flowerNum[2] += m_mathManager.GetComponent<GM_MathManager>().GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.WHITE);
        m_gameObj[2].transform.FindChild("FlowerCnt").GetComponent<Text>().text = m_flowerNum[2].ToString() + "本";
        for( int i =0;i<3;i++){
            if (m_flowerNum[i] > 500) m_flowerNum[i] = 500;
            m_gameObj[i].transform.FindChild("Meter").transform.localScale = new Vector3((float)m_flowerNum[i]/500.0f, 1, 1);
        }
    }

    //===============================================================
    // 未公開関数
    void Update(){
        Action();
    }

    void Start()
    {

    }

}
