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

    //タイムアップ演出オブジェクト
    [SerializeField]
    private Text timeUpObj;    //Unity上でセット
    
    //Init関数をシーンチェンジマネージャに呼ばせるようになったらAwakeへ変更すること。
    void Awake()
    {
        //シーンチェンジマネージャーを取得
        sceneChangeManager = transform.parent.GetComponent<SceneChangeManager>();
        //マスマネージャー保存
        mathManager = gameObject.GetComponentInChildren<GM_MathManager>();
        //プレイヤーマネージャー保存
        playerManager = gameObject.GetComponentInChildren<PlayerManager>();

        //初期化
        Init();

    }

    //ゲーム開始時にこの初期化関数が呼ばれる。
	public void Init() {
        
        gameTime = 0;
        stageFlg[0] = stageFlg[1] = false;

        //マスマネージャ初期化
        mathManager.Init();

        //プレイヤーマネージャー初期化
        playerManager.Init();

        //タイムアップ演出をオフに
        timeUpObj.gameObject.SetActive(false);
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
        }
        if (stageFlg[1] == false && mathManager.totalFlowerLevel > 1500)
        {
            stageFlg[1] = true;
            mathManager.StartStage(GM_MathManager.EMathStageNo.STAGE3);
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
                //リザルト画面へ
                sceneChangeManager.SceneChange(SceneChangeManager.ESceneNo.SCENE_TITLE);
            }


        }

	}
}
