using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CountdownMgr : MonoBehaviourPunCallbacks
{
    Text timerText;

    float timeToNextRound;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GameMgr.instance.timerText;
        timeToNextRound = GameMgr.instance.startTime;

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
                photonView.RPC("ZoneCheck", RpcTarget.AllBuffered);
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
    public void ZoneCheck()
    {
        if (!GetComponent<PlayerBehavior>().GetInZone())
        {
            GetComponent<PlayerDeathEvent>().Death();
        }
    }
}
