using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        this.playerCamera = transform.Find("Camera").GetComponent<Camera>();

        GetComponent<CarMovement>().enabled = photonView.IsMine;
        //GetComponent<CountdownMgr>().enabled = photonView.IsMine;
        //GetComponent<PlayerDeathEvent>().enabled = photonView.IsMine;
        playerCamera.enabled = photonView.IsMine;
        GetComponentInChildren<AudioListener>().enabled = photonView.IsMine;

        GetComponent<PlayerDeathEvent>().photonView.RPC("UpdatePlayerCount", RpcTarget.AllBuffered, 1);
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
