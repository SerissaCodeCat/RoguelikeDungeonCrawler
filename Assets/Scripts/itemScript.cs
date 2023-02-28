using UnityEngine;
using System.Collections;

public class itemScript : MonoBehaviour {
    protected GameObject playerRef;
    protected PlayerControls playerScriptRef;

    // Use this for initialization
    void Start()
    {

    }

    protected void Awake()
    {
        playerRef = GameObject.Find("Player");
        playerScriptRef = (PlayerControls)playerRef.GetComponent(typeof(PlayerControls));
    }

    // Update is called once per frame
    protected void Update()
    {
    }
    virtual public void itemAction()
    {
    }
}