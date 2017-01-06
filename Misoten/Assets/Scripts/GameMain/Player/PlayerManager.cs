using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    //カメラオブジェクト(Unity上でセットしてほしい)
    public Camera CAMERA0;
    public Camera CAMERA1;
    public Camera CAMERA2;

    //プレイヤープレハブ(Unity上でセット)
    public GameObject[] SET_PLAYER_UNIT_PREFABS = new GameObject[3];   //マネージャーが生成するプレイヤーユニット情報

    //マネージャー内で各プレイヤーを保持する。
    //private GameObject[] playerUnits;
    private PlayerUnit[] playerUnits = new PlayerUnit[3];

    public GM_MathManager mathManager;  //Unity上でセット。プレイヤーがマス情報参照するときに使用。

    //初期化関数
    public void Init()
    {
        //もしすでにプレイヤーオブジェクトがあれば削除する
        DestroyPlayers();

        //プレイヤーオブジェクトを生成する
        GameObject[] playerObjects;
        playerObjects = new GameObject[3];
        for (int i = 0; i < 3; ++i)
        {
            playerObjects[i] = Instantiate(SET_PLAYER_UNIT_PREFABS[GM_StaticParam.g_selectCharacter[i]]);
            playerUnits[i] = playerObjects[i].GetComponent<PlayerUnit>();
            playerUnits[i].PLAYER_NO = i;
            playerUnits[i].Init(this);
            

            //プレイヤーの色を指定する
            switch (GM_StaticParam.g_selectCharacter[i])
            {
                case 0:
                    playerUnits[i].PLAYER_COLOR = GM_MathFlowerParam.EFlowerColor.RED;
                    break;
                case 1:
                    playerUnits[i].PLAYER_COLOR = GM_MathFlowerParam.EFlowerColor.GREEN;

                    break;
                case 2: 
                    playerUnits[i].PLAYER_COLOR = GM_MathFlowerParam.EFlowerColor.BLUE;
                    break;
            }

            playerObjects[i].transform.parent = transform;
        }

        Vector3 Initpos = transform.position;
        //とりあえずバラバラの位置へ
        Initpos.x += 1;
        Initpos.z += 1;
        playerUnits[0].transform.position = Initpos;        

        Initpos.x += -2;
        playerUnits[1].transform.position = Initpos;

        Initpos.z += -2;
        playerUnits[2].transform.position = Initpos;

        //カメラを各プレイヤーへ渡す
        playerUnits[0].GetComponent<PlayerAnimCon>().playerCamera = CAMERA0;
        playerUnits[1].GetComponent<PlayerAnimCon>().playerCamera = CAMERA1;
        playerUnits[2].GetComponent<PlayerAnimCon>().playerCamera = CAMERA2;
    }
    //終了処理
    public void Delete()
    {
        DestroyPlayers();
    }


    //Startのタイミングでは使用して大丈夫。各種プレイヤー本体を渡す
    public GameObject GetPlayerUnit(int playerNo)
    {
        if (playerUnits[0] == null)
        {
            return null;
        }

        if (playerNo < 0 || playerNo >= 3)
        {
            return null;
        }
        else
        {
            return playerUnits[GM_StaticParam.g_selectCharacter[playerNo]].gameObject;
        }
    }

    public void StartPlayers()
    {
        if (playerUnits[0] == null)
        {
            return;
        }

        for (int i = 0; i < 3; ++i)
        {
            playerUnits[i].StartPlayer();
        }
    }
    public void StopPlayers()
    {
        if (playerUnits[0] == null)
        {
            return;
        }

        for (int i = 0; i < 3; ++i)
        {
            playerUnits[i].StopPlayer();
        }
    }
    private void DestroyPlayers()
    {
        for (int i = 0; i < 3; ++i)
        {
            if (playerUnits[i] != null)
            {
                playerUnits[i].StopPlayer();
                Destroy(playerUnits[i].gameObject);
                playerUnits[i] = null;
            }
        }
    }
}
