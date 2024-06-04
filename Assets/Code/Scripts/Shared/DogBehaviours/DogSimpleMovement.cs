using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogSimpleMovement : DogMovement {


    [SerializeField] float speed;

    protected override void Start() {

    }


    public override void WalkToTarget(Transform target) {

        this.transform.position += speed * Time.deltaTime * (target.position - this.transform.position).normalized;
    }

    public override void StopWalking() {
        
    }

    public override float GetSpeed() {
        return speed;
    }
}
