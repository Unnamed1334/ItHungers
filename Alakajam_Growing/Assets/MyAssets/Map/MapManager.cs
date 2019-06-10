using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    public Transform basePostion;

    public GameObject teleporterPrefab;
    public GameObject securityPrefab;
    public GameObject growthPrefab;
    public GameObject powerPrefab;

    public GameObject teleporterInstance;
    public List<GameObject> securityInstances = new List<GameObject>();


    public Pionter pointerInstance;

    private static MapManager instance;

    public int securitylevel = 0;

    // Store the doors. 
    public List<TestDoor> doors = new List<TestDoor>();

    // Use this for initialization
    void Awake() {
        instance = this;
    }

    void Start() {
        // Place the Teleporter
        teleporterInstance = Instantiate(teleporterPrefab, RandomPosition() + 2.5f * Vector3.up, Quaternion.identity);

        // Place the Security Codes
        for(int i = 0; i < 5; i++) {
            securityInstances.Add(Instantiate(securityPrefab, RandomPosition() + Vector3.up, Quaternion.identity));
        }

        int randomCode = Random.Range(0, securityInstances.Count);

        pointerInstance.target = securityInstances[randomCode].transform.position;

        // Place some initial Growth
        for (int i = 0; i < 8; i++) {
            Instantiate(growthPrefab, RandomPosition() + 3 * Vector3.up, Quaternion.identity);
        }

        // Place some power
        for (int i = 0; i < 40; i++) {
            Instantiate(powerPrefab, RandomPosition() + Vector3.up, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void UpdateSecurity (int newSecurity) {
        securitylevel = newSecurity;

        for (int i = 0; i < doors.Count; i++) {
            if(doors[i] != null) {
                doors[i].TryUnlock(securitylevel);
            }
        }

        if (securitylevel >= 4) {
            GameObject.Find("GlowyThing").GetComponent<Teleporter>().ActivateTeleporter();

            pointerInstance.target = GameObject.Find("GlowyThing").transform.position;
        }
        else {
            for (int i = 0; i < securityInstances.Count; i++) {
                if (securityInstances[i] == null) {
                    securityInstances.RemoveAt(i);
                    i--;
                }
            }

            int randomCode = Random.Range(0, securityInstances.Count);

            pointerInstance.target = securityInstances[randomCode].transform.position;
        }
    }

    public static MapManager getInstance() {
        return instance;
    }

    private Vector3 RandomPosition() {

        int xOfset = Random.Range(0, 33);
        int yOfset = Random.Range(0, 33);
        Vector3 position = basePostion.position + 10 * xOfset * Vector3.right - 10 * yOfset * Vector3.forward;

        while((xOfset < 5 && yOfset < 2) || Physics.OverlapSphere(position, 1).Length != 0) {
            xOfset = Random.Range(0, 33);
            yOfset = Random.Range(0, 33);
            position = basePostion.position + 10 * xOfset * Vector3.right - 10 * yOfset * Vector3.forward;
        }

        RaycastHit hit;
        Physics.Raycast(position, Vector3.down, out hit);

        return hit.point;
    }
}
