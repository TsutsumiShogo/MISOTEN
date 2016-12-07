using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM_SceneManager : MonoBehaviour {

    //==========定数定義==========
    public float GAME_TIME = 300.0f;

    //==========変数定義==========
    public float gameTime;     //今のゲームの残り時間

    //シーンチェンジマネージャー
    private SceneChangeManager sceneChangeManager;

    //マス管理マネージャー
    private GM_MathManager mathManager;
    private bool[] stageFlg = new bool[2];  //STAGE2,3のフラグ

    //プレイヤーマネージャー
    private PlayerManager playerManager;

    //ミニマップマネージャ
    private GM_MiniMapManager minimapManager;


    //タイムアップ演出オブジェクト
    [SerializeField]
    private Text timeUpObj;    //Unity上でセット

    //ステージ外周あたり判定オブジェクト
    [SerializeField]
    private GameObject[] stageOutColObj = new GameObject[2];    //Unity上でセット

    //フェードユニット(リザルトへの切り替え部分)
    [SerializeField]
    private GM_UIFadeUnit fadeUnit; //Unity上でセット

    
    //Init関数をシーンチェンジマネージャに呼ばせるようになったらAwakeへ変更すること。
    void Awake()
    {
        //シーンチェンジマネージャーを取得
        sceneChangeManager = transform.parent.GetComponent<SceneChangeManager>();
        //マスマネージャー保存
        mathManager = gameObject.GetComponentInChildren<GM_MathManager>();
        //プレイヤーマネージャー保存
        playerManager = gameObject.GetComponentInChildren<PlayerManager>();
        //ミニマップマネージャ保存
        minimapManager = gameObject.GetComponentInChildren<GM_MiniMapManager>();
    }

    //ゲーム開始時にこの初期化関数が呼ばれる。
	public void Init() {
        
        gameTime = 0;
        stageFlg[0] = stageFlg[1] = false;

        //マスマネージャ初期化
        mathManager.Init();

        //プレイヤーマネージャー初期化
        playerManager.Init();
        playerManager.StartPlayers();
        //ミニマップマネージャ初期化
        minimapManager.Init();

        //タイムアップ演出をオフに
        timeUpObj.gameObject.SetActive(false);

        //ステージ外周オブジェクトを有効化
        stageOutColObj[0].SetActive(true);
        stageOutColObj[1].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        //ゲームタイムを進める
        gameTime += Time.deltaTime;

        //ステージ解放処理
        if (stageFlg[0] == false && mathManager.totalFlowerLevel > 500)
        {
            stageFlg[0] = true;
            mathManager.StartStage(GM_MathManager.EMathStageNo.STAGE2);
            minimapManager.StartStage(GM_MathManager.EMathStageNo.STAGE2);

            //ステージ外周オブジェクトを消す
            stageOutColObj[0].SetActive(false);
        }
        if (stageFlg[1] == false && mathManager.totalFlowerLevel > 1500)
        {
            stageFlg[1] = true;
            mathManager.StartStage(GM_MathManager.EMathStageNo.STAGE3);
            minimapManager.StartStage(GM_MathManager.EMathStageNo.STAGE3);

            //ステージ外周オブジェクトを消す
            stageOutColObj[1].SetActive(false);
        }
        
        //ゲーム終了の時間になったらタイムアップ演出をする
        if (gameTime > GAME_TIME)
        {
            timeUpObj.gameObject.SetActive(true);
            float _percent = (gameTime - GAME_TIME) / 3.0f;
            if (_percent > 1.0f)
            {
                _percent = 1.0f;
            }

            //α増加
            Color color = timeUpObj.color;
            color.a = 0.5f + _percent * 0.5f;

            //タイムアップ演出終了
            if (gameTime > GAME_TIME + 5.0f)
            {
                //リザルト画面へフェードインフェードアウト(自身で制御すること)
                fadeUnit.SceneChangeResult();
            }



        }

	}
}
