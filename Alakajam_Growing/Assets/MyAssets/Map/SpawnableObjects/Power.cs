using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour {


    public bool triggered = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player") && !triggered) {
            triggered = true;
            GameObject.Find("PlayerShip").GetComponent<PlayerMovement>().energy += 90 + 30 * Random.value;
            Destroy(gameObject);
        }
    }
}
