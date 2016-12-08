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

    //オブジェクト
    private GameObject tutorialObject;      //テキストなどの親オブジェクト
    private Image backGround;               //説明時の背景表示部分

    private List<TS_CharaDrawData> charactorImages = new List<TS_CharaDrawData>();    //キャラクター表示部分
    private Text charaName;                 //キャラ名表示部分
    private Text tutorialText;              //説明テキスト表示部分
    

    //内部変数
    private Color nextBackGroundColor;

	// Use this for initialization
	void Awake () {
        tutorialObject = transform.FindChild("TS_Tutorial").gameObject;
        backGround = transform.FindChild("TS_BackGround").GetComponent<Image>();

        charactorImages.Add(transform.FindChild("TS_Tutorial/TS_Sango").GetComponent<TS_CharaDrawData>());
        charactorImages.Add(transform.FindChild("TS_Tutorial/TS_Hisui").GetComponent<TS_CharaDrawData>());
        charactorImages.Add(transform.FindChild("TS_Tutorial/TS_Aoi").GetComponent<TS_CharaDrawData>());
        charaName = transform.FindChild("TS_Tutorial/CharaNameBack/TS_CharaName").GetComponent<Text>();
        tutorialText = transform.FindChild("TS_Tutorial/TextBack/TS_Text").GetComponent<Text>();
        
	}
    public void Init()
    {
        backGround.color = BACK_GROUND_COLOR;
        tutorialObject.SetActive(true);
        charactorImages[0].Init();
        charactorImages[1].Init();
        charactorImages[2].Init();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActiveSwtich(bool _activeFlg)
    {
        //テキスト表示部の有効切り替え
        tutorialObject.SetActive(_activeFlg);
        if (_activeFlg == true)
        {
            nextBackGroundColor = BACK_GROUND_COLOR;
        }
        else
        {
            nextBackGroundColor.r = nextBackGroundColor.g = nextBackGroundColor.b = nextBackGroundColor.a = 0.0f;
        }
    }

    public void ChangeText(int textNo)
    {

    }
}
