using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystick_UpDown : MonoBehaviour {

    public float m_speed = 5.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //joystick_UpDown 벡터
    public void UpDown_Move(Vector3 vUpDown_Move)
    {
        if (vUpDown_Move.magnitude > 0.1f) //조이스틱이 10%이상 움직였을때
        {
            float MoveDist = m_speed * Time.deltaTime;

            if (Input.GetMouseButton(0))//마우스(터치)를 클릭상태하고 있을때
            {
                Debug.Log("vUpDown_Move:" + vUpDown_Move);
                Vector3 vDirPos = new Vector3(vUpDown_Move.x, 0, vUpDown_Move.y).normalized;// * 10;
                vDirPos = transform.position + vDirPos;
                //transform.LookAt(vDirPos); //바라보는 곳으로 회전

                transform.Translate(Vector3.forward * MoveDist); //앞으로 이동
                transform.Translate(Vector3.back * MoveDist); //뒤로 이동
                //Debug.Log("PreVector:" + vDirPos);
            }
        }
    }
}
