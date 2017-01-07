using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RE_NextButton : MonoBehaviour {

    public float m_startPos = 400.0f;
    public float m_afterPos = 420.0f;
    private float m_time = 0.5f;
    private bool m_flg = true;

    //===============================================================
    // 公開関数

    // Init - 初期化
    //---------------------------------
    // 
    public void Init(){
        GetComponent<Text>().color = new Color(1, 1, 1, 0);
        transform.FindChild("Image").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        transform.localPosition = new Vector3(m_startPos,transform.localPosition.y, transform.localPosition.z);
    }
    // Action - 更新処理
    //---------------------------------
    //
    public TotalScore.RE_TOTAL_STEP Action()
    {
        GetComponent<Text>().color = new Color(1, 1, 1, 1);
        transform.FindChild("Image").GetComponent<Image>().color = new Color(1, 1, 1, 1);
        Move();
        if (XboxController.GetButtonA_All() || Input.GetKeyDown(KeyCode.A))
        {
            SoundManager.PlaySe("decision_1", 2);
            GetComponent<Text>().color = new Color(1, 1, 1,0);
            transform.FindChild("Image").GetComponent<Image>().color = new Color(1, 1, 1, 0);
            return TotalScore.RE_TOTAL_STEP.RE_TOTAL_STEP_MAX;
        }
        return TotalScore.RE_TOTAL_STEP.NEXT_BUTTON;
    }
    //===============================================================
    // 未公開関数
	
    // Move - 演出
    //---------------------------------
    //	
    private void Move() {
		if( m_flg){
            
            transform.localPosition = new Vector3(transform.localPosition.x + (20.0f * Time.deltaTime / m_time), transform.localPosition.y, transform.localPosition.z);
            if(transform.localPosition.x >= m_afterPos){
                transform.localPosition = new Vector3(m_afterPos, transform.localPosition.y, transform.localPosition.z);
                m_flg = false;
            }
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x - (20.0f * Time.deltaTime / m_time), transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.x <= m_startPos)
            {
                transform.localPosition = new Vector3(m_startPos, transform.localPosition.y, transform.localPosition.z);
                m_flg = true;
            }
        }
	}
}
