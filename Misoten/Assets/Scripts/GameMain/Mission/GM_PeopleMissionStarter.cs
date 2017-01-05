using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_PeopleMissionStarter : MonoBehaviour
{
    //オブジェクト
    [SerializeField]
    private List<GameObject> drawObjects;   //表示に使用するオブジェクト
    [SerializeField]
    private TextMesh textObj;               //テキスト
    private GM_MissionManager manager;      //ミッションマネージャ

    private GameObject peopleObj;           //住人オブジェクト。表示位置に使用する。

    //内部変数
    private string drawText;    //外部から関数を通して書き込まれる
    private float peopleMissionDrawTime = 0.0f;

    void Awake()
    {
        //親からマネージャ取得
        manager = transform.parent.GetComponent<GM_MissionManager>();
    }

    public void Init()
    {
        drawText = "";
        peopleMissionDrawTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //もし住人がいなければ
        if (peopleObj == null)
        {
            manager.ChangeMob();
            return;
        }

        //表示テキストを更新
        textObj.text = drawText;

        //住人の位置に合わせる
        transform.position = peopleObj.transform.position;

        //表示期間中なら表示する
        if (peopleMissionDrawTime > 0.0f)
        {
            //表示時間カウントを減らす
            peopleMissionDrawTime -= Time.deltaTime;

            for (int i = 0; i < drawObjects.Count; ++i)
            {
                drawObjects[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < drawObjects.Count; ++i)
            {
                drawObjects[i].SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //プレイヤーとの接触だった場合
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //マネージャの住人ミッション開始処理を申請(何回も申請していい。)
            manager.StartPeopleMissionSignal();

            //表示時間を設定
            peopleMissionDrawTime = 1.0f;
        }
    }
    void OnTriggerStay(Collider col)
    {
        //プレイヤーとの接触だった場合
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //表示時間を設定
            peopleMissionDrawTime = 1.0f;
        }
    }

    //ミッション内容表示オブジェクトの位置に使用する住人の指定
    public void SetPeople(GameObject _obj)
    {
        peopleObj = _obj;
    }

    //テキストセット
    public void SetText(string _text)
    {
        drawText = _text;
    }
}
