using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDoor : MonoBehaviour {

    public int securityRequired = 1;

	// Use this for initialization
	void Start () {
        MapManager.getInstance().doors.Add(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TryUnlock(int level) {
        if(level >= securityRequired) {
            Destroy(gameObject);
        }
    }
}
