using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
public class RoundEvent : MonoBehaviour
{
    public enum RaiseEventCode
    {
        RoundStartCode = 0,
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventCode.RoundStartCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int index = (int)data[0];

            if (GetComponent<ZoneSpawner>() != null)
            {
                //Debug.Log("OnEvent()");
                GetComponent<ZoneSpawner>().DestroyActiveZone(index);
                
            }
            if (GetComponent<CountdownMgr>() != null)
            {
                GetComponent<CountdownMgr>().RestartTimer();
            }

        }
    }

    public void StartRound()
    {
        int index = 0;

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            index = (int)Random.Range(0, GameMgr.instance.ZonePoints.Length);
        }

        object[] data = new object[] { index };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOptions = new SendOptions
        {
            Reliability = false
        };

        PhotonNetwork.RaiseEvent((byte)RaiseEventCode.RoundStartCode, data, raiseEventOptions, sendOptions);

    }
}
