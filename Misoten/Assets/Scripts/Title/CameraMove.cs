using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

    public GameObject Camera;               // カメラのオブジェクト
    public GameObject MainMenu;             // メインメニューのオブジェクト
    public GameObject CharacterSelect;      // キャラクターセレクトのオブジェクト
    public GameObject Ranking;              // ランキングのオブジェクト
    public GameObject PushA;                // ゲームスタートのテキスト

    private Vector3 defaultPos;             // カメラの初期座標
    private Vector3 menuPos;                // メインメニュー画面の座標
    private Vector3 charaselePos;           // キャラクターセレクト画面の座標
    private Vector3 rankPos;                // ランキング画面の座標

    private static float distance;          // カメラ位置の調整用変数

    private bool GameStartFlg;              // ゲーム開始フラグ
    private bool stateChangeFlg;            // 遷移キャンセルフラグ
    private bool CharaSelectFlg;
    private bool RankingFlg;

    // Use this for initialization
    void Start () {
        distance = 466.0f;          // カメラ位置の調整

        defaultPos = Camera.transform.position;     // カメラの座標取得
        menuPos = MainMenu.transform.position;      // メインメニュー画面の座標取得
        menuPos.z -= distance;  
                           
        charaselePos = CharacterSelect.transform.position;  // キャラクターセレクト画面の座標取得
        charaselePos.z -= distance;

        rankPos = Ranking.transform.position;       // ランキング画面の座標取得
        rankPos.z -= distance;

        GameStartFlg = false;       // ゲームスタート：OFF
        stateChangeFlg = false;     // 画面遷移：OFFｗ
        CharaSelectFlg = false;     // キャラクターセレクト画面：OFF
        RankingFlg = false;         // ランキング画面：OFF
    }
	
	// Update is called once per frame
	void Update () {

        #region 入力：Aキー
        if (Input.GetKeyDown(KeyCode.A) && !GameStartFlg && !stateChangeFlg)
        {
            GameStartFlg = true;
            stateChangeFlg = true;
            PushA.SetActive(false);
            StartCoroutine(StateChange(gameObject.transform.position, menuPos));    // タイトルからメインメニューへ遷移
        }
        #endregion
        #region  入力：←キー
        if (Input.GetKeyDown(KeyCode.LeftArrow) && GameStartFlg && !CharaSelectFlg && !RankingFlg && !stateChangeFlg)
        {
            CharaSelectFlg = true;
            stateChangeFlg = true;
            StartCoroutine(StateChange(gameObject.transform.position, charaselePos));   // メインメニューからキャラクターセレクト画面へ遷移
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && GameStartFlg && !CharaSelectFlg && RankingFlg && !stateChangeFlg)
        {
            RankingFlg = false;
            stateChangeFlg = true;
            StartCoroutine(StateChange(gameObject.transform.position, menuPos));        // キャラクターセレクト画面からメインメニューへ遷移
        }
        #endregion
        #region  入力：→キー
        if (Input.GetKeyDown(KeyCode.RightArrow) && GameStartFlg && !CharaSelectFlg && !RankingFlg && !stateChangeFlg)
        {
            RankingFlg = true;
            stateChangeFlg = true;
            StartCoroutine(StateChange(gameObject.transform.position, rankPos));    // メインメニューからランキング画面へ遷移
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && GameStartFlg && CharaSelectFlg && !RankingFlg && !stateChangeFlg)
        {
            CharaSelectFlg = false;
            stateChangeFlg = true;
            StartCoroutine(StateChange(gameObject.transform.position, menuPos));    // ランキング画面からメインメニューへ遷移
        }
        #endregion
        #region  入力：Bキー
        if (Input.GetKeyDown(KeyCode.B) && GameStartFlg && !CharaSelectFlg && !RankingFlg && !stateChangeFlg)
        {
            GameStartFlg = false;
            stateChangeFlg = true;
            StartCoroutine(StateChange(gameObject.transform.position, defaultPos));     // メインメニューからタイトルへ遷移
        }
        else if (Input.GetKeyDown(KeyCode.B) && GameStartFlg && CharaSelectFlg && !RankingFlg && !stateChangeFlg)
        {
            CharaSelectFlg = false;
            stateChangeFlg = true;
            StartCoroutine(StateChange(gameObject.transform.position, menuPos));        // キャラクターセレクト画面からメインメニューへ遷移
        }
        else if (Input.GetKeyDown(KeyCode.B) && GameStartFlg && !CharaSelectFlg && RankingFlg && !stateChangeFlg)
        {
            RankingFlg = false;
            stateChangeFlg = true;
            StartCoroutine(StateChange(gameObject.transform.position, menuPos));        // ランキング画面からメインメニューへ遷移
        }
        #endregion
    }

    #region 画面の切り替え関数(カメラの移動)
    private IEnumerator  StateChange(Vector3 beforePos, Vector3 afterPos)
    {
        float Elapsedtime = 0.0f;       // 経過時間の加算
        while (Elapsedtime <= 1.0f)
        {
            Elapsedtime += Time.deltaTime * 0.5f;
            gameObject.transform.position = Vector3.Lerp(beforePos, afterPos, Elapsedtime);     // 画面遷移
            yield return null;
        }
        if (!GameStartFlg)
        {
            PushA.SetActive(true);
        }
        stateChangeFlg = false;
        yield return 0;
    }
    #endregion
}
