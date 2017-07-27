using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetUpLocalPlayer : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		if(isLocalPlayer) {
            GetComponent<PlayerController>().enabled = true;
            GetComponent<GravityShift>().enabled = true;
            transform.Find("MainCamera").GetComponent<Camera>().enabled = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
		

	}
}
