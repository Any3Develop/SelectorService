using System;
using UnityEngine;
using UnityEngine.AI;

public class SomeUnitAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent = null;
    public event EventHandler OnDestinationComplete;
    public bool IsStopped { get; private set; }

    public void SetDestination(Vector3 destination)
    {
        _navMeshAgent.SetDestination(destination);
    }

    private void Update()
    {
        if (!_navMeshAgent.pathPending)
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    IsStopped = true;
                    OnDestinationComplete?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        IsStopped = false;
    }
}
