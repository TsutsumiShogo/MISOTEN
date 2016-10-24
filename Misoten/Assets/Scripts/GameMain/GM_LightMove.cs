using UnityEngine;
using System.Collections;

//ゲームメインでのライトの管理を行う
public class GM_LightMove : MonoBehaviour {

    //Unity上でセット
    [SerializeField]
    private GM_SceneManager gmManager;
    public Color morningColor;  //朝の色
    public Color dayColor;      //昼の色
    public Color eveningColor;  //夕方の色
    public float sunAngleRangeDeg = 120; //太陽の最大傾き具合  //0~180

    //ライトオブジェクト
    private Light thisLight;

	// Use this for initialization1
	void Awake () {
        thisLight = gameObject.GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
	    //ローカル変数定義
        float percent = 0.0f;

        //ゲームの進行度合いをpercent形式に変換
        percent = gmManager.gameTime / gmManager.GAME_TIME;

        //percent範囲計算
        if (percent < 0.0f)
        {
            percent = 0.0f;
        }
        if (percent > 1.0f)
        {
            percent = 1.0f;
        }

        //色計算
        CalcColorProcess(percent);

        //角度計算
        CalcDegreeProcess(percent);
	}

    //太陽光の色を計算する
    private void CalcColorProcess(float percent)
    {
        //ローカル変数定義
        float morningPercent = 0.0f;
        float dayPercent = 0.0f;
        float eveningPercent = 0.0f;

        //色算出のため３色毎にpercentを作る
        if (percent < 0.5f)
        {
            //朝と昼の計算
            morningPercent = 1.0f - percent * 2.0f;
            dayPercent = percent * 2.0f;
        }
        else
        {
            //昼と夕方の計算
            dayPercent = 1.0f - (percent - 0.5f) * 2.0f;
            eveningPercent = (percent - 0.5f) * 2.0f;
        }
        //色情報セット
        if (percent < 0.5f)
        {
            thisLight.color = morningColor * morningPercent + dayColor * dayPercent;
        }
        else
        {
            thisLight.color = dayColor * dayPercent + eveningColor * eveningPercent;
        }
    }

    //太陽光の角度を計算する
    private void CalcDegreeProcess(float percent)
    {
        //ローカル変数定義
        Vector3 _rotation = Vector3.zero;

        //X角度計算(太陽の高さ)
        _rotation.x = 20.0f + 20.0f * Mathf.Sin(3.14159265f * percent);

        //Y角度計算(太陽の傾き)
        _rotation.y = sunAngleRangeDeg * -0.5f + sunAngleRangeDeg * percent;

        //情報をセット
        transform.rotation = Quaternion.Euler(_rotation);
        
    }
}
