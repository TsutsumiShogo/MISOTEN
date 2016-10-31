using UnityEngine;
using System.Collections;

public class GM_SceneManager : MonoBehaviour {

    //==========定数定義==========
    public float GAME_TIME = 300.0f;

    //==========変数定義==========
    public float gameTime;     //今のゲームの残り時間

    //マス管理マネージャー
    [SerializeField]
    private GM_MathManager mathManager;     //Unity上でセットする
    private bool[] stageFlg = new bool[2];  //STAGE2,3のフラグ

    void Awake()
    {
        Init();
        stageFlg[0] = stageFlg[1] = false;
    }

	public void Init() {
        mathManager = gameObject.GetComponentInChildren<GM_MathManager>();
        gameTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        gameTime += Time.deltaTime;

        //ステージフラグが立ってないのに時間が過ぎていたら
        if (stageFlg[0] == false && gameTime > 15.0f)
        {
            stageFlg[0] = true;
            mathManager.StartStage(GM_MathManager.EMathStageNo.STAGE2);
        }
        if (stageFlg[1] == false && gameTime > 30.0f)
        {
            stageFlg[1] = true;
            mathManager.StartStage(GM_MathManager.EMathStageNo.STAGE3);
        }
	}
}
