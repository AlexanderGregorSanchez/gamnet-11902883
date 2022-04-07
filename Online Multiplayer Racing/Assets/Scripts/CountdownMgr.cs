using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CountdownMgr : MonoBehaviourPunCallbacks
{
    public Text timerText;

    public float timerToStartRace = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        //timerText = RacingGameMgr.instance.countdownText;

        if (RacingGameMgr.instance == null)
        {
            Debug.Log("DEATH GAME");
            timerText = DeathRaceGameMgr.instance.countdownText;
        }
        else if (DeathRaceGameMgr.instance == null)
        {
            Debug.Log("RACE GAME");
            timerText = RacingGameMgr.instance.countdownText;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (timerToStartRace > 0)
            {
                timerToStartRace -= Time.deltaTime;
                photonView.RPC("SetTime", RpcTarget.AllBuffered, timerToStartRace);
            }
            else if (timerToStartRace <= 0)
            {
                photonView.RPC("StartRace", RpcTarget.AllBuffered);
            }
        }

    }

    [PunRPC]
    public void SetTime(float time)
    {
        if (timerText != null)
        {
            if (time > 0)
                timerText.text = time.ToString("F1");
            else
                timerText.text = "";
        }

    }

    [PunRPC]
    public void StartRace()
    {
        Debug.Log("START");
        GetComponent<CarMovement>().isControlEnabled = true;
        this.enabled = false;
    }
}
