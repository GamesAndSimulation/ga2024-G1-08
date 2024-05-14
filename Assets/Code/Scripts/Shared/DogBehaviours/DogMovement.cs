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
        navAgent.speed = 2.5f;
        navAgent.SetDestination(target.position);
    }

    public void StopWalking()
    {
        navAgent.SetDestination(transform.position);
    }



}
