using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joyStick_Handle : MonoBehaviour
{
    public RectTransform m_handle;
    public Vector3 m_vecDir;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.localPosition = m_handle.localPosition;
        //Debug.Log(rect.localPosition);
        m_vecDir = rect.localPosition;
    }
}

public class CopyOfJoystick : MonoBehaviour
{
    public RectTransform m_handle;
    public Vector3 m_vecDir;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.position = m_handle.position;

        m_vecDir = rect.localPosition - Vector3.zero;
    }
}