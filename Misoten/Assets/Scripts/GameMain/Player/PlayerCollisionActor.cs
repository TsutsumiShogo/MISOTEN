using UnityEngine;
using System.Collections;

public class PlayerCollisionActor : MonoBehaviour {

    private PlayerUnit playerUnit;
    private PlayerControll playerControll;

	// Use this for initialization
	void Awake () {
        playerUnit = transform.GetComponent<PlayerUnit>();
        playerControll = transform.GetComponent<PlayerControll>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        //ハチとの接触だった場合
        if (col.gameObject.layer == LayerMask.NameToLayer("Bee"))
        {
            Vector3 _knockBackVec = Vector3.zero;
            _knockBackVec = transform.position - col.transform.position;
            _knockBackVec.y = 0;
            _knockBackVec.Normalize();
            _knockBackVec *= 30.0f;
            _knockBackVec.y = 0.0f;

            playerUnit.KnockBack(_knockBackVec);

            playerControll.SetNotMoveTime(0.5f);
        }
    }
}
