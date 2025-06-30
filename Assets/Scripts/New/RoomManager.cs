using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public GameObject player;
    public Transform spanPoint;
    public CinemachineFreeLook virtualCam;
    void Start()
    {
        Debug.Log(message:"Connecting. . . ");
        PhotonNetwork.ConnectUsingSettings();
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
        GameObject _player=PhotonNetwork.Instantiate(player.name,spanPoint.position,Quaternion.identity);
        PhotonView view=_player.GetComponent<PhotonView>();
        if(view!=null && view.IsMine && virtualCam!=null)
        {
            virtualCam.Follow=_player.transform;
            virtualCam.LookAt=_player.transform;

        }
    }
}
