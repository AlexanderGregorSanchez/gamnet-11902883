using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBehavior : MonoBehaviour
{

    bool isInZone = false;
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetInZone(bool status)
    {
        isInZone = status;
    }
    public bool GetInZone()
    {
        return isInZone;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
