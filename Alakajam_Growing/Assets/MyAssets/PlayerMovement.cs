using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Transform playerCamera;

    public float energy = 600;
    public TextMesh timeOutput;

    public float force = 15;

    public float speed = 7;

    private Rigidbody rb;

    public bool hasControl = false;
    public bool startupDone = false;
    public float startupTimer = 30;

    public bool textEnabled = true;

    public GameObject light;

    public List<Biomass> attached = new List<Biomass>();

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {
        if(textEnabled) {
            energy -= Time.deltaTime;
            int min = Mathf.FloorToInt(energy / 60);
            int sec = Mathf.FloorToInt(energy % 60);
            int mill = Mathf.FloorToInt((1000 * energy) % 1000);


            timeOutput.text = System.String.Format("{0:00}:{1:00}.{2:000}", min, sec, mill);


            //Take damage
            for (int i = 0; i < attached.Count; i++) {
                if (attached[i] != null) {
                    energy -= attached[i].biomass * Time.deltaTime;
                }
                else {
                    attached.RemoveAt(i);
                    i--;
                }
            }
            if (energy < 0) {
                light.SetActive(false);
                hasControl = false;
                timeOutput.text = "WARNING: HULL BREACH";
            }
            if (energy < 30) {
                OnDeath();
            }

            startupTimer -= Time.deltaTime;
            if (startupTimer > 10) {
                light.SetActive(false);
                timeOutput.text = "Startup in progress" + '\n' + "Time remaining: " + System.String.Format("{0:00}.{1:000}", Mathf.FloorToInt(startupTimer), Mathf.FloorToInt((1000 * startupTimer) % 1000));
            }
            else if (startupTimer > 5) {
                light.SetActive(true);
                timeOutput.text = "Startup in progress" + '\n' + "Time remaining: " + System.String.Format("{0:00}.{1:000}", Mathf.FloorToInt(startupTimer), Mathf.FloorToInt((1000 * startupTimer) % 1000));
            }
            else if (startupTimer > 0) {
                light.SetActive(true);
                timeOutput.text = "WARNING: Power is low" + '\n' + "Time remaining: " + System.String.Format("{0:00}.{1:000}", Mathf.FloorToInt(startupTimer), Mathf.FloorToInt((1000 * startupTimer) % 1000));
            }
            else if (!startupDone && startupTimer < 0) {
                startupDone = true;
                hasControl = true;
                rb.velocity = 10 * Vector3.up;
            }

        }
    }

    void FixedUpdate() {
        if(hasControl) {
            Vector3 displacement = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) {
                displacement += playerCamera.forward;
            }
            if (Input.GetKey(KeyCode.S)) {
                displacement -= playerCamera.forward;
            }
            if (Input.GetKey(KeyCode.A)) {
                displacement -= playerCamera.right;
            }
            if (Input.GetKey(KeyCode.D)) {
                displacement += playerCamera.right;
            }
            if (Input.GetKey(KeyCode.Q)) {
                displacement += Vector3.up;
            }
            if (Input.GetKey(KeyCode.Z)) {
                displacement += Vector3.down;
            }
            float distance = displacement.magnitude;

            rb.AddForce(-Physics.gravity);

            //Debug.Log(rb.velocity +","+ displacement);
            rb.velocity = Vector3.MoveTowards(rb.velocity, speed * displacement.normalized, Time.fixedDeltaTime * force / rb.mass);

            //Vector3 rawForce = displacement - 0.5f * rb.velocity;
            //rb.AddForce(force * rawForce.normalized, ForceMode.Force);
        }
    }


    public void OnAttach(Biomass hungry) {
        attached.Add(hungry);
    }


    public void OnDeath() {
        Application.Quit();
    }
}
