using System;
using UnityEngine;

public static class AttackEvents
{
    public static event Action<GameObject, GameObject, AttackData> OnIncomingAttack;

    public static void Broadcast(GameObject attacker, GameObject target, AttackData data) 
    {
        Debug.Log($"Attack broadcast: {attacker.name} -> {target.name} [{data.attackName}]");
        OnIncomingAttack?.Invoke(attacker, target, data);
    }
}
