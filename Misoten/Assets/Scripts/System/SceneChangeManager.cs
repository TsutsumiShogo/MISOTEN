using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

    //次のシーン番号
    private ESceneNo nextSceneNo;

    //現在のシーン番号
    private ESceneNo nowSceneNo;

    //切り替え完了フラグ
    private bool changeEndFlg;

    //各シーンオブジェクトの一番親のオブジェクト
    [SerializeField]
    private List<GameObject> SceneTopObjects;

    //各シーンキャンバスの一番親のオブジェクト
    [SerializeField]
    private List<GameObject> SceneCanvasTopObjects;

    //フェードユニット
    private SY_Fade sy_Fade;

    void Awake()
    {
        sy_Fade = GameObject.Find("SY_Fade").GetComponent<SY_Fade>();
    }
	void Start () {
        int SCENE_NUM = (int)ESceneNo.SCENE_NUM_MAX;

        //最初に全てのオブジェクトを無効化する
        for (int i = 0; i < SCENE_NUM; ++i)
        {
            SceneObjectSwitch((ESceneNo)i, false);
        }

        //仮の値をセット
        nowSceneNo = ESceneNo.SCENE_TITLE;
        nextSceneNo = nowSceneNo;
        changeEndFlg = true;

        //START_SCENE_NOが指すシーンを有効化
        SceneChange(START_SCENE_NO);

	}

    void Update()
    {
        //切り替え完了済みなら何もしない
        if (changeEndFlg == true)
        {
            return;
        }

        //フェードアウトの完了を待つ
        if (sy_Fade.CheckEndFadeOut() == false)
        {
            return;
        }

        //オブジェクト切り替え
        //オブジェクトを切り替える
        SceneObjectSwitch(nowSceneNo, false);
        SceneObjectSwitch(nextSceneNo, true);
        //シーン番号切り替え
        nowSceneNo = nextSceneNo;
        //シーン開始処理
        SceneStartProcess(nextSceneNo);

        //フェードイン信号発信
        sy_Fade.FadeInStart();

        //切り替え完了
        changeEndFlg = true;
    }

	
    //シーンの切り替え
    public bool SceneChange(ESceneNo _changeSceneNo)
    {
        //範囲チェック
        if (_changeSceneNo < 0 || _changeSceneNo >= ESceneNo.SCENE_NUM_MAX)
        {
            return false;
        }

        //次のシーン番号を保存
        nextSceneNo = _changeSceneNo;

        //フェードアウト信号発信
        sy_Fade.FadeOutStart();

        //切り替え未完了
        changeEndFlg = false;

        return true;
    }

    //=====================非公開関数===========================

    //シーンのアクティブを切り替える
    private void SceneObjectSwitch(ESceneNo sceneNo, bool activeFlg)
    {
        SceneTopObjects[(int)sceneNo].SetActive(activeFlg);
        SceneCanvasTopObjects[(int)sceneNo].SetActive(activeFlg);
    }

    //シーン開始時の初期化処理
    private void SceneStartProcess(ESceneNo startSceneNo)
    {
        //シーン切り替え時に何か共通でしたい処理があればここに関数を追加


        //各自シーン開始時に何かしたい処理があればここに関数を追加
        switch (startSceneNo)
        {
            case ESceneNo.SCENE_TITLE:
                GameObject.Find("T_SceneManager").GetComponent<T_SceneManager>().Init();
                ObjectManager.Clean();              // オブジェクト初期化
                break;

            case ESceneNo.SCENE_STORY:
                //チュートリアルシーンマネージャーで初期化を伝達
                TS_SceneManager tutorialManager;
                tutorialManager = SceneTopObjects[(int)ESceneNo.SCENE_STORY].GetComponent<TS_SceneManager>();
                tutorialManager.Init();
                break;

            case ESceneNo.SCENE_GAME:
                //ゲームメインシーンマネージャーで初期化を伝達
                GM_SceneManager gameManager;
                ObjectManager.Clean();              // オブジェクト初期化
                gameManager = SceneTopObjects[(int)ESceneNo.SCENE_GAME].GetComponent<GM_SceneManager>();
                gameManager.Init();
                SoundManager.PlayBgm2("game_bgm");  // BGM再生
                GameObject.Find("MobsManager").GetComponent<MobsManager>().Init();  // モブ初期化
                Debug.Log("初期化処理");

                //ゲームメインUIマネージャーで初期化を伝達
                GM_UIManager gameUiManager;
                gameUiManager = SceneCanvasTopObjects[(int)ESceneNo.SCENE_GAME].GetComponent<GM_UIManager>();
                gameUiManager.Init();

                break;
            default:
                break;
        }//EndSwitch

    }//EndFunc
}
