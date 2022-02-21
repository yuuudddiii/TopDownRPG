using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
   
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;

        Fighter fighter;
        GameObject player;
        Health health;

        Vector3 guardPosition;
        int currentWaypointIndex = 0;

        float timeSinceLastSawPlayer = Mathf.Infinity;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();

            guardPosition = transform.position;
           
            
        }
        private void Update()
        {
            if (health.dead) return;
            if (this.GetComponent<Health>().dead) return;

            attackIfInRange(player);
        }

        private void stopAttackIfOutOfRange(GameObject player)
        {
            if (fighter.target != null && distanceToPlayer(player) >= chaseDistance)
            {
                fighter.target = null;
            }
        }

        private void attackIfInRange(GameObject player)
        {
            if (distanceToPlayer(player) <= chaseDistance && fighter.canAttack(player))
            {
                fighter.attack(player);
                timeSinceLastSawPlayer = 0;
            }

            else if (timeSinceLastSawPlayer <= suspicionTime)
            {
                GetComponent<ActionScheduler>().cancelCurrentAction();
            }

            else
            {
                patrolBehaviour();

            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void patrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    CycleWaypoint();
                }
                nextPosition = getCurrentWaypoint();
            }
            GetComponent<Mover>().StartMoveAction(nextPosition);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, getCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private Vector3 getCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private float distanceToPlayer(GameObject player)
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }

}