using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsRestore : MonoBehaviour {

    public Vector3 target;

    public float force = 15;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        //target = transform.position;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate() {
        Vector3 current = transform.position;
        Vector3 displacement = target - current;
        float distance = displacement.magnitude;

        rb.AddForce(-Physics.gravity);

        if (rb.velocity.magnitude > 5) {
            Vector3 rawForce = -rb.velocity;
            rb.AddForce(force * rawForce.normalized, ForceMode.Force);

        }
        else if (distance > .25f) {
            Vector3 rawForce = displacement - 0.5f * rb.velocity;
            rb.AddForce(force * rawForce.normalized, ForceMode.Force);
        }

    }
}
