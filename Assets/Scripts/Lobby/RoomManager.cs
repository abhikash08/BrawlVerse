using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using JetBrains.Annotations;
using UnityEngine.Rendering;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    public GameObject EndgameUI;
    // Start is called before the first frame update
    [Header("Player Object")]
    public GameObject player;

    [Header("Player Spawn Point")]
    public Transform[] spawnPoints;

    [Header("Free Look Camera")]
    public CinemachineFreeLook freeLook;

    [Header("Camera UI")]
    public GameObject roomCam;

    [Header("UI")]
    public GameObject nickNameUI;
    public GameObject connectingUI;

    [Header("Room Name")]
    public string roomName = "test";

    string nickName = "unnamed";
    [Header("Player Color")]
    public Material[] playerColors;
    [HideInInspector]
    public int kills;
    [HideInInspector]
    public int deaths;
    [HideInInspector]
    public int damage;
    public bool Joined = false;

    private void Awake()
    {
        Instance = this;
    }
    public void SetNickname(string _name)
    {
        nickName = _name;
    }
    public void OnJoinButtonPressed()
    {
        Debug.Log(message: "Connecting. . . ");
        Debug.Log(roomName);
        PhotonNetwork.JoinOrCreateRoom(roomName, new Photon.Realtime.RoomOptions(), null);

        nickNameUI.SetActive(false);
        connectingUI.SetActive(true);
    }
    public override void OnJoinedRoom()
    {
        if (NetworkedTimer.Instance != null)
        {
            NetworkedTimer.Instance.InitializeTimer();
        }
        Debug.Log("Room Joined");
        roomCam.SetActive(false);
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerHealth>().isLocalPlayer = true;
        PhotonView view = _player.GetComponent<PhotonView>();
        view.RPC("SetPlayerName", RpcTarget.AllBuffered, nickName);
        PhotonNetwork.LocalPlayer.NickName = nickName;
        if (view != null && view.IsMine && freeLook != null)
        {
            Transform lookAt = _player.transform.GetChild(1);
            freeLook.Follow = lookAt;
            freeLook.LookAt = lookAt;
        }
    }
    public void SetHashes()
    {
        try
        {
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties; // Get the custom properties of the local player
            hash["kills"] = kills;
            hash["deaths"] = deaths;
            hash["damage"] = damage;
            
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        }
        catch
        {

        }
    }
}