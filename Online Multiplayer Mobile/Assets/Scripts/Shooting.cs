using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Camera camera;
    public GameObject hitEffectPrefab;


    [Header("HP Related Stuff")]
    public float startHealth = 100;
    private float health;
    public Image healthBar;

    private Animator animator;

    public int winThreshold = 10;
    private int killCount;
    private bool dead = false;

    public GameObject killFeedPrefab;
    public GameObject killFeedParent;

    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.fillAmount = health / startHealth;

        animator = this.GetComponent<Animator>();

        photonView.RPC("ShowKillFeed", RpcTarget.All, "lkjlkj", "asdasd");
    }

    // Update is called once per frame
    void Update()
    {
        if (killCount == winThreshold)
        {
            photonView.RPC("Win", RpcTarget.All);
            killCount = 0;
        }

    }
    [PunRPC]
    public void Win()
    {
        GameObject winText = GameObject.Find("Win Text");
        winText.GetComponent<Text>().text = photonView.Owner.NickName + " has " + winThreshold + " kills and [WON]!";
    }

    public void Fire()
    {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out hit, 200))
        {
            photonView.RPC("CreateHitEffects", RpcTarget.All, hit.point);
            if (hit.collider.gameObject.GetComponent<Shooting>().dead == false)
            {
                if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25);

                    if (hit.collider.gameObject.GetComponent<Shooting>().health <= 0)
                    {
                        dead = true;
                        AddToKillCount();
                    }
                }
            }

        }
    }
    [PunRPC]
    public void TakeDamage(int dmg, PhotonMessageInfo info)
    {
        this.health -= dmg;
        this.healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            Die();
            ShowKillFeed(info.Sender.NickName, info.photonView.Owner.NickName);
        }
    }

    public void AddToKillCount()
    {
        killCount++;
    }
    public void ShowKillFeed(string killer, string victim)
    {
        KillFeed.instance.AddNewKillListing(killer, victim);
    }

    [PunRPC]
    public void CreateHitEffects(Vector3 pos)
    {
        GameObject hitEffectGameObject = Instantiate(hitEffectPrefab, pos, Quaternion.identity);
        Destroy(hitEffectGameObject, 0.2f);
    }

    public void Die()
    {
        if (photonView.IsMine)
        {
            animator.SetBool("isDead", true);
            StartCoroutine(RespawnCountDown());
        }
    }

    IEnumerator RespawnCountDown()
    {
        GameObject respawnText = GameObject.Find("Respawn Text");
        float respawnTime = 5.0f;

        while (respawnTime > 0)
        {
            dead = true;
            yield return new WaitForSeconds(1.0f);
            respawnTime--;

            transform.GetComponent<PlayerMovementController>().enabled = false;
            respawnText.GetComponent<Text>().text = "You were Killed. Respawning in " + respawnTime.ToString(".00");
        }

        dead = false;

        animator.SetBool("isDead", false);
        respawnText.GetComponent<Text>().text = "";

        this.transform.position = GameMgr.instance.spawnPointList[Random.Range(0, GameMgr.instance.spawnPointHolder.transform.childCount)].transform.position;
        transform.GetComponent<PlayerMovementController>().enabled = true;

        photonView.RPC("RegainHealth", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void RegainHealth()
    {
        health = 100;
        healthBar.fillAmount = health / startHealth;
    }


}
