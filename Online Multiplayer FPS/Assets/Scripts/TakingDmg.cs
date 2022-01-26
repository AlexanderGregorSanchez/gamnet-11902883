using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TakingDmg : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Image healthBar;

    public float startHealth = 100;
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
    }

    [PunRPC]
    public void TakeDmg(int dmg)
    {
        health -= dmg;
        Debug.Log(health);

        healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (photonView.IsMine)
        {
            GameMgr.instance.LeaveRoom();
        }
       
    }
}
