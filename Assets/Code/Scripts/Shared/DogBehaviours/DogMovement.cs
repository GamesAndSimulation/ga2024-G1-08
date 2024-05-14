using Unity.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DogMovement : MonoBehaviour
{

    private NavMeshAgent navAgent;
    private Transform currentTarget;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void MoveToTarget(Transform target)
    {
        if(target != null && navAgent != null && target != currentTarget)
        {
            currentTarget = target;
            navAgent.SetDestination(currentTarget.position);
        }
    }

}
