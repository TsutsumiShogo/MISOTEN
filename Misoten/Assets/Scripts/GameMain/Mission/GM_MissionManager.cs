using UnityEngine;
using System.Collections;

public class GM_MissionManager : MonoBehaviour {

    //定数定義
    [SerializeField]
    private float CLEAR_SCORE_POINT = 50000;

    //オブジェクト
    [SerializeField]
    private GameObject MISSION_PREFAB;  //生成するミッションオブジェクト
    public GM_Mission nowMission;       //生成したミッションオブジェクト
    private GM_UIMissionAnnounce announce;  //アナウンスオブジェクト


    //変数宣言
    private float nonMissionTime;       //ミッションが無い時間を計測

    void Awake()
    {
        announce = GameObject.Find("Canvas").transform.Find("GameMainUI/Game/MissionAnnounce").GetComponent<GM_UIMissionAnnounce>();
    }

	public void Init () {
        //ミッションがあれば削除
        if (nowMission)
        {
            Destroy(nowMission);
        }
        nowMission = null;
        nonMissionTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        //ミッションが無ければ生成
        if (nowMission == null)
        {
            nonMissionTime += Time.deltaTime;

            //一定の秒数経過後に生成する
            if (nonMissionTime > 10.0f)
            {
                //ミッションオブジェクトの生成
                CreateMission(GM_Mission.EMissionType.FLOWER_COLOR_MISSTION, GM_MathFlowerParam.EFlowerColor.RED, 999.0f, transform.position);
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
    }
}
