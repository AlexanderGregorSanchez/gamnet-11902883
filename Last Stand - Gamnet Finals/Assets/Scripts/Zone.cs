using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{

    public float size;

    List<GameObject> playersInZone;

    // Start is called before the first frame update
    void Start()
    {
        playersInZone = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.CompareTag("Player"))
        {
            //Debug.Log("ENTER()");
            other.gameObject.transform.parent.GetComponent<PlayerBehavior>().SetInZone(true);
            playersInZone.Add(other.gameObject.transform.parent.gameObject);
            //Destroy(this.gameObject, 5.0f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.parent.CompareTag("Player"))
        {
            //Debug.Log("EXIT()");
            playersInZone.Remove(other.gameObject.transform.parent.gameObject);
            other.gameObject.transform.parent.GetComponent<PlayerBehavior>().SetInZone(false);
        }

    }
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale = new Vector3(size, size, size);
    }

    public void SetSize(float value)
    {
        size = value;
    }

    private void OnDestroy()
    {
        //Debug.Log("Destroy Zone");
        if (playersInZone.Count > 0)
        {
            foreach (GameObject player in playersInZone)
            {
                if (player != null)
                    player.GetComponent<PlayerBehavior>().SetInZone(false);
            }
        }

    }
}
