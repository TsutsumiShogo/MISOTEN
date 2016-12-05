using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TS_CharaDrawData : MonoBehaviour {
    //データ
    public List<Sprite> charaSpriteDataList;    //キャラクターのイラストデータ格納場所

    public List<int> charaDrawSpriteNoList;     //テキスト番号と表示画像番号の対応リスト
    public List<bool> charaFrontFlgList;        //テキスト番号とキャラクター強調の対応リスト

    [SerializeField]
    private Color FRONT_COLOR;
    [SerializeField]
    private Color FRONT_NOT_COLOR;

    [SerializeField]
    private float CHANGE_NEED_TIME = 0.5f;

    //オブジェクト
    public Image thisImage;

    //内部変数
    private float nowTime;
    private Color oldColor;
    private Color nextColor;

	// Use this for initialization
	void Awake () {
        thisImage = transform.GetComponent<Image>();
	}
    public void Init()
    {
        nowTime = 0.0f;
        thisImage.sprite = charaSpriteDataList[charaDrawSpriteNoList[0]];
        if (charaFrontFlgList[0] == true)
        {
            thisImage.color = FRONT_COLOR;
        }
        else
        {
            thisImage.color = FRONT_NOT_COLOR;
        }
    }

    void Update()
    {
        //時間が終了していたら何もしない
        if (nowTime <= 0.0f)
        {
            thisImage.color = nextColor;
            return;
        }

        //切り替え中
        float _percent;
        _percent = 1.0f - (nowTime / CHANGE_NEED_TIME);

        thisImage.color = Color.Lerp(oldColor, nextColor, _percent);
    }

    //キャラ画像変更処理を開始する
    public void StartChangeImage(int _textNo)
    {
        //正しい範囲かチェック
        if (_textNo < 0 || _textNo >= charaFrontFlgList.Count)
        {
            return;
        }

        //遷移時間を設定
        nowTime = CHANGE_NEED_TIME;

        //今の色を保存
        oldColor = thisImage.color;

        //次の色を決める
        if (charaFrontFlgList[_textNo] == true)
        {
            nextColor = FRONT_COLOR;
        }
        else
        {
            nextColor = FRONT_NOT_COLOR;
        }
    }
}
