using UnityEngine;
using System.Collections;

public class Bee : MonoBehaviour
{

    public Transform player;    //プレイヤーを代入
    public float speed = 3; //移動速度
    private bool m_hitFlg = false;
    // Use this for initialization
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 playerPos = player.position;    //プレイヤーの位置
            Vector3 direction = playerPos - transform.position; //方向
            direction = direction.normalized;   //単位化（距離要素を取り除く）
            transform.position = transform.position + (direction * speed * Time.deltaTime);
            transform.LookAt(player);   //プレイヤーの方を向く
        }
        player = null;
    }

    // 接触判定
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" )
        {
            player = other.transform;
        }
    }

   
    
}