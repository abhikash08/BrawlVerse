using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrySupport_Attack : MonoBehaviour
{
    public static event Action<Vector3, Vector3, float> PushAttack;

    Vector3 playerPosition;
    Vector3 attackDirection;
    [SerializeField] float attackRange = 3f;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            StartCoroutine(AttackInitiated()); //To open parry window
            playerPosition = transform.position;
            attackDirection = transform.forward;
            DoDamage(playerPosition,attackDirection,attackRange); // Use animation events for precise positioning
        }
    }

    private IEnumerator AttackInitiated() {
        ParryManager.OpenParryWindow?.Invoke();
        yield return new WaitForSeconds(0.5f); // 0.5f is parryWindowTime. Use different time for different attacks.
        ParryManager.CloseParryWindow?.Invoke(); // Close Parry window
    }

    public void DoDamage(Vector3 playerPosition, Vector3 attackDirection, float attackRange) {
        PushAttack?.Invoke(playerPosition, attackDirection, attackRange);
    }
}
