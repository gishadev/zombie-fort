using UnityEngine;
using UnityEngine.AI;

namespace gishadev.fort.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public void SetDestination(Vector3 target)
        {
            _agent.SetDestination(target);
            Resume();
        }

        public void Stop() => _agent.isStopped = true;

        public void Resume() => _agent.isStopped = false;
    }
}