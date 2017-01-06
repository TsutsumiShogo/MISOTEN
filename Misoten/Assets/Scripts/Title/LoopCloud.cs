using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopCloud : MonoBehaviour {

    public float m_time = 20.0f;
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(
            transform.localPosition.x - 900.0f * Time.deltaTime / m_time,
            transform.localPosition.y,
            transform.localPosition.z);
        if (transform.localPosition.x <= -900.0f)
        {
            transform.localPosition = new Vector3(900.0f, 0, 0);
        }
    }
}
