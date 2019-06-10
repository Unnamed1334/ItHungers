using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeoutDestroy : MonoBehaviour {

    public float timeout = .1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (timeout < 0) {
            Destroy(gameObject);
        }
        else {
            timeout -= Time.deltaTime;
        }
	}
}
