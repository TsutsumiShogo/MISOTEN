using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
    // ==========================================
    //  列挙型定義
    public enum SceneType
    {
        TITLE,              // タイトル
        MENU,               // メニュー
        CHARCTER_SELECT,    // キャラセレクト
        RANKING,            // ランキング
    };

    private SceneType m_nowSceneType;       // 現在のシーンを保持

    public GameObject Camera;               // カメラのオブジェクト
    public GameObject MainMenu;             // メインメニューのオブジェクト
    public GameObject CharacterSelect;      // キャラクターセレクトのオブジェクト
    public GameObject PushA;                // ゲームスタートのテキスト
    public GameObject[] Ranking = new GameObject[4];    // ランキングのオブジェクト
    public GameObject Logo;
    public GameObject selectMode;

    public float CameraMove_UpDown = 4.0f;
    public float CameraMove_RankingUpDown = 5.0f;
    public float CameraMove_RankingTop = 0.5f;
    public float CameraMove_LeftRight = 0.5f;

    private Vector3 defaultCameraPos;       // カメラの初期座標
    private Vector3 menuPos;                // メインメニュー画面の座標
    private Vector3 charaselePos;           // キャラクターセレクト画面の座標
    private Vector3[] rankPos = new Vector3[4];                // ランキング画面の座標

    private static float distance;          // カメラ位置の調整用変数

    private bool GameStartFlg;              // ゲーム開始フラグ
    private bool StateChangeFlg;            // 遷移キャンセルフラグ
    public bool CharaSelectFlg;            // キャラクター画面遷移フラグ
    public bool RankingFlg;                // ランキング画面遷移フラグ
    private bool OnRankingFlg;              // ランキング画面操作フラグ
    private bool RankingUpDownFlg;          // ランキング画面での上下操作判別フラグ

    public bool ButterflyMoveFlg;          // 蝶の操作フラグ
    public bool CharaSeleStartFlg;          // キャラクター選択画面での入力操作オンオフ切り替え
    // Use this for initialization
    void Start()
    {
        Init();
    }
    public void Init()
    {
        /*distance = 466.0f;          // カメラ位置の調整

        defaultCameraPos = Camera.transform.position;     // カメラの座標取得
        menuPos = MainMenu.transform.position;      // メインメニュー画面の座標取得
        menuPos.z -= distance;

        charaselePos = CharacterSelect.transform.position;  // キャラクターセレクト画面の座標取得
        charaselePos.z -= distance;

        // ランキング画面の座標取得
        rankPos[0] = Ranking[0].transform.position; // ランキング画面の1枚目の座標取得
        rankPos[1] = Ranking[1].transform.position; // ランキング画面の2枚目の座標取得
        rankPos[2] = Ranking[2].transform.position; // ランキング画面の3枚目の座標取得
        rankPos[3] = Ranking[3].transform.position; // ランキング画面の4枚目の座標取得

        rankPos[0].z -= distance;
        rankPos[1].z -= distance;
        rankPos[2].z -= distance;
        rankPos[3].z -= distance;

        GameStartFlg = false;       // タイトルへ戻る
        StateChangeFlg = false;     // 画面遷移：OFF
        CharaSelectFlg = true;      // キャラクターセレクト画面：ON
        RankingFlg = false;         // メインメニューへ戻る
        OnRankingFlg = false;       // ランキング画面の操作を切る
        RankingUpDownFlg = false;
        CharaSeleStartFlg = false;
        */
    }

    // Update is called once per frame
    void Update()
    {

        #region 入力：Aキー
        // タイトルからメインメニューへ遷移
        if (( Input.GetKeyDown(KeyCode.A) || XboxController.GetButtonA_All()) && !GameStartFlg && !StateChangeFlg)
        {
            GetComponent<AudioSource>().Play();
            //Logo.SetActive(false);
            //StateChangeFlg = true;      // 画面遷移させる
            //PushA.SetActive(false);     // 「Push A」の非表示
            StartCoroutine(StateChange(gameObject.transform.position, menuPos)); // メインメニュー画面の位置にカメラを移動
        }
        // メインメニューからキャラクターセレクト画面へ遷移
        else if ( (Input.GetKeyDown(KeyCode.A) || XboxController.GetButtonA_All()) && GameStartFlg && CharaSelectFlg && !RankingFlg && !StateChangeFlg)
        {
            ButterflyMoveFlg = false;
            selectMode.GetComponent<SelectMode>().DesideAction();
            StateChangeFlg = true;      // 画面遷移させる
            StartCoroutine(StateChange(gameObject.transform.position, charaselePos));   // キャラクターセレクト画面の位置にカメラを移動
        }
        // メインメニューからランキング画面へ遷移
        else if ((Input.GetKeyDown(KeyCode.A)|| XboxController.GetButtonA_All()) && GameStartFlg && !CharaSelectFlg && RankingFlg && !StateChangeFlg)
        {
            ButterflyMoveFlg = false;

            StateChangeFlg = true;      // 画面遷移させる
            StartCoroutine(StateChange(gameObject.transform.position, rankPos[3]));    // ランキング画面の4枚目の位置にカメラを移動
        }
        #endregion


        #region 入力：↑キー
        // ランキング画面での↑キー操作
        if ((Input.GetKeyDown(KeyCode.UpArrow)) && GameStartFlg && !CharaSelectFlg && RankingFlg && !StateChangeFlg)
        {
            RankingUpDownFlg = true;
            StateChangeFlg = true;      // 画面遷移させる

            // ランキング画面の2枚目からランキング画面の1枚目へ遷移
            if (gameObject.transform.position == rankPos[1])
            {
                StartCoroutine(StateChange(gameObject.transform.position, rankPos[0]));     // ランキング画面の1枚目の位置にカメラを移動
            }
            // ランキング画面の3枚目からランキング画面の2枚目へ遷移
            else if (gameObject.transform.position == rankPos[2])
            {
                StartCoroutine(StateChange(gameObject.transform.position, rankPos[1]));     // ランキング画面の2枚目の位置にカメラを移動
            }
            // ランキング画面の4枚目からランキング画面の3枚目へ遷移
            else if (gameObject.transform.position == rankPos[3])
            {
                StartCoroutine(StateChange(gameObject.transform.position, rankPos[2]));     // ランキング画面の3枚目の位置にカメラを移動 
            }
        }
        #endregion


        #region 入力：↓キー
        // ランキング画面での↓キー操作
        if (Input.GetKeyDown(KeyCode.DownArrow) && GameStartFlg && !CharaSelectFlg && RankingFlg && !StateChangeFlg)
        {
            RankingUpDownFlg = false;
            StateChangeFlg = true;      // 画面遷移させる

            // ランキング画面の1枚目からランキング画面の2枚目へ遷移
            if (gameObject.transform.position == rankPos[0])
            {
                StartCoroutine(StateChange(gameObject.transform.position, rankPos[1]));     // ランキング画面の2枚目の位置にカメラを移動
            }
            // ランキング画面の2枚目からランキング画面の3枚目へ遷移
            else if (gameObject.transform.position == rankPos[1])
            {
                StartCoroutine(StateChange(gameObject.transform.position, rankPos[2]));     // ランキング画面の3枚目の位置にカメラを移動
            }
            // ランキング画面の3枚目からランキング画面の4枚目へ遷移
            else if (gameObject.transform.position == rankPos[2])
            {
                StartCoroutine(StateChange(gameObject.transform.position, rankPos[3]));     // ランキング画面の4枚目の位置にカメラを移動
            }
        }
        #endregion


        #region  入力：←キー
        // メインメニューからキャラクターセレクト画面へ遷移
        if (Input.GetKeyDown(KeyCode.LeftArrow) && GameStartFlg && !CharaSelectFlg && RankingFlg && ButterflyMoveFlg)
        {
            CharaSelectFlg = true;     // キャラクターセレクト：ON
            RankingFlg = false;        // ランキング画面：OFF
        }
        #endregion


        #region  入力：→キー
        // メインメニューからランキング画面へ遷移
        if (Input.GetKeyDown(KeyCode.RightArrow) && GameStartFlg && CharaSelectFlg && !RankingFlg && ButterflyMoveFlg)
        {
            RankingFlg = true;          // ランキング画面：OFF
            CharaSelectFlg = false;     // キャラクターセレクト：ON
        }

        #endregion


        #region  入力：Bキー
        // メインメニューからタイトルへ遷移
        if (Input.GetKeyDown(KeyCode.Backspace) && GameStartFlg && CharaSelectFlg && !RankingFlg && !StateChangeFlg && gameObject.transform.position == menuPos)
        {
            GameStartFlg = false;       // タイトルへ戻る
            StateChangeFlg = true;      // 画面遷移させる
            StartCoroutine(StateChange(gameObject.transform.position, defaultCameraPos));     // タイトル画面の位置にカメラを移動  
        }
        // キャラクターセレクト画面からメインメニューへ遷移
        else if (Input.GetKeyDown(KeyCode.Backspace) && GameStartFlg && CharaSelectFlg && !RankingFlg && !StateChangeFlg)
        {
            StateChangeFlg = true;      // 画面遷移させる
            CharaSeleStartFlg = false;
            StartCoroutine(StateChange(gameObject.transform.position, menuPos));        // メインメニュー画面の位置にカメラを移動
        }
        // ランキング画面からメインメニューへ遷移
        else if (Input.GetKeyDown(KeyCode.Backspace) && GameStartFlg && !CharaSelectFlg && RankingFlg && !StateChangeFlg)
        {
            RankingFlg = false;         // メインメニューへ戻る
            OnRankingFlg = false;       // ランキング画面の操作を切る
            StateChangeFlg = true;      // 画面遷移させる

            // ランキング画面の4枚目ならメインメニューへ遷移
            if (gameObject.transform.position == rankPos[3])
            {
                StartCoroutine(StateChange(gameObject.transform.position, menuPos));        // メインメニュー画面の位置にカメラを移動
            }
            // ランキング画面の1～3枚目ならランキング画面の4枚目へ遷移
            else if (gameObject.transform.position != rankPos[3])
            {
                StartCoroutine(StateChange(gameObject.transform.position, rankPos[3]));      // ランキング画面の4枚目の位置にカメラを移動  
            }
        }
        #endregion
    }

    #region 画面の切り替え関数(カメラの移動)
    private IEnumerator StateChange(Vector3 beforePos, Vector3 afterPos)
    {
        float Elapsedtime = 0.0f;       // 経過時間の初期化
        Vector3 nowPos = beforePos;
        Vector3 distanceVec = afterPos - beforePos;
        float distance = distanceVec.magnitude;

        #region 戻る時の加減速移動
        if (StateChangeFlg)
        {
            while (distance > 0.5f)
            {
                nowPos = nowPos + distanceVec * CameraMove_UpDown * Time.deltaTime;
                gameObject.transform.position = nowPos;
                distanceVec = afterPos - nowPos;
                distance = distanceVec.magnitude;
                yield return null;
            }
            gameObject.transform.position = afterPos;
            if (!GameStartFlg && gameObject.transform.position == defaultCameraPos)
            {
                ButterflyMoveFlg = false;
                PushA.SetActive(true);
            }
            else if (!GameStartFlg && gameObject.transform.position == menuPos)
            {
                GameStartFlg = true;
                ButterflyMoveFlg = true;
            }
            else if(GameStartFlg && gameObject.transform.position == menuPos)
            {
                ButterflyMoveFlg = true;
            }
        }
        #endregion

        #region キャラクターセレクトまたはランキング画面への遷移処理
        else if ((GameStartFlg && CharaSelectFlg || RankingFlg) && !OnRankingFlg)
        {
            Elapsedtime = 0.0f;

            while (Elapsedtime <= 1.0f)
            {
                Elapsedtime += Time.deltaTime * CameraMove_LeftRight;
                gameObject.transform.position = Vector3.Lerp(beforePos, afterPos, Elapsedtime);     // ランキング画面の4枚目からランキング画面の1枚目へ画面遷移
                yield return null;
            }
            if (CharaSelectFlg)
            {
                CharaSeleStartFlg = true;
            }
        }
        #endregion

        #region ランキング画面の4枚目からランキング画面の1枚目へ画面遷移
        if (RankingFlg && !OnRankingFlg && gameObject.transform.position == rankPos[3])
        {
            Elapsedtime = 0.0f;
            OnRankingFlg = true;
            while (Elapsedtime <= 1.0f)
            {
                Elapsedtime += Time.deltaTime * CameraMove_RankingTop;
                gameObject.transform.position = Vector3.Lerp(rankPos[3], rankPos[0], Elapsedtime);     // ランキング画面の4枚目からランキング画面の1枚目へ画面遷移
                yield return null;
            }
        }
        #endregion

        #region ランキング画面の1～4枚目間の遷移処理
        else if (RankingFlg && OnRankingFlg)
        {
            while (distance > 1.0f)
            {
                nowPos = nowPos + distanceVec * CameraMove_RankingUpDown * Time.deltaTime;
                gameObject.transform.position = nowPos;
                distanceVec = afterPos - nowPos;
                distance = distanceVec.magnitude;
                yield return null;
            }
            gameObject.transform.position = afterPos;
        }
        #endregion

        #region ランキング画面からメインメニュー画面への遷移
       if (!RankingFlg && !OnRankingFlg && gameObject.transform.position == rankPos[3])
        {
            Elapsedtime = 0.0f;
            while (Elapsedtime <= 1.0f)
            {
                Elapsedtime += Time.deltaTime * CameraMove_RankingTop;
                gameObject.transform.position = Vector3.Lerp(rankPos[3], menuPos, Elapsedtime);     // ランキング画面の4枚目からランキング画面の1枚目へ画面遷移
                yield return null;
            }
            CharaSelectFlg = true;
            RankingFlg = false;
            ButterflyMoveFlg = true;
        }
        #endregion

        StateChangeFlg = false;

        yield return 0;
    }
    #endregion
}