using UnityEngine;
using System.Collections;

public class Bee : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public float s = .2f;

    void Update()
    {
        transform.position = new Vector3(transform.position.x
         , 2 + (0.5f * Mathf.Sin(Time.frameCount * s)), transform.position.z);
    }
}
