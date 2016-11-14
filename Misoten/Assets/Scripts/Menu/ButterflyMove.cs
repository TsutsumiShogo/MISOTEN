using UnityEngine;
using System.Collections;

public class ButterflyMove : MonoBehaviour {

    public GameObject Camera;
    public GameObject Butterfly;                // 蝶のオブジェクト
    public GameObject Select_CharSeleFlower;    // キャラクター選択の花のオブジェクト
    public GameObject Select_RankingFlower;     // ランキングの花のオブジェクト

    private Vector3 CharSeleFlowerPos;             // 初期座標
    private Vector3 RankingFlowerPos;
    private Vector3 DefaultSide;
    private Vector3 AfterSide;

    private float Butterfly_FlyHight;       // 蝶の飛行中の高さ

    private bool Select_CharaSeleFlg;       // 選択項目の切り替えフラグ     true:キャラクター選択を指定    false:ランキングを指定
    private bool ButterflyStopFlg;          // 蝶の静止確認フラグ          true:静止     false:移動

    void Start()
    {
        Init();
    }

    // Use this for initialization
    public void Init () {
        Butterfly_FlyHight = 0.0f;          // 花に止まっている状態

        CharSeleFlowerPos = Select_CharSeleFlower.transform.position;
        CharSeleFlowerPos.x += 25.0f;
        CharSeleFlowerPos.y += 80.0f;
        CharSeleFlowerPos.z -= 30.0f;

        RankingFlowerPos = Select_RankingFlower.transform.position;
        RankingFlowerPos.x -= 25.0f;
        RankingFlowerPos.y += 80.0f;
        RankingFlowerPos.z -= 30.0f;
        DefaultSide = Butterfly.transform.localScale;
        AfterSide = DefaultSide;
        AfterSide.x *= -1.0f;

        Select_CharaSeleFlg = true;         // キャラクター選択を選択中
        ButterflyStopFlg = true;            // 蝶が静止中
    }

    // Update is called once per frame
    void Update () {
        
        // 蝶が静止中
        if (ButterflyStopFlg)
        {
            Butterfly_FlyHight = 0.0f;
        }
        //　蝶が移動中
        else if (!ButterflyStopFlg)
        {
            Butterfly_FlyHight = 0.3f;
        }
        // 蝶の上下揺れ
     //   Butterfly.transform.position = new Vector3(Butterfly.transform.position.x, Select_CharSeleFlower.transform.position.y, Butterfly.transform.position.z + Mathf.Sin(Time.frameCount * 0.095f) * Butterfly_FlyHight);

        if (!Camera.GetComponent<CameraMove>().ButterflyMoveFlg)
        {
            Butterfly.transform.position = CharSeleFlowerPos;
            Select_CharaSeleFlg = true;
        }


        #region  入力：←キー
        if (Input.GetKeyDown(KeyCode.LeftArrow) && Camera.GetComponent<CameraMove>().ButterflyMoveFlg && !Select_CharaSeleFlg && ButterflyStopFlg)
        {
            Select_CharaSeleFlg = true;
            ButterflyStopFlg = false;
            Butterfly.transform.localScale = DefaultSide;
            StartCoroutine(Butterfly_MoveAndSideChange(RankingFlowerPos, CharSeleFlowerPos));
        }
        #endregion


        #region  入力：→キー
        if (Input.GetKeyDown(KeyCode.RightArrow) && Camera.GetComponent<CameraMove>().ButterflyMoveFlg && Select_CharaSeleFlg && ButterflyStopFlg)
        {
            Select_CharaSeleFlg = false;
            ButterflyStopFlg = false;
            Butterfly.transform.localScale = AfterSide;
            StartCoroutine(Butterfly_MoveAndSideChange(CharSeleFlowerPos, RankingFlowerPos));
        }
        #endregion


    }

    #region 蝶の移動関数
    private IEnumerator Butterfly_MoveAndSideChange(Vector3 BeforePos, Vector3 AfterPos)
    {
        float Elapsedtime = 0.0f;       // 経過時間の初期化

        while (Elapsedtime <= 1.0f)
        {
            Elapsedtime += Time.deltaTime;
            BeforePos.y += (2 + Mathf.Sin(Time.frameCount * 0.095f) * Butterfly_FlyHight);
            Butterfly.transform.position = Vector3.Lerp(BeforePos, AfterPos, Elapsedtime);    // 蝶の移動
            yield return null;
        }
        Butterfly.transform.position = AfterPos;
        ButterflyStopFlg = true;
        
        yield return 0;
    }
    #endregion
}
