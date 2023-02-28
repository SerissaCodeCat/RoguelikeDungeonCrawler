using UnityEngine;
using System.Collections;

public class ratBehaviourScript : creatureBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
    protected override void initialiseStats()
    {
        power = 1;
        agility = 0;
        resilliance = 1;
        thought = 0;
        spirit = 0;

        experianceValue = 200;

        activeDistance = 6.0f;
        detectedDistance = 4.0f;
        attackDistance = 1.0f;
    }
    // Update is called once per frame
    void Update () {
	
	}
}
