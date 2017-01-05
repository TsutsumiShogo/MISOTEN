using UnityEngine;
using System.Collections;

public class GM_MissionManager : MonoBehaviour {

    //定数定義
    [SerializeField]
    private float CLEAR_SCORE_POINT = 300;

    //オブジェクト
    [SerializeField]
    private GameObject MISSION_PREFAB;  //生成するミッションオブジェクト
    public GM_Mission nowMission;       //生成したミッションオブジェクト
    private GM_UIMissionAnnounce announce;  //アナウンスオブジェクト
    private GM_PeopleMissionStarter peopleMission;  //住人ミッション

    private GM_SceneManager sceneManager;   //シーンマネージャー(試合の経過時間が欲しい)
    private GM_MathManager mathManager;     //マスマネージャ(セルの位置欲しい)
    private MobsManager mobManager;         //モブマネージャ

    //変数宣言
    private bool canMissionCreateFlg;       //ミッション作成許可
    private float nonMissionTime;       //ミッションが無い時間を計測

    //住人ミッション系追加
    private bool peopleMissionFlg;      //住人ミッション開始フラグ
    private int peopleMissionClearCount;            //ミッション達成数
    private int nowPeopleMissionClearCount;         //現在の達成数
    private GM_MathFlowerParam.EFlowerColor peopleMissionClearColor; //ミッションの色
    

    void Awake()
    {
        sceneManager = GameObject.Find("SceneChangeManager").transform.Find("GameMainObjects").GetComponent<GM_SceneManager>();
        mathManager = GameObject.Find("SceneChangeManager").transform.Find("GameMainObjects/Stage/MathManager").GetComponent<GM_MathManager>();
        announce = GameObject.Find("SceneCanvas").transform.Find("GameMainUI/Game/MissionAnnounce").GetComponent<GM_UIMissionAnnounce>();
        mobManager = GameObject.Find("SceneChangeManager").transform.Find("GameMainObjects/MobsManager").GetComponent<MobsManager>();
        peopleMission = transform.FindChild("PeopleMission").GetComponent<GM_PeopleMissionStarter>();
    }

	public void Init () {
        //ミッションがあれば削除
        if (nowMission)
        {
            Destroy(nowMission.gameObject);
            nowMission = null;
        }
        peopleMission.Init();
        canMissionCreateFlg = false;    //ミッション作成不許可
        nowMission = null;
        nonMissionTime = 0.0f;
        peopleMissionFlg = false;
        peopleMissionClearCount = 0;
        peopleMissionClearColor = GM_MathFlowerParam.EFlowerColor.WHITE;

	}
    //ミッション作成許可フラグの操作
    public void MissionCreateFlgChange(bool _flg)
    {
        canMissionCreateFlg = _flg;
    }
	
	// Update is called once per frame
	void Update () {
        //アナウンス部のタイム表記部分に情報を知らせる(nullが許可されてる)
        if (announce.announceFlg == false)
        {
            announce.MissionTimeCount(nowMission);
        }
        else
        {
            announce.MissionTimeCount(null);
        }

        //住人ミッションの更新
        CheckSuccessPeopleMission();

        //ミッション作成の許可が無ければ何もしない
        if (canMissionCreateFlg == false)
        {
            return;
        }

        //ミッションが無ければ生成
        if (nowMission == null)
        {
            nonMissionTime += Time.deltaTime;

            //一定の秒数経過後に生成する
            if (nonMissionTime > 15.0f)
            {
                //生成位置を決定する
                Vector3 _createPos = Vector3.zero;
                int _index;

                //一応複数回回るかも
                for(int i=0; i<mathManager.cells.Count; ++i){
                    _index = Random.Range(0,mathManager.cells.Count);

                    //花か家マスじゃないとクリアできないので
                    if (mathManager.cells[_index].cellType == GM_MathCell.ECellType.CELL_FLOWER ||
                        mathManager.cells[_index].cellType == GM_MathCell.ECellType.CELL_HOUSE)
                    {
                        //ステージの開放具合でミッション作成位置を変更する
                        if (sceneManager.StageOpenSituation() == 0)
                        {
                            if (mathManager.cells[_index].stageNo == GM_MathManager.EMathStageNo.STAGE1)
                            {
                                _createPos = mathManager.cells[_index].transform.position;
                                break;
                            }
                        }
                        else
                        {
                            if (mathManager.cells[_index].stageNo == GM_MathManager.EMathStageNo.STAGE1 ||
                                mathManager.cells[_index].stageNo == GM_MathManager.EMathStageNo.STAGE2)
                            {
                                _createPos = mathManager.cells[_index].transform.position;
                                break;
                            }
                        }
                    }
                    
                }//EndFor

                //ミッションオブジェクトの生成
                GM_MathFlowerParam.EFlowerColor _missionClearColor;
                _missionClearColor = (GM_MathFlowerParam.EFlowerColor)Random.Range(0, (int)(GM_MathFlowerParam.EFlowerColor.WHITE) + 1);
                if (sceneManager.gameTime < 60)
                {
                    CreateMission(GM_Mission.EMissionType.FLOWER_GROWTH_MISSION, _missionClearColor, 40.0f, _createPos);
                }
                else
                {
                    CreateMission(GM_Mission.EMissionType.FLOWER_COLOR_MISSTION, _missionClearColor, 40.0f, _createPos);
                }
                nonMissionTime = 0.0f;
            }
        }
        else//ミッションがあれば待機
        {
            nonMissionTime = 0.0f;
        }
	}

