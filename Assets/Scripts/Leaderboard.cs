using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;
public class Leaderboard : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerHolder;

    [Header("Options")]
    public float refreshRate = 1f;
    [Space]
    [Header("UI")]
    public GameObject[] slots;
    public TextMeshProUGUI[] scoretexts;
    public TextMeshProUGUI[] nametexts;
    public TextMeshProUGUI[] kdtexts;

    private void Start()
    {
        InvokeRepeating("Refresh", 1f, refreshRate);
    }
    public void Refresh()
    {
        foreach (var slot in slots)
        {
            slot.SetActive(false);
        }
        var sortedPlayerslist = (from player in PhotonNetwork.PlayerList orderby player.CustomProperties["damage"] descending select player).ToList(); //

        int i = 0;
        foreach (var player in sortedPlayerslist)
        {
            slots[i].SetActive(true); // Activate the slot for each player in the sorted list
            if (player.NickName == "")
            {
                nametexts[i].text = "unnamed"; // If the player's nickname is empty, set it to "unnamed"
            }

            nametexts[i].text = player.NickName; // Set the player's nickname in the UI
             // Set the player's score in the UI

            if (player.CustomProperties["kills"] != null)
            {
                kdtexts[i].text = player.CustomProperties["kills"] + "/" + player.CustomProperties["deaths"]; // Set the player's kills and deaths in the UI
                scoretexts[i].text = player.CustomProperties["damage"].ToString();
            }
            else
            {
                kdtexts[i].text = "0/0"; // If the player has no kills or deaths, set it to "0/0" 
            }

            i++;

        }

    }
    private void Update()
    {
        playerHolder.SetActive(Input.GetKey(KeyCode.Tab)); // Toggle the visibility of the player holder based on the Tab key input 
    }
}
