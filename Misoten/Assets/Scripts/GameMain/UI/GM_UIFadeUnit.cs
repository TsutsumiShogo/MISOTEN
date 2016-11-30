using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM_UIFadeUnit : MonoBehaviour {

    //定数定義
    public float CHANGE_TIME = 1.0f;

    //フェード画像
    private Image thisImage;

    //切り替えるオブジェクト(全てUnity上でセット)
    [SerializeField]
    private GameObject gameCanvas;
    [SerializeField]
    private GameObject gameObjects;
    [SerializeField]
    private GameObject resultCanvas;
    [SerializeField]
    private GameObject resultObjects;

    //内部変数
    private bool updateFlg;
    private float changeTime;

	// Use this for initialization
	void Awake () {
        thisImage = transform.GetComponent<Image>();
	}
    public void Init()
    {
        gameCanvas.SetActive(true);
        gameObjects.SetActive(true);
        resultCanvas.SetActive(false);
        resultObjects.SetActive(false);
        thisImage.color = new Color(0, 0, 0, 0.0f);
        changeTime = 0.0f;
    }

	// Update is called once per frame
	void Update () {
        if (updateFlg == false)
        {
            return;
        }
        if (changeTime > CHANGE_TIME)
        {
            //フェード完了
            thisImage.color = new Color(0, 0, 0, 0.0f);
            return;
        }

        float halfTime = CHANGE_TIME * 0.5f;
        float _percent = 0.0f;
        //時間を進める
        changeTime += Time.deltaTime;

        //切り替え処理
        if (changeTime >= halfTime)
        {
            //ミニマップで削除処理を走らせる
            GameObject.Find("MiniMapManager").GetComponent<GM_MiniMapManager>().Delete();

            //オブジェクト切り替え
            gameCanvas.SetActive(false);
            gameObjects.SetActive(false);
            resultCanvas.SetActive(true);
            resultObjects.SetActive(true);
        }

        //フェードモード切替
        if (changeTime < halfTime)
        {
            _percent = changeTime / halfTime;

        }
        else
        {
            _percent = (halfTime - (changeTime - halfTime)) / halfTime;
        }

        //範囲チェック
        if (_percent < 0.0f) _percent = 0.0f;
        if (_percent > 1.0f) _percent = 1.0f;
        
        //色変更
        thisImage.color = new Color(0, 0, 0, _percent);

	}

    public void SceneChangeResult()
    {
        if (updateFlg == true)
        {
            return;
        }
        updateFlg = true;
    }
}
