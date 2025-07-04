using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoDraw : MonoBehaviour
{
    [SerializeField] private Transform origin;
    [SerializeField] private float range;
    void OnDrawGizmosSelected()
    {
        // Visualize the grab radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin.position, range);
    }
}
