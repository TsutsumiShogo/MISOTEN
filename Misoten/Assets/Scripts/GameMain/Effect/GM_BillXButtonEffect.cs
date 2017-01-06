using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_BillXButtonEffect : MonoBehaviour {

    //リソース
    [SerializeField]
    private List<Sprite> XBUTTON_TEXTURE;

    //オブジェクト
    private SpriteRenderer thisRender;
    private PlayerCheckCollisionBill checkCollisionBill;

    //内部変数
    private float changeTime;   //ポチポチ切り替え時間

	// Use this for initialization
	void Awake () {
        checkCollisionBill = transform.parent.FindChild("CheckCollisionBill").GetComponent<PlayerCheckCollisionBill>();
        thisRender = transform.GetComponent<SpriteRenderer>();
        changeTime = 0.0f;
	}
    public void Init()
    {
        changeTime = 0.0f;
        thisRender.sprite = null;
    }
	
	// Update is called once per frame
	void Update () {
        //エラーチェック
        if (XBUTTON_TEXTURE.Count < 2)
        {
            Debug.LogError("XButtonTexture足りない。２枚入れて");
            this.gameObject.SetActive(false);
            return;
        }

        //描画フラグ確認
        if (checkCollisionBill.CheckCollisionBill() == false)
        {
            thisRender.sprite = null;
            return;
        }
        //ビル最大レベルなら描画しない
        if (checkCollisionBill.billParam.flowerLevel == GM_MathFlowerParam.EFlowerLevel.Level3)
        {
            thisRender.sprite = null;
            return;
        }

        //プレイヤーに合わせて回転したくない
        transform.rotation = Quaternion.Euler(0, 0, 0);

        changeTime += Time.deltaTime;

        //点滅させるための時間
        if(changeTime > 0.5f){
            changeTime -= 1.0f;
        }

        //画像切り替え
        if (changeTime < 0.0f)
        {
            thisRender.sprite = XBUTTON_TEXTURE[0];
        }
        else
        {
            thisRender.sprite = XBUTTON_TEXTURE[1];
        }
	}
}
