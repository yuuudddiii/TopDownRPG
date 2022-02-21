using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using System;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Fighter fighter;
        Health health;
        void Start()
        {
            fighter = this.GetComponent<Fighter>();
            health = this.GetComponent<Health>();
        }


        void Update()
        {
            if (health.dead) return;
            if (interactWithCombat()) return;
            if (interactWithMovement()) return;
        }

        private bool interactWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!fighter.canAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0))
                {
                    fighter.attack(target.gameObject);
                }

                return true;
            }

            return false;
        }

        private bool interactWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

    }
}
