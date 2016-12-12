using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour {

    public static GameObject[] objectList = new GameObject[3000];
    public static Renderer[] rendererList= new Renderer[3000];
    public static GM_MathFlowerParam.EFlowerColor[] colorList = new GM_MathFlowerParam.EFlowerColor[2000];
    private static int Id = 0;
    private static GameObject prefabFlower;     // 花のプレハブ
    private static GameObject prefabHouse;      // 家のプレハブ
    private static GameObject prefabMiddleBil;  // 中ビルのプレハブ
    private int m_actionId = 0;                 // アクション数
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
    private int m_pointNum = 0;
    private int[] m_processingPoint = new int[3];
    private float m_oldtime = 0;
	// Use this for initialization
	void Awake () {
        
        prefabFlower = (GameObject)Resources.Load("Prefabs/GameMain/Flower");
        prefabHouse = (GameObject)Resources.Load("Prefabs/GameMain/House");
        prefabMiddleBil = (GameObject)Resources.Load("Prefabs/GameMain/MiddleBill");
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
        m_pointNum = 0;
        m_actionId = 0;
        // 処理を三回に分けて行う
        m_processingPoint[0] = 600;
        m_processingPoint[1] = 1300;
        m_processingPoint[2] = 2000;
	}

    void start()
    {
        
    }

    // object一括更新処理
	// Update is called once per frame
	void Update () {
      
        if (true)
        {
            m_oldtime = Time.deltaTime;
            for (; m_actionId < m_processingPoint[m_pointNum]; m_actionId++)
            {
                if (objectList[m_actionId] != null)
                {
                    ObjectParam _obj = objectList[m_actionId].GetComponent<ObjectParam>();
                    switch (_obj.m_type)
                    {
                        // 花の更新処理
                        case GM_MathFlowerParam.EFlowerType.Flower1:
                            _obj.flowerUpdate();
                            break;
                        // 中ビルの更新処理
                        case GM_MathFlowerParam.EFlowerType.Bill:
                            _obj.mibbleBillUpdate();
                            break;
                    }
                }
            }
            m_pointNum++;
            if (m_pointNum > 2)
            {
                m_pointNum = 0;
                m_actionId = 0;
            }
        }
	}

    //---------------------------------------------------------------
    // オブジェクト生成関数
    //---------------------------------------------------------------
    public static int CreateObj( 
        Vector3 _position,                          // 座標
        GM_MathFlowerParam.EFlowerType _type,       // オブジェクトタイプ
        GM_MathFlowerParam.EFlowerColor _color,
        GM_MathFlowerParam _param)     // 花の色(花以外はNONE)
    {
       
        switch (_type)
        {
            // 花生成
            case GM_MathFlowerParam.EFlowerType.Flower1:
                // プレハブインスタンス
                Vector3 pos = new Vector3(_position.x, _position.y + 0.5f, _position.z);
                objectList[Id] = Instantiate(prefabFlower, pos, Quaternion.Euler(15, 0, 0)) as GameObject;
                rendererList[Id] = objectList[Id].GetComponent<Renderer>();
                colorList[Id] = _color;
                objectList[Id].GetComponent<ObjectParam>().FlowerInit();
                objectList[Id].GetComponent<ObjectParam>().scallOn();
                objectList[Id].GetComponent<ObjectParam>().SetParam(_param);
                SoundManager.PlaySe("flower_spone",1);        // 花SE
                break;
                
                // 家
            case GM_MathFlowerParam.EFlowerType.House:
                // プレハブインスタンス
                Vector3 pos_h = new Vector3(_position.x + 6.44f, _position.y, _position.z);
                objectList[Id] = Instantiate(prefabHouse, pos_h, Quaternion.Euler(0, 0, 0)) as GameObject;
                rendererList[Id] = objectList[Id].transform.FindChild("pCube26").GetComponent<Renderer>();
                objectList[Id].GetComponent<ObjectParam>().HouseInit();
                break;

            // ビル生成
            case GM_MathFlowerParam.EFlowerType.Bill:
                // プレハブインスタンス
                Vector3 pos_ = new Vector3(_position.x+6.44f, _position.y, _position.z);
                objectList[Id] = Instantiate(prefabMiddleBil, pos_, Quaternion.Euler(0, 0, 0)) as GameObject;
                rendererList[Id] = objectList[Id].transform.FindChild("pCube20").GetComponent<Renderer>();
                objectList[Id].GetComponent<ObjectParam>().MiddleBillInit();
                objectList[Id].GetComponent<ObjectParam>().SetParam(_param);
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
                    if (level >= 3) objectList[no].GetComponent<ObjectParam>().startFlowerParticle();
                    objectList[no].GetComponent<ObjectParam>().scallOn();
                    break;

                    // 家
                case GM_MathFlowerParam.EFlowerType.House:
                    rendererList[no].material = gMaterialsMiddleBill[level - 2];
                    break;

                    // 中ビル
                case GM_MathFlowerParam.EFlowerType.Bill:
                    SoundManager.PlaySe("cheer",3);       // 歓声
                    objectList[no].GetComponent<ObjectParam>().LevelUpEff();     // レベルアップ時エフェクト
                    rendererList[no].material = gMaterialsMiddleBill[level - 2];
                    if (level == 3){
                        rendererList[no].materials = new Material[2]{
                            rendererList[no].materials[0],
                            gMaterialsMiddleBill[level - 1]
                        };
                    }
                    break;
            }
        }
       
    }

    //---------------------------------------------------------------
    // 色変更処理
    //---------------------------------------------------------------
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

    //---------------------------------------------------------------
    // オブジェクト消去
    //--------------------------------------------------------------- 
    public static void Clean(){
        for (int i = 0; i < 10; i++){
            if (objectList[i] != null){
                Destroy( objectList[i] );   // オブジェクト破棄
            }
        }
        Id = 0;
    }
}
