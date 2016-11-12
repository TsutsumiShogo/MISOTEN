using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour {

    public static GameObject[] objectList = new GameObject[2000];
    public static Renderer[] rendererList= new Renderer[2000];
    public static GM_MathFlowerParam.EFlowerColor[] colorList = new GM_MathFlowerParam.EFlowerColor[2000];
    private static int Id = 0;
    private static GameObject prefab;
    public Material[] mMaterialsRed;
    public Material[] mMaterialsBlue;
    public Material[] mMaterialsGreen;
    public static Material[] gMaterialsRed;
    public static Material[] gMaterialsBlue;
    public static Material[] gMaterialsGreen;
    
	// Use this for initialization
	void Start () {
        prefab = (GameObject)Resources.Load("Prefabs/Flower");
        gMaterialsRed = mMaterialsRed;
        gMaterialsBlue = mMaterialsBlue;
        gMaterialsGreen = mMaterialsGreen;
        //gMaterials = mMaterials;
        Id = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    //---------------------------------------------------------------
    // オブジェクト生成関数
    //---------------------------------------------------------------
    public static int CreateObj( 
        Vector3 _position,                          // 座標
        GM_MathFlowerParam.EFlowerType _type,       // オブジェクトタイプ
        GM_MathFlowerParam.EFlowerColor _color)     // 花の色(花以外はNONE)
    {
       
        switch (_type)
        {
            // 花
            case GM_MathFlowerParam.EFlowerType.Flower1:
                // プレハブインスタンス
                Vector3 pos = new Vector3(_position.x, _position.y + 0.5f, _position.z);
                objectList[Id] = Instantiate(prefab, pos, Quaternion.Euler(60, 180, 0)) as GameObject;
                rendererList[Id] = objectList[Id].GetComponent<Renderer>();
                colorList[Id] = _color;
                break;
        }

       
        
        return Id++;
    }
    //---------------------------------------------------------------
    // レベルアップ処理
    //---------------------------------------------------------------
    public static void LevelUp( int no, int level, GM_MathFlowerParam.EFlowerType _type )
    {
        if (objectList[no] != null){
            // オブジェクトタイプを判定
            switch (_type)
            {
                // 花
                case GM_MathFlowerParam.EFlowerType.Flower1:
                    //　色分け
                    switch (colorList[no])
                    {
                            // 赤
                        case GM_MathFlowerParam.EFlowerColor.RED:
                            rendererList[no].material = gMaterialsRed[level-2];
                            break;
                            // 青
                        case GM_MathFlowerParam.EFlowerColor.BLUE:
                            rendererList[no].material = gMaterialsBlue[level - 2];
                            break;
                            // 緑
                        case GM_MathFlowerParam.EFlowerColor.GREEN:
                            rendererList[no].material = gMaterialsGreen[level - 2];
                            break;
                    }
                    //objectList[no].GetComponent<Renderer>().material = gMaterials[ (int)colorList[Id], level-1];
                    break;
            }
        }
    }
}
