using UnityEngine;
using System.Collections;

public class ArrowMove : MonoBehaviour {

    public GameObject LeftArrow;
    public GameObject RightArrow;
    private float speed;
    private float length;
	// Use this for initialization
	void Start ()
    {
        Init();
        LeftArrow.SetActive(true);
        RightArrow.SetActive(true);
    }

	void Init()
    { 
        speed = 0.1f;
        length = 0.2f;
    }
	// Update is called once per frame
	void Update () {
        LeftArrow.transform.position = new Vector3(LeftArrow.transform.position.x - Mathf.Sin(Time.frameCount * speed) * length, LeftArrow.transform.position.y, LeftArrow.transform.position.z);
        RightArrow.transform.position = new Vector3(RightArrow.transform.position.x + Mathf.Sin(Time.frameCount * speed) * length, RightArrow.transform.position.y, RightArrow.transform.position.z);
    }
}
