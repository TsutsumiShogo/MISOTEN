using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM_MathCell : MonoBehaviour {

    //孫オブジェクトから自らセットしに来る
    public List<GM_MathFlowerParam> flowerParams;
    //マネージャオブジェクト
    public GM_MathManager manager;
    //マス目描画スクリプト
    public GM_MathCellColorControll mathCellColorCon;


    //このセルのステージ番号
    public GM_MathManager.EMathStageNo stageNo;     //Unity上でセット

    public bool startFlg;   //このセルの行動を開始して良いか

	//初回のみ
	void Awake () {
        //親の親オブジェクトにMathManagerがあるので保存
        manager = transform.parent.parent.GetComponent<GM_MathManager>();
	    //親の親オブジェクトにMathManagerがあるのでそこに自らをセット
        manager.cells.Add(this);
        //マス目描画スクリプトを保存
        mathCellColorCon = GetComponentInChildren<GM_MathCellColorControll>();
	}
	
    //ゲーム開始時に初期化
    public void Init()
    {
        //各パラメーターを初期化
        for (int i = 0; i < flowerParams.Count; ++i)
        {
            flowerParams[i].Init();
        }

        //マス色管理システムを初期化
        mathCellColorCon.Init();

        startFlg = false;
    }
    //セル利用開始時に呼ばれる
    public void CellStart()
    {
        mathCellColorCon.ChangeMathColor(Color.white);
        //各あたり判定を有効にする
        for (int i = 0; i < flowerParams.Count; ++i)
        {
            flowerParams[i].ColliderSwitch(true);
        }
        startFlg = true;
    }

    //自然成長
    public void PrantGrowth(int addExp)
    {
        //自然成長を伝達
        for (int i = 0; i < flowerParams.Count; ++i)
        {
            flowerParams[i].PrantGrowth(addExp);
        }
    }

    //このセルの中の花のレベルの総数を計算
    public int GetTotalFlowerLevel()
    {
        int levelCount = 0;

        for (int i = 0; i < flowerParams.Count; ++i)
        {
            levelCount += (int)flowerParams[i].flowerLevel;
        }

        return levelCount;
    }
    //このセルの中の花のレベルの最大総数を計算
    public int GetMaxTotalFlowerLevel()
    {
        int levelCount = 0;

        for (int i = 0; i < flowerParams.Count; ++i)
        {
            levelCount += (int)GM_MathFlowerParam.EFlowerLevel.Level3;
        }

        return levelCount;
    }
}
