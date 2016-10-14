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
        Vector3 Initpos = transform.position;
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

        //とりあえずバラバラの位置へ
        Initpos.x += 1;
        Initpos.z += 1;
        playerUnits[0].transform.position = Initpos;

        Initpos.x += -2;
        playerUnits[1].transform.position = Initpos;

        Initpos.z += -2;
        playerUnits[2].transform.position = Initpos;

        Initpos.x += 2;
        playerUnits[3].transform.position = Initpos;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Startのタイミングでは使用して大丈夫。各種プレイヤー本体を渡す
    public GameObject GetPlayerUnit(int playerNo)
    {
        if (playerNo < 0 || playerNo >= 4)
        {
            return null;
        }
        else
        {
            return playerUnits[playerNo].gameObject;
        }
    }

    public void StartPlayers(int _dummy)
    {
        for (int i = 0; i < 3; ++i)
        {
            //playerUnits[i].StartPlayer(0);
        }
    }
    public void StopPlayers(int _dummy)
    {
        for (int i = 0; i < 3; ++i)
        {
            //playerUnits[i].StopPlayer(0);
        }
    }
}
