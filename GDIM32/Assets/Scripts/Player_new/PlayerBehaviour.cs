using UnityEngine;
using Photon.Pun;

public class PlayerBehaviour : MonoBehaviour
{
    private PlayerStats stats;

    #region Var: Combat
    [Header("Combat System")]
    public Transform m_FireTransform;
    public float fire_break = 3.0f;

    private float fire_lastTime;
    private float fire_curTime;
    #endregion

    #region Var: Movement
    [Header("Movement System")]
    private FOV Fov;
    public Transform currentFacing;      // Current Direction that Player is facing (used for FOV)

    private Rigidbody2D m_Rigidbody;
    private float m_MovementInputVertiValue;
    private float m_MovementInputHoriValue;
    #endregion

    #region Audio Component
    [Header("Audio")]
    public AudioSource ShootAS;
    public AudioClip Fire_AudioClip;
    public AudioSource MoveAS;
    public AudioClip Walk_AudioClip;
    #endregion

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();    
    }

    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputVertiValue = 0f;
        m_MovementInputHoriValue = 0f;
    }

    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(stats.Type == PlayerStats.PlayerType.Sheild)
        {
            Fov = GameObject.FindGameObjectWithTag("FOV_Sheild").GetComponent<FOV>();
        }
        else
        {
            Fov = GameObject.FindGameObjectWithTag("FOV_Gun").GetComponent<FOV>();
        }
        Vector3 targetPosition = currentFacing.position;
        Vector3 aimDir = (targetPosition - transform.position).normalized;
        Vector3 origin = transform.position;
        Fov.SetAimDirection(aimDir);
        Fov.SetOrigin(origin);

        MoveAS.clip = Walk_AudioClip;
    }

    // Update is called once per frame
    void Update()
    {
        // record the current time
        fire_curTime = Time.time;

        Vector3 targetPosition = currentFacing.position;
        Vector3 aimDir = (targetPosition - transform.position).normalized;
        Vector3 origin = transform.position;
        Fov.SetAimDirection(aimDir);
        Fov.SetOrigin(origin);

        WalkAudio();
    }

    public void Fire(bool isFire)
    {
        if (!isFire) { return; }
        if (fire_curTime - fire_lastTime < fire_break) { return; }
        // m_Fired = true;

        if (this.transform.tag == "Player1")
        {
            GameObject bullet = PhotonNetwork.Instantiate("Bullet_Sheild", m_FireTransform.position, m_FireTransform.rotation);
            //bullet.GetComponent<PhotonView>().RPC("SetBulletVolecity", RpcTarget.All, stats.bulletSpeed * m_FireTransform.right);
            bullet.GetComponent<Rigidbody2D>().velocity = stats.bulletSpeed * m_FireTransform.right;
        }
        if (this.transform.tag == "Player2")
        {
            GameObject bullet = PhotonNetwork.Instantiate("Bullet_Gun", m_FireTransform.position, m_FireTransform.rotation);
            //bullet.GetComponent<PhotonView>().RPC("SetBulletVolecity", RpcTarget.All, stats.bulletSpeed * m_FireTransform.right);
            bullet.GetComponent<Rigidbody2D>().velocity = stats.bulletSpeed * m_FireTransform.right;
        }

        // Play the clip
        ShootAS.clip = Fire_AudioClip;
        ShootAS.Play();

        // Record the time
        fire_lastTime = Time.time;
    }

    public void Move(Vector3 inputVec)
    {
        m_Rigidbody.velocity = inputVec * stats.MoveSpeed * Time.deltaTime;
    }

    public void Turn()
    {
        //float turn = - (m_TurnInputValue * m_TurnSpeed * Time.deltaTime);
        //transform.Rotate(Vector3.forward * turn);        
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        Vector3 dir = (mousePoint - transform.position);
        dir = new Vector3(dir.x, dir.y, 0f).normalized;
        float theta = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg;
        theta = dir.x < 0f ? 180f + theta : theta;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, theta);

    }

    private void WalkAudio()
    {
        // If there is no input:
        if (Mathf.Abs(m_MovementInputVertiValue) < 0.1f && Mathf.Abs(m_MovementInputHoriValue) < 0.1f)
        {
            if (MoveAS.isPlaying)
            {
                MoveAS.Stop();
            }
        }
        else
        {
            if (!MoveAS.isPlaying)
            {
                MoveAS.Play();
            }
        }
    }
}
