using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ZoneSpawner : MonoBehaviour
{
    public GameObject zonePrefabs;

    public float downSizeAmount;

    float sizeStepDown;

    GameObject activeZone;

    int prevSpawnIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("regular"))
        {
            downSizeAmount = 1.0f;
        }
        else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("hard"))
        {
            downSizeAmount = 2.0f;
        }

        SetZoneSpawn(0);
    }

    public void SetZoneSpawn(int index)
    {
        SpawnZone(GameMgr.instance.ZonePoints[index].gameObject.transform.position);
    }

    public void DestroyActiveZone(int index)
    {
        if (index == prevSpawnIndex)
            index++;
        if (index >= GameMgr.instance.ZonePoints.Length)
            index = 0;
        Destroy(activeZone);
        activeZone = null;
        SetZoneSpawn(index);
        prevSpawnIndex = index;
    }

    [PunRPC]
    public void SpawnZone(Vector3 pos)
    {

        if (activeZone == null)
        {
            GameObject zone = Instantiate(zonePrefabs, pos, Quaternion.identity);
            zone.transform.GetComponent<Zone>().size = zone.transform.GetComponent<Zone>().size - sizeStepDown;
            if (zone.transform.GetComponent<Zone>().size < 0.5f)
                zone.transform.GetComponent<Zone>().size = 0.5f;
            activeZone = zone;
            sizeStepDown += downSizeAmount;
            
        }

    }


}
