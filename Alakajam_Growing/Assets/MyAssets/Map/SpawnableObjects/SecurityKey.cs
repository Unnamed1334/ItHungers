using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityKey : MonoBehaviour {

    public bool triggered = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other) {
        if(other.tag.Equals("Player") && !triggered) {
            triggered = true;
            MapManager.getInstance().UpdateSecurity(MapManager.getInstance().securitylevel + 1);
            Destroy(gameObject);
        }
    }
}
