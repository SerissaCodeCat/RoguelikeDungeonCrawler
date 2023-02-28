using UnityEngine;
using System.Collections;

public class BlackRatBehaviourScript : creatureBehaviour
{

    // Use this for initialization
    void Start()
    {

    }
    protected override void initialiseStats()
    {
        power = 1;
        agility = 5;
        resilliance = 3;
        thought = 0;
        spirit = 0;

        experianceValue = 600;

        activeDistance = 6.0f;
        detectedDistance = 4.0f;
        attackDistance = 1.0f;
    }
    // Update is called once per frame
    void Update()
    {

    }
}