using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterCtrl : MonoBehaviour
{
    //public enum MonsterStatus { Idle, Run, Attack, Damage, Death };
    //public MonsterStatus status = MonsterStatus.Idle;

    public Rigidbody m_rigidbody;
    public Transform m_target;
    NavMeshAgent m_cNavMeshAgent;
    public Animator m_animator;
    PlayerCtrl m_playerCtrl;
    public SphereCollider m_mAtk;
    public BoxCollider m_mBox;
    public GameObject m_headPos;

    // 사정거리
    public float m_fMinDist;

    //몬스터 스테이더스
    public float m_maxHp = 60;
    public float m_monsPower = 10;
    public Image m_hpBar;

    //몬스터 사망여부
    private bool m_isDeth = false;

    // Use this for initialization
    void Start()
    {
        //자신의 하위에 있는 Rigidbody 컴포넌트를 찾아와서 변수 m_rigidbody에 넣음.
        m_rigidbody = GetComponent<Rigidbody>();
        m_cNavMeshAgent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
        GameObject m_player = GameObject.Find("Player");

        m_target = m_player.GetComponent<Transform>();
        m_playerCtrl = m_player.GetComponent<PlayerCtrl>();
        //StartCoroutine(moveMonster());

        //몬스터 축 고정
        transform.eulerAngles = new Vector3(transform.rotation.x, 180.0f, transform.rotation.z);
    }

    // Update is called once per frame
    void Update()
    {
        TrackingTarget();
        MonsterDead();  
    }

    /*IEnumerator moveMonster()
    {
        while (true)
        {
            float dir1 = Random.Range(-1.0f, 1.0f);
            float dir2 = Random.Range(-1.0f, 1.0f);

            yield return new WaitForSeconds(2);
            m_rigidbody.velocity = new Vector3(dir1, 0, dir2);
        }
    }*/

    //Collider가 다른 트리거 이벤트에 침입했을 때 OnTriggerEnter가 호출됩니다.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_mAtk.enabled = true; //헤드 콜라이드가 켜짐.
            m_animator.SetTrigger("M_Atk");
        }

        if (other.gameObject.tag == "pAtk")
        {
            m_animator.SetTrigger("M_Dmg");
            m_hpBar.fillAmount -= m_playerCtrl.m_playerPower / m_maxHp;
        }

        if (m_hpBar.fillAmount <= 0)
        {
            MonsterDeth();
        }

        if (m_playerCtrl.m_hpBar.fillAmount <= 0)
        {
            m_mAtk.enabled = false;
            m_mBox.enabled = false;
        }
    }

    void MonsterDeth()
    {
        m_isDeth = true;
        //status = MonsterStatus.Death;
        m_cNavMeshAgent.enabled = false;
        m_animator.SetTrigger("M_Deth");
            
        foreach(Collider coll in gameObject.GetComponentsInChildren<BoxCollider>())
        {
            coll.enabled = false;
        }
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }
    }

    void MonsterDead()
    {
        AnimatorClipInfo[] aniClipinfo = m_animator.GetCurrentAnimatorClipInfo(0);
        AnimatorStateInfo aniStatusInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        if (aniClipinfo.Length > 0)
        {

            if (aniClipinfo[0].clip.name == "Death" && aniStatusInfo.normalizedTime >= 1.6)
            {
                gameObject.SetActive(false);
            }
        }
    }


    //트리거가 다른 Collider에 계속 닿아 있는 동안 "거의" 매 프레임 호출됩니다.
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_animator.SetTrigger("M_Atk");
        }
    }

    //Collider가 /other/의 트리거에 닿는 것을 중지했을 때 호출됩니다.
    void OnTriggerExit(Collider other)
    {

    }

    void TrackingTarget()
    {
        //몬스터 자신의 위치
        Vector3 v_Pos = transform.position;
        //타겟의 위치
        Vector3 v_TargetPos = m_target.position;
        Vector3 dir = v_TargetPos - v_Pos; //사이의 거리
        dir.Normalize();
        //몬스터와 타겟 사이의 거리
        float fDist = Vector3.Distance(v_Pos, v_TargetPos);
        Ray ray = new Ray(v_Pos, dir);
        RaycastHit hitinfo;

        //몬스터와 타겟의 거리만큼 광선을 쏠때
        if (Physics.Raycast(ray, out hitinfo, fDist))
        {
            //몬스터와 타겟 사이의 거리가 m_fMinDist(5)보다 작아졌을때,
            if (fDist < m_fMinDist)
            {
                //부딪힌 태그가 Room일때
                if (hitinfo.collider.gameObject.tag == "Room")
                {
                    m_cNavMeshAgent.enabled = false;
                    m_animator.SetBool("M_Run", false);
                }
                //Room이 아니면
                else
                {
                    //달리면서 플레이어를 추적한다.
                    m_cNavMeshAgent.enabled = true;
                    m_headPos.SetActive(true);
                    m_animator.SetBool("M_Run", true);
                    m_cNavMeshAgent.SetDestination(v_TargetPos);
                }

                if(m_playerCtrl.m_hpBar.fillAmount <= 0)
                {
                    m_animator.SetBool("M_Run", false);
                }
            }
            //몬스터와 플레이어 사이의 거리가 m_fMinDist(5)보다 커지면,
            else
            {
                m_headPos.SetActive(false);
                m_animator.SetBool("M_Run", false);
                m_animator.SetBool("M_Atk", false);
            }
        }
    }
}