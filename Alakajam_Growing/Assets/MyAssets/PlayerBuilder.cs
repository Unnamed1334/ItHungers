using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilder : MonoBehaviour {

    public GameObject turretPrefab;

    public float displacement = 5f;
    public LayerMask solidLayer;

    public Transform hitIndicator;
    public Transform positionIndicator;
    public LineRenderer connectoer;

    public int maxTurrets = 4;
    public List<GameObject> turretList = new List<GameObject>();

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        float dx = Input.GetAxis("Mouse ScrollWheel");

        displacement = Mathf.Clamp( displacement + 100 * dx * Time.fixedDeltaTime, 1, 15);

        // Check for destroyed drones
        for(int i = 0; i < turretList.Count; i++) {
            if(turretList[i] == null) {
                turretList.RemoveAt(i);
                i--;
            }
        }

        Ray testRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(testRay, out hit, 50, solidLayer)) {
            hitIndicator.gameObject.SetActive(true);
            positionIndicator.gameObject.SetActive(true);
            connectoer.gameObject.SetActive(true);

            hitIndicator.position = hit.point;
            hitIndicator.up = hit.normal;
            positionIndicator.position = hit.point + displacement * hit.normal;
            connectoer.SetPosition(0, hit.point);
            connectoer.SetPosition(1, hit.point + displacement * hit.normal);
            Color lineColor;
            if(!Physics.Raycast(hit.point, hit.normal, displacement, solidLayer)) {
                lineColor = Color.blue;
                positionIndicator.gameObject.SetActive(true);

                // Place the object
                if(Input.GetMouseButtonDown(0)) {
                    GameObject newTurret = Instantiate(turretPrefab, transform.position - 1.5f * Vector3.up, Quaternion.identity);
                    newTurret.GetComponent<PhysicsRestore>().target = hit.point + displacement * hit.normal;
                    newTurret.GetComponent<Rigidbody>().velocity = -2 * Vector3.up;

                    turretList.Add(newTurret);
                    if(turretList.Count > maxTurrets) {
                        Destroy(turretList[0]);
                        turretList.RemoveAt(0);
                    }
                }

            }
            else {
                lineColor = Color.red;
                positionIndicator.gameObject.SetActive(false);
            }
            connectoer.startColor = lineColor;
            connectoer.endColor = lineColor;


        }
        else {
            hitIndicator.gameObject.SetActive(false);
            positionIndicator.gameObject.SetActive(false);
            connectoer.gameObject.SetActive(false);
        }
    }
}
