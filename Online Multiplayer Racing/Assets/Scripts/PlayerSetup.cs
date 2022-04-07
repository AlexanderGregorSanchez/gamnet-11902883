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
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("rc"))
        {
            GetComponent<CarMovement>().enabled = photonView.IsMine;
            GetComponent<LapController>().enabled = photonView.IsMine;
            playerCamera.enabled = photonView.IsMine;
        }
        else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("dr"))
        {
            GetComponent<CarMovement>().enabled = photonView.IsMine;
            GetComponent<Shooting>().enabled = photonView.IsMine;
            GetComponent<LapController>().enabled = false;
            playerCamera.enabled = photonView.IsMine;
        }

        //GetComponent<AudioListener>().enabled = photonView.IsMine;
        GetComponentInChildren<AudioListener>().enabled = photonView.IsMine;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
