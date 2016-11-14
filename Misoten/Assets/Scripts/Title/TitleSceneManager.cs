using UnityEngine;
using System.Collections;

public class TitleSceneManager : MonoBehaviour {

    public GameObject Camera;
    public GameObject Butterfly;
	public void Init()
    {
        Camera.GetComponent<CameraMove>().Init();
        Butterfly.GetComponent<ButterflyMove>().Init();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
