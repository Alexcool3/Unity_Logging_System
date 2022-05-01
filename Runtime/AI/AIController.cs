using UnityEngine;
using UnityEngine.AI;

namespace Logging_System
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;

        [ContextMenu("Set Destination")]
        public void SetDestination()
        {
            agent?.SetDestination(new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f)));
        }

        public void UpdatePath()
        {
            if (agent?.remainingDistance <= 1.0f)
            {
                SetDestination();
            }
        }
    }
}
