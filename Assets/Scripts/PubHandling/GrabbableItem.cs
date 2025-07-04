using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
    private bool isGrabbedByPlayer = false;
    private bool isPickedByCart = false;

    public void OnGrabbedByPlayer()
    {
        isGrabbedByPlayer = true;
        isPickedByCart = false;
        transform.parent = null;
        gameObject.tag = "HeldItem";
    }

    public void OnReleasedByPlayer()
    {
        isGrabbedByPlayer = false;
        gameObject.tag = "Item";
    }

    public void OnPickedByCart(Transform holder)
    {
        isPickedByCart = true;
        isGrabbedByPlayer = false;
        transform.SetParent(holder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void OnDroppedByCart()
    {
        isPickedByCart = false;
        transform.SetParent(null);
        gameObject.tag = "Item";
    }

    public bool IsAvailable()
    {
        return !isGrabbedByPlayer && !isPickedByCart && transform.parent == null;
    }
}
