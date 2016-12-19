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

    private GM_SceneManager sceneManager;   //シーンマネージャー(試合の経過時間が欲しい)
    private GM_MathManager mathManager;     //マスマネージャ(セルの位置欲しい)

    //変数宣言
    private bool canMissionCreateFlg;         //ミッション作成許可
    private float nonMissionTime;       //ミッションが無い時間を計測

    void Awake()
    {
        sceneManager = GameObject.Find("SceneChangeManager").transform.Find("GameMainObjects").GetComponent<GM_SceneManager>();
        mathManager = GameObject.Find("SceneChangeManager").transform.Find("GameMainObjects/Stage/MathManager").GetComponent<GM_MathManager>();
        announce = GameObject.Find("Canvas").transform.Find("GameMainUI/Game/MissionAnnounce").GetComponent<GM_UIMissionAnnounce>();
    }

	public void Init () {
        //ミッションがあれば削除
        if (nowMission)
        {
            Destroy(nowMission);
        }
        canMissionCreateFlg = false;    //ミッション作成不許可
        nowMission = null;
        nonMissionTime = 0.0f;
	}
    //ミッション作成許可フラグの操作
    public void MissionCreateFlgChange(bool _flg)
    {
        canMissionCreateFlg = _flg;
    }
	
	// Update is called once per frame
	void Update () {
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

                for(int i=0; i<mathManager.cells.Count; ++i){
                    _index = Random.Range(0,mathManager.cells.Count);
                    if (mathManager.cells[_index].stageNo == GM_MathManager.EMathStageNo.STAGE1)
                    {
                        _createPos = mathManager.cells[_index].transform.position;
                    }
                }

                //ミッションオブジェクトの生成
                GM_MathFlowerParam.EFlowerColor _missionClearColor;
                _missionClearColor = (GM_MathFlowerParam.EFlowerColor)Random.Range(0, (int)(GM_MathFlowerParam.EFlowerColor.WHITE) + 1);
                if (sceneManager.gameTime < 60)
                {
                    CreateMission(GM_Mission.EMissionType.FLOWER_GROWTH_MISSION, _missionClearColor, 999.0f, _createPos);
                }
                else
                {
                    CreateMission(GM_Mission.EMissionType.FLOWER_COLOR_MISSTION, _missionClearColor, 999.0f, _createPos);
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
}
