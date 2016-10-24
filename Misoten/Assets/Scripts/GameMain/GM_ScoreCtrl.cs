using UnityEngine;
using System.Collections;

// static変数とstatic関数でどこからでも呼べるようにしてます。
// スコアの管理を行う。
public class GM_ScoreCtrl : MonoBehaviour {

    //プレイヤー毎のスコアポイント値
    private static int[] playerScore = new int[3];

    //初期化
    static public void Reset()
    {
        playerScore[0] = playerScore[1] = playerScore[2] = 0;
    }

    //スコアを上書き
    static public bool SetPlayerScore(int setScorePoint, int playerNo)
    {
        //配列の範囲外は失敗
        if (playerNo < 0 || playerNo >= 3)
        {
            return false;
        }

        //スコア値を保存
        playerScore[playerNo] = setScorePoint;

        return true;
    }

    //スコアの加算処理
    static public bool AddPlayerScore(int addScorePoint, int playerNo)
    {
        //配列の範囲外は失敗
        if (playerNo < 0 || playerNo >= 3)
        {
            return false;
        }

        //スコア値を加算
        playerScore[playerNo] += addScorePoint;

        return true;
    }

    //スコアの取得
    static public int GetPlayerScore(int playerNo)
    {
        //配列の範囲外は失敗
        if (playerNo < 0 || playerNo >= 3)
        {
            return -1;
        }

        //スコア値を返す
        return playerScore[playerNo];
    }
    //全プレイヤーの合算したスコアの取得
    static public int GetPlayersScore()
    {
        return playerScore[0] + playerScore[1] + playerScore[2];
    }
}
