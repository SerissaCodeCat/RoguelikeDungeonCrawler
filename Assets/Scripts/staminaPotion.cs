﻿using UnityEngine;
using System.Collections;

public class staminaPotion : itemScript {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    public override void itemAction()
    {
        playerScriptRef.gainStaminaPotion();
    }
}
