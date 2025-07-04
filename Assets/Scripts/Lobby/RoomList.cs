using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomList : MonoBehaviourPunCallbacks
{
    public static RoomList instance;
    [Header("Room Manager")]
    public GameObject RoomManagerGameObject;
    public RoomManager roomManager;

    [Header("UI")]
    public GameObject roomNameUI;
    public Transform parentUI;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    private void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);
        PhotonNetwork.ConnectUsingSettings();
    }
    public void ChangeRoomToCreateName(string roomName)
    {
        roomManager.roomName = roomName;
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(cachedRoomList.Count <= 0)
        {
            cachedRoomList = roomList;
        }
        else
        {
            foreach(var room in roomList)
            {
                for(int i = 0; i < cachedRoomList.Count; i++)
                {
                    if(cachedRoomList[i].Name == room.Name)
                    {
                        List<RoomInfo> newList = cachedRoomList;
                        if (room.RemovedFromList)
                        {
                            newList.Remove(newList[i]);
                        }
                        else
                        {
                            newList[i] = room;
                        }
                        cachedRoomList = newList;
                    }
                }
            }
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        foreach(Transform child in parentUI)
        {
            Destroy(child.gameObject);
        }
        foreach(var room in cachedRoomList)
        {
            var roomItem = Instantiate(roomNameUI, parentUI);
            roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
            roomItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount.ToString() + "/" + "10";

            roomItem.GetComponent<RoomButton>()._roomName = room.Name;
        }
    }

    public void JoinRoomByString(string roomName)
    {
        roomManager.roomName = roomName;
        RoomManagerGameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

}
