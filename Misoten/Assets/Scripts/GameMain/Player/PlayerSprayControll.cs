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

    [SerializeField]
    private GameObject[] sprayEffect = new GameObject[3];//Unity上でセット
    [SerializeField]
    private ParticleSystem areaEffect;//Unity上でセット

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

        //エフェクトオブジェクトを全て無効にする
        for (int i = 0; i < 3; ++i)
        {
            sprayEffect[i].SetActive(false);
        }
        areaEffect.Stop(true);
	}
	
	// Update is called once per frame
	void Update () {
        if (sprayActiveTime >= 0.00f)
        {
            sprayActiveTime -= Time.deltaTime;
        }
        else
        {
            //エフェクトオブジェクトを全て無効にする
            for (int i = 0; i < 3; ++i)
            {
                sprayEffect[i].SetActive(false);
            }
            areaEffect.Stop(true);
        }
	}

    //スプレーを起動させる
    public void Spray(EPlayerSprayMode _mode, float _activeTime)
    {
        sprayMode = _mode;
        sprayActiveTime = _activeTime;
        sprayEffect[(int)_mode].SetActive(true);
        if (areaEffect.isPlaying == false)
        {
            areaEffect.Play(true);
        }
    }
    //スプレーの大きさ変更
    public void ChangeScale(float _scale)
    {
        Vector3 scale = Vector3.one;
        Vector3 areaEffectScale = Vector3.one;  //エリアエフェクトはRotationかかってて分けないと使えない…
        scale.y = 0.01f;
        scale.x = scale.z = _scale * 2.0f;
        areaEffectScale.z = 1.0f;
        areaEffectScale.x = areaEffectScale.y = _scale * 2.0f;

        //エリアエフェクト用
        areaEffect.transform.localScale = areaEffectScale;

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
