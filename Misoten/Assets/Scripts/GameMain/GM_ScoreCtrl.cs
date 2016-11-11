using UnityEngine;
using System.Collections;

// static変数とstatic関数でどこからでも呼べるようにしてます。
// スコアの管理を行う。
public class GM_ScoreCtrl : MonoBehaviour {

    //プレイヤー毎のスコアポイント値
    private static float[] playerScore = new float[4];  //1~3P と 4P(システム自然成長など)

    //初期化
    static public void Reset()
    {
        playerScore[0] = playerScore[1] = playerScore[2] = playerScore[3] = 0.0f;
    }

    //スコアを上書き
    static public bool SetPlayerScore(float setScorePoint, int playerNo)
    {
        //配列の範囲外は失敗
        if (playerNo < 0 || playerNo >= 4)
        {
            return false;
        }

        //スコア値を保存
        playerScore[playerNo] = setScorePoint;

        return true;
    }

    //スコアの加算処理
    static public bool AddPlayerScore(float addScorePoint, int playerNo)
    {
        //配列の範囲外は失敗
        if (playerNo < 0 || playerNo >= 4)
        {
            return false;
        }

        //スコア値を加算
        playerScore[playerNo] += addScorePoint;

        return true;
    }

    //スコアの取得
    static public float GetPlayerScore(int playerNo)
    {
        //配列の範囲外は失敗
        if (playerNo < 0 || playerNo >= 4)
        {
            return -1;
        }

        //スコア値を返す
        return playerScore[playerNo];
    }
    //全プレイヤーの合算したスコアの取得
    static public float GetPlayersScore()
    {
        return playerScore[0] + playerScore[1] + playerScore[2] + playerScore[3];
    }


    //オブジェクト化された時のみ使用される(Unity表示用)
    [SerializeField]
    private int[] playerScoreReadOnly = new int[4];
    void Update()
    {
        //値更新
        for (int i = 0; i < 4; i++)
        {
            playerScoreReadOnly[i] = (int)playerScore[i];
        }
    }
}
