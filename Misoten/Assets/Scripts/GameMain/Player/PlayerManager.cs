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

    void Awake()
    {
        //プレイヤーオブジェクトを生成する
        GameObject[] playerObjects;
        playerObjects = new GameObject[3];
        for (int i = 0; i < 3; ++i)
        {
            playerObjects[i] = Instantiate(SET_PLAYER_UNIT_PREFABS[0]);
            playerUnits[i] = playerObjects[i].GetComponent<PlayerUnit>();
            playerUnits[i].PLAYER_NO = i;

            playerObjects[i].transform.parent = transform;
        }
        
        //カメラを各プレイヤーへ渡す
        playerUnits[0].GetComponent<PlayerAnimCon>().playerCamera = CAMERA0;
        playerUnits[1].GetComponent<PlayerAnimCon>().playerCamera = CAMERA1;
        playerUnits[2].GetComponent<PlayerAnimCon>().playerCamera = CAMERA2;

    }

    //初期化関数
    public void Init()
    {
        Vector3 Initpos = transform.position;
        //とりあえずバラバラの位置へ
        Initpos.x += 1;
        Initpos.z += 1;
        playerUnits[0].transform.position = Initpos;

        Initpos.x += -2;
        playerUnits[1].transform.position = Initpos;

        Initpos.z += -2;
        playerUnits[2].transform.position = Initpos;

        //プレイヤーの色を指定する
        playerUnits[0].PLAYER_COLOR = GM_MathFlowerParam.EFlowerColor.RED;
        playerUnits[1].PLAYER_COLOR = GM_MathFlowerParam.EFlowerColor.GREEN;
        playerUnits[2].PLAYER_COLOR = GM_MathFlowerParam.EFlowerColor.BLUE;
    }


    //Startのタイミングでは使用して大丈夫。各種プレイヤー本体を渡す
    public GameObject GetPlayerUnit(int playerNo)
    {
        if (playerNo < 0 || playerNo >= 3)
        {
            return null;
        }
        else
        {
            return playerUnits[playerNo].gameObject;
        }
    }

    public void StartPlayers()
    {
        for (int i = 0; i < 3; ++i)
        {
            playerUnits[i].StartPlayer();
        }
    }
    public void StopPlayers()
    {
        for (int i = 0; i < 3; ++i)
        {
            playerUnits[i].StopPlayer();
        }
    }
}
