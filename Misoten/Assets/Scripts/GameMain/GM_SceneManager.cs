﻿using UnityEngine;
using System.Collections;

public class GM_SceneManager : MonoBehaviour {

    //==========定数定義==========
    public float GAME_TIME = 300.0f;

    //==========変数定義==========
    public float gameTime;     //今のゲームの残り時間

	public void Init() {
        gameTime = GAME_TIME;
	}
	
	// Update is called once per frame
	void Update () {
        gameTime += Time.deltaTime;
	}
}