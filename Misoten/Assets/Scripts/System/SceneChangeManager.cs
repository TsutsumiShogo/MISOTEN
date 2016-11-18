using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneChangeManager : MonoBehaviour {


    //===========定数定義===========
    //シーン番号
    public enum ESceneNo
    {
        SCENE_TITLE,
        SCENE_STORY,
        SCENE_GAME,
        SCENE_NUM_MAX,
    };

    //===========変数定義===========

    //プログラム開始時のシーン番号を指定する。
    [SerializeField]
    private ESceneNo START_SCENE_NO;            //Unity上でセットする

    //現在のシーン番号
    private ESceneNo nowSceneNo;

    //各シーンオブジェクトの一番親のオブジェクト
    [SerializeField]
    private List<GameObject> SceneTopObjects;

    //各シーンキャンバスの一番親のオブジェクト
    [SerializeField]
    private List<GameObject> SceneCanvasTopObjects;

	// Use this for initialization
	void Start () {
        int SCENE_NUM = (int)ESceneNo.SCENE_NUM_MAX;

        //最初に全てのオブジェクトを無効化する
        for (int i = 0; i < SCENE_NUM; ++i)
        {
            SceneObjectSwitch((ESceneNo)i, false);
        }

        //仮の値をセット
        nowSceneNo = ESceneNo.SCENE_TITLE;

        //START_SCENE_NOが指すシーンを有効化
        SceneChange(START_SCENE_NO);

	}
	
    //シーンの切り替え
    public bool SceneChange(ESceneNo changeSceneNo)
    {
        //範囲チェック
        if (changeSceneNo < 0 || changeSceneNo >= ESceneNo.SCENE_NUM_MAX)
        {
            return false;
        }

        //オブジェクトを切り替える
        SceneObjectSwitch(nowSceneNo, false);
        SceneObjectSwitch(changeSceneNo, true);

        //シーン番号切り替え
        nowSceneNo = changeSceneNo;

        //シーン開始処理
        SceneStartProcess(changeSceneNo);

        return true;
    }

    //=====================非公開関数===========================

    //シーンのアクティブを切り替える
    private void SceneObjectSwitch(ESceneNo sceneNo, bool activeFlg)
    {
        SceneTopObjects[(int)sceneNo].SetActive(activeFlg);
        SceneCanvasTopObjects[(int)sceneNo].SetActive(activeFlg);
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
            case ESceneNo.SCENE_STORY:
                break;
            case ESceneNo.SCENE_GAME:
                //ゲームメインシーンマネージャーで初期化を伝達
                GM_SceneManager gameManager;
                gameManager = SceneTopObjects[(int)ESceneNo.SCENE_GAME].GetComponent<GM_SceneManager>();
                gameManager.Init();

                break;
        }//EndSwitch

    }//EndFunc
}
