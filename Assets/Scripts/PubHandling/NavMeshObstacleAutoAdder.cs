using UnityEngine;
using UnityEngine.AI;

public class NavMeshObstacleAutoAdder : MonoBehaviour
{
    public Transform[] obstacleRoots; // Assign: Wall1, Roof, Stage, Outside_Eating etc.

    void Start()
    {
        AddNavMeshObstacles();
    }

    public void AddNavMeshObstacles()
    {
        int addedObstacles = 0;
        int addedColliders = 0;

        foreach (Transform root in obstacleRoots)
        {
            if (root == null) continue;

            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child == root) continue; // Skip the empty root itself

                // If it has no collider but has a MeshRenderer → add BoxCollider
                if (child.GetComponent<Collider>() == null && child.GetComponent<MeshRenderer>() != null)
                {
                    child.gameObject.AddComponent<BoxCollider>();
                    addedColliders++;
                }

                // Now add NavMeshObstacle if not already there
                if (child.GetComponent<Collider>() != null && child.GetComponent<NavMeshObstacle>() == null)
                {
                    var obstacle = child.gameObject.AddComponent<NavMeshObstacle>();
                    obstacle.carving = true;
                    obstacle.carveOnlyStationary = false;
                    obstacle.shape = NavMeshObstacleShape.Box;
                    addedObstacles++;
                }
            }
        }

        Debug.Log($"[NavMesh] Added {addedObstacles} NavMeshObstacles and {addedColliders} missing Colliders.");
    }
}
