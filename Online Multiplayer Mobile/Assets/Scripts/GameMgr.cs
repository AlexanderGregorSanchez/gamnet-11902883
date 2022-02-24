using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameMgr : MonoBehaviour
{
    public static GameMgr instance;

    public GameObject playerPrefab;

    public GameObject spawnPointHolder;
    public List<GameObject> spawnPointList;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnPointHolder.transform.childCount; i++)
        {
            spawnPointList.Add(spawnPointHolder.transform.GetChild(i).gameObject);
        }

        if (PhotonNetwork.IsConnectedAndReady)
        {
            Vector3 randPoint = spawnPointList[Random.Range(0, spawnPointHolder.transform.childCount)].transform.position;
            PhotonNetwork.Instantiate(playerPrefab.name, randPoint, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
