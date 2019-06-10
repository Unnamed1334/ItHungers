using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pionter : MonoBehaviour {

    public Vector3 target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.forward = target - transform.position;
	}
}
