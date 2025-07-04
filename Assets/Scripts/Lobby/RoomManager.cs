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

    [Header("Room Name")]
    public string roomName = "test";

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
        Debug.Log(roomName);
        PhotonNetwork.JoinOrCreateRoom(roomName, new Photon.Realtime.RoomOptions(), null);

        nickNameUI.SetActive(false);
        connectingUI.SetActive(true);
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
