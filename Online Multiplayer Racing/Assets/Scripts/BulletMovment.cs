using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovment : MonoBehaviour
{
    float speed = 50;

    public string nameOfKiller; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float translation = speed * Time.deltaTime;

        transform.Translate(0, 0, translation);

    }
}
