using UnityEngine;
using System.Collections;

public class PlayerSprayControll : MonoBehaviour {

    //定数定義
    public enum EPlayerSprayMode
    {
        SEED,
        GRAW,
        COLOR,
    };
    
    //変数定義
    private PlayerUnit playerUnit;
    private EPlayerSprayMode sprayMode;         //スプレーがどういう行動を起こす予定か
    private float sprayActiveTime = -0.001f;    //正でスプレーが何か行動を起こしている

    void Awake()
    {
        playerUnit = transform.parent.GetComponent<PlayerUnit>();
    }

    //初期化
	void Init () {
        sprayActiveTime = -0.001f;
	}
	
	// Update is called once per frame
	void Update () {
        if (sprayActiveTime >= 0.00f)
        {
            sprayActiveTime -= Time.deltaTime;
        }
	}

    //スプレーを起動させる
    public void Spray(EPlayerSprayMode _mode, float _activeTime)
    {
        sprayMode = _mode;
        sprayActiveTime = _activeTime;
    }

    //あたり判定
    void OnTriggerStay(Collider col)
    {
        //Debug.Log("OnTrigger");
        //起動状態をチェック
        if (sprayActiveTime < 0.00f)
        {
            //何もしない
            return;
        }

        //マスと当たったのかチェック
        GM_MathFlowerParam _flowerParam;
        _flowerParam = col.gameObject.GetComponent<GM_MathFlowerParam>();
        if (_flowerParam == null)
        {
            //マスと当たってなかった
            Debug.LogWarning("PlayerSprayCon : OnTriggerStay : flowerParam == null");
            return;
        }

        //モードに合わせて処理を変更
        switch (sprayMode)
        {
            case EPlayerSprayMode.SEED:
                SowSeed(_flowerParam);
                break;
            case EPlayerSprayMode.GRAW:
                GrowPlant(_flowerParam);
                break;
            case EPlayerSprayMode.COLOR:
                ColorSpray(_flowerParam);
                break;
        }

    }


    //種まき関数
    private void SowSeed(GM_MathFlowerParam _flowerParam)
    {
        //花を有効化(種まき完了)
        _flowerParam.PrantStart();
    }

    //植物を成長させる
    private void GrowPlant(GM_MathFlowerParam _flowerParam)
    {
        //花に経験値を加算(植物成長)
        _flowerParam.PrantGrowth(3);    //引数：毎フレーム経験値加算量
    }

    //色を付ける
    private void ColorSpray(GM_MathFlowerParam _flowerParam)
    {
        GM_MathFlowerParam.EFlowerColor playerColor;

        playerColor = playerUnit.PLAYER_COLOR;
        //花に色を付ける
        //_flowerParam.AddColor();
    }
}
