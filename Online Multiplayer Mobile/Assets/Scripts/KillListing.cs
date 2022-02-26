using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillListing : MonoBehaviour
{
    [SerializeField] Text killerDisplay;
    [SerializeField] Text victimDisplay;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5f);
    }

    public void SetNames(string killerName, string victimName)
    {
        killerDisplay.text = killerName;
        victimDisplay.text = victimName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
