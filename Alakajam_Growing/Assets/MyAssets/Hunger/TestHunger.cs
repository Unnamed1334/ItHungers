using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHunger : MonoBehaviour {

    public LayerMask growthLayer;
    public LayerMask solidLayer;
    public LayerMask playerLayer;
    private GrowthManager growthManager;
    public GameObject projectile;

    public float size = 1;

    public float freeSpace = 1;
    public float range = 5;
    public float attempts = 5;

    public float timer = 2;
    public float age = 0;
    public float ageScale = 2;
    public float ageMax = 120;

    // Time to try to place a new node
    public float growthtime = .5f;

    public List<GameObject> vines = new List<GameObject>();

    private Biomass bm;

    // Use this for initialization
    void Start () {
        growthManager = GrowthManager.getInstance();
        bm = GetComponent<Biomass>();
    }
	
	// Update is called once per frame
	void Update () {
		if(timer <= 0) {
            timer = growthtime * (1 + Random.value);

            bool foundPlacement = false;

            // Small attept at optimization
            int atteptsMod = 0;
            if (age > 120) {
                atteptsMod = 3;
            }
            else if (age > 60) {
                atteptsMod = 2;
            }
            else if (age > 30) {
                atteptsMod = 1;
            }

            for (int i = 0; i < attempts - atteptsMod; i++) {
                Ray testRay = new Ray(transform.position + .5f * size * Random.onUnitSphere, Random.insideUnitSphere);
                // Ensure the start point is not in a wall
                if(Physics.OverlapSphere(testRay.origin, .1f, solidLayer).Length == 0) {
                    RaycastHit hit;
                    if (Physics.Raycast(testRay, out hit, range)) {
                        if (AtteptPlacement(hit)) {
                            foundPlacement = true;
                            break;
                        }
                    }
                }
            }
            if(!foundPlacement) {
                Collider[] playerBuildings = Physics.OverlapSphere(transform.position, range, playerLayer);
                if (playerBuildings.Length > 0) {
                    int randomBuilding = Random.Range(0, playerBuildings.Length);
                    Ray testRay = new Ray(transform.position, playerBuildings[randomBuilding].transform.position - transform.position);
                    RaycastHit hit;
                    //drawLine(testRay.origin, testRay.GetPoint(range), Color.magenta);
                    if (Physics.Raycast(testRay, out hit, range)) {
                        if (AtteptPlacement(hit)) {
                            foundPlacement = true;
                        }
                    }
                }
            }
        }
        else {
            timer -= growthManager.growthModifier * Time.deltaTime;
        }

        if(age < ageMax) {
            age += growthManager.growthModifier * Time.deltaTime;
            size = .25f + ageScale * Mathf.Sqrt(age);
            bm.biomass = 5 * size;
            transform.localScale = size * Vector3.one;
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

    private bool AtteptPlacement (RaycastHit hit) {
        Rigidbody hitRB = null;
        Transform currentRBCheck = hit.collider.transform;
        while (currentRBCheck != null && hitRB == null) {
            hitRB = currentRBCheck.GetComponent<Rigidbody>();
            if (hitRB == null) {
                currentRBCheck = currentRBCheck.transform.parent;
            }
        }

        GameObject newVine = null;
        if (hitRB != null) {
            //Make a physics vine
            newVine = Instantiate(growthManager.attackVinePrefab);
            AttackVine vinescript = newVine.GetComponent<AttackVine>();
            vinescript.startPoint = transform;
            vinescript.endPoint = hitRB.transform;
            vinescript.endRB = hitRB;

            hitRB.SendMessage("OnAttach", newVine.GetComponent<Biomass>());
            vines.Add(newVine);
        }

        Collider[] oldGrowth = Physics.OverlapSphere(hit.point, freeSpace / 2, growthLayer);
        // Check for overlap with old growth
        if (oldGrowth.Length == 0) {
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Growth")) {
                GameObject newNode = Instantiate(growthManager.nodePrefab);
                newNode.transform.position = hit.point + .25f * hit.normal;
                newNode.transform.forward = hit.normal;

                if (hitRB != null) {
                    newNode.transform.parent = hitRB.transform;
                    hitRB.SendMessage("OnAttach", GetComponent<Biomass>());
                    hitRB.mass += 1f;
                    // Physics vine made above
                    newNode.GetComponent<TestHunger>().vines.Add(newVine);
                }
                else {
                    //Make a vine
                    newVine = Instantiate(growthManager.vinePrefab);
                    newVine.transform.position = .5f * (transform.position + newNode.transform.position);
                    newVine.transform.forward = (transform.position - newNode.transform.position);
                    newVine.transform.localScale = new Vector3(.25f, .25f, (transform.position - newNode.transform.position).magnitude / 2);

                    vines.Add(newVine);
                    newNode.GetComponent<TestHunger>().vines.Add(newVine);
                }

                return true;
            }
        }

        return false;
    } 

    public void OnDeath() {
        for(int i = 0; i < vines.Count; i++) {
            if(vines[i] != null) {
                Destroy(vines[i]);
            }
        }
        if(transform.parent != null && transform.parent.GetComponent<Rigidbody>()) {
            transform.parent.GetComponent<Rigidbody>().mass -= 1f;
        }
        Destroy(gameObject);
    }
}
