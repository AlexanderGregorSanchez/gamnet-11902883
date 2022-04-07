using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooting : MonoBehaviourPunCallbacks
{
    [Header("GUN Related Stuff")]
    public GameObject muzzle;
    public GameObject bulletPrefab;
    public GameObject laserPrefab;
    private Transform muzzleTransform;
    private bool isLaser = false;
    public float fireRate = 0.5f;


    [Header("HP Related Stuff")]
    public float startHealth = 100;
    private float health;

    private bool dead = false;
    private bool shoot = true;

    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        muzzleTransform = muzzle.transform;
        if (muzzle.transform.parent.name.Contains("Laser"))
            isLaser = true;
    }
    private void FixedUpdate()
    {
        if (dead == false && shoot)
        {
            if (Input.GetMouseButton(0))
                Fire();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.root.CompareTag("Bullet"))
        {
            if (!dead)
            {
                string name = collision.gameObject.transform.root.GetComponent<BulletMovment>().nameOfKiller;
                Destroy(collision.transform.parent.gameObject);
                photonView.RPC("TakeDamage", RpcTarget.AllBuffered, 10, name);
            }

        }
    }
    public void Fire()
    {
        StartCoroutine("FireRate", fireRate);
    }

    IEnumerator FireRate(float delay)
    {
        shoot = false;
        

        if (isLaser)
        {
            RaycastHit hit;
            if (Physics.Raycast(muzzleTransform.position, muzzleTransform.TransformDirection(Vector3.forward), out hit, 200))
            {
                photonView.RPC("SpawnLaser", RpcTarget.All, muzzleTransform.position, muzzleTransform.rotation);

                if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 20, photonView.Owner.NickName);
                }
            }
        }
        else
        {
            //Debug.Log("BULLET");
            photonView.RPC("SpawnBullet", RpcTarget.All, muzzleTransform.position, muzzleTransform.rotation);

        }
        yield return new WaitForSeconds(delay);
        shoot = true;
    }

    [PunRPC]
    public void TakeDamage(int value, string killer)
    {
        if (dead == false)
        {
            this.health -= value;
        }



        if (health == 0 && dead == false)
        {
            dead = true;

            GetComponent<PvPDeathEvent>().PlayerKilled(killer);

        }

    }
    [PunRPC]
    public void SpawnLaser(Vector3 muzzlePos, Quaternion muzzleRot)
    {
        GameObject laserGameObject = Instantiate(laserPrefab, muzzlePos, muzzleRot);
        Destroy(laserGameObject, 0.5f);
    }
    [PunRPC]
    public void SpawnBullet(Vector3 muzzlePos, Quaternion muzzleRot)
    {
        GameObject bulletGameObject = Instantiate(bulletPrefab, muzzlePos, muzzleRot);
        bulletGameObject.GetComponent<BulletMovment>().nameOfKiller = photonView.Owner.NickName;
        Destroy(bulletGameObject, 3.0f);
    }

}
