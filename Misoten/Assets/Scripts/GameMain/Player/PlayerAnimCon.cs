using UnityEngine;
using System.Collections;

public class PlayerAnimCon : MonoBehaviour {

    private PlayerUnit playerUnit;  //プレイヤーUnit
    private Animator animator;      //プレイヤーのAnimator本体

    public Camera playerCamera;     //追随カメラオブジェクト

	// Use this for initialization
	void Awake () {
        playerUnit = transform.GetComponent<PlayerUnit>();
        //animator = transform.Find("Model").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //カメラ制御
        if (playerCamera != null)
        {
            Vector3 camPosVec; ;
            camPosVec = Vector3.back * 4.0f;
            camPosVec += Vector3.up * 5.2f;

            playerCamera.transform.position = transform.position + camPosVec;

        }
	}
}
