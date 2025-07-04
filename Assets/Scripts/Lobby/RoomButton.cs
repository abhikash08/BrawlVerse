using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomButton : MonoBehaviour
{
    public string _roomName;

    public void OnButtonPressed()
    {
        RoomList.instance.JoinRoomByString( _roomName );
    }
}
