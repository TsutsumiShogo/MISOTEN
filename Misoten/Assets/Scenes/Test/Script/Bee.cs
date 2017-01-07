using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;


public class Bee : MonoBehaviour
{
    
    public Transform player;    //プレイヤーを代入
    public float speed = 3; //移動速度
    private bool m_hitFlg = false;
    private const float WATI_TIME = 5.0f;   // 蜂の追跡判定エリアを縮小
    public float m_oldTime = 0;
    public float m_nowTime = 0;
    private float m_defaultSize = 4;
    private float m_smallSize = 1;

    private int m_currentRoot;

    [SerializeField]
    private GameObject[] m_wayPoints = new GameObject[4];

    // Use this for initialization
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        m_currentRoot = 0;
    }
    // Update is called once per frame

    void Update()
    {
        Vector3 pos = m_wayPoints[m_currentRoot].transform.position;

        if(Vector3.Distance(transform.position, pos) < 5.0f){
            Debug.Log("ポイント到達");
            m_currentRoot = (m_currentRoot < m_wayPoints.Length - 1) ? m_currentRoot + 1 : 0;
        }

        GetComponent<NavMeshAgent>().SetDestination(pos);
        

        if (player != null){
            Vector3 playerPos = player.position;    //プレイヤーの位置
            Vector3 direction = playerPos - transform.position; //方向
            direction = direction.normalized;   //単位化（距離要素を取り除く）
            transform.position = transform.position + (direction * speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            transform.LookAt(player);   //プレイヤーの方を向く
            //GetComponent<NavMeshAgent>().Stop();
            GetComponent<NavMeshAgent>().updatePosition = false;
            GetComponent<NavMeshAgent>().nextPosition = transform.position;
        }
        else
        {
            GetComponent<NavMeshAgent>().updatePosition = true;
        }
        player = null;

        if (m_hitFlg){
            m_nowTime += Time.deltaTime;
            if (WATI_TIME < m_nowTime){
                m_hitFlg = false;
                this.GetComponent<SphereCollider>().radius = m_defaultSize;
            }
        }
    }

    // 接触判定
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" )
        {
            player = other.transform;
        }
    }


   

    // 蜂に接触
    public void HitBee(){
        m_hitFlg = true;    // 接触Flag
        this.GetComponent<SphereCollider>().radius = m_smallSize;
       
        m_nowTime = Time.deltaTime;
    }
   
    
}