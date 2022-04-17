using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameMgr : MonoBehaviour
{
    public static GameMgr instance;
    public float startTime;
    public Text timerText;
    public GameObject playerPrefab;
    public GameObject[] SpawnPoints;
    public GameObject[] ZonePoints;
    public GameObject spectatorCameraPoint;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("regular"))
        {
            startTime = 10.0f;
        }
        else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("hard"))
        {
            startTime = 8.0f;
        }



        if (PhotonNetwork.IsConnectedAndReady)
        {
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Vector3 spawnPos = SpawnPoints[actorNumber - 1].transform.position;
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPos, Quaternion.Euler(SpawnPoints[actorNumber - 1].transform.rotation.eulerAngles));
        }
    }



}
