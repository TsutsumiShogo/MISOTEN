using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM_UIGreenPercent : MonoBehaviour {

    //自身のテキスト
    private Text thisText;

    //マスマネージャー
    private GM_MathManager mathManager;

	// Use this for initialization
	void Awake () {
        mathManager = GameObject.Find("MathManager").GetComponent<GM_MathManager>();
        thisText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        float _percent = mathManager.totalFlowerLevel / (float)mathManager.MAX_TOTAL_FLOWER_LEVEL;
        int _percentInt = (int)(_percent * 100.0f);
        thisText.text = "緑化率:" + _percentInt.ToString() + "％";
	}

    

}
