using UnityEngine;

public class PlayerGrabState : PlayerBaseState
{
    private float grabTimer;
    private Rigidbody grabbedObject;

    public PlayerGrabState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }

    public override void EnterState()
    {
        TryGrabNearbyObject();
        ctx.animator.SetFloat("Speed", ctx.Speed); // Optional: Add animation support
    }

    public override void UpdateState()
    {
        HandleMovement();

        if (grabbedObject == null)
        {
            ctx.SwitchState(factory.Idle());
            return;
        }

        grabTimer -= Time.deltaTime;

        // Move the grabbed object to the grab point
        grabbedObject.MovePosition(ctx.grabPoint.position);

        if (grabTimer <= 0f)
        {
            ReleaseObject();
            ctx.SwitchState(factory.Idle());
        }

        if (ctx.moveInput.magnitude <= 0.1f) // Idle if no movement
        {
            ctx.animator.SetFloat("Speed", 0f);
        }
        else
        {
            ctx.animator.SetFloat("Speed", ctx.Speed);
        }
    }

    public override void ExitState()
    {
        ReleaseObject();
    }

    public override void HandleJumpInput()
    {
        if (ctx.isGrounded)
        {
            ReleaseObject(); // Optional: drop before jump
            ctx.SwitchState(factory.Jump());
        }
    }

    private void HandleMovement()
    {
        Vector3 direction = new Vector3(ctx.moveInput.x, 0f, ctx.moveInput.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            Vector3 moveDir = ctx.GetMoveDirection(direction);
            ctx.characterController.Move(moveDir * ctx.Speed * Time.deltaTime);
            ctx.HandleRotation(); // Use existing rotation logic
        }
    }

    private void TryGrabNearbyObject()
    {
        if (ctx.grabCooldownTimer > 0f) return;

        Collider[] colliders = Physics.OverlapSphere(ctx.transform.position, ctx.grabRadius, ctx.pushableLayer);
        if (colliders.Length == 0) return;

        float closestDist = float.MaxValue;
        Rigidbody closestRb = null;

        foreach (var col in colliders)
        {
            Rigidbody rb = col.attachedRigidbody;
            if (rb != null)
            {
                float dist = Vector3.Distance(ctx.transform.position, rb.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestRb = rb;
                }
            }
        }

        if (closestRb != null)
        {
            grabbedObject = closestRb;
            grabbedObject.isKinematic = true;
            grabbedObject.velocity = Vector3.zero;
            grabbedObject.transform.position += Vector3.up * 0.5f;
            grabbedObject.transform.SetParent(ctx.grabPoint);

            grabTimer = ctx.grabDuration;
            ctx.grabCooldownTimer = ctx.grabCooldown;

            Debug.Log("Grabbed: " + grabbedObject.name);
        }
    }

    private void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.transform.SetParent(null);
            grabbedObject.isKinematic = false;
            Debug.Log("Released: " + grabbedObject.name);
            grabbedObject = null;
        }
    }
}
