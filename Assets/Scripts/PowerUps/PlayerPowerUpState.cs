using UnityEngine;

public class PlayerPowerUpState : PlayerBaseState
{
    private PowerUpType powerUpType;
    private float powerUpDuration;
    private float powerUpTimer;

    public PlayerPowerUpState(PlayerStateMachine ctx, PlayerStateFactory factory, PowerUpType type, float duration) : base(ctx, factory)
    {
        powerUpType = type;
        powerUpDuration = duration;
    }

    public override void EnterState()
    {
        powerUpTimer = powerUpDuration;

        switch (powerUpType)
        {
            case PowerUpType.BubbleShield:
                ActivateBubbleShield();
                break;
            case PowerUpType.PullThroughAir:
                ActivateAirPull();
                break;
        }
    }

    public override void UpdateState()
    {
        powerUpTimer -= Time.deltaTime;

        // You can allow movement or other actions here if desired
        HandleMovement();

        if (powerUpTimer <= 0f)
        {
            DeactivatePowerUp();
            ctx.SwitchState(factory.Idle()); // or return to previous state
        }
    }

    public override void ExitState()
    {
        DeactivatePowerUp();
    }

    public override void HandleJumpInput()
    {
        if (ctx.isGrounded)
            ctx.SwitchState(factory.Jump());
    }

    private void HandleMovement()
    {
        if (ctx.isDashing)
            return; // Skip movement

        Vector3 direction = new Vector3(ctx.moveInput.x, 0f, ctx.moveInput.y).normalized;
        ctx.animator.SetFloat("Speed", 0f);
        if (direction.magnitude >= 0.1f)
        {
            Vector3 moveDir = ctx.GetMoveDirection(direction);
            ctx.characterController.Move(moveDir * ctx.Speed * Time.deltaTime);
            ctx.animator.SetFloat("Speed", ctx.Speed);
            ctx.HandleRotation();
        }
        
    }

    private void ActivateBubbleShield()
    {
        Debug.Log("Activated Bubble Shield!");
        // Example: enable shield visual, change damage handling
        ctx.EnableShield(true);
    }

    private void ActivateAirPull()
    {
        Debug.Log("Activated Pull Through Air!");
        // Example: give force or animation
        ctx.PullPlayerThroughAir();
    }

    private void DeactivatePowerUp()
    {
        if (powerUpType == PowerUpType.BubbleShield)
        {
            ctx.EnableShield(false);
        }

        Debug.Log("Power-up expired: " + powerUpType);
    }
}
