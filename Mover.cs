
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
   
    public class Mover : MonoBehaviour, IAction
    {
        NavMeshAgent navMeshAgent;
        Health health;
        void Start()
        {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (health.dead) navMeshAgent.enabled = false;
            updateAnimator();
        }

        public void Cancel ()
        {
            navMeshAgent.isStopped = true;
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            Cancel();
            MoveTo(destination);

        }
        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        public void updateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }
}