using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Jiefu Ling (jieful2), Xiao Jing (xjing2), Liujiahao Xie(liujiahx), Yurui Leng(yuruil), Yuhao Song (yuhaos5)
// This script is used to control player's firing.

public class PlayerShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public Rigidbody2D m_bullet;
    public Transform m_FireTransform;
    public float bulletSpeed = 10.0f;
    public float fire_break = 3.0f;

    private string m_FireButton;
    // private bool m_Fired;
    private float fire_lastTime;
    private float fire_curTime;

    // Audio Component
    public AudioSource AS;
    public AudioClip Fire_AudioClip;

    // Start is called before the first frame update
    void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;
    }

    // Update is called once per frame
    void Update()
    {
        // record the current time
        fire_curTime = Time.time;

        // If player press m_fireButton and the time exceed the break: Fire
        if (Input.GetButtonDown(m_FireButton) && fire_curTime - fire_lastTime >= fire_break)
        {
            Fire();
        }
    }

    private void Fire()
    {
        // m_Fired = true;

        Rigidbody2D shellInstance = 
            Instantiate(m_bullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody2D;

        // set the velocity
        shellInstance.velocity = bulletSpeed * m_FireTransform.right;

        // set the tag
        shellInstance.tag = this.tag;

        // Play the clip
        AS.clip = Fire_AudioClip;
        AS.Play();

        // Record the time
        fire_lastTime = Time.time;
    }
}
