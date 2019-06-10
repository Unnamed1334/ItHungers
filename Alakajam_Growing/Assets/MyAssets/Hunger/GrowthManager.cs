using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthManager : MonoBehaviour {

    private static GrowthManager instance;

    public GameObject nodePrefab;
    public GameObject vinePrefab;
    public GameObject attackVinePrefab;


    public float growthModifier = 10;
    public float bonusGrowthTime = 10;

    // Use this for initialization
    void Awake () {
        instance = this;
    }
	
	// Update is called once per frame
	void Update () {
		if(bonusGrowthTime < 0) {
            growthModifier = 1;
        }
        else {
            bonusGrowthTime -= Time.deltaTime;
        }
	}

    public static GrowthManager getInstance() {
        return instance;
    }
}
