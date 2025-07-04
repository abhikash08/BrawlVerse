using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CartController : MonoBehaviour
{
    public Transform pickupPoint;
    private NavMeshAgent agent;
    private GameObject carriedItem;
    private bool isBusy = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Debug.Log($"[Cart] Initialized at pickup position: {pickupPoint?.position}");
    }

    public void StartDelivery(GameObject itemPrefab, Transform deliveryPoint, Transform tableTop)
    {
        if (isBusy || itemPrefab == null || deliveryPoint == null || tableTop == null)
        {
            Debug.LogError("[Cart] Cannot start delivery - missing references or already busy.");
            return;
        }

        isBusy = true;
        Debug.Log($"[Cart] Starting delivery of item: {itemPrefab.name} to: {deliveryPoint.parent?.name ?? "Unknown"}");
        StartCoroutine(HandleDelivery(itemPrefab, deliveryPoint, tableTop));
    }

    private IEnumerator HandleDelivery(GameObject itemPrefab, Transform deliveryPoint, Transform tableTop)
    {
        // Go to pickup
        agent.SetDestination(pickupPoint.position);
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= 0.5f);

        // Pickup
        carriedItem = Instantiate(itemPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        carriedItem.transform.SetParent(transform);
        Debug.Log($"[Cart] Picked up item: {carriedItem.name}");

        // Go to delivery point
        agent.SetDestination(deliveryPoint.position);
        Debug.Log($"[Cart] Heading to delivery target: {deliveryPoint.parent?.name ?? "Unknown"}");

        float stuckTimer = 10f;
        while (agent.pathPending || agent.remainingDistance > 0.5f)
        {
            stuckTimer -= Time.deltaTime;
            if (stuckTimer <= 0f)
            {
                Debug.LogError($"[Cart] STUCK! Could not reach delivery target: {deliveryPoint.parent?.name}");
                ResetCart();
                yield break;
            }
            yield return null;
        }

        // Move item to table top
        carriedItem.transform.SetParent(null);
        Vector3 start = carriedItem.transform.position;
        Vector3 end = tableTop.position + Vector3.up * 0.05f; // slight vertical lift to land above

        float duration = 1f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            carriedItem.transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        carriedItem.transform.position = end;
        Debug.Log($"[Cart] Delivered item {carriedItem.name} to table {deliveryPoint.parent?.name}");

        // Return to pickup
        agent.SetDestination(pickupPoint.position);
        Debug.Log("[Cart] Returning to pickup point...");

        stuckTimer = 10f;
        while (agent.pathPending || agent.remainingDistance > 0.5f)
        {
            stuckTimer -= Time.deltaTime;
            if (stuckTimer <= 0f)
            {
                Debug.LogError("[Cart] STUCK! Could not return to pickup point.");
                ResetCart();
                yield break;
            }
            yield return null;
        }

        Debug.Log("[Cart] Reached pickup point. Ready for next delivery.");
        isBusy = false;
    }

    private void ResetCart()
    {
        Debug.LogWarning("[Cart] Resetting cart due to issue during delivery.");
        if (carriedItem != null) Destroy(carriedItem);
        transform.position = pickupPoint.position;
        agent.ResetPath();
        isBusy = false;
    }
}

