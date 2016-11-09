using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour {

    public static GameObject[] objectList = new GameObject[1000];
    private static int Id = 0;
    private static GameObject prefab;
    public Material[] mMaterialsA;
    public static Material[] gMaterialsA;

	// Use this for initialization
	void Start () {
        prefab = (GameObject)Resources.Load("Prefabs/Flower");
        gMaterialsA = mMaterialsA;
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
        // プレハブインスタンス
        Vector3 pos = new Vector3(_position.x, _position.y + 0.5f, _position.z);
        objectList[Id] = Instantiate(prefab, pos, Quaternion.Euler(60, 180, 0)) as GameObject;
        
        return Id++;
    }
    //---------------------------------------------------------------
    // レベルアップ処理
    //---------------------------------------------------------------
    public static void LevelUp( int no, int level, GM_MathFlowerParam.EFlowerType _type )
    {
        if (objectList[no] != null){
            switch (_type)
            {
                case GM_MathFlowerParam.EFlowerType.Flower1:
                    objectList[no].GetComponent<Renderer>().material = gMaterialsA[level-1];
                    break;
            }
        }
    }
}
