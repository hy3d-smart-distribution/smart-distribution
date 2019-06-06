using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideCanvas : MonoBehaviour {
    public GameObject scanCanvas;
    public GameObject listCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void HideScanCanvas(bool t) {
        if (t == true)
        {
            scanCanvas.SetActive(false);
        }
        else if (t == false) {
            scanCanvas.SetActive(true);
        }
    }
    
}
