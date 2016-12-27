using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Ranking : MonoBehaviour {

    [SerializeField]
    private GameObject m_rankingList;   // ランキングリスト
    //===============================================================
    // 公開関数

    // Init - 初期化
    //---------------------------------
    // 
    public void Init(){
        transform.localPosition = new Vector3(1000, 0, 0);
        m_rankingList.GetComponent<RankList>().ResultInit();
    }
    // Set - 初期化
    //---------------------------------
    //
    public void Set()
    {
        m_rankingList.GetComponent<RankList>().SetRank();
    }
    // Action - 更新
    //---------------------------------
    // bool - 終了フラグ
    //
    public bool Action(){
        
        return m_rankingList.GetComponent<RankList>().ResultAction();
    }

    //===============================================================
    // 未公開関数


}
