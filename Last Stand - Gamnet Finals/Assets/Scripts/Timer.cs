using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Text timerText;
    public float timeToNextRound;


    // Start is called before the first frame update
    void Start()
    {
        timerText = GameMgr.instance.timerText;
        timeToNextRound = GameMgr.instance.startTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
