using UnityEngine;
using System.Collections;

public class FloorScaling : MonoBehaviour {

    public int texturePixelSize = 64;
    public int dungeonX;
    public int dungeonY;
	// Use this for initialization
	void Start ()
    {
        GetComponent<Renderer>().material.mainTextureScale = new Vector3(dungeonX, dungeonY, 1);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
