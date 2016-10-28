using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM_MathCell : MonoBehaviour {

    //孫オブジェクトから自らセットしに来る
    public List<GM_MathFlowerParam> flowerParams;
    //マネージャオブジェクト
    public GM_MathManager manager;


	//初回のみ
	void Awake () {
        //親の親オブジェクトにMathManagerがあるので保存
        manager = transform.parent.parent.GetComponent<GM_MathManager>();

	    //親の親オブジェクトにMathManagerがあるのでそこに自らをセット
        manager.cells.Add(this);
	}
	
    //初期化
    public void Init()
    {
        //各パラメーターを初期化
        for (int i = 0; i < flowerParams.Count; ++i)
        {
            flowerParams[i].Init();
        }
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
}
