using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM_SceneManager : MonoBehaviour {

    //==========定数定義==========
    public float GAME_TIME = 300.0f;

    //==========変数定義==========
    public float gameTime;     //今のゲームの残り時間
    private float startTime;    //ゲーム開始演出の時間-2~1
    private bool playerStartFlg;    //プレイヤー開始してるかどうか

    //シーンチェンジマネージャー
    private SceneChangeManager sceneChangeManager;

    //マス管理マネージャー
    private GM_MathManager mathManager;
    private bool[] stageFlg = new bool[2];  //STAGE2,3のフラグ

    //プレイヤーマネージャー
    private PlayerManager playerManager;

    //ミニマップマネージャ
    private GM_MiniMapManager minimapManager;

    //ミッションマネージャー
    private GM_MissionManager missionManager;

    //ゲーム開始演出オブジェクト
    private Text startObj;
    //タイムアップ演出オブジェクト
    private Text timeUpObj;

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
        //ミッションマネージャ保存
        missionManager = transform.Find("GameObjects/MissionManager").GetComponent<GM_MissionManager>();
        //ゲーム開始演出テキスト保存
        startObj = GameObject.Find("Canvas").transform.Find("GameMainUI/Game/Start").GetComponent<Text>();
        //タイムアップ演出テキスト保存
        timeUpObj = GameObject.Find("Canvas").transform.Find("GameMainUI/Game/TimeUp").GetComponent<Text>();
    }

    //ゲーム開始時にこの初期化関数が呼ばれる。
	public void Init() {
        
        gameTime = 0;
        stageFlg[0] = stageFlg[1] = false;

        //スコア初期化
        for (int i = 0; i < 4; ++i)
        {
            GM_ScoreCtrl.SetPlayerScore(0.0f, i);
        }

        //マスマネージャ初期化
        mathManager.Init();

        //プレイヤーマネージャー初期化
        playerManager.Init();
        playerStartFlg = false;
        
        //ミニマップマネージャ初期化
        minimapManager.Init();

        //ミッションマネージャ初期化
        missionManager.Init();

        //ゲーム開始演出のテキスト初期化
        Color _col = startObj.color;
        startObj.text = "よ～い";
        _col.a = 1.0f;
        startObj.color = _col;
        startTime = -5.0f;                     //プレイヤーが動き出すまでの時間(-2.0f以下でお願い)
        startObj.gameObject.SetActive(false);

        //タイムアップ演出をオフに
        timeUpObj.gameObject.SetActive(false);

        //ステージ外周オブジェクトを有効化
        stageOutColObj[0].SetActive(true);
        stageOutColObj[1].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        //スタート演出中
        if (UpdateStartObj() == false)
        {
            return;
        }
        //プレイヤースタートしてなければスタート処理
        if (playerStartFlg == false)
        {
            //プレイヤー行動開始
            playerManager.StartPlayers();
            //ミッションの作成を許可
            missionManager.MissionCreateFlgChange(true);

            playerStartFlg = true;
        }

        //ゲームタイムを進める
        gameTime += Time.deltaTime;

        //ステージ解放処理
        if (stageFlg[0] == false && mathManager.totalFlowerLevel > 500)
        {
            stageFlg[0] = true;
            mathManager.StartStage(GM_MathManager.EMathStageNo.STAGE2);
            minimapManager.StartStage(GM_MathManager.EMathStageNo.STAGE2);
            stageOutColObj[0].SetActive(false);
            GameObject.Find("MobsManager").GetComponent<MobsManager>().move();      // モブ追加
        }
        if (stageFlg[1] == false && mathManager.totalFlowerLevel > 1500)
        {
            stageFlg[1] = true;
            mathManager.StartStage(GM_MathManager.EMathStageNo.STAGE3);
            minimapManager.StartStage(GM_MathManager.EMathStageNo.STAGE3);
            GameObject.Find("MobsManager").GetComponent<MobsManager>().move();      // モブ追加
            //ステージ外周オブジェクトを消す
            stageOutColObj[1].SetActive(false);
        }
        
        //ゲーム終了の時間になったらタイムアップ演出をする
        if (gameTime > GAME_TIME)
        {
            //ミッションの作成を停止
            missionManager.MissionCreateFlgChange(false);

            //タイムアップオブジェクト有効化
            timeUpObj.gameObject.SetActive(true);
            float _percent = (gameTime - GAME_TIME) / 3.0f;
            if (_percent > 1.0f)
            {
                _percent = 1.0f;
            }

            //α増加
            Color color = timeUpObj.color;
            color.a = 0.5f + _percent * 0.5f;

            //プレイヤー停止信号を送信
            playerManager.StopPlayers();

            //タイムアップ演出終了
            if (gameTime > GAME_TIME + 5.0f)
            {
                //リザルト画面へフェードインフェードアウト(自身で制御すること)
                fadeUnit.SceneChangeResult();

                //プレイヤーオブジェクトの削除
                playerManager.Delete();
            }

        }//Endif ゲーム終了

	}//EndFunc()

    //開始後はtrueを返す
    private bool UpdateStartObj()
    {
        //演出終了
        if (startTime >= 1.0f)
        {
            return true;
        }

        //時間を進める
        startTime += Time.deltaTime;
        if (startTime > 1.0f)
        {
            startTime = 1.0f;
        }

        //よーいの出現タイミングをはかる
        if (startTime > -2.0f)
        {
            if (startObj.gameObject.active == false)
            {
                startObj.gameObject.SetActive(true);
            }
        }

        //ゲーム開始までまだ時間がある
        if (startTime < 0.0f)
        {
            return false;
        }

        //ゲーム開始
        startObj.text = "スタート！";

        //αを徐々に0へ
        float _alpha = 1.0f - startTime;
        Color _setColor = startObj.color;

        _setColor.a = _alpha;
        startObj.color = _setColor;

        return true;
    }//EndFunc()

    //ステージの開放状況を外部へ渡す0→1→2
    public int StageOpenSituation()
    {
        int _openNum = 0;

        if (stageFlg[0] == true)
        {
            _openNum = 1;
        }
        if (stageFlg[1] == true)
        {
            _openNum = 2;
        }

        return _openNum;
    }

}