using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ships/Scout")]
public class CoalitionScout : Ship {
    // Use this for initialization
    void Start () {
        attack = 10;
        health = 100;
        nodeSpeed = 1.0;

        //dummy var (for now)
        flightSpeed = 1;

        //weapons
        turretSlots = new Turret[1];
        cannonSlots = new Cannon[1];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}