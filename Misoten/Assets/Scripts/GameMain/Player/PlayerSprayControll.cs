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
    private PlayerStatus playerStatus;

    private EPlayerSprayMode sprayMode;         //スプレーがどういう行動を起こす予定か
    private float sprayActiveTime = -0.001f;    //正でスプレーが何か行動を起こしている
    private float activeTime = 0.0f;            //スプレーが行動を開始してからの経過時間

    [SerializeField]
    private MeshRenderer sprayMeshRender;       //Unity上でセット
    [SerializeField]
    private SphereCollider thisCollider;

    void Awake()
    {
        playerUnit = transform.parent.GetComponent<PlayerUnit>();
        playerStatus = transform.parent.GetComponent<PlayerStatus>();
        thisCollider = transform.GetComponent<SphereCollider>();
    }

    //初期化
	void Init () {
        sprayActiveTime = -0.001f;
        activeTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (sprayActiveTime >= 0.00f)
        {
            sprayActiveTime -= Time.deltaTime;
            activeTime += Time.deltaTime;
        }
        else
        {
            activeTime = 0.0f;
        }

        //スプレー描画
        float _percent;
        _percent = activeTime / playerStatus.SPRAY_MAX_SCALE_NEED_TIME;

        if (_percent < 0.0f)
        {
            _percent = 0.0f;
        }
        if (_percent > 1.0f)
        {
            _percent = 1.0f;
        }
        Color matColor = sprayMeshRender.material.color;
        switch (sprayMode)
        {
            case EPlayerSprayMode.SEED:
                matColor.g = 1.0f;
                matColor.r = matColor.b = matColor.a = 0.0f;
                break;
            case EPlayerSprayMode.GRAW:
                matColor.b = 1.0f;
                matColor.r = matColor.g = matColor.a = 0.0f;
                break;
            case EPlayerSprayMode.COLOR:
                matColor.r = 1.0f;
                matColor.g = matColor.b = matColor.a = 0.0f;
                break;
        }
        matColor.a = _percent;
        sprayMeshRender.material.color = matColor;
	}

    //スプレーを起動させる
    public void Spray(EPlayerSprayMode _mode, float _activeTime)
    {
        sprayMode = _mode;
        sprayActiveTime = _activeTime;
    }
    //スプレーの大きさ変更
    public void ChangeScale(float _scale)
    {
        Vector3 scale = Vector3.one;
        scale.x = scale.z = _scale * 2.0f;

        sprayMeshRender.transform.localScale = scale;
        thisCollider.radius = _scale;

    }

    //あたり判定
    void OnTriggerStay(Collider col)
    {
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
            return;
        }

        //当たったマスがビルなら何もしない
        if (_flowerParam.flowerType == GM_MathFlowerParam.EFlowerType.Bill ||
            _flowerParam.flowerType == GM_MathFlowerParam.EFlowerType.BigBill)
        {
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
        _flowerParam.PrantStart(playerUnit.PLAYER_NO, playerUnit.PLAYER_COLOR);
    }

    //植物を成長させる
    private void GrowPlant(GM_MathFlowerParam _flowerParam)
    {
        //花に経験値を加算(植物成長)
        _flowerParam.AddExp(playerUnit.PLAYER_NO, 180.0f * Time.deltaTime);    //引数：加算経験値量
    }

    //色を付ける
    private void ColorSpray(GM_MathFlowerParam _flowerParam)
    {
        GM_MathFlowerParam.EFlowerColor playerColor;

        playerColor = playerUnit.PLAYER_COLOR;
        //花に色を付ける
        _flowerParam.AddColor(playerUnit.PLAYER_NO, playerColor);
    }
}
