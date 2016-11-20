using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour {

    public static GameObject[] objectList = new GameObject[2000];
    public static Renderer[] rendererList= new Renderer[2000];
    public static GM_MathFlowerParam.EFlowerColor[] colorList = new GM_MathFlowerParam.EFlowerColor[2000];
    private static int Id = 0;
    private static GameObject prefabFlower;     // 花のプレハブ
    private static GameObject prefabMiddleBil;  // 中ビルのプレハブ
    public Material[] mMaterialsRed;
    public Material[] mMaterialsBlue;
    public Material[] mMaterialsGreen;
    public Material mMaterialsYellow;
    public Material mMaterialsCyan;
    public Material mMaterialsMagenta;
    public Material mMaterialsWhite;
    public Material[] mMaterialsMiddleBill;
    public static Material[] gMaterialsRed;
    public static Material[] gMaterialsBlue;
    public static Material[] gMaterialsGreen;
    public static Material gMaterialsYellow;
    public static Material gMaterialsCyan;
    public static Material gMaterialsMagenta;
    public static Material gMaterialsWhite;
    public static Material[] gMaterialsMiddleBill;
    
	// Use this for initialization
	void Awake () {
        prefabFlower = (GameObject)Resources.Load("Prefabs/Flower");
        prefabMiddleBil = (GameObject)Resources.Load("Prefabs/MiddleBill");
        gMaterialsRed = mMaterialsRed;
        gMaterialsBlue = mMaterialsBlue;
        gMaterialsGreen = mMaterialsGreen;
        gMaterialsYellow = mMaterialsYellow;
        gMaterialsCyan = mMaterialsCyan;
        gMaterialsMagenta = mMaterialsMagenta;
        gMaterialsWhite = mMaterialsWhite;
        gMaterialsMiddleBill = mMaterialsMiddleBill;
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
            // 花生成
            case GM_MathFlowerParam.EFlowerType.Flower1:
                // プレハブインスタンス
                Vector3 pos = new Vector3(_position.x, _position.y + 0.5f, _position.z);
                objectList[Id] = Instantiate(prefabFlower, pos, Quaternion.Euler(60, 180, 0)) as GameObject;
                rendererList[Id] = objectList[Id].GetComponent<Renderer>();
                colorList[Id] = _color;
                objectList[Id].GetComponent<flower>().Init();
                objectList[Id].GetComponent<flower>().scallOn();
                break;

            // ビル生成
            case GM_MathFlowerParam.EFlowerType.Bill:
                // プレハブインスタンス
                Vector3 pos_ = new Vector3(_position.x+6.44f, _position.y, _position.z);
                objectList[Id] = Instantiate(prefabMiddleBil, pos_, Quaternion.Euler(0, 0, 0)) as GameObject;
                rendererList[Id] = objectList[Id].transform.FindChild("pCube20").GetComponent<Renderer>();
                Debug.Log("make a bill");
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
                    objectList[no].GetComponent<flower>().scallOn();
                    //objectList[no].GetComponent<Renderer>().material = gMaterials[ (int)colorList[Id], level-1];
                    break;

                    // ビル
                case GM_MathFlowerParam.EFlowerType.Bill:
                    rendererList[no].material = gMaterialsMiddleBill[level - 2];
                    break;
            }
        }
       
    }

    //---------------------------------------------------------------
    // 色変更処理
    //--------------------------------------------------------------
    public static void ChengeColor(int no, GM_MathFlowerParam.EFlowerColor _color){
        if (objectList[no] != null){
            switch (_color)
            {
                case GM_MathFlowerParam.EFlowerColor.RED:
                    rendererList[no].material = gMaterialsRed[1];
                    break;
                case GM_MathFlowerParam.EFlowerColor.BLUE:
                    rendererList[no].material = gMaterialsBlue[1];
                    break;
                case GM_MathFlowerParam.EFlowerColor.GREEN:
                    rendererList[no].material = gMaterialsGreen[1];
                    break;
                case GM_MathFlowerParam.EFlowerColor.YELLOW:
                    rendererList[no].material = gMaterialsYellow;
                    break;
                case GM_MathFlowerParam.EFlowerColor.CYAN:
                    rendererList[no].material = gMaterialsCyan;
                    break;
                case GM_MathFlowerParam.EFlowerColor.MAGENTA:
                    rendererList[no].material = gMaterialsMagenta;
                    break;
                case GM_MathFlowerParam.EFlowerColor.WHITE:
                    rendererList[no].material = gMaterialsWhite;
                    break;
            }

        }
    }
}
