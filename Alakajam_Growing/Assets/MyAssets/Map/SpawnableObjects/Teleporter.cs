using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    public bool teleporterActive = false;

    public bool triggered = false;

    public bool escapeTriggered = false;
    public float escapeTimer = 30;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(escapeTriggered) {
            escapeTimer -= Time.deltaTime;

            GameObject.Find("PlayerShip").GetComponent<PlayerMovement>().textEnabled = false;
            GameObject.Find("PowerTimer").GetComponent<TextMesh>().text = "Congratulations" + '\n' + "You Escaped.";
            

            if(escapeTimer <= 0) {
                Application.Quit();
            }
        }
    }

    public void ActivateTeleporter () {
        teleporterActive = true;
        transform.localScale = 5 * Vector3.one;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player") && !triggered && teleporterActive) {
            triggered = true;
            escapeTriggered = true;
            Debug.Log("Escape");
        }
    }
}
