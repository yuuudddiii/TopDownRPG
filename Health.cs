using UnityEngine;
using RPG.Combat;
using System.Collections;
using System;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        public float health = 100f;

        public bool dead = false;

        public void takeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);

            if (health == 0) death();   
        }

        private void death()
        {
            if (dead) return;
            dead = true;
            GetComponent<ActionScheduler>().cancelCurrentAction();
            GetComponent<Animator>().SetTrigger("death");
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
