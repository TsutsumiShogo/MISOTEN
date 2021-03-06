﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**************************************************************
 * GM_MathMath.cs
 *   ミニマップでマス単位で管理する必要が出てきたので作成。
 *   情報読み取り専用
**************************************************************/
public class GM_MathMath : MonoBehaviour {
    //親のセル
    public GM_MathCell parentCell;   //親がセットしに来る
    //子の花リスト
    public List<GM_MathFlowerParam> flowerParams = new List<GM_MathFlowerParam>();


    //花リストに追加
    public void AddFlowerParam(GM_MathFlowerParam _obj)
    {
        flowerParams.Add(_obj);
    }

    //マス内の最小レベルを計算して返す
    public GM_MathFlowerParam.EFlowerLevel GetMinMathLevel()
    {
        GM_MathFlowerParam.EFlowerLevel mathLevel = GM_MathFlowerParam.EFlowerLevel.Level3;   //自身の子の花(ビル)の最低レベルをセットする

        for (int i = 0; i < flowerParams.Count; ++i)
        {
            if ((int)mathLevel > (int)flowerParams[i].flowerLevel)
            {
                mathLevel = flowerParams[i].flowerLevel;
            }
        }

        return mathLevel;
    }
}
