using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public float health = 100;

    public bool validTarget = false;
    public float validtimer = 0;

    public Vector3 target;
    public float distance;
    public GameObject projectile;

    public float range = 5;
    public float attempts = 5;

    public float timer = 0;
    public float firerate = .5f;

    public bool drawTestRays = false;

    public List<Biomass> attached = new List<Biomass>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Try a random point
        Ray testRay;
        RaycastHit hit;

        // Random scan
        for (int i = 0; i < 15; i++) {
            testRay = new Ray(transform.position, Random.insideUnitSphere);
            if (Physics.Raycast(testRay, out hit, range)) {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Growth")) {
                    //Was the old cosest destroyed
                    if (!validTarget) {
                        validTarget = true;
                        target = hit.point;
                    }
                    else {
                        float newDistance = (transform.position - hit.point).magnitude;
                        if (newDistance < distance) {
                            target = hit.point;
                            distance = newDistance;
                        }
                    }
                }
                if (drawTestRays) {
                    drawLine(transform.position, hit.point, Color.blue);
                }
            }
            else if (drawTestRays) {
                drawLine(transform.position, testRay.GetPoint(range), Color.blue);
            }
        }

        // Center of mass scan
        Collider[] nearbyGrowth = Physics.OverlapSphere(transform.position, range, LayerMask.NameToLayer("Growth"));
        for (int i = 0; i < 10; i++) {
            if (nearbyGrowth.Length > 0) {
                int randomGrowth = Random.Range(0, nearbyGrowth.Length);
                testRay = new Ray(transform.position, nearbyGrowth[randomGrowth].transform.position - transform.position);
                if (Physics.Raycast(testRay, out hit, range)) {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Growth")) {
                        //Was the old cosest destroyed
                        if (!validTarget) {
                            validTarget = true;
                            target = hit.point;
                        }
                        else {
                            float newDistance = (transform.position - hit.point).magnitude;
                            if (newDistance < distance) {
                                target = hit.point;
                                distance = newDistance;
                            }
                        }
                    }
                    if (drawTestRays) {
                        drawLine(transform.position, hit.point, Color.blue);
                    }
                }
                else if (drawTestRays) {
                    drawLine(transform.position, testRay.GetPoint(range), Color.blue);
                }
            }
        }

        // Near Last target
        // Check within 1 meter of the last valid target
        for (int i = 0; i < 15 && !validTarget; i++) {
            testRay = new Ray(transform.position, target + (1 + validtimer) * Random.insideUnitSphere - transform.position);
            if (Physics.Raycast(testRay, out hit, range)) {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Growth")) {
                    //Was the old cosest destroyed
                    if (!validTarget) {
                        validTarget = true;
                        target = hit.point;
                    }
                    else {
                        float newDistance = (transform.position - hit.point).magnitude;
                        if (newDistance < distance) {
                            target = hit.point;
                            distance = newDistance;
                        }
                    }
                }
                if (drawTestRays) {
                    drawLine(transform.position, hit.point, Color.yellow);
                }
            }
            else if (drawTestRays) {
                drawLine(transform.position, testRay.GetPoint(range), Color.yellow);
            }
        }

        // Keep a time of how long it has been since finding a valid target
        // Used to be less focued as time passes
        if (!validTarget) {
            validtimer += Time.deltaTime;
        }
        else {
            validtimer = 0;
        }

        if (timer <= 0 && validTarget) {
            timer = firerate;

            testRay = new Ray(transform.position, target - transform.position);

            if (Physics.Raycast(testRay, out hit, range)) {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Growth")) {
                    hit.collider.SendMessage("OnDeath");
                    drawLine(transform.position, hit.point, Color.red);
                }
                else {
                    validTarget = false;
                }
            }
            else {
                validTarget = false;
            }
        }   
        else {
            timer -= Time.deltaTime;
        }

        //Take damage
        for(int i = 0; i < attached.Count; i++) {
            if(attached[i] != null) {
                health -= attached[i].biomass * Time.deltaTime;
            }
            else {
                attached.RemoveAt(i);
                i--;
            }
        }
        if(health < 0) {
            OnDeath();
        }
        

    }

    private void drawLine(Vector3 start, Vector3 end, Color color) {
        GameObject newInst = Instantiate(projectile);
        LineRenderer line = newInst.GetComponent<LineRenderer>();
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        line.startColor = color;
        line.endColor = color;
    }

    public void OnAttach(Biomass hungry ) {
        attached.Add(hungry);
    }

    public void OnDeath() {
        Destroy(gameObject);
    }
}
