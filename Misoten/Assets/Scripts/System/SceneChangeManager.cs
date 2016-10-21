using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneChangeManager : MonoBehaviour {


    //===========定数定義===========
    //シーン番号
    public enum ESceneNo
    {
        SCENE_TITLE,
        SCENE_MENU,
        SCENE_CHARASELECT,
        SCENE_RANKING,
        SCENE_STORY,
        SCENE_GAME,
        SCENE_RESULT,
        SCENE_NUM_MAX,
    };

    //===========変数定義===========

    //現在のシーン番号
    private ESceneNo nowSceneNo;

    //各シーンオブジェクトの一番親のオブジェクト
    [SerializeField]
    private List<GameObject> SceneTopObjects;

    //各シーンキャンバスの一番親のオブジェクト
    [SerializeField]
    private List<GameObject> SceneCanvasTopObjects;


    public bool gameChangeFlg = false;

	// Use this for initialization
	void Awake () {
        nowSceneNo = ESceneNo.SCENE_TITLE;
	}

    void Update()
    {
        if (gameChangeFlg == true)
        {
            gameChangeFlg = false;
            SceneChange(ESceneNo.SCENE_GAME);
        }
    }
	
    //シーンの切り替え
    public bool SceneChange(ESceneNo changeSceneNo)
    {
        //範囲チェック
        if (changeSceneNo < 0 || changeSceneNo >= ESceneNo.SCENE_NUM_MAX)
        {
            return false;
        }

        //親オブジェクトを切り替える
        //OFF
        SceneTopObjects[(int)nowSceneNo].SetActive(false);
        SceneCanvasTopObjects[(int)nowSceneNo].SetActive(false);
        //ON
        SceneTopObjects[(int)changeSceneNo].SetActive(true);
        SceneCanvasTopObjects[(int)changeSceneNo].SetActive(true);

        //シーン番号切り替え
        nowSceneNo = changeSceneNo;

        //シーン開始処理
        SceneStartProcess(changeSceneNo);

        return true;
    }

    //必要ないかもしれんけど一応Start関数っぽいの用意。
    private void SceneStartProcess(ESceneNo startSceneNo)
    {
        //シーン切り替え時に何か共通でしたい処理があればここに関数を追加


        //各自シーン開始時に何かしたい処理があればここに関数を追加
        switch (startSceneNo)
        {
            case ESceneNo.SCENE_TITLE:
                break;
            case ESceneNo.SCENE_MENU:
                break;
            case ESceneNo.SCENE_CHARASELECT:
                break;
            case ESceneNo.SCENE_RANKING:
                break;
            case ESceneNo.SCENE_STORY:
                break;
            case ESceneNo.SCENE_GAME:
                //ゲームメインシーンマネージャーで初期化を伝達
                GameMainSceneManager gameManager;
                gameManager = SceneTopObjects[(int)ESceneNo.SCENE_GAME].GetComponent<GameMainSceneManager>();
                gameManager.Init();

                break;
            case ESceneNo.SCENE_RESULT:
                break;
        }//EndSwitch

    }//EndFunc
}
