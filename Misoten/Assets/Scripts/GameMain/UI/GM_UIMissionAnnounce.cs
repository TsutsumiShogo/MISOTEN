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

    //定数定義
    [SerializeField]
    private float BACKGROUND_ANIM_TIME = 0.5f;
    [SerializeField]
    private float MESSAGE_MOVE_TIME = 10.0f;

    //変数定義
    private float nowTime;

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

            return;
        }

        //================以下はミッションアナウンス動作中=====================

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
        switch (_missionType)
        {
            case GM_Mission.EMissionType.FLOWER_GROWTH_MISSION:
                missionMessageObj.text = "指定のエリアで花を咲かせてみよう！";
                break;
            case GM_Mission.EMissionType.FLOWER_COLOR_MISSTION:
                missionMessageObj.text = "指定のエリアを";
                switch (_missionColor)
                {
                    case GM_MathFlowerParam.EFlowerColor.RED:
                        missionMessageObj.text = missionMessageObj.text + "<color=red>赤</color>";
                        break;
                    case GM_MathFlowerParam.EFlowerColor.GREEN:
                        missionMessageObj.text = missionMessageObj.text + "<color=green>緑</color>";
                        break;
                    case GM_MathFlowerParam.EFlowerColor.BLUE:
                        missionMessageObj.text = missionMessageObj.text + "<color=blue>青</color>";
                        break;
                    case GM_MathFlowerParam.EFlowerColor.CYAN:
                        missionMessageObj.text = missionMessageObj.text + "<color=cyan>シアン</color>";
                        break;
                    case GM_MathFlowerParam.EFlowerColor.MAGENTA:
                        missionMessageObj.text = missionMessageObj.text + "<color=magenta>マゼンタ</color>";
                        break;
                    case GM_MathFlowerParam.EFlowerColor.YELLOW:
                        missionMessageObj.text = missionMessageObj.text + "<color=yellow>イエロー</color>";
                        break;
                    case GM_MathFlowerParam.EFlowerColor.WHITE:
                        missionMessageObj.text = missionMessageObj.text + "<color=white>力を合わせて白</color>";
                        break;
                }
                missionMessageObj.text = missionMessageObj.text + "の花で埋め尽くそう！";
                break;
            case GM_Mission.EMissionType.BILL_GROWTH_MISSION:
                missionMessageObj.text = "指定のエリアのビルを最後まで成長させよう！";
                break;
            case GM_Mission.EMissionType.BIGBILL_GROWTH_MISSION:
                missionMessageObj.text = "指定のエリアの大きなビルを最後まで成長させよう！";
                break;
        }

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
}
