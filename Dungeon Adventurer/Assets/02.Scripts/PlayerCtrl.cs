using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    public Rigidbody m_rigidbody;
    public float m_speed = 5.0f; //이동속도
    public float m_rotSpeed = 1.0f; //회전속도
    public float m_jumpPower = 4.0f; //점프파워

    public bool m_isJumping = false;
    public bool m_isPunching = false;
    public bool m_isKicking = false;

    public CapsuleCollider m_Rpunch;
    public CapsuleCollider m_Rkick;
    public BoxCollider m_pBox;
    public Animator m_animator;
    MonsterCtrl m_monsCtrl;

    //플레이어 스테이더스
    public float m_maxHp = 100;
    public float m_playerPower = 10;
    public Image m_hpBar;

    //플레이어 사망여부
    private bool m_isDeth = false;

    // Use this for initialization
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
        GameObject m_monster = GameObject.Find("Monster");
        m_monsCtrl = m_monster.GetComponent<MonsterCtrl>();
    }

    void FixedUpdate()
    {
        Moving(); //캐릭터 좌우상하 이동
        Punch(); //펀치 모션
        Kick(); //킥 모션
        PlayerDead(); //죽는 스크립트   
    }

    void Update()
    {
        if (m_isJumping == true) //점프
        {
            m_animator.SetBool("P_Jump", true);
            m_isJumping = false;
        }
        else
        {
            m_animator.SetBool("P_Jump", false);
        }

        if (m_isPunching == true) //펀치
        {
            m_Rpunch.enabled = true;
            m_animator.SetTrigger("P_Punch");
            m_isPunching = false;
        }
        else
        {
            m_isPunching = false;
            m_Rpunch.enabled = false;
        }

        if(m_isKicking == true) //킥
        {
            m_Rkick.enabled = true;
            m_animator.SetTrigger("P_Kick");
            m_isKicking = false;
        }
        else
        {
            m_isKicking = false;
            m_Rkick.enabled = false;
        }
    }

    public void JButton()//점프버튼
    {
        m_isJumping = true;
        m_rigidbody.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
        //m_rigidbody.AddForce(0, 300, 0);
    }

    void Punch()
    {
        if (Input.GetMouseButtonDown(0)) //마우스 좌클릭
        {
            m_Rpunch.enabled = true; //콜라이더 떳다 켜기
            m_animator.SetTrigger("P_Punch");
        }

        if (Input.GetMouseButtonUp(0)) //(0) 마우스 좌클릭을 땜
        {
            m_Rpunch.enabled = false;
        }
    }

    public void Pbutton() //펀치버튼
    {
        m_isPunching = true;
    }

    void Kick()
    {
        if (Input.GetMouseButtonDown(1)) //마우스 우클릭
        {
            m_Rkick.enabled = true;
            m_animator.SetTrigger("P_Kick");
        }

        if (Input.GetMouseButtonUp(1)) //마우스 우클릭을 땜
        {
            m_Rkick.enabled = false;
        }
    }

    public void Kbutton() //킥버튼
    {
        m_isKicking = true;
    }

    void PlayerDeth()
    {
        m_isDeth = true;
        m_pBox.enabled = false;
        m_animator.SetBool("P_Deth", true);

        foreach (Collider coll in gameObject.GetComponentsInChildren<CapsuleCollider>())
        {
            coll.enabled = false;
        }
    }

    void PlayerDead()
    {
        AnimatorClipInfo[] aniClipinfo = m_animator.GetCurrentAnimatorClipInfo(0);
        AnimatorStateInfo aniStatusInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        if (aniClipinfo.Length > 0)
        {
            if (aniClipinfo[0].clip.name == "Dead" && aniStatusInfo.normalizedTime >= 2.5)
            {
                //gameObject.SetActive(false);
                SceneManager.LoadScene("gameOver_Scene");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    { //other.gameObject.tag == "mAtk"
        //몬스터랑 부딪히면 데미지를 입음.
        if (other.gameObject.tag == "mAtk")
        {
            m_animator.SetTrigger("P_Dmg");
            m_hpBar.fillAmount -= m_monsCtrl.m_monsPower / m_maxHp;
        }

        if (m_hpBar.fillAmount <= 0)
        {
            PlayerDeth();
        }
        //Debug.Log("" + gameObject.name);
    }

    void OnTriggerStay(Collider other)
    {

    }

    void Moving() //키 입력으로 캐릭터를 이동하자.
    {
        //이동거리
        float moveDis = m_speed * Time.deltaTime;

        //W키(앞으로)를 눌렀을때.
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveDis);
            m_animator.SetBool("P_Run", true);
        }
        else m_animator.SetBool("P_Run", false);    

        //S(뒤로)키를 눌렀을때
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * moveDis);
        }

        //A(왼쪽으로)키를 눌렀을때
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * -m_rotSpeed);
        }

        //D(오른쪽으로)키를 눌렀을때
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * m_rotSpeed);
        }

        //스페이스(점프)를 눌렀을때
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            //m_rigidbody.AddForce(0, 300, 0);
            m_isJumping = true;
            m_rigidbody.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
            //m_animator.SetBool("P_Jump", true);
        }
        //else m_animator.SetBool("P_Jump", false);
    }
    /*Vector3 vDirPos;

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + vDirPos * 5 , 1);
    }*/
   
    //회전 조이스틱 벡터
    public void Joystick_Rot_Move(Vector3 vJoystickRotMove)
    {
        if (vJoystickRotMove.magnitude > 0.15f) //조이스틱이 %이상 움직였을때
        {
            if (Input.GetMouseButton(0))//마우스(터치)를 클릭상태하고 있을때
            {
                //Debug.Log("vJoysickMove:" + vJoystickRotMove);
                Vector3 vDirPos = new Vector3(vJoystickRotMove.x, 0, vJoystickRotMove.y); // * 10;
                transform.LookAt(vDirPos); //바라보는 곳으로 회전
                //Debug.Log("PreVector:" + vDirPos);
            }
        }
    }

    //이동 조이스틱 벡터
    public void Joystick_UpDown_Move(Vector3 vUpDown_Move)
    {
        if (vUpDown_Move.magnitude > 0.1f) //조이스틱이 10%이상 움직였을때
        {
            if (Input.GetMouseButton(0))//마우스(터치)를 클릭상태하고 있을때
            {
                
                Debug.Log("vUpDown_Move:" + vUpDown_Move);
                if (vUpDown_Move.y > 0)
                {
                    transform.Translate(Vector3.forward * m_speed * Time.deltaTime); //앞으로 이동
                    m_animator.SetBool("P_Run", true);
                }
                else if(vUpDown_Move.y < 0)
                {
                    transform.Translate(Vector3.back * m_speed * Time.deltaTime); //뒤로 이동
                    m_animator.SetBool("P_Run", false);
                }
                //vDirPos = transform.position + vDirPos;
                //transform.LookAt(vDirPos); //바라보는 곳으로 회전
                //Debug.Log("PreVector:" + vDirPos);
            }
        }
    }

    /*void Run()
    {
        m_movement.Set(m_hMove, 0, m_vMove);
        m_movement = m_movement.normalized * m_speed * Time.deltaTime;
        m_rigidbody.MovePosition(transform.position + m_movement);
    }

    void Rotation()
    {
        if (m_hMove == 0 && m_vMove == 0)
            return;

        Quaternion newRotation = Quaternion.LookRotation(m_movement);

        m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, newRotation, m_rotSpeed * Time.deltaTime);
    }

    void AnimationUpdate()
    {
        if(m_hMove == 0 && m_vMove == 0)
        {
            m_animator.SetBool("P_Run", false);
        }
        else
        {
            m_animator.SetBool("P_Run", true);
    }*/
}
