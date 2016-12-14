using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM_UIGreenPercent : MonoBehaviour {

    //自身のテキスト
    private Text greenScoreName;    //"緑化スコア"
    private Text greenScorePoint;   //10000
    private Text greenScorePointName;   //"点"

    //マスマネージャー
    [SerializeField]
    private string mathManagerObjName;
    private GM_MathManager mathManager;

    //強調効果クラス
    private GM_UIGreenPercentEffect pointEffectObj = new GM_UIGreenPercentEffect();

    //内部変数
    private int oldScorePoint;      //前のフレームの正しいスコアポイント
    private float drawScorePoint;     //描画するスコア(不正確)
    private float strongEffectDelayTime;    //強調効果ディレイタイム
    private float colorStrongTime;

	// Use this for initialization
	void Awake () {
        mathManager = GameObject.Find(mathManagerObjName).GetComponent<GM_MathManager>();
        greenScoreName = transform.FindChild("GreenScoreName").GetComponent<Text>();
        greenScorePoint = transform.FindChild("GreenScorePoint").GetComponent<Text>();
        greenScorePointName = transform.FindChild("GreenScorePointName").GetComponent<Text>();
        Init();
	}
    public void Init()
    {
        pointEffectObj.Init();
    }
	
	// Update is called once per frame
	void Update () {
        int _strongLevel = 0;
        int _scorePoint = mathManager.totalFlowerScore;
        float _addScorePoint;

        //強調効果レベルを取得
        _strongLevel = GetStrongLevel();
        //強調効果をするなら
        if (_strongLevel > 0)
        {
            //強調効果を伝える
            pointEffectObj.StrongSignal(_strongLevel);
        }
        //エフェクトサイズの更新
        pointEffectObj.Update();
        //テキストオブジェクトのサイズをセット
        Vector3 _scale = Vector3.one;
        _scale.y = pointEffectObj.GetSize();
        greenScorePoint.transform.localScale = _scale;
        //テキストオブジェクトの色をセット
        greenScorePoint.color = pointEffectObj.GetTextColor();

        //スコアの徐々に増えていく感じの演出のため表示するスコアを計算
        _addScorePoint = _scorePoint - drawScorePoint;
        drawScorePoint = drawScorePoint + _addScorePoint * Time.deltaTime;
        if(drawScorePoint > _scorePoint){
            drawScorePoint = _scorePoint;
        }
        //テキストオブジェクトに表示する点数を入れる
        int _intDrawScorePoint = (int)drawScorePoint;
        greenScorePoint.text = _intDrawScorePoint.ToString();

        //古いスコアポイントを残す
        oldScorePoint = _scorePoint;
	}

    //強調効果をするかしないか決める。
    private int GetStrongLevel()
    {
        int _nowScorePoint = mathManager.totalFlowerScore;
        int _scoreDistance = 0;
        int _strongLevel = 0;

        //スコアがどれだけ増えたか計算
        _scoreDistance = _nowScorePoint - oldScorePoint;

        //増えた量に応じてレベル算出
        if (_scoreDistance > 20.0f)
        {
            _strongLevel = 1;
        }
        if (_scoreDistance > 70.0f)
        {
            _strongLevel = 2;
        }
        if (_scoreDistance > 150.0f)
        {
            _strongLevel = 3;
        }

        return _strongLevel;
    }

}


//緑化スコアのサイズエフェクトをするクラス
public class GM_UIGreenPercentEffect
{
    //内部変数定義
    private float effectEndTime;
    private float nowTime;
    private float nowSize;
    private float useSizeWidth; //使用される拡縮幅

    private Color useColor = Color.white; //強調効果に使用される色
    
    //初期化
    public void Init()
    {
        effectEndTime = 0.5f;
        nowTime = 10.0f;
        nowSize = 1.0f;
        useSizeWidth = 0.0f;
        useColor = Color.white;
    }

    //毎フレーム更新
    public void Update()
    {
        //時間を進める
        nowTime += Time.deltaTime;
        if (nowTime >= 10.0f)
        {
            nowTime = 10.0f;
            nowSize = 1.0f;
            return;
        }

        //強調効果を計算
        if (nowTime < effectEndTime)
        {
            nowSize = 1.0f + Mathf.Sin(nowTime * 32.0f * 3.1415f*2.0f) * useSizeWidth;
        }
        else
        {
            nowSize = 1.0f;
        }
    }

    //強調効果開始(1~3)
    public void StrongSignal(int _strongLevel)
    {
        switch (_strongLevel)
        {
            case 0:
                effectEndTime = 0.5f;
                useColor = Color.white;
                useSizeWidth = 0.0f;
                break;
            case 1:
                effectEndTime = 0.5f;
                useColor = Color.yellow;
                useSizeWidth = 0.2f;
                break;
            case 2:
                effectEndTime = 0.75f;
                useColor.r = 1.0f;
                useColor.g = 0.5f;
                useColor.b = 0.2f;
                useColor.a = 1.0f;
                useSizeWidth = 0.3f;
                break;
            case 3:
                effectEndTime = 1.0f;
                useColor = Color.red;
                useSizeWidth = 0.4f;
                break;
            default:
                effectEndTime = 1.0f;
                useColor = Color.red;
                useSizeWidth = 0.3f;
                break;
        }
        nowTime = 0.0f;
    }

    //サイズ取得
    public float GetSize()
    {
        return nowSize;
    }

    //エフェクトの残り時間を取得(引数：割合で返してもらうかどうか)
    public float GetEffectLimitTime(bool _percentFlg)
    {
        float _limitTime;
        _limitTime = effectEndTime - nowTime;
        if (_limitTime < 0.0f)
        {
            _limitTime = 0.0f;
        }

        //割合で返すかどうか
        if (_percentFlg == false)
        {
            //値で返す
            return _limitTime;
        }
        else
        {
            //割合で返す
            _limitTime = _limitTime / effectEndTime;
            return _limitTime;

        }
    }
    //色を取得
    public Color GetTextColor()
    {
        Color _color;
        float _percent = GetEffectLimitTime(true);

        //色付きの時間を長くしたい
        _percent = _percent + _percent;
        if (_percent > 1.0f)
        {
            _percent = 1.0f;
        }

        _color = Color.Lerp(Color.white, useColor, _percent);
        return _color;
    }
}