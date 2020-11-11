using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    public BoidManager myManager;
    float speed;
    bool turning = false;

    void Start() {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    void Update() {
        // determine the bounding box of the manager cube
        Bounds b = new Bounds(myManager.transform.position, myManager.swimLimits*5);
        // if fish is outside the bounds of the cube then start turning around
        if(!b.Contains(transform.position)) {
            turning = true;
        } else turning = false;

        if(turning) {
            // turn towards the centre of the manager cube
            Vector3 direction = myManager.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        } else {

        if(Random.Range(0,100) < 10)
            speed = Random.Range(myManager.minSpeed,myManager.maxSpeed);
        if(Random.Range(0,100) < 20)
            ApplyRules();
        }

        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules() {
        GameObject[] gos;
        gos = myManager.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach (GameObject go in gos) {
            // ignore self
            if(go != this.gameObject) {
                // check for fish of interest
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if(nDistance <= myManager.viewingDistance) {
                    // add pos to average position
                    vcentre += go.transform.position;
                    groupSize++;

                    // seperation (avoid crowding other group members)
                    if(nDistance < myManager.seperationDistance) {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    // global speed
                    Boid anotherBoid = go.GetComponent<Boid>();
                    gSpeed = gSpeed + anotherBoid.speed;
                }
            }
        }

        if(groupSize > 0) {
            // alignment (align with the average heading of the group)
            if(myManager.followSphere) {
                vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
                speed = gSpeed / groupSize;
            } else {
            vcentre = vcentre / groupSize;
            speed = gSpeed / groupSize;
            }

            // cohesion (move towards the average position of the group)            
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if(direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }
    }
}