//-----------------------------------------------------------------------------
// FILE_NAME : TotalScore.cs
// SCENE　   : GameScene
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class TotalScore : MonoBehaviour {

    private Text m_thisText;

    public Text m_rankText;

    // MathManager
    [SerializeField]
    private GM_MathManager m_mathManager;


	// Use this for initialization
	void Start () {
        
        m_thisText = transform.FindChild("score").GetComponent<Text>();
        float _percent = m_mathManager.totalFlowerLevel / (float)m_mathManager.MAX_TOTAL_FLOWER_LEVEL;
        int score = (int)(_percent * 100.0f);

        if (score >= 100)
            m_rankText.text = "SSS";
        else if (score >= 90)
            m_rankText.text = "SS";
        else if (score >= 80)
            m_rankText.text = "S";
        else if (score >= 70)
            m_rankText.text = "A";
        else if (score >= 50)
            m_rankText.text = "B";
        else if (score >= 30)
            m_rankText.text = "C";
        else if (score >= 20)
            m_rankText.text = "D";
        else
            m_rankText.text = "E";

        m_thisText.text = score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
