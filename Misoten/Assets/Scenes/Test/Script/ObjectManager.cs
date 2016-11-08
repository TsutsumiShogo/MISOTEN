using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour {

    public static GameObject[] objectList;
    private static int Id;

	// Use this for initialization
	void Start () {
        Id = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // 
    public static int CreateObj(){

        return Id++;
    }
}
