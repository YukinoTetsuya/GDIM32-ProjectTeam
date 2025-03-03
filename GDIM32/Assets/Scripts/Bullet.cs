using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Author: Jiefu Ling (jieful2); Yurui Leng(yuruil)
// This script is used to define bullet behaviour
// Bullet should cross the gameobject that generate it. Should cause damage to enemy and friend
// Bullet should destroy when it hit something else.

public class Bullet : MonoBehaviour
{
    public float Damage = 30f;

    private AudioSource AS;
    public AudioClip HitOnWall;
    private Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        AS = GameObject.Find("SoundSystem").GetComponent<AudioSource>();
        if (AS == null)
        {
            Debug.Log("G");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name == "Shield Collider")
            PhotonNetwork.Destroy(this.gameObject);

        if (collision.gameObject.tag != this.tag)
        {
            if (collision.gameObject.name.StartsWith("Enemy"))
            {

                //PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

                //playerHealth.TakeDamage(Damage);
                collision.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, Damage);                
            }

            if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
            {
                //collision.GetComponent<PlayerStats>().TakeDamage(Damage);
                collision.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, Damage);
            }

            if (collision.gameObject.tag != "FOV_Sheild" && collision.gameObject.tag != "FOV_Gun" && !collision.gameObject.name.StartsWith("Bullet"))
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (this.gameObject.tag != "Enemy")
        {
            if (collision.gameObject.tag == "Map")
            {
                AS.clip = HitOnWall;
                AS.Play();
            }
        }
    }

}
