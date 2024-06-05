using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogSimpleMovement : DogMovement {


    [SerializeField] float speed;

    protected override void Start() {

    }


    public override void WalkToTarget(Transform target) {

        Vector3 toMove = new Vector3(target.position.x - this.transform.position.x, 0, target.position.z - this.transform.position.z);

        this.transform.position += speed * Time.deltaTime * toMove.normalized;

        this.transform.forward = toMove.normalized;
    }

    public override void StopWalking() {
        
    }

    public override float GetSpeed() {
        return speed;
    }
}
