using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    // Start is called before the first frame update
    [Header("Player Object")]
    public GameObject player;

    [Header("Player Spawn Point")]
    public Transform spanPoint;

    [Header("Free Look Camera")]
    public CinemachineFreeLook freeLook;

    [Header("Camera UI")]
    public GameObject roomCam;

    [Header("UI")]
    public GameObject nickNameUI;
    public GameObject connectingUI;

    string nickName = "unnamed";
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
        PhotonNetwork.ConnectUsingSettings();

        nickNameUI.SetActive(false);
        connectingUI.SetActive(true);
    }
    void Start()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Server");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined the Lobby");
        PhotonNetwork.JoinOrCreateRoom("test",new Photon.Realtime.RoomOptions(),null);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined");
        roomCam.SetActive(false);
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        GameObject _player = PhotonNetwork.Instantiate(player.name, spanPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerHealth>().isLocalPlayer = true;
        PhotonView view = _player.GetComponent<PhotonView>();
        view.RPC("SetPlayerName", RpcTarget.AllBuffered, nickName);
        if (view != null && view.IsMine && freeLook != null)
        {
            Transform lookAt = _player.transform.GetChild(1);
            freeLook.Follow = lookAt;
            freeLook.LookAt = lookAt;
        }
    }
}
