using Unity.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DogMovement : MonoBehaviour
{

    public NavMeshAgent navAgent;

    protected virtual void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }


    public virtual void WalkToTarget(Transform target)
    {
        navAgent.speed = 8.0f;
        navAgent.SetDestination(target.position);
    }

    public virtual void StopWalking()
    {
        navAgent.SetDestination(transform.position);
    }

    public virtual float GetSpeed()
    {
        return navAgent.velocity.magnitude;
    }

}
