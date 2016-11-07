using UnityEngine;
using System.Collections;

public class GM_SceneManager : MonoBehaviour {

    //==========定数定義==========
    public float GAME_TIME = 300.0f;

    //==========変数定義==========
    public float gameTime;     //今のゲームの残り時間

    //マス管理マネージャー
    private GM_MathManager mathManager;     //Unity上でセットする
    private bool[] stageFlg = new bool[2];  //STAGE2,3のフラグ

    //プレイヤーマネージャー
    private PlayerManager playerManager;    //Unity上でセットする

    //Init関数をシーンチェンジマネージャに呼ばせるようになったらAwakeへ変更すること。
    void Awake()
    {
        //マスマネージャー保存
        mathManager = gameObject.GetComponentInChildren<GM_MathManager>();
        //プレイヤーマネージャー保存
        playerManager = gameObject.GetComponentInChildren<PlayerManager>();

    }

    //ゲーム開始時にこの初期化関数が呼ばれる。
	public void Init() {
        
        gameTime = 0;
        stageFlg[0] = stageFlg[1] = false;

        //マスマネージャ初期化
        mathManager.Init();

        //プレイヤーマネージャー初期化
        playerManager.Init();
	}
	
	// Update is called once per frame
	void Update () {
        gameTime += Time.deltaTime;

        //ステージフラグが立ってないのに時間が過ぎていたら
        if (stageFlg[0] == false && gameTime > 5.0f)
        {
            stageFlg[0] = true;
            mathManager.StartStage(GM_MathManager.EMathStageNo.STAGE2);
        }
        if (stageFlg[1] == false && gameTime > 10.0f)
        {
            stageFlg[1] = true;
            mathManager.StartStage(GM_MathManager.EMathStageNo.STAGE3);
        }
	}
}
