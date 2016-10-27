using UnityEngine;
using System.Collections;

public class GM_MathFlowerParam : MonoBehaviour {

    public enum EFlowerLevel
    {
        Level0, //種まき前
        Level1, //植物成長中
        Level2, //成長完了
        Level3, //着色済み
    };
    public enum EFlowerColor
    {
        NONE,
        RED,
        GREEN,
        BLUE,
        CYAN,
        MAGENTA,
        YELLOW,
    };

    public EFlowerLevel flowerLevel;
    public EFlowerColor flowerColor;

	// 初期化関数
	public void Init () {
        flowerLevel = EFlowerLevel.Level0;
        flowerColor = EFlowerColor.NONE;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
