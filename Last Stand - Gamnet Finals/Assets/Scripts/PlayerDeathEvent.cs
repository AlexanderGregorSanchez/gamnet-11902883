using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerDeathEvent : MonoBehaviourPunCallbacks
{
    private int playerLeft;
    public bool go;
    private bool allplayer = false;

    Text winnerText;

    // Start is called before the first frame update
    void Start()
    {
        winnerText = GameMgr.instance.timerText;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerLeft > 1 && !allplayer)
        {
            allplayer = true;
            go = true;

        }


        if (gameObject.transform.position.y <= -2.0f)
        {
            Death();
        }

        if (playerLeft == 1 && GetComponent<PlayerBehavior>().isDead == false && go)
        {
            go = false;
            photonView.RPC("LastMan", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }
    }

    [PunRPC]
    public void LastMan(string name)
    {
        GetComponent<CountdownMgr>().enabled = false;
        winnerText.text = name + " is the Last Man Standing";
        winnerText.color = Color.green;
        DiablePlayer();
    }

    public void Death()
    {
        DiablePlayer();

        GetComponent<PlayerBehavior>().isDead = true;

        photonView.RPC("UpdatePlayerCount", RpcTarget.AllBuffered, -1);
    }

    [PunRPC]
    public void UpdatePlayerCount(int value)
    {
        playerLeft += value;
    }

    void DiablePlayer()
    {
        GetComponentInChildren<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().mass = 0.0f;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponentInChildren<MeshRenderer>().enabled = false;

        GetComponent<CarMovement>().enabled = false;

        gameObject.transform.position = GameMgr.instance.spectatorCameraPoint.transform.position;
        gameObject.transform.rotation = GameMgr.instance.spectatorCameraPoint.transform.rotation;
    }
}
