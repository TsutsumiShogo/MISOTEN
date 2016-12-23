using UnityEngine;
using System.Collections;

//=====================================================
// GM_StatixParam.cs
// ゲーム中どこからでも参照できるパラメータ
//=====================================================

public class GM_StaticParam : MonoBehaviour {



    public static int[] g_selectCharacter = new int[3]; //　選択したキャラを保持

    public static int g_titleStartStep = 0;

	void Awake(){
        g_titleStartStep = 0;
    }
	
	

}