    //ミッションを作成する
    public void CreateMission(GM_Mission.EMissionType _type, GM_MathFlowerParam.EFlowerColor _clearColor, float _time, Vector3 _pos)
    {
        //ミッション作成の許可が無ければ何もしない
        if (canMissionCreateFlg == false)
        {
            return;
        }

        //ミッションがすでにあれば何もしない
        if (nowMission)
        {
            return;
        }

        //ミッションオブジェクト作成
        GameObject obj;
        obj = Instantiate(MISSION_PREFAB);
        nowMission = obj.GetComponent<GM_Mission>();
        nowMission.transform.parent = transform;

        //ミッション初期化
        nowMission.Init(_time, _type, _clearColor);
        nowMission.transform.position = _pos;

        //アナウンス再生
        announce.AnnounceMessage(_type, _clearColor);
    }

    //ミッションオブジェクトから失敗のシグナルが来た
    public void FailedSignal()
    {
        Destroy(nowMission.gameObject);

        //ミッション失敗アナウンスを流す
        announce.FailedAnnounceMessage();
    }
    //ミッションオブジェクトから成功のシグナルが来た
    public void SuccessSignal()
    {
        Destroy(nowMission.gameObject);

        //スコア加算
        for (int i = 0; i < 3; ++i)
        {
            GM_ScoreCtrl.AddPlayerScore(CLEAR_SCORE_POINT, i);
        }

        //ミッション成功アナウンスを流す
        announce.SuccessAnnounceMessage((int)CLEAR_SCORE_POINT*3);
    }
    //住人ミッションの作成(何回も来る恐れあり)
    public void StartPeopleMissionSignal()
    {
        //既に開始していたら何もしない
        if (peopleMissionFlg == true)
        {
            return;
        }

        //住人ミッションのクリアカラーを決定
        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0:
                peopleMissionClearColor = GM_MathFlowerParam.EFlowerColor.CYAN;
                break;
            case 1:
                peopleMissionClearColor = GM_MathFlowerParam.EFlowerColor.MAGENTA;
                break;
            case 2:
                peopleMissionClearColor = GM_MathFlowerParam.EFlowerColor.YELLOW;
                break;
            case 3:
                peopleMissionClearColor = GM_MathFlowerParam.EFlowerColor.WHITE;
                break;
        }

        //決定されたクリアカラーの現在の花の数を集計
        nowPeopleMissionClearCount = mathManager.GetFlowerColorNum(peopleMissionClearColor);

        //住人ミッションの達成数を決定
        peopleMissionClearCount = nowPeopleMissionClearCount + 30;

        //達成不可にならないようにチェック
        int _allFlowerNum = mathManager.GetFlowerColorNum(GM_MathFlowerParam.EFlowerColor.NONE);
        if (peopleMissionClearCount > _allFlowerNum)
        {
            peopleMissionClearCount = _allFlowerNum;
        }

        //ミッションテキスト代入
        peopleMission.SetText(GetPeopleMissionDrawText());

        //開始フラグを立てる
        peopleMissionFlg = true;

    }
    //住人ミッションの表示テキストを決定
    private string GetPeopleMissionDrawText()
    {
        string _drawText = "";

        switch (peopleMissionClearColor)
        {
            case GM_MathFlowerParam.EFlowerColor.CYAN:
                _drawText = "<color=green>緑</color> + <color=blue>青</color>";
                break;
            case GM_MathFlowerParam.EFlowerColor.MAGENTA:
                _drawText = "<color=red>赤</color> + <color=blue>青</color>";
                break;
            case GM_MathFlowerParam.EFlowerColor.YELLOW:
                _drawText = "<color=red>赤</color> + <color=green>緑</color>";
                break;
            case GM_MathFlowerParam.EFlowerColor.WHITE:
                _drawText = "<color=red>赤</color> + <color=green>緑</color> + <color=blue>青</color>";
                break;
            default:
                Debug.LogError("住人ミッションのクリアカラーの指定がおかしい");
                break;
        }

        _drawText = _drawText + " の花があと ";

        int _nokori = peopleMissionClearCount - nowPeopleMissionClearCount;
        if (_nokori < 0)
        {
            _nokori = 0;
        }

        _drawText = _drawText + _nokori.ToString() + "本欲しいなぁ";

        return _drawText;
    }

    //住人ミッション達成チェック
    private void CheckSuccessPeopleMission()
    {
        //ミッション開始フラグが立ってないなら何もしない
        if (peopleMissionFlg == false)
        {
            return;
        }

        //ミッション達成度合いをチェック
        nowPeopleMissionClearCount = mathManager.GetFlowerColorNum(peopleMissionClearColor);

        //達成なら成功処理を通す
        if (peopleMissionClearCount <= nowPeopleMissionClearCount)
        {
            SuccessPeopleMissionSignal();
        }
    }

    //住人ミッションの成功処理
    public void SuccessPeopleMissionSignal()
    {
        //スコア加算
        for (int i = 0; i < 3; ++i)
        {
            GM_ScoreCtrl.AddPlayerScore(CLEAR_SCORE_POINT*3.0f, i);
        }

        //ミッション成功アナウンスを流す
        announce.SuccessAnnounceMessage((int)CLEAR_SCORE_POINT * 3 * 3);

        //住人ミッションの位置を変更
        ChangeMob();

        //住人ミッション開始フラグは下しておく
        peopleMissionFlg = false;
    }

    //住人ミッションの位置を変更
    public void ChangeMob()
    {
        //住人ミッションの位置を変更する
        GameObject _mob;
        _mob = mobManager.GetRundomMob();
        if (_mob != null)
        {
            peopleMission.SetPeople(_mob);
        }
        else
        {
            Debug.LogError("MissionManager:住人ミッションの住人選択に失敗。");
        }
    }
}
