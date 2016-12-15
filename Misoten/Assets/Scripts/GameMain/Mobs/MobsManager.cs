using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobsManager : MonoBehaviour {

    private enum MobType
    {
        Boy = 0,        // 男の子
        Ryman,          // サラリーマン
        Girl            // 女の子
    };

    // 半径
    [SerializeField]
    private float[] m_radius = new float[3];        // Unityから設定
    [SerializeField]
    private float[] m_childNum = new float[3];      // Mob追加

    // Boyプレハブ
    [SerializeField]
    private GameObject[] m_prefabBoy = new GameObject[3];
    // Rymanプレハブ
    [SerializeField]
    private GameObject[] m_prefabRyman = new GameObject[3];
    // Girl
    [SerializeField]
    private GameObject[] m_prefabGirl = new GameObject[3];

    private GameObject[] m_objList = new GameObject[500];
    private int m_objId = 0;
    private int m_stageNo = 0;
    private int m_cnt = 0;
    private bool m_moveFlg = false;
    //-----------------------------------------------------
    // 初期化
    //-----------------------------------------------------
    private void Awake(){
        //prefab = (GameObject)Resources.Load("Prefabs/GameMain/Boy_Ouen");
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
        for (m_objId = 0; m_objId < m_childNum[0]; m_objId++){
            switch (m_objId % 3)
            {
                case (int)MobType.Boy:
                    m_objList[m_objId] = Instantiate(m_prefabBoy[0], transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                    break;
                case (int)MobType.Girl:
                    m_objList[m_objId] = Instantiate(m_prefabGirl[0], transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                    break;
                case (int)MobType.Ryman:
                    m_objList[m_objId] = Instantiate(m_prefabRyman[0], transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                    break;
            }
            //m_objList[m_objId] = Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            m_objList[m_objId].transform.SetParent(this.transform);
        }
    }

    //-------------------------------------------
    // モブを配置
    private void SetMob(){

        //オブジェクト間の角度差
        float angleDiff = 360f / (float)m_childNum[m_stageNo];

        for (m_objId = 0; m_objId < m_childNum[m_stageNo]; m_objId++)
        {
           
            float angle = (90 - angleDiff * m_objId) * Mathf.Deg2Rad;
            if (m_objList[m_objId] != null)
            {
                Vector3 _position;
                _position.x = m_objList[m_objId].transform.position.x + m_radius[m_stageNo] * Mathf.Cos(angle);
                _position.y = 1.0f;
                _position.z = m_objList[m_objId].transform.position.z + m_radius[m_stageNo] * Mathf.Sin(angle);
                m_objList[m_objId].transform.position = _position;
                
                if (_position.x < 0){       // X座標で向きを変える
                    m_objList[m_objId].transform.rotation = Quaternion.Euler(0, 180, 0);
                }                
            }
        }
    }

    public void stageUpOn()
    {
        m_stageNo++;
        m_moveFlg = true;
        m_cnt = 50;
    }

    public void move(){

        if (m_cnt < 50)
        {
            //オブジェクト間の角度差
            float angleDiff = 360f / (float)m_childNum[m_stageNo];
            for (int i = 0; i < m_childNum[m_stageNo]; i++)
            {
                float angle = (90 - angleDiff * m_objId) * Mathf.Deg2Rad;
                if (m_objList[m_objId] != null)
                {
                    m_objList[m_objId].transform.position = new Vector3(
                        m_objList[m_objId].transform.position.x + (m_radius[m_stageNo]+ (m_cnt*0.1f)) * Mathf.Cos(angle),
                        1,
                        m_objList[m_objId].transform.position.z + (m_radius[m_stageNo] + (m_cnt * 0.1f)) * Mathf.Sin(angle)
                        );
                }
            }
        }
    }

}
