using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Attack")]
public class AttackData : ScriptableObject
{
    public string attackName;         // e.g., "Headbutt"
    public string inputActionName;    // e.g., "Attack1", "HeavyAttack"
    public int damage;
    public float range;
    public float cooldown;
    public AnimationClip animation;
    public float pushForce = 5f; // set in your AttackData asset or prefab

    public string AttackOriginName;
    //public GameObject attackOrigin;
}
