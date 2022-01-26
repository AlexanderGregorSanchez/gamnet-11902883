using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LaunchMgr : MonoBehaviourPunCallbacks
{
    public GameObject EnterGamePanel;
    public GameObject ConnectingStatusPanel;
    public GameObject LobbyPanel;


    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        EnterGamePanel.SetActive(true);
        ConnectingStatusPanel.SetActive(false);
        LobbyPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NickName + " Connected to Photon servers");
        ConnectingStatusPanel.SetActive(false); 
        LobbyPanel.SetActive(true);
    }

    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning(message);
        CreateAndJoinRandomRoom();
    }

    public void ConnectToPhotonServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            EnterGamePanel.SetActive(false);
            ConnectingStatusPanel.SetActive(true);
        }
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
        
    }

    private void CreateAndJoinRandomRoom()
    {
        string randRoomName = "Room " + Random.Range(0, 10000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 20;

        PhotonNetwork.CreateRoom(randRoomName, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " has entered " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("Game Scene");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " has entered " + PhotonNetwork.CurrentRoom.Name + ". Room now has " + PhotonNetwork.CurrentRoom.PlayerCount + " players");
    }
}

