using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    //몬스터가 출현할 위치를 담을 배열
    public Transform[] m_point;

    //몬스터 프리팹을 넣을 변수
    public GameObject m_monsPrefab;

    //몬스터의 발생 갯수
    public int m_maxMonster = 4;


    // Use this for initialization
    void Start()
    {
        CreateMonster();
    }

    void CreateMonster()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 position = m_point[i].position;
            Instantiate(m_monsPrefab, position, Quaternion.identity);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}