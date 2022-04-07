using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class RacingGameMgr : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public Transform[] startingPositions;
    public GameObject[] finisherTexts;

    public static RacingGameMgr instance;

    public Text countdownText;

    public List<GameObject> lapTriggers = new List<GameObject>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;

            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(Constants.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                Vector3 spawnPos = startingPositions[actorNumber - 1].position;
                PhotonNetwork.Instantiate(carPrefabs[(int)playerSelectionNumber].name, spawnPos, Quaternion.identity);
            }
        }

        foreach (GameObject go in finisherTexts)
        {
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
