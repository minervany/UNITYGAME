using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject m_player; //플레이어
    public SpawnManager m_spawnManager; //스폰
    public GUImanager m_guiManager; //gui

    /*static GameManager m_Instance;
    public static GameManager GetInstance()
    {
        return m_Instance;
    }*/

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PlayerCtrl cPlayerCtrl = m_player.GetComponent<PlayerCtrl>();
        cPlayerCtrl.Joystick_Rot_Move(m_guiManager.m_joystick_Rotation.m_vecDir); //회전
        cPlayerCtrl.Joystick_UpDown_Move(m_guiManager.m_joystick_UpDown.m_vecDir); //이동
	}
}
