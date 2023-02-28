using UnityEngine;
using System.Collections;

public class kniferGoblinBehaviourScript : creatureBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
    protected override void initialiseStats()
    {
        power = 3;
        agility = 2;
        resilliance = 1;
        thought = 0;
        spirit = 0;

        experianceValue = 600;

        activeDistance = 10.0f;
        detectedDistance = 4.0f;
        attackDistance = 1.0f;
    }
    // Update is called once per frame
    void Update () {
	
	}
}
