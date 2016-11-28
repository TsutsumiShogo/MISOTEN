﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GM_UIBillGrowthNumDrawer : MonoBehaviour {

    //ビルリスト
    private GM_MathBillList mathManager;

    //ビル成長中を表示するUI
    [SerializeField]
    private List<Image> billGrowthImageList;    //Unity上でセット

    //担当カメラ
    [SerializeField]
    private Camera playerCamera;                //Unity上でセット

    [SerializeField]
    private int PLAYER_NO;                      //Unity上でセット

    //成長中ビルUIのスプライト
    public Sprite[] growthNumSprits = new Sprite[4];

    

	// Use this for initialization
	void Awake () {
        mathManager = GameObject.Find("MathManager").GetComponent<GM_MathBillList>();
	}
	
	// Update is called once per frame
	void Update () {
        //UIを一度無効にする
        for (int i = 0; i < billGrowthImageList.Count; ++i)
        {
            billGrowthImageList[i].gameObject.SetActive(false);
        }

        //成長中のビルが存在するか確認
        int growthBillNum = mathManager.growingBillList.Count;
        if (growthBillNum <= 0)
        {
            return;
        }

        //成長中のビル画面内にあればUI表示
        int selectUiNo = 0;
        for (int i = 0; i < growthBillNum; ++i)
        {
            //ビルの位置取得
            Vector3 objectPos = mathManager.growingBillList[i].transform.position;
            //画面内判定位置を修正
            if (playerCamera.transform.position.x < objectPos.x)
            {
                objectPos.x -= 3.0f;
            }
            else
            {
                objectPos.x += 3.0f;
            }
            objectPos.y += 2.5f;
            
            //画面外なら何もしない
            if (IsOutScreen(objectPos) == true)
            {
                continue;
            }

            //画面内なら有効にして位置変更
            billGrowthImageList[selectUiNo].gameObject.SetActive(true);
            billGrowthImageList[selectUiNo].rectTransform.position = TransScreenPosition(objectPos, PLAYER_NO);

            //成長させている人数に合わせてスプライト変更
            billGrowthImageList[selectUiNo].sprite = growthNumSprits[mathManager.growingBillList[i].GetGrowthNowPlayerNum()];

            selectUiNo++;
        }
	}

    // 画面外チェック
    private bool IsOutScreen(Vector3 worldPosition)
    {
        Vector3 viewportRate = playerCamera.WorldToViewportPoint(worldPosition);
        if (0.1 < viewportRate.x && viewportRate.x <= 0.9)
        {
            if (0.1 < viewportRate.y && viewportRate.y <= 0.9)
            {
                if (viewportRate.z > 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private Vector3 TransScreenPosition(Vector3 targetWorldPosition, int playerNo){
        Vector3 viewportRate;
        Vector3 screenPos;
        float screenSizeX = Screen.width * 0.5f;
        float screenSizeY = Screen.height * 0.5f;
        float offSetX = 0;
        float offSetY = 0;

        //スクリーン座標のオフセット
        switch (playerNo)
        {
            case 0:
                offSetX = Screen.width * 0.25f;
                offSetY = Screen.height * 0.5f;
                break;
            case 1:
                offSetX = Screen.width * 0.5f;
                break;
            case 2:
                break;
        }

        viewportRate = playerCamera.WorldToViewportPoint(targetWorldPosition);

        screenPos.x = offSetX + viewportRate.x * screenSizeX;
        screenPos.y = offSetY + viewportRate.y * screenSizeY;
        screenPos.z = 0.0f;

        return screenPos;
    }
}