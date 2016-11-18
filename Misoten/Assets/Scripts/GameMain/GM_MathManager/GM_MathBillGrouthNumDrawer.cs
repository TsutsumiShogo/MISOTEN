using UnityEngine;
using System.Collections;

public class GM_MathBillGrouthNumDrawer : MonoBehaviour {

    //Unity上でセット
    public Material[] drawMaterials = new Material[4];

    //親のマスフラワースクリプト
    private GM_MathFlowerParam parentFlowerParam;

    //自身のシェーダー
    private MeshRenderer thisRender;

	// Use this for initialization
	void Awake () {
        parentFlowerParam = transform.parent.GetComponent<GM_MathFlowerParam>();
        thisRender = transform.GetComponent<MeshRenderer>();
	}

    void Update()
    {
        int growthNum = parentFlowerParam.GetGrowthNowPlayerNum();

        if (growthNum >= 0 && growthNum <= 3)
        {
            thisRender.material = drawMaterials[growthNum];
        }
    }
}
