using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] foodPrefabs; // 6 food prefabs
    public Transform[] dropPoints;   // 6 fixed spawn points
    public Transform pickupPoint;    // 1 central pickup point
    public Transform[] userTables;   // 13 user tables (each has a delivery & table top)
    public CartController cart;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            int index = Random.Range(0, foodPrefabs.Length);
            GameObject prefab = foodPrefabs[index];
            Transform dropPoint = dropPoints[index];

            if (prefab == null || dropPoint == null)
            {
                Debug.LogError("[SpawnManager] Prefab or DropPoint is null.");
                continue;
            }

            GameObject foodInstance = Instantiate(prefab, dropPoint.position, Quaternion.identity);
            foodInstance.name = prefab.name + "_Instance";
            Debug.Log($"[SpawnManager] Spawned food: {foodInstance.name} at drop point: {dropPoint.name}");

            yield return new WaitForSeconds(1f);

            // Pick a random table
            Transform randomTable = userTables[Random.Range(0, userTables.Length)];

            if (randomTable == null)
            {
                Debug.LogError("[SpawnManager] Selected table is null.");
                continue;
            }

            Transform deliveryPoint = randomTable.Find("DeliveryPoint");
            Transform tableTop = randomTable.Find("Top");

            if (deliveryPoint == null || tableTop == null)
            {
                Debug.LogError($"[SpawnManager] Table {randomTable.name} missing DeliveryPoint or Top.");
                continue;
            }

            // Start delivery using prefab (not instance!)
            cart.StartDelivery(prefab, deliveryPoint, tableTop);
        }
    }
}
