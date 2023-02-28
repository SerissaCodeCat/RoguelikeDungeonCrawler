using UnityEngine;
using System.Collections;

public class koboldBehavior : creatureBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    protected override void initialiseStats()
    {
        power = 1;
        agility = 3;
        resilliance = 1;
        thought = 1;
        spirit = 1;

        experianceValue = 500;

        activeDistance = 6.0f;
        detectedDistance = 4.0f;
        attackDistance = 1.0f;
    }

    // Update is called once per frame
    void Update ()
    {
	}
}
