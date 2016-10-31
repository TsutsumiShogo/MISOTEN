using UnityEngine;
using System.Collections;

public class GM_MathCellColorControll : MonoBehaviour {

    //マス目描画オブジェクト
    private SpriteRenderer mathSprite;

    //変数定義
    public Color nextColor;        //目標の色
    public Color oldColor;         //前回の色
    private float nowTime;          //経過時間
    private float endTime;   //経過完了までに必要な時間

	// Use this for initialization
	void Awake () {
        mathSprite = GetComponent<SpriteRenderer>();
	}

    //初期化
    public void Init(){
        Color col;
        col.r = col.g = col.b = 1.0f;
        col.a = 0.0f;
        mathSprite.color = col;
        nextColor = col;
        oldColor = col;
        nowTime = endTime = 2.0f;
        nowTime += 0.1f;
    }

    void Update()
    {
        //経過完了済みなら何もしない
        if (nowTime >= endTime)
        {
            return;
        }

        //変数定義
        Color nowColor = mathSprite.color;
        float percent;

        //時間経過
        nowTime += Time.deltaTime;

        //割合計算
        percent = nowTime / endTime;

        //割合範囲指定
        if (percent < 0.0f) percent = 0.0f;
        if (percent > 1.0f) percent = 1.0f;
        
        //色情報計算
        nowColor = Color.Lerp(oldColor, nextColor, percent);

        //色情報セット
        mathSprite.color = nowColor;

    }

    //色変更はこれで行う
    public void ChangeMathColor(Color changeColor)
    {
        oldColor = nextColor;
        nextColor = changeColor;
        nowTime = 0.0f;
    }
}
