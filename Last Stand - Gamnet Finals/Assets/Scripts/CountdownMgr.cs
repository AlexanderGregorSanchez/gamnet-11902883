using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CountdownMgr : MonoBehaviourPunCallbacks
{
    Text timerText;
    float timeToNextRound;
    bool go;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GameMgr.instance.timerText;
        timeToNextRound = GameMgr.instance.startTime;
        go = true;
    }

    public void RestartTimer()
    {
        go = true;
        timeToNextRound = GameMgr.instance.startTime;
    }

    // Update is called once per frame
    void Update()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            if (timeToNextRound > 0)
            {
                timeToNextRound -= Time.deltaTime;
                photonView.RPC("SetTime", RpcTarget.AllBuffered, timeToNextRound);
            }
            else if (timeToNextRound <= 0)
            {
                photonView.RPC("ZoneCheck", RpcTarget.AllBuffered);

                if (go)
                {
                    go = false;
                    GetComponent<RoundEvent>().StartRound();
                }

            }


        }

    }

    [PunRPC]
    public void SetTime(float time)
    {
        if (timerText != null)
        {
            if (time > 0)
            {
                timerText.text = time.ToString("F1");
                timerText.color = Color.black;
            }

            else
            {
                timerText.text = "DANGER!!!";
                timerText.color = Color.red;
            }

        }

    }

    [PunRPC]
    public void ZoneCheck()
    {

        if (!GetComponent<PlayerBehavior>().GetInZone() && !GetComponent<PlayerBehavior>().isDead)
        {
            //Debug.Log("ZoneCheck");
            GetComponent<PlayerDeathEvent>().Death();
        }

        //if (PhotonNetwork.IsMasterClient)
        //    GetComponent<RoundEvent>().StartRound();

    }
}
