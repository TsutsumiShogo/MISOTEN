using UnityEngine;
using System.Collections;

public class GM_MissionManager : MonoBehaviour {

    [SerializeField]
    private GameObject MISSION_PREFAB;

    public GM_Mission nowMission;

	public void Init () {
        //ミッションがあれば削除
        if (nowMission)
        {
            Destroy(nowMission);
        }
        nowMission = null;
	}
	
	// Update is called once per frame
	void Update () {
	    
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

        //ミッション初期化
        nowMission.Init(_time, _type, _clearColor);
        nowMission.transform.position = _pos;
    }

    //ミッションオブジェクトから失敗のシグナルが来た
    public void FailedSignal()
    {
        Destroy(nowMission);
    }
    //ミッションオブジェクトから成功のシグナルが来た
    public void SuccessSignal()
    {
        Destroy(nowMission);

        //スコア加算

    }
}
