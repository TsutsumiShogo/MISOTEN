using UnityEngine;
using System.Collections;

public class GM_MiniMapPlayers : MonoBehaviour {

    //定数定義
    
    //オブジェクト
    [SerializeField]
    private Sprite[] PLAYER_ICON_TEXTURE = new Sprite[3];   //プレイヤーのアイコン３種類
    [SerializeField]
    private SpriteRenderer[] playerIcon = new SpriteRenderer[3];    //ミニマップに表示するアイコンオブジェクト
    //変数定義

    //内部変数
    private PlayerManager playerManager;
    

	// Use this for initialization
	void Awake () {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
	}

    //プレイヤーのアイコンを1~3P決める(できればenum型にしたい)
    void Init(int iconNo1, int iconNo2, int iconNo3)
    {
        //アイコンのテクスチャを変更する
        playerIcon[0].sprite = PLAYER_ICON_TEXTURE[iconNo1];
        playerIcon[1].sprite = PLAYER_ICON_TEXTURE[iconNo2];
        playerIcon[2].sprite = PLAYER_ICON_TEXTURE[iconNo3];

        //アイコンをプレイヤーの位置へ
        IconMove();
    }

	// Update is called once per frame
	void Update () {
        IconMove();
	}

    //アイコン移動処理
    private void IconMove() {
        GameObject playerObject;
        Vector3 iconPos;
	    //プレイヤーの位置へアイコンを動かす
        for (int i = 0; i < 3; ++i)
        {
            //プレイヤーオブジェクト取得
            playerObject = playerManager.GetPlayerUnit(i);

            //アイコン位置計算
            iconPos = playerObject.transform.position;
            iconPos += transform.position;

            //プレイヤーアイコンを移動
            playerIcon[i].transform.position = iconPos;
        }
    }
}
