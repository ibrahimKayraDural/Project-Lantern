using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackViaEvent : MonoBehaviour
{
    [SerializeField] float AttackRadius = 1;
    [SerializeField] UnityEvent OnAttackHitPlayer;

    public void InitalizeAttack()
    {
        Collider2D[] overlappedCols = Physics2D.OverlapCircleAll(transform.position, AttackRadius);
        foreach (Collider2D col in overlappedCols)
        {
            if (col.TryGetComponent(out TopDownCharacterController playerController))
            {
                playerController.GetDamaged();
                OnAttackHitPlayer?.Invoke();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }
}
