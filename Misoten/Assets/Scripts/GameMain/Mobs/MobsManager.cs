using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobsManager : MonoBehaviour {

    // 半径
    [SerializeField]
    private float m_radius;     //Unityから設定
    private float m_childNum = 30;     // モブの数
    private GameObject prefab;
    private GameObject[] m_objList = new GameObject[300];
    private int m_objId = 0;
    //-----------------------------------------------------
    // 初期化
    //-----------------------------------------------------
    private void Awake(){
        prefab = (GameObject)Resources.Load("Prefabs/GameMain/Boy");
        CreateMobs();
        SetMob();
    }
    
    //Inspectorの内容(半径)が変更された時に実行
    private void OnValidate (){
        SetMob();
    }

    //-------------------------------------------
    //  モブ生成
    private void CreateMobs()
    {
        for (m_objId = 0; m_objId < m_childNum; m_objId++){
            m_objList[m_objId] = Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            m_objList[m_objId].transform.SetParent(this.transform);
        }
    }

    //-------------------------------------------
    // モブを配置
    private void SetMob(){

        //オブジェクト間の角度差
        float angleDiff = 360f / (float)m_childNum;

        for( m_objId = 0; m_objId <m_childNum; m_objId++ ){
           
            float angle = (90 - angleDiff * m_objId) * Mathf.Deg2Rad;
            if (m_objList[m_objId] != null)
            {
                m_objList[m_objId].transform.position = new Vector3(
                    m_objList[m_objId].transform.position.x + m_radius * Mathf.Cos(angle),
                    1,
                    m_objList[m_objId].transform.position.z + m_radius * Mathf.Sin(angle)
                    );
            }
        }
    }

}
