using UnityEngine;
using System.Collections;

public class growBill : MonoBehaviour {

    public Material[] m_Materials;
    private float m_maxScall = 3.0f;
    private float m_maxUp = 2.8f;
    private float m_speed = 0.01f;
    private GameObject m_Cube;

    int subLv = 0;

	// Use this for initialization
	void Start () {
        //this.transform.localPosition = this.transform.root.transform.localPosition;  
        transform.localScale = new Vector3(transform.localScale.x, 0.0f, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, -0.2f, transform.position.z);
        m_Cube = GameObject.Find("pCube20");
	}
	
	// Update is called once per frame
	void Update () {
        if (XboxController.GetButtonHoldY(0)){
            if(subLv<2)GrowUp();
        }
	}

    //----------------------------------------------
    // 成長演出
    //----------------------------------------------
    void GrowUp(){
        Vector3 scall = transform.localScale;       // サイズを取得
        Vector3 pos = transform.position;           // 座標を取得

        // サイズ拡大
        if (scall.y < m_maxScall){
            scall.y += m_speed;
        }
        // 座標
        if (pos.y < m_maxUp){
            pos.y += m_speed;
        }

        transform.position = pos;
        transform.localScale = scall;

        if ((scall.y >= m_maxScall) && (pos.y >= m_maxUp)){
            subLv++;
            Init();
            LevelUp(subLv);
        }

    }
    //----------------------------------------------
    // サイズ初期化
    //----------------------------------------------
    void Init(){
        transform.localScale = new Vector3(transform.localScale.x, 0.0f, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, -0.2f, transform.position.z);
    }
    //----------------------------------------------
    // マテリアル変更
    //----------------------------------------------
    void LevelUp(int level){
        m_Cube.GetComponent<Renderer>().material = m_Materials[level];
        if (level == 2){
            m_Cube.GetComponent<Renderer>().materials[1] = m_Materials[level+1];
        }
    }
}
