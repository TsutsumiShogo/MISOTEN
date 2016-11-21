using UnityEngine;
using System.Collections;

public class GM_UIManager : MonoBehaviour {

    [SerializeField]
    private GM_UIFadeUnit fadeUnit;

    //初期化を他のUIオブジェクトへ伝達
    public void Init()
    {
        fadeUnit.Init();
    }
	
}
