using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM_UIGameTime : MonoBehaviour {

    [SerializeField]
    private GameObject[] sunPoints = new GameObject[3];  //Unity上でセット

    //ゲームのシーンマネージャ
    private GM_SceneManager sceneManager;

    //時間表示テキスト
    private Text timeCount;
    //太陽オブジェクト
    private GameObject sun;


	// Use this for initialization
	void Awake () {
        sceneManager = GameObject.Find("GameMainObjects").GetComponent<GM_SceneManager>();
        timeCount = transform.Find("TimeCount").GetComponent<Text>();
        sun = transform.Find("Sun").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        //===================時間表示テキストの更新=======================
        float countDown = sceneManager.GAME_TIME - sceneManager.gameTime;
        int min, sec;
        if (countDown < 0.0f)
        {
            countDown = 0;
        }
        //秒、分の値を計算
        min = (int)(countDown / 60.0f);
        sec = (int)(countDown) % 60;
        if (sec < 10)
        {
            timeCount.text = "TIME " + min.ToString() + ":" + "0" + sec.ToString();
        }
        else
        {
            timeCount.text = "TIME " + min.ToString() + ":" + sec.ToString();
        }

        //===================太陽の更新=========================

        //太陽の位置を割合で数値化
        float _percent = sceneManager.gameTime / sceneManager.GAME_TIME;
        if (_percent < 0.0f)
        {
            _percent = 0.0f;
        }
        if (_percent > 1.0f)
        {
            _percent = 1.0f;
        }

        //移動する範囲をＸＹそれぞれ計算
        float width = sunPoints[2].transform.position.x - sunPoints[0].transform.position.x;
        float height = sunPoints[1].transform.position.y - sunPoints[0].transform.position.y;
        
        //中央位置を算出
        Vector3 centerPos = (sunPoints[0].transform.position + sunPoints[2].transform.position) * 0.5f;

        //太陽の位置を計算
        Vector3 sunPos = centerPos;
        sunPos.x = sunPoints[0].transform.position.x + width * _percent;
        sunPos.y = centerPos.y + height * Mathf.Sin(3.14159265f * _percent);
        
        //太陽の位置を適用
        sun.transform.position = sunPos;
	}
}
