using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkedTimer : MonoBehaviourPunCallbacks, IPunObservable
{
    public static NetworkedTimer Instance;
    
    [Header("Timer Settings")]
    public float gameDuration = 180f;
    public TMP_Text timerText;
    public GameObject gameOverPanel;
    
    [SerializeField] private float timeRemaining;
    [SerializeField] private bool timerRunning = false;
    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Only initialize if we're in a room
        if (PhotonNetwork.InRoom)
        {
            InitializeTimer();
        }
    }

    public override void OnJoinedRoom()
    {
        InitializeTimer();
    }

    public void InitializeTimer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            timeRemaining = gameDuration;
            timerRunning = true;
            photonView.RPC("UpdateTimerDisplay", RpcTarget.All, timeRemaining);
        }
        
        gameOverPanel?.SetActive(false);
        gameEnded = false;
    }

    private void Update()
    {
        if (!PhotonNetwork.InRoom || gameEnded) return;
        
        if (PhotonNetwork.IsMasterClient && timerRunning)
        {
            timeRemaining -= Time.deltaTime;
            
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerRunning = false;
                gameEnded = true;
                photonView.RPC("EndGame", RpcTarget.AllBuffered);
            }
            
            // Only send updates periodically to reduce network traffic
            if (Time.frameCount % 30 == 0) // Every ~0.5 seconds
            {
                photonView.RPC("UpdateTimerDisplay", RpcTarget.All, timeRemaining);
            }
        }
    }

    [PunRPC]
    private void UpdateTimerDisplay(float time)
    {
        timeRemaining = time;
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    [PunRPC]
    private void EndGame()
    {
        gameOverPanel?.SetActive(true);
        timerRunning = false;
        gameEnded = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(timeRemaining);
            stream.SendNext(timerRunning);
            stream.SendNext(gameEnded);
        }
        else
        {
            timeRemaining = (float)stream.ReceiveNext();
            timerRunning = (bool)stream.ReceiveNext();
            gameEnded = (bool)stream.ReceiveNext();
        }
    }

    public void ReturnToMenu()
    {
        PhotonNetwork.LeaveRoom();
        gameOverPanel?.SetActive(false);
    }
}