using UnityEngine;
using System.Collections;

public class GM_UIManager : MonoBehaviour {

    [SerializeField]
    private GM_UIFadeUnit fadeUnit;
    [SerializeField]
    private GM_UIMissionAnnounce missionAnnounce;
    [SerializeField]
    private GM_UIGreenPercent greenScore;

    //初期化を他のUIオブジェクトへ伝達
    public void Init()
    {
        fadeUnit.Init();
        missionAnnounce.Init();
        greenScore.Init();
    }
	
}
