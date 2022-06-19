using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class PlayerCombat : MonoBehaviour, ActionInterface 
    {
        [SerializeField] float   WeaponRange = 1f;
        [SerializeField] float  TimeBetweenAttack = 0.85f;
        [SerializeField] float WeaponDamage = 8f;
        Health target;
        float timeSinceLastAttack =  Mathf.Infinity;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead())    return;
            if (!InRange())
            {
                GetComponent<Mover>().Moveto(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }

        }

        private void AttackBehaviour()
        {
            if(timeSinceLastAttack > TimeBetweenAttack)
            {
                transform.LookAt(target.transform);
                GetComponent<Animator>().SetTrigger("Attack");
                timeSinceLastAttack = 0;
            }
        }

        //  Attack Animation Event
        void Hit()
        {
            target.Damage(WeaponDamage);
        }
        private bool InRange()
        {
            return (Vector3.Distance(transform.position, target.transform.position) < WeaponRange);
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget==null)    return false;
            Health testTarget = combatTarget.GetComponent<Health>();
            return (testTarget!=null && !testTarget.IsDead());
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }    

        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("StopAttack");
            target = null;
        }


    }
}