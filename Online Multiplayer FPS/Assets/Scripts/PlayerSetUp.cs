using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetUp : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject cameraPOV;

    [SerializeField]
    TextMeshProUGUI playerNameText;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<MovementController>().enabled = true;
            cameraPOV.GetComponent<Camera>().enabled = true;
        }
        else
        {
            transform.GetComponent<MovementController>().enabled = false;
            cameraPOV.GetComponent<Camera>().enabled = false;
        }

        playerNameText.text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
