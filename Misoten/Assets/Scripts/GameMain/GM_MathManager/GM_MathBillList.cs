using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM_MathBillList : MonoBehaviour {

    private GM_MathManager mathManager;

    //ビルリスト
    public List<GM_MathFlowerParam> billList;

    //成長中のビル一覧
    public List<GM_MathFlowerParam> growingBillList;

    void Awake()
    {
        mathManager = transform.GetComponent<GM_MathManager>();
    }

    void Update()
    {
        //成長中ビルリストの内容を削除
        growingBillList.Clear();

        //全てのビルを検索
        for (int i = 0; i < billList.Count; ++i)
        {
            //成長中かどうか確認
            if (billList[i].GetGrowthNowPlayerNum() > 0)
            {
                //成長中ビルリストに追加
                growingBillList.Add(billList[i]);
            }
        }
    }

    //リストを作成する
    public void MakeBillList()
    {
        //今あるリストの内容を全て削除
        billList.Clear();

        //全ての花パラメーターを検索
        for(int i=0; i<mathManager.cells.Count; ++i){
            for(int j=0; j<mathManager.cells[i].flowerParams.Count; ++j){
                
                //ビルを発見
                if(mathManager.cells[i].flowerParams[j].flowerType == GM_MathFlowerParam.EFlowerType.Bill ||
                    mathManager.cells[i].flowerParams[j].flowerType == GM_MathFlowerParam.EFlowerType.BigBill)
                {
                    //ビルリストに追加
                    billList.Add(mathManager.cells[i].flowerParams[j]);
                }
            }
        }

    }
}
