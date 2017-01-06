using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class TS_UIManager : MonoBehaviour {

    //定数定義
    [SerializeField]
    private List<string> CHARA_NAME_LIST;   //Unity上でセット
    [SerializeField]
    private List<string> TEXT_LIST;         //Unity上でセット

    [SerializeField]
    private Color BACK_GROUND_COLOR;
    [SerializeField]
    private float CHANGE_TIME = 2.0f;

    //オブジェクト
    private GM_UIGreenPercent greenScore;   //緑化スコア
    private GameObject tutorialObject;      //テキストなどの親オブジェクト
    private Image backGround;               //説明時の背景表示部分

    private List<TS_CharaDrawData> charactorImages = new List<TS_CharaDrawData>();    //キャラクター表示部分
    private Text charaName;                 //キャラ名表示部分
    private Text tutorialText;              //説明テキスト表示部分
    private Text actionTimeText;            //行動可能時間表示部分

    //内部変数
    private bool activeFlg;
    private float changeTime;

	// Use this for initialization
	void Awake () {
        greenScore = transform.FindChild("GreenScore").GetComponent<GM_UIGreenPercent>();
        tutorialObject = transform.FindChild("TS_Tutorial").gameObject;
        backGround = transform.FindChild("TS_BackGround").GetComponent<Image>();

        charactorImages.Add(transform.FindChild("TS_Tutorial/TS_Sango").GetComponent<TS_CharaDrawData>());
        charactorImages.Add(transform.FindChild("TS_Tutorial/TS_Hisui").GetComponent<TS_CharaDrawData>());
        charactorImages.Add(transform.FindChild("TS_Tutorial/TS_Aoi").GetComponent<TS_CharaDrawData>());

        charactorImages.Add(transform.FindChild("TS_Tutorial/TS_Tane").GetComponent<TS_CharaDrawData>());
        charactorImages.Add(transform.FindChild("TS_Tutorial/TS_Seityou").GetComponent<TS_CharaDrawData>());
        charactorImages.Add(transform.FindChild("TS_Tutorial/TS_Col").GetComponent<TS_CharaDrawData>());

        charaName = transform.FindChild("TS_Tutorial/CharaNameBack/TS_CharaName").GetComponent<Text>();
        tutorialText = transform.FindChild("TS_Tutorial/TextBack/TS_Text").GetComponent<Text>();
        actionTimeText = transform.FindChild("TS_ActionTime").GetComponent<Text>();
	}
    public void Init()
    {
        greenScore.Init();
        backGround.color = BACK_GROUND_COLOR;

        for (int i = 0; i < charactorImages.Count; ++i)
        {
            charactorImages[i].Init();
        }

        activeFlg = true;
        changeTime = CHANGE_TIME + 1.0f;
        SetTimeText(0.0f);
        ActiveSwtich(activeFlg);
    }
	
	// Update is called once per frame
	void Update () {
        //切り替え中かどうか
        if (changeTime < CHANGE_TIME)
        {
            changeTime += Time.deltaTime;
            if (changeTime > CHANGE_TIME)
            {
                changeTime = CHANGE_TIME;
            }

            Color _setColor = BACK_GROUND_COLOR;
            float _percent;
            _percent = changeTime / CHANGE_TIME;

            //どっちに切り替え中なのか
            if (activeFlg == true)
            {
                //有効な方向に切り替え中
                _setColor.a = _percent * BACK_GROUND_COLOR.a;
                backGround.color = _setColor;
            }
            else
            {
                //無効な方向に切り替え中
                _setColor.a = BACK_GROUND_COLOR.a - BACK_GROUND_COLOR.a * _percent;
                backGround.color = _setColor;
            }
        }
	}

    public void ActiveSwtich(bool _activeFlg)
    {
        changeTime = 0.0f;
        activeFlg = _activeFlg;
        //テキスト表示部の有効切り替え
        tutorialObject.SetActive(_activeFlg);
    }

    public void ChangeText(int textNo)
    {
        //文字切り替え
        if (textNo >= 0 && textNo < CHARA_NAME_LIST.Count)
        {
            charaName.text = CHARA_NAME_LIST[textNo];
        }
        if (textNo >= 0 && textNo < TEXT_LIST.Count)
        {
            tutorialText.text = TEXT_LIST[textNo];
        }

        //キャラクター強調効果切り替え
        for (int i = 0; i < charactorImages.Count; i++)
        {
            charactorImages[i].StartChangeImage(textNo);
        }
    }

    public void SetTimeText(float _drawTime)
    {
        int _intTime = (int)_drawTime;

        actionTimeText.text = "残り" + _intTime.ToString() + "秒";
    }
}
