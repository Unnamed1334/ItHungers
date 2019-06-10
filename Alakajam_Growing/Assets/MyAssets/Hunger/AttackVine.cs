using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackVine : MonoBehaviour {

    public float force = 10;
    public float maxDistance = 12;

    public Transform startPoint;
    public Rigidbody startRB;

    public Transform endPoint;
    public Rigidbody endRB;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {

    }

    void LateUpdate() {
        if(startPoint != null && endPoint != null) {
            transform.position = .5f * (startPoint.position + endPoint.position);
            transform.up = (startPoint.position - endPoint.position);
            transform.localScale = new Vector3(.25f, (startPoint.position - endPoint.position).magnitude / 2, .25f);
        }
        else {
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        if (startPoint != null && endPoint != null) {
            if (startRB != null) {
                startRB.AddForce(force * (endPoint.position - startPoint.position).normalized, ForceMode.Force);
            }
            if (endRB != null) {
                endRB.AddForce(force * (startPoint.position - endPoint.position).normalized, ForceMode.Force);
                }
            }
        else {
            Destroy(gameObject);
        }
    }

    public void OnDeath() {
        Destroy(gameObject);
    }
}
