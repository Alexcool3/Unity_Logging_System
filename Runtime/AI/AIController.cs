using UnityEngine;
using UnityEngine.AI;

namespace Logging_System
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;

        [Header("Walking Properties")]
        [SerializeField] private Vector2 xRange = new Vector2();
        [SerializeField] private Vector2 zRange = new Vector2();

        [ContextMenu("Set Destination")]
        public void SetDestination()
        {
            agent?.SetDestination(new Vector3(Random.Range(-xRange.x, xRange.y), 0.0f, Random.Range(zRange.x, zRange.y)));
        }

        [ContextMenu("Update Path")]
        public void UpdatePath()
        {
            if (agent?.remainingDistance <= 1.0f)
            {
                SetDestination();
            }
        }
    }
}
