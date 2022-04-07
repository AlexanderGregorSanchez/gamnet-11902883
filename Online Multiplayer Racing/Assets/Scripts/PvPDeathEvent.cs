using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PvPDeathEvent : MonoBehaviourPunCallbacks
{
    private bool go = true;
    public enum RaiseEventCode
    {
        WhoDiedEventCode = 0,
        LastMan = 0

    }

    private int playersLeft = 3;
    private string lastManName = "NOT PASSED";

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventCode.WhoDiedEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            string nickNameOfKilledPlayer = (string)data[0];
            playersLeft = (int)data[1];
            lastManName = (string)data[2];


            GameObject orderUiText = DeathRaceGameMgr.instance.finisherTexts[playersLeft];
            orderUiText.SetActive(true);

            orderUiText.GetComponent<Text>().text =  nickNameOfKilledPlayer + " was killed";
            orderUiText.GetComponent<Text>().color = Color.red;

            if (playersLeft <= 1)
                LastManStanding();

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playersLeft = PhotonNetwork.CountOfPlayers;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //[PunRPC]
    public void LastManStanding()
    {

        Debug.Log("WINNER");
        GameObject winnerUiText = DeathRaceGameMgr.instance.countdownText.gameObject;
        winnerUiText.GetComponent<Text>().text = lastManName + " is The Last Man Standing";
        winnerUiText.GetComponent<Text>().color = Color.green;


    }

    public void PlayerKilled(string killer)
    {
        photonView.RPC("DeactivatePlayer", RpcTarget.All, photonView.ViewID);
        playersLeft--;
        string nickName = photonView.Owner.NickName;

        object[] data = new object[] {nickName, playersLeft, killer};

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOptions = new SendOptions
        {
            Reliability = false
        };

        PhotonNetwork.RaiseEvent((byte)RaiseEventCode.WhoDiedEventCode, data, raiseEventOptions, sendOptions);
    }


    [PunRPC]
    public void DeactivatePlayer(int id)
    {
        if (id == photonView.ViewID)
        {
            GetComponent<Collider>().enabled = false;
            //DeathRaceGameMgr.instance.alivePlayers -= 1;
            GetComponent<PlayerSetup>().playerCamera.transform.parent = null;
            GetComponent<CarMovement>().enabled = false;
            
            Destroy(this, 1.0f);

        }
    }
}
