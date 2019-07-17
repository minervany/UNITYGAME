using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamDontClear : MonoBehaviour {
    [SerializeField]
    private Camera cam;

    private void Awake()
    {
        if (cam == null)
            cam = this.GetComponent<Camera>();

        Initalize();
    }

    public void Initalize()
    {
        cam.clearFlags = CameraClearFlags.Color;
    }

    void OnPostRender()
    {
        cam.clearFlags = CameraClearFlags.Nothing;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
