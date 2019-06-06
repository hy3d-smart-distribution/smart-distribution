using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class KEfadeScene : MonoBehaviour {
    private AndroidPerm.AndroidPermission permissionCheck;
    public Image fade;
	float fades = 5.0f;
	float time = 0;
	// Use this for initialization
	void Start () {

    }

	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (fades > 0.0f && time >= 0.1f) {
			fades -= 0.06f;
			fade.color = new Color (fade.color.r, fade.color.g, fade.color.b, fades);
		} else if (fades <= 0.0f) {
			SceneManager.LoadScene ("hy3d");

        }
	}
}
