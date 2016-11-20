using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM_UIGreenPercent : MonoBehaviour {

    //自身のテキスト
    private Text thisText;

    //マスマネージャ
    [SerializeField]
    private GM_MathManager mathManager;     //Unity上でセット

	// Use this for initialization
	void Awake () {
        thisText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        float _percent = mathManager.totalFlowerLevel / (float)mathManager.MAX_TOTAL_FLOWER_LEVEL;
        int _percentInt = (int)(_percent * 100.0f);
        thisText.text = "緑化率:" + _percentInt.ToString() + "％";
	}

    

}
