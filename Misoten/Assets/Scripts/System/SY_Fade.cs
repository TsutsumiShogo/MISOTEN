using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SY_Fade : MonoBehaviour {

    //定数定義
    private float FADE_END_TIME;    //モードの遷移時間

    //オブジェクト
    private Image thisImage;

    //内部変数
    private float nowTime;
    private bool fadeMode;      //trueで隠す

	// Use this for initialization
	void Awake () {
        thisImage = transform.GetComponent<Image>();

        FADE_END_TIME = 0.5f;
        nowTime = FADE_END_TIME + 0.1f;
        fadeMode = false;
	}
	
	// Update is called once per frame
	void Update () {
        //完了済みなら何もしない
        if (nowTime >= FADE_END_TIME)
        {
            return;
        }

        //時間を進める
        nowTime += Time.deltaTime;
        if (nowTime > FADE_END_TIME)
        {
            nowTime = FADE_END_TIME;
        }

        //進行度合いを求める
        float _percent = nowTime / FADE_END_TIME;

        //モード別で動作切り替え(trueで隠す方向)
        float _alpha;
        if (fadeMode == true)
        {
            _alpha = _percent;
        }
        else
        {
            _alpha = 1.0f - _percent;
        }

        //αを適用
        Color _setColor = thisImage.color;
        _setColor.a = _alpha;
        thisImage.color = _setColor;

	}
    //フェードアウト開始
    public void FadeOutStart()
    {
        //時間初期化
        fadeMode = true;
        nowTime = 0.0f;
    }
    //フェードイン開始
    public void FadeInStart()
    {
        //時間初期化
        fadeMode = false;
        nowTime = 0.0f;
    }
    //フェードアウト完了ならtrue
    public bool CheckEndFadeOut()
    {
        if (fadeMode == true && nowTime >= FADE_END_TIME)
        {
            return true;
        }
        return false;
    }
    //フェードイン完了ならfalse
    public bool CheckEndFadeIn()
    {
        if (fadeMode == false && nowTime >= FADE_END_TIME)
        {
            return true;
        }
        return false;
    }
}
