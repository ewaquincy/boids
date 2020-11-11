using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour {

    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swimLimits = new Vector3(5,5,5);
    
    public Vector3 goalPos;
    public GameObject goalPosition;    

    [Header("Fish Settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    [Space(10)]

    public bool followSphere;
    [Range(1.0f, 20.0f)]
    public float viewingDistance;
    [Range(1.0f, 5.0f)]
    public float seperationDistance;

    // Start is called before the first frame update
    void Start() {
        allFish = new GameObject[numFish];
        for(int i = 0; i < numFish; i++) {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x,swimLimits.x),
                                                                Random.Range(-swimLimits.y,swimLimits.y),
                                                                Random.Range(-swimLimits.z,swimLimits.z));
            allFish[i] = (GameObject) Instantiate(fishPrefab, pos, Random.rotation);
            allFish[i].GetComponent<Boid>().myManager = this;
        }
        InvokeRepeating("SetGoalPos", 0.0f, 5.0f);      
    }

    void SetGoalPos() {
        goalPos = goalPosition.transform.position;
    }
}
