using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopCloud : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        transform.Translate(-0.1f, 0, 0);
        if (transform.localPosition.x < -900.0f)
        {
            transform.localPosition = new Vector3(900.0f, 0, 0);
        }
    }
}
