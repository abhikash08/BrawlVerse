using System.Collections;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class PlayerHealth : MonoBehaviourPun
{
    public float health = 100f;
    public PlayerStateMachine _playerStateMachine;  // Reference to your existing shield script
    public bool isLocalPlayer;
    void Start()
    {
        if (_playerStateMachine == null)
        {
            _playerStateMachine = GetComponent<PlayerStateMachine>(); // Try auto-assign if on same object
        }
    }

    [PunRPC]
    public void TakeDamage(int damageAmount)
    {
        // If shield active, ignore damage
        if (_playerStateMachine != null && _playerStateMachine.isShieldActive)
        {
            Debug.Log("Shield is active! No damage taken.");
            return;
        }

        health -= damageAmount;
        Debug.Log("Player health: " + health);

        if (health <= 0)
        {
            Debug.Log("Player died!");
            Die();
            if (isLocalPlayer) RoomManager.Instance.SpawnPlayer();
            Destroy(gameObject);
        }
    }

    void Die()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(RespawnAfterDelay(5f));
            if(gameObject != null ) PhotonNetwork.Destroy(gameObject);
            
        }
    }

    IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        FindObjectOfType<RoomManager>().SpawnPlayer();
    }
}