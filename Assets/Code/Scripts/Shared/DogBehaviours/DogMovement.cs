using Unity.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DogMovement : MonoBehaviour
{

    private NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }


    public void WalkToTarget(Transform target)
    {
        navAgent.speed = 8.0f;
        navAgent.SetDestination(target.position);
    }

    public void StopWalking()
    {
        navAgent.SetDestination(transform.position);
    }

    public float GetSpeed()
    {
        return navAgent.velocity.magnitude;
    }



}
