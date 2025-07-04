using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float pushForce = 10f;
    public float knockbackTime = 0.5f;
    public float fallThreshold = -10f;
    public Vector2 respawnAreaX = new Vector2(-5f, 5f);
    public Vector2 respawnAreaZ = new Vector2(-5f, 5f);

    private NavMeshAgent agent;
    private Rigidbody rb;
    private bool isKnockedBack = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        // Auto-assign player if not set
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        // Respawn if bot falls off
        if (transform.position.y < fallThreshold)
        {
            Respawn();
            return;
        }

        // Follow player
        if (!isKnockedBack && player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Push the player on contact
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 pushDir = (collision.transform.position - transform.position).normalized;
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
                playerRb.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
    }

    public void Knockback(Vector3 force)
    {
        if (isKnockedBack) return;

        isKnockedBack = true;
        agent.enabled = false;
        rb.AddForce(force, ForceMode.Impulse);
        Invoke(nameof(RecoverFromKnockback), knockbackTime);
    }

    void RecoverFromKnockback()
    {
        isKnockedBack = false;
        agent.enabled = true;
    }

    void Respawn()
    {
        // Random respawn position within range
        float x = Random.Range(respawnAreaX.x, respawnAreaX.y);
        float z = Random.Range(respawnAreaZ.x, respawnAreaZ.y);
        transform.position = new Vector3(x, 3f, z);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isKnockedBack = false;
        agent.enabled = true;
    }

    // Optional: call this from your player's attack code
    public void OnHitByPlayer(Vector3 direction, float force)
    {
        Knockback(direction.normalized * force);
    }
}

