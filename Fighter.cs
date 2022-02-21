using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField]
        float weaponRange = 2f;

        [SerializeField]
        float attackSpeed = 1f;

        float timeSinceLastAttack = 0;

        public Transform target;
        float targetDistance;

        bool attacking;

        private void Start()
        {
            
        }

        private void Update()
        {
            if (target != null) print("Target: " + target.name);
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            

            targetDistance = Vector3.Distance(transform.position, target.position);
            if (targetDistance > weaponRange)
            {
                GetComponent<Mover>().MoveTo(target.position);
            }

            else
            {
                GetComponent<Mover>().Cancel();
                updateAnimator();
            }
        }

        public bool canAttack(GameObject target)
        {
            if (target == null) return false;

            Health targetHealth = target.GetComponent<Health>();
            return targetHealth != null && !targetHealth.dead;
        }
        public void Cancel()
        {
            cancelAttack();
            target = null;
        }

        public void attack(GameObject combatTarget)
        {

            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        private void updateAnimator()
        {
            transform.LookAt(target);
            if (timeSinceLastAttack > attackSpeed && target.GetComponent<Health>().health > 0)
            {
                attack();
                timeSinceLastAttack = 0;
            }

        }

        private void attack()
        {
            GetComponent<Animator>().ResetTrigger("cancelAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event
        // If animation is called, target should not be null
        void Hit()
        {
            if (target == null) return;

            if (target.GetComponent<Health>() != null && target.GetComponent<Health>().health > 0)
                target.GetComponent<Health>().takeDamage(12);
        }

        private void cancelAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("cancelAttack");
        }
    }
}
