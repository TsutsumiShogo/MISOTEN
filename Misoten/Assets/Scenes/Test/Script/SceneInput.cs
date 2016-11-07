using UnityEngine;
using System.Collections;

public class SceneInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (XboxController.GetButtonA(0))
            Debug.Log("PushA");
        if (XboxController.GetButtonB(0))
            Debug.Log("PushB");
        if (XboxController.GetButtonX(0))
            Debug.Log("PushX");
        if (XboxController.GetButtonY(0))
            Debug.Log("PushY");
        if (XboxController.GetButtonL(0))
            Debug.Log("PushL");
        if (XboxController.GetButtonR(0))
            Debug.Log("PushR");

        if (XboxController.GetLeftTriggerUp(0))
            Debug.Log("Up");
        if (XboxController.GetLeftTriggerDown(0))
            Debug.Log("Down");
        if (XboxController.GetLeftTriggerLeft(0))
            Debug.Log("Left");
        if (XboxController.GetLeftTriggerRight(0))
            Debug.Log("Right");
	}

}
