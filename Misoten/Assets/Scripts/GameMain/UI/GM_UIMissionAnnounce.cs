using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM_UIMissionAnnounce : MonoBehaviour {

    //オブジェクト(全てUnity上でセット)
    [SerializeField]
    private GameObject backGroundObj;
    [SerializeField]
    private Text missionMessageObj;
    [SerializeField]
    private GameObject startPointObj;
    [SerializeField]
    private GameObject endPointObj;
    [SerializeField]
    private Text missionTimeTextObj;

    //定数定義
    [SerializeField]
    private float BACKGROUND_ANIM_TIME = 0.5f;
    [SerializeField]
    private float MESSAGE_MOVE_TIME = 10.0f;

    //変数定義
    private float nowTime;

    public bool announceFlg;        //アナウンス動作中ならtrue

    //初期化
	public void Init () {
        //時間を終了時間まで持っていく
        nowTime = BACKGROUND_ANIM_TIME * 2 + MESSAGE_MOVE_TIME + 0.1f;

        //背景yスケールを0へ
        Vector3 _scale = Vector3.one;
        _scale.y = 0.0f;
        backGroundObj.transform.localScale = _scale;

        //メッセージをスタートポイントの位置へ
        missionMessageObj.transform.position = startPointObj.transform.position;

        announceFlg = false;
	}
	
	// Update is called once per frame
	void Update () {

        //計算しやすいように
        float _phaseEndTime = 0.0f; //そのフェーズに必要な時間
        float _phaseNowTime = 0.0f; //現在のフェーズの経過時間
        float _percent = 0.0f;      //現在のフェーズの進行度合い
        Vector3 _vec3 = Vector3.zero;   //スケール値もしくは位置情報。

        //カウントが終了時間を示しているのでアナウンスは終了状態を維持
        if (nowTime > BACKGROUND_ANIM_TIME * 2 + MESSAGE_MOVE_TIME)
        {
            //背景オブジェクトのyスケールは0
            backGroundObj.transform.localScale = _vec3;
            
            //テキストメッセージをスタートポイントへ固定
            missionMessageObj.transform.position = startPointObj.transform.position;

            //動作フラグは下す
            announceFlg = false;
            return;
        }

        //================以下はミッションアナウンス動作中=====================
        //動作フラグを立てる
        announceFlg = true;

        //段階の時間を進める
        nowTime += Time.deltaTime;

        //初期段階。背景オブジェクト出現
        if (nowTime < BACKGROUND_ANIM_TIME)
        {
            _phaseEndTime = BACKGROUND_ANIM_TIME;
            _phaseNowTime = nowTime;
            _percent = _phaseNowTime / _phaseEndTime;
            
            //背景オブジェクトのyスケールを大きくしていく
            _vec3.y = _percent;
            _vec3.x = _vec3.z = 1.0f;
            backGroundObj.transform.localScale = _vec3;

            //テキストメッセージをスタートポイントへ固定
            missionMessageObj.transform.position = startPointObj.transform.position;

            return;
        }

        //中間の段階。テキストメッセージをスクロール
        if (nowTime < BACKGROUND_ANIM_TIME + MESSAGE_MOVE_TIME)
        {
            _phaseEndTime = MESSAGE_MOVE_TIME;
            _phaseNowTime = nowTime - BACKGROUND_ANIM_TIME;
            _percent = _phaseNowTime / _phaseEndTime;

            //背景オブジェクトのyスケールを最大で固定
            _vec3.x = _vec3.y = _vec3.z = 1.0f;
            backGroundObj.transform.localScale = _vec3;

            //テキストメッセージの移動(startPoint→endPointへスクロール処理)
            _vec3 = startPointObj.transform.position + (endPointObj.transform.position - startPointObj.transform.position) * _percent;
            missionMessageObj.transform.position = _vec3;

            return;
        }

        //最終段階。背景オブジェクトを閉じる
        if (nowTime < BACKGROUND_ANIM_TIME + MESSAGE_MOVE_TIME + BACKGROUND_ANIM_TIME)
        {
            _phaseEndTime = BACKGROUND_ANIM_TIME;
            _phaseNowTime = nowTime - BACKGROUND_ANIM_TIME - MESSAGE_MOVE_TIME;
            _percent = _phaseNowTime / _phaseEndTime;

            //背景オブジェクトのyスケールを小さくしていく
            _vec3.y = 1.0f - _percent;
            _vec3.x = _vec3.z = 1.0f;
            backGroundObj.transform.localScale = _vec3;

            //テキストメッセージをスタートポイントへ固定
            missionMessageObj.transform.position = startPointObj.transform.position;

            return;
        }

	}

    //ミッションの種類別でアナウンスメッセージを流す
    public void AnnounceMessage(GM_Mission.EMissionType _missionType, GM_MathFlowerParam.EFlowerColor _missionColor)
    {
        //カウントを戻す
        nowTime = 0.0f;

        //ミッションの種類毎にテキストメッセージの内容を変更
        missionMessageObj.text = GetMissionAnnounceText(_missionType, _missionColor);

        //テキストメッセージをスタートポイントに移動させる
        missionMessageObj.transform.position = startPointObj.transform.position;
    }
    //ミッション成功のアナウンスを流す
    public void SuccessAnnounceMessage(int _score)
    {
        //カウントを戻す
        nowTime = 0.0f;

        missionMessageObj.text = "やったね！ミッション成功！ " + _score.ToString() + " ポイント入手！";

        //テキストメッセージをスタートポイントに移動させる
        missionMessageObj.transform.position = startPointObj.transform.position;
    }
    //ミッション失敗のアナウンスを流す
    public void FailedAnnounceMessage()
    {
        //カウントを戻す
        nowTime = 0.0f;

        missionMessageObj.text = "残念。ミッション失敗～";

        //テキストメッセージをスタートポイントに移動させる
        missionMessageObj.transform.position = startPointObj.transform.position;
    }

    //ミッションの内容テキストを取得する
    public string GetMissionAnnounceText(GM_Mission.EMissionType _missionType, GM_MathFlowerParam.EFlowerColor _missionColor)
    {
        string _missionText;
        _missionText = "";

        //ミッションの種類毎にテキストメッセージの内容を変更
        switch (_missionType)
        {
            case GM_Mission.EMissionType.FLOWER_GROWTH_MISSION:
                _missionText = "指定のエリアで花を咲かせてみよう！";
                break;
            case GM_Mission.EMissionType.FLOWER_COLOR_MISSTION:
                _missionText = "指定のエリアを";
                switch (_missionColor)
                {
                    case GM_MathFlowerParam.EFlowerColor.RED:
                        _missionText = _missionText + "<color=red>赤</color>";
                        break;
                    case GM_MathFlowerParam.EFlowerColor.GREEN:
                        _missionText = _missionText + "<color=green>緑</color>";
                        break;
                    case GM_MathFlowerParam.EFlowerColor.BLUE:
                        _missionText = _missionText + "<color=blue>青</color>";
                        break;
                    case GM_MathFlowerParam.EFlowerColor.CYAN:
                        _missionText = _missionText + "<color=green>緑</color> + <color=blue>青</color>";
                        break;
                    case GM_MathFlowerParam.EFlowerColor.MAGENTA:
                        _missionText = _missionText + "<color=red>赤</color> + <color=blue>青</color>";
                        break;
                    case GM_MathFlowerParam.EFlowerColor.YELLOW:
                        _missionText = _missionText + "<color=red>赤</color> + <color=green>緑</color>";
                        break;
                    case GM_MathFlowerParam.EFlowerColor.WHITE:
                        _missionText = _missionText + "<color=white>力を合わせて</color><color=red>赤</color> + <color=green>緑</color> + <color=blue>青</color>";
                        break;
                }
                _missionText = _missionText + "の花で埋め尽くそう！";
                break;
            case GM_Mission.EMissionType.BILL_GROWTH_MISSION:
                _missionText = "指定のエリアのビルを最後まで成長させよう！";
                break;
            case GM_Mission.EMissionType.BIGBILL_GROWTH_MISSION:
                _missionText = "指定のエリアの大きなビルを最後まで成長させよう！";
                break;
        }

        return _missionText;

    }//EndFunc

    //null送ってもらったらテキストを消すよ
    public void MissionTimeCount(GM_Mission _missionObj)
    {
        //ミッションが無いのでテキストを消去
        if (_missionObj == null)
        {
            missionTimeTextObj.text = "";
            return;
        }

        //ミッションがあるのでテキストを代入
        int _timeCount = (int)_missionObj.timeCountDown;
        missionTimeTextObj.text = GetMissionAnnounceText(_missionObj.missionType, _missionObj.clearColor);
        missionTimeTextObj.text = missionTimeTextObj.text + "　残り制限時間 " + _timeCount.ToString() + "秒";
    }
}
